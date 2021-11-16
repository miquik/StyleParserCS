using StyleParserCS.utils;
using System.IO;
using System.Text;

/*
 * =============================================================================
 * 
 *   Copyright (c) 2014-2017, The UNBESCAPE team (http://www.unbescape.org)
 * 
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 * 
 *       http://www.apache.org/licenses/LICENSE-2.0
 * 
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 * 
 * =============================================================================
 */
namespace StyleParserCS.unbescape
{

    /// <summary>
    /// <para>
    ///   Internal class in charge of performing the real unescape operations.
    /// </para>
    /// 
    /// @author Daniel Fern&aacute;ndez
    /// 
    /// @since 1.0.0
    /// 
    /// </summary>
    internal sealed class CssUnescapeUtil
    {



        /*
		 * CSS STRING AND IDENTIFIER UNESCAPE OPERATIONS
		 * ---------------------------------------------
		 *
		 *   See: http://www.w3.org/TR/CSS21/syndata.html#value-def-identifier
		 *        http://mathiasbynens.be/notes/css-escapes
		 *        http://mothereff.in/css-escapes
		 *
		 *   (Note that, in the following examples, and in order to avoid escape problems during the compilation
		 *    of this class, the backslash symbol is replaced by '%')
		 *
		 *   - BACKSLASH ESCAPES:
		 *        U+0020 -> %  (escape + whitespace)
		 *        U+0021 -> %!
		 *        U+0022 -> %"
		 *        U+0023 -> %#
		 *        U+0024 -> %$
		 *        U+0025 -> %%
		 *        U+0026 -> %&
		 *        U+0027 -> %'
		 *        U+0028 -> %(
		 *        U+0029 -> %)
		 *        U+002A -> %*
		 *        U+002B -> %+
		 *        U+002C -> %,
		 *        U+002D -> %-  [ ONLY USED WHEN IDENTIFIER STARTS WITH -- OR -{DIGIT} ]
		 *        U+002E -> %.
		 *        U+002F -> %/
		 *        U+003A -> %:  [ NOT USED FOR ESCAPING, NOT RECOGNIZED BY IE < 8 ]
		 *        U+003B -> %;
		 *        U+003C -> %<
		 *        U+003D -> %=
		 *        U+003E -> %>
		 *        U+003F -> %?
		 *        U+0040 -> %@
		 *        U+005B -> %[
		 *        U+005C -> %%
		 *        U+005D -> %]
		 *        U+005E -> %^
		 *        U+005F -> %_  [ ONLY USED AT THE BEGINNING OF AN INDENTIFIER, TO AVOID PROBLEMS WITH IE6 ]
		 *        U+0060 -> %`
		 *        U+007B -> %{
		 *        U+007C -> %|
		 *        U+007D -> %}
		 *        U+007E -> %~
		 *
		 *   - UNICODE ESCAPE [HEXA]
		 *        Compact representation: %??* (variable-length. Optionally followed by a whitespace U+0020 - required
		 *                                      if after escape comes a hexadecimal char (0-9a-f) or a whitespace U+0020)
		 *        6-digit representation: %?????? (fixed-length. Not required to be followed by whitespace, unless after
		 *                                        escape comes a whitespace U+0020)
		 *
		 *        Characters > U+FFFF :
		 *              - Standard:      %?????? or %??* (but not supported by older WebKit browsers)
		 *              - Non-standard:  %u????%u???? (surrogate character pair, only in older WebKit browsers)
		 *
		 *   - ESCAPED LINE FEED (strings only): These are Line Continuators. According to the CSS specification (see
		 *        https://www.w3.org/TR/CSS2/syndata.html#escaped-characters and
		 *        https://www.w3.org/TR/CSS2/syndata.html#strings), a backslash followed by a line feed should be ignored
		 *        in output (it is a way to span CSS literals over multiple lines).
		 *
		 */



        /*
		 * Prefixes defined for use in escape and unescape operations
		 */
        private const char ESCAPE_PREFIX = '\\';

        /*
		 * Small utility char arrays for hexadecimal conversion.
		 */
        private static char[] HEXA_CHARS_UPPER = "0123456789ABCDEF".ToCharArray();
        private static char[] HEXA_CHARS_LOWER = "0123456789abcdef".ToCharArray();




        private CssUnescapeUtil() : base()
        {
        }






        /*
		 * This methods (the two versions) are used instead of Integer.parseInt(str,radix) in order to avoid the need
		 * to create substrings of the text being unescaped to feed such method.
		 * -  No need to check all chars are within the radix limits - reference parsing code will already have done so.
		 */

        //ORIGINAL LINE: static int parseIntFromReference(final String text, final int start, final int end, final int radix)
        internal static int parseIntFromReference(string text, int start, int end, int radix)
        {
            int result = 0;
            for (int i = start; i < end; i++)
            {
                //ORIGINAL LINE: final char c = text.charAt(i);
                char c = text[i];
                int n = -1;
                for (int j = 0; j < HEXA_CHARS_UPPER.Length; j++)
                {
                    if (c == HEXA_CHARS_UPPER[j] || c == HEXA_CHARS_LOWER[j])
                    {
                        n = j;
                        break;
                    }
                }
                result = (radix * result) + n;
            }
            return result;
        }

        //ORIGINAL LINE: static int parseIntFromReference(final char[] text, final int start, final int end, final int radix)
        internal static int parseIntFromReference(char[] text, int start, int end, int radix)
        {
            int result = 0;
            for (int i = start; i < end; i++)
            {
                //ORIGINAL LINE: final char c = text[i];
                char c = text[i];
                int n = -1;
                for (int j = 0; j < HEXA_CHARS_UPPER.Length; j++)
                {
                    if (c == HEXA_CHARS_UPPER[j] || c == HEXA_CHARS_LOWER[j])
                    {
                        n = j;
                        break;
                    }
                }
                result = (radix * result) + n;
            }
            return result;
        }





        /*
		 * Perform an unescape operation based on String.
		 */
        //ORIGINAL LINE: static String unescape(final String text)
        internal static string unescape(string text)
        {

            if (string.ReferenceEquals(text, null))
            {
                return null;
            }

            StringBuilder strBuilder = null;

            const int offset = 0;
            //ORIGINAL LINE: final int max = text.length();
            int max = text.Length;

            int readOffset = offset;
            int referenceOffset = offset;

            for (int i = offset; i < max; i++)
            {

                //ORIGINAL LINE: final char c = text.charAt(i);
                char c = text[i];

                /*
				 * Check the need for an unescape operation at this point
				 */

                if (c != ESCAPE_PREFIX || (i + 1) >= max)
                {
                    continue;
                }

                int codepoint = -1;

                if (c == ESCAPE_PREFIX)
                {

                    //ORIGINAL LINE: final char c1 = text.charAt(i + 1);
                    char c1 = text[i + 1];

                    switch (c1)
                    {
                        // line feed. When escaped, this is a line continuator
                        case '\n':
                            codepoint = -2;
                            referenceOffset = i + 1;
                            break;
                        case ' ':
                        case '!':
                        case '"':
                        case '#':
                        case '$':
                        case '%':
                        case '&':
                        case '\'':
                        case '(':
                        case ')':
                        case '*':
                        case '+':
                        case ',':
                        // hyphen: will only be escaped when identifer starts with '--' or '-{digit}'
                        case '-':
                        case '.':
                        case '/':
                        // colon: will not be used for escaping: not recognized by IE < 8
                        case ':':
                        case ';':
                        case '<':
                        case '=':
                        case '>':
                        case '?':
                        case '@':
                        case '[':
                        case '\\':
                        case ']':
                        case '^':
                        // underscore: will only be escaped at the beginning of an identifier (in order to avoid issues in IE6)
                        case '_':
                        case '`':
                        case '{':
                        case '|':
                        case '}':
                        case '~':
                            codepoint = (int)c1;
                            referenceOffset = i + 1;
                            break;
                    }

                    if (codepoint == -1)
                    {

                        if ((c1 >= '0' && c1 <= '9') || (c1 >= 'A' && c1 <= 'F') || (c1 >= 'a' && c1 <= 'f'))
                        {
                            // This is a hexa escape

                            int f = i + 2;
                            while (f < (i + 7) && f < max)
                            {
                                //ORIGINAL LINE: final char cf = text.charAt(f);
                                char cf = text[f];
                                if (!((cf >= '0' && cf <= '9') || (cf >= 'A' && cf <= 'F') || (cf >= 'a' && cf <= 'f')))
                                {
                                    break;
                                }
                                f++;
                            }

                            codepoint = parseIntFromReference(text, i + 1, f, 16);

                            // Fast-forward to the first char after the parsed codepoint
                            referenceOffset = f - 1;

                            // If there is a whitespace after the escape, just ignore it.
                            if (f < max && text[f] == ' ')
                            {
                                referenceOffset++;
                            }

                            // Don't continue here, just let the unescape code below do its job


                        }
                        else if (c1 == '\r' || c1 == '\f')
                        {

                            // The only characters that cannot be escaped by means of a backslash are
                            // carriage return and form feed (besides hexadecimal digits).
                            i++;
                            continue;

                        }
                        else
                        {

                            // We weren't able to consume any valid escape chars, just consider it a normal char,
                            // which is allowed by the CSS escape syntax.

                            codepoint = (int)c1;
                            referenceOffset = i + 1;

                        }

                    }

                }


                /*
				 * At this point we know for sure we will need some kind of unescape, so we
				 * can increase the offset and initialize the string builder if needed, along with
				 * copying to it all the contents pending up to this point.
				 */

                if (strBuilder == null)
                {
                    strBuilder = new StringBuilder(max + 5);
                }

                if (i - readOffset > 0)
                {
                    // TOCHECK!!
                    strBuilder.Append(text, readOffset, (i - readOffset));
                }

                i = referenceOffset;
                readOffset = i + 1;

                /*
				 * --------------------------
				 *
				 * Perform the real unescape
				 *
				 * --------------------------
				 */

                if (codepoint > '\uFFFF')
                {
                    strBuilder.Append(Character.ToChars(codepoint));
                }
                else if (codepoint != -2)
                { // We use -2 to signal the line continuator, which should be ignored in output
                    strBuilder.Append((char)codepoint);
                }

            }


            /*
			 * -----------------------------------------------------------------------------------------------
			 * Final cleaning: return the original String object if no unescape was actually needed. Otherwise
			 *                 append the remaining escaped text to the string builder and return.
			 * -----------------------------------------------------------------------------------------------
			 */

            if (strBuilder == null)
            {
                return text;
            }

            if (max - readOffset > 0)
            {
                strBuilder.Append(text, readOffset, (max - readOffset));
            }

            return strBuilder.ToString();

        }






        /*
		 * Perform an unescape operation based on a Reader, writing the results to a Writer.
		 *
		 * Note this reader is going to be read char-by-char, so some kind of buffering might be appropriate if this
		 * is an inconvenience for the specific Reader implementation.
		 */
        //ORIGINAL LINE: static void unescape(final java.io.Reader reader, final java.io.Writer writer) throws java.io.IOException
        internal static void unescape(TextReader reader, TextWriter writer)
        {

            if (reader == null)
            {
                return;
            }

            int escapei = 0;
            //ORIGINAL LINE: final char[] escapes = new char[6];
            char[] escapes = new char[6];
            int c1, c2, ce; // c1: current char, c2: next char, ce: current escape char

            c2 = reader.Read();

            while (c2 >= 0)
            {

                c1 = c2;
                c2 = reader.Read();

                /*
				 * Check the need for an unescape operation at this point
				 */

                if ((char)c1 != ESCAPE_PREFIX || c2 < 0)
                {
                    writer.Write(c1);
                    continue;
                }

                int codepoint = -1;

                if ((char)c1 == ESCAPE_PREFIX)
                {

                    switch (c2)
                    {
                        // line feed. When escaped, this is a line continuator
                        case '\n':
                            codepoint = -2;
                            c1 = c2;
                            c2 = reader.Read();
                            break;
                        case ' ':
                        case '!':
                        case '"':
                        case '#':
                        case '$':
                        case '%':
                        case '&':
                        case '\'':
                        case '(':
                        case ')':
                        case '*':
                        case '+':
                        case ',':
                        // hyphen: will only be escaped when identifer starts with '--' or '-{digit}'
                        case '-':
                        case '.':
                        case '/':
                        // colon: will not be used for escaping: not recognized by IE < 8
                        case ':':
                        case ';':
                        case '<':
                        case '=':
                        case '>':
                        case '?':
                        case '@':
                        case '[':
                        case '\\':
                        case ']':
                        case '^':
                        // underscore: will only be escaped at the beginning of an identifier (in order to avoid issues in IE6)
                        case '_':
                        case '`':
                        case '{':
                        case '|':
                        case '}':
                        case '~':
                            codepoint = c2;
                            c1 = c2;
                            c2 = reader.Read();
                            break;
                    }

                    if (codepoint == -1)
                    {

                        if ((c2 >= '0' && c2 <= '9') || (c2 >= 'A' && c2 <= 'F') || (c2 >= 'a' && c2 <= 'f'))
                        {
                            // This is a hexa escape

                            escapei = 0;
                            ce = c2;
                            while (ce >= 0 && escapei < 6)
                            {
                                if (!((ce >= '0' && ce <= '9') || (ce >= 'A' && ce <= 'F') || (ce >= 'a' && ce <= 'f')))
                                {
                                    break;
                                }
                                escapes[escapei] = (char)ce;
                                ce = reader.Read();
                                escapei++;
                            }

                            c1 = escapes[5];
                            c2 = ce;

                            codepoint = parseIntFromReference(escapes, 0, escapei, 16);

                            // If there is a whitespace after the escape, just ignore it.
                            if (c2 == ' ')
                            {
                                c1 = c2;
                                c2 = reader.Read();
                            }

                            // Don't continue here, just let the unescape code below do its job


                        }
                        else if (c2 == '\r' || c2 == '\f')
                        {

                            // The only characters that cannot be escaped by means of a backslash are
                            // carriage return and form feed (besides hexadecimal digits).
                            writer.Write(c1);
                            continue;

                        }
                        else
                        {

                            // We weren't able to consume any valid escape chars, just consider it a normal char,
                            // which is allowed by the CSS escape syntax.

                            codepoint = c2;

                            c1 = c2;
                            c2 = reader.Read();

                        }

                    }

                }

                /*
				 * --------------------------
				 *
				 * Perform the real unescape
				 *
				 * --------------------------
				 */

                if (codepoint > '\uFFFF')
                {
                    writer.Write(Character.ToChars(codepoint));
                }
                else if (codepoint != -2)
                { // We use -2 to signal the line continuator, which should be ignored in output
                    writer.Write((char)codepoint);
                }

            }

        }






        /*
		 * Perform an unescape operation based on char[].
		 */
        //ORIGINAL LINE: static void unescape(final char[] text, final int offset, final int len, final java.io.Writer writer) throws java.io.IOException
        internal static void unescape(char[] text, int offset, int len, TextWriter writer)
        {

            if (text == null)
            {
                return;
            }

            //ORIGINAL LINE: final int max = (offset + len);
            int max = (offset + len);

            int readOffset = offset;
            int referenceOffset = offset;

            for (int i = offset; i < max; i++)
            {

                //ORIGINAL LINE: final char c = text[i];
                char c = text[i];

                /*
				 * Check the need for an unescape operation at this point
				 */

                if (c != ESCAPE_PREFIX || (i + 1) >= max)
                {
                    continue;
                }

                int codepoint = -1;

                if (c == ESCAPE_PREFIX)
                {

                    //ORIGINAL LINE: final char c1 = text[i + 1];
                    char c1 = text[i + 1];

                    switch (c1)
                    {
                        // line feed. When escaped, this is a line continuator
                        case '\n':
                            codepoint = -2;
                            referenceOffset = i + 1;
                            break;
                        case ' ':
                        case '!':
                        case '"':
                        case '#':
                        case '$':
                        case '%':
                        case '&':
                        case '\'':
                        case '(':
                        case ')':
                        case '*':
                        case '+':
                        case ',':
                        // hyphen: will only be escaped when identifer starts with '--' or '-{digit}'
                        case '-':
                        case '.':
                        case '/':
                        // colon: will not be used for escaping: not recognized by IE < 8
                        case ':':
                        case ';':
                        case '<':
                        case '=':
                        case '>':
                        case '?':
                        case '@':
                        case '[':
                        case '\\':
                        case ']':
                        case '^':
                        // underscore: will only be escaped at the beginning of an identifier (in order to avoid issues in IE6)
                        case '_':
                        case '`':
                        case '{':
                        case '|':
                        case '}':
                        case '~':
                            codepoint = (int)c1;
                            referenceOffset = i + 1;
                            break;
                    }

                    if (codepoint == -1)
                    {

                        if ((c1 >= '0' && c1 <= '9') || (c1 >= 'A' && c1 <= 'F') || (c1 >= 'a' && c1 <= 'f'))
                        {
                            // This is a hexa escape

                            int f = i + 2;
                            while (f < (i + 7) && f < max)
                            {
                                //ORIGINAL LINE: final char cf = text[f];
                                char cf = text[f];
                                if (!((cf >= '0' && cf <= '9') || (cf >= 'A' && cf <= 'F') || (cf >= 'a' && cf <= 'f')))
                                {
                                    break;
                                }
                                f++;
                            }

                            codepoint = parseIntFromReference(text, i + 1, f, 16);

                            // Fast-forward to the first char after the parsed codepoint
                            referenceOffset = f - 1;

                            // If there is a whitespace after the escape, just ignore it.
                            if (f < max && text[f] == ' ')
                            {
                                referenceOffset++;
                            }

                            // Don't continue here, just let the unescape code below do its job


                        }
                        else if (c1 == '\r' || c1 == '\f')
                        {

                            // The only characters that cannot be escaped by means of a backslash are
                            // carriage return and form feed (besides hexadecimal digits).
                            i++;
                            continue;

                        }
                        else
                        {

                            // We weren't able to consume any valid escape chars, just consider it a normal char,
                            // which is allowed by the CSS escape syntax.

                            codepoint = (int)c1;
                            referenceOffset = i + 1;

                        }

                    }

                }


                /*
				 * At this point we know for sure we will need some kind of unescape, so we
				 * can write all the contents pending up to this point.
				 */

                if (i - readOffset > 0)
                {
                    writer.Write(text, readOffset, (i - readOffset));
                }

                i = referenceOffset;
                readOffset = i + 1;

                /*
				 * --------------------------
				 *
				 * Perform the real unescape
				 *
				 * --------------------------
				 */

                if (codepoint > '\uFFFF')
                {
                    writer.Write(Character.ToChars(codepoint));
                }
                else if (codepoint != -2)
                { // We use -2 to signal the line continuator, which should be ignored in output
                    writer.Write((char)codepoint);
                }

            }


            /*
			 * -----------------------------------------------------------------------------------------------
			 * Final cleaning: append the remaining escaped text to the string builder and return.
			 * -----------------------------------------------------------------------------------------------
			 */

            if (max - readOffset > 0)
            {
                writer.Write(text, readOffset, (max - readOffset));
            }

        }



    }


}