using StyleParserCS.utils;
using System;
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

// https://stackoverflow.com/questions/23751261/codepointat-equivalent-in-c-sharp
namespace StyleParserCS.unbescape
{

    /// <summary>
    /// <para>
    ///   Internal class in charge of performing the real escape operations.
    /// </para>
    /// 
    /// @author Daniel Fern&aacute;ndez
    /// 
    /// @since 1.0.0
    /// 
    /// </summary>
    internal sealed class CssIdentifierEscapeUtil
    {



        /*
		 * CSS IDENTIFIER ESCAPE OPERATIONS
		 * --------------------------------
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


        /*
		 * Structures for holding the Backslash Escapes
		 */
        private static int BACKSLASH_CHARS_LEN = '~' + 1; // 0x7E + 1 = 0x7F
        private static char BACKSLASH_CHARS_NO_ESCAPE = (char)0x0;
        private static char[] BACKSLASH_CHARS;

        /*
		 * Structured for holding the 'escape level' assigned to chars (not codepoints) up to ESCAPE_LEVELS_LEN.
		 * - The last position of the ESCAPE_LEVELS array will be used for determining the level of all
		 *   codepoints >= (ESCAPE_LEVELS_LEN - 1)
		 */
        private static readonly char ESCAPE_LEVELS_LEN = (char)(0x9f + 2); // Last relevant char to be indexed is 0x9f
        private static readonly sbyte[] ESCAPE_LEVELS;



        static CssIdentifierEscapeUtil()
        {

            /*
			 * Initialize Single Escape Characters
			 */
            BACKSLASH_CHARS = new char[BACKSLASH_CHARS_LEN];
            Array.Fill(BACKSLASH_CHARS, BACKSLASH_CHARS_NO_ESCAPE);
            BACKSLASH_CHARS[0x20] = ' ';
            BACKSLASH_CHARS[0x21] = '!';
            BACKSLASH_CHARS[0x22] = '"';
            BACKSLASH_CHARS[0x23] = '#';
            BACKSLASH_CHARS[0x24] = '$';
            BACKSLASH_CHARS[0x25] = '%';
            BACKSLASH_CHARS[0x26] = '&';
            BACKSLASH_CHARS[0x27] = '\'';
            BACKSLASH_CHARS[0x28] = '(';
            BACKSLASH_CHARS[0x29] = ')';
            BACKSLASH_CHARS[0x2A] = '*';
            BACKSLASH_CHARS[0x2B] = '+';
            BACKSLASH_CHARS[0x2C] = ',';
            // hyphen: will only be escaped when identifer starts with '--' or '-{digit}'
            BACKSLASH_CHARS[0x2D] = '-';
            BACKSLASH_CHARS[0x2E] = '.';
            BACKSLASH_CHARS[0x2F] = '/';
            // colon: will not be used for escaping: not recognized by IE < 8
            // BACKSLASH_CHARS[0x3A] = ':';
            BACKSLASH_CHARS[0x3B] = ';';
            BACKSLASH_CHARS[0x3C] = '<';
            BACKSLASH_CHARS[0x3D] = '=';
            BACKSLASH_CHARS[0x3E] = '>';
            BACKSLASH_CHARS[0x3F] = '?';
            BACKSLASH_CHARS[0x40] = '@';
            BACKSLASH_CHARS[0x5B] = '[';
            BACKSLASH_CHARS[0x5C] = '\\';
            BACKSLASH_CHARS[0x5D] = ']';
            BACKSLASH_CHARS[0x5E] = '^';
            // underscore: will only be escaped at the beginning of an identifier (in order to avoid issues in IE6)
            BACKSLASH_CHARS[0x5F] = '_';
            BACKSLASH_CHARS[0x60] = '`';
            BACKSLASH_CHARS[0x7B] = '{';
            BACKSLASH_CHARS[0x7C] = '|';
            BACKSLASH_CHARS[0x7D] = '}';
            BACKSLASH_CHARS[0x7E] = '~';



            /*
			 * Initialization of escape levels.
			 * Defined levels :
			 *
			 *    - Level 1 : Basic escape set
			 *    - Level 2 : Basic escape set plus all non-ASCII
			 *    - Level 3 : All non-alphanumeric characters
			 *    - Level 4 : All characters
			 *
			 */
            ESCAPE_LEVELS = new sbyte[ESCAPE_LEVELS_LEN];

            /*
			 * Everything is level 3 unless contrary indication.
			 */
            Array.Fill(ESCAPE_LEVELS, (sbyte)3);

            /*
			 * Everything non-ASCII is level 2 unless contrary indication.
			 */
            for (char c = (char)0x80; c < ESCAPE_LEVELS_LEN; c++)
            {
                ESCAPE_LEVELS[c] = 2;
            }

            /*
			 * Alphanumeric characters are level 4.
			 */
            for (char c = 'A'; c <= 'Z'; c++)
            {
                ESCAPE_LEVELS[c] = 4;
            }
            for (char c = 'a'; c <= 'z'; c++)
            {
                ESCAPE_LEVELS[c] = 4;
            }
            for (char c = '0'; c <= '9'; c++)
            {
                ESCAPE_LEVELS[c] = 4;
            }

            /*
			 * Backslash Escapes will be level 1 (always escaped)
			 */
            ESCAPE_LEVELS[0x20] = 1;
            ESCAPE_LEVELS[0x21] = 1;
            ESCAPE_LEVELS[0x22] = 1;
            ESCAPE_LEVELS[0x23] = 1;
            ESCAPE_LEVELS[0x24] = 1;
            ESCAPE_LEVELS[0x25] = 1;
            ESCAPE_LEVELS[0x26] = 1;
            ESCAPE_LEVELS[0x27] = 1;
            ESCAPE_LEVELS[0x28] = 1;
            ESCAPE_LEVELS[0x29] = 1;
            ESCAPE_LEVELS[0x2A] = 1;
            ESCAPE_LEVELS[0x2B] = 1;
            ESCAPE_LEVELS[0x2C] = 1;
            // hyphen: will only be escaped when identifer starts with '--' or '-{digit}'
            ESCAPE_LEVELS[0x2D] = 1;
            ESCAPE_LEVELS[0x2E] = 1;
            ESCAPE_LEVELS[0x2F] = 1;
            // colon: will not be used for escaping: not recognized by IE < 8
            ESCAPE_LEVELS[0x3A] = 1;
            ESCAPE_LEVELS[0x3B] = 1;
            ESCAPE_LEVELS[0x3C] = 1;
            ESCAPE_LEVELS[0x3D] = 1;
            ESCAPE_LEVELS[0x3E] = 1;
            ESCAPE_LEVELS[0x3F] = 1;
            ESCAPE_LEVELS[0x40] = 1;
            ESCAPE_LEVELS[0x5B] = 1;
            ESCAPE_LEVELS[0x5C] = 1;
            ESCAPE_LEVELS[0x5D] = 1;
            ESCAPE_LEVELS[0x5E] = 1;
            // underscore: will only be escaped at the beginning of an identifier (in order to avoid issues in IE6)
            ESCAPE_LEVELS[0x5F] = 1;
            ESCAPE_LEVELS[0x60] = 1;
            ESCAPE_LEVELS[0x7B] = 1;
            ESCAPE_LEVELS[0x7C] = 1;
            ESCAPE_LEVELS[0x7D] = 1;
            ESCAPE_LEVELS[0x7E] = 1;


            /*
			 * Two ranges of non-displayable, control characters:
			 * U+0000 to U+001F and U+007F to U+009F.
			 */
            for (char c = (char)0x00; c <= (char)0x1F; c++)
            {
                ESCAPE_LEVELS[c] = 1;
            }
            for (char c = (char)0x7F; c <= (char)0x9F; c++)
            {
                ESCAPE_LEVELS[c] = 1;
            }

        }



        private CssIdentifierEscapeUtil() : base()
        {
        }




        //ORIGINAL LINE: static char[] toCompactHexa(final int codepoint, final char next, final int level)
        internal static char[] toCompactHexa(int codepoint, char next, int level)
        {

            // In identifiers, whitespace will always be escaped (no need for trailing whitespace)
            // If level is 4, hexadecimal characters will be escaped (no need to for trailing whitespaces)
            //ORIGINAL LINE: final boolean needTrailingSpace = (level < 4 && ((next >= '0' && next <= '9') || (next >= 'A' && next <= 'F') || (next >= 'a' && next <= 'f')));
            bool needTrailingSpace = (level < 4 && ((next >= '0' && next <= '9') || (next >= 'A' && next <= 'F') || (next >= 'a' && next <= 'f')));

            if (codepoint == 0)
            {
                return (needTrailingSpace ? new char[] { '0', ' ' } : new char[] { '0' });
            }
            int div = 20;
            char[] result = null;
            while (result == null && div >= 0)
            {
                if (((int)((uint)codepoint >> div)) % 0x10 > 0)
                {
                    result = new char[(div / 4) + (needTrailingSpace ? 2 : 1)];
                }
                div -= 4;
            }
            div = 0;
            for (int i = (needTrailingSpace ? result.Length - 2 : result.Length - 1); i >= 0; i--)
            {
                result[i] = HEXA_CHARS_UPPER[((int)((uint)codepoint >> div)) % 0x10];
                div += 4;
            }
            if (needTrailingSpace)
            {
                result[result.Length - 1] = ' ';
            }

            return result;

        }


        //ORIGINAL LINE: static char[] toSixDigitHexa(final int codepoint, final char next, final int level)
        internal static char[] toSixDigitHexa(int codepoint, char next, int level)
        {

            // In identifiers, whitespace will always be escaped (no need for trailing whitespace)
            const bool needTrailingSpace = false;

            //ORIGINAL LINE: final char[] result = new char[6 + (needTrailingSpace? 1 : 0)];
            char[] result = new char[6 + (needTrailingSpace ? 1 : 0)];
            if (needTrailingSpace)
            {
                result[6] = ' ';
            }
            result[5] = HEXA_CHARS_UPPER[codepoint % 0x10];
            result[4] = HEXA_CHARS_UPPER[((int)((uint)codepoint >> 4)) % 0x10];
            result[3] = HEXA_CHARS_UPPER[((int)((uint)codepoint >> 8)) % 0x10];
            result[2] = HEXA_CHARS_UPPER[((int)((uint)codepoint >> 12)) % 0x10];
            result[1] = HEXA_CHARS_UPPER[((int)((uint)codepoint >> 16)) % 0x10];
            result[0] = HEXA_CHARS_UPPER[((int)((uint)codepoint >> 20)) % 0x10];
            return result;
        }



        /*
		 * Perform an escape operation, based on String, according to the specified level and type.
		 */
        //ORIGINAL LINE: static String escape(final String text, final CssIdentifierEscapeType escapeType, final CssIdentifierEscapeLevel escapeLevel)
        internal static string escape(string text, CssIdentifierEscapeType escapeType, CssIdentifierEscapeLevel escapeLevel)
        {

            if (string.ReferenceEquals(text, null))
            {
                return null;
            }

            //ORIGINAL LINE: final int level = escapeLevel.getEscapeLevel();
            int level = escapeLevel.EscapeLevel;
            //ORIGINAL LINE: final boolean useBackslashEscapes = escapeType.getUseBackslashEscapes();
            bool useBackslashEscapes = escapeType.UseBackslashEscapes;
            //ORIGINAL LINE: final boolean useCompactHexa = escapeType.getUseCompactHexa();
            bool useCompactHexa = escapeType.UseCompactHexa;

            StringBuilder strBuilder = null;

            const int offset = 0;
            //ORIGINAL LINE: final int max = text.length();
            int max = text.Length;

            int readOffset = offset;

            for (int i = offset; i < max; i++)
            {

                //ORIGINAL LINE: final int codepoint = Character.CodePointAt(text, i);
                int codepoint = Character.CodePointAt(text, i);


                /*
				 * Shortcut: most characters will be ASCII/Alphanumeric, and we won't need to do anything at
				 * all for them
				 */
                if (codepoint <= (ESCAPE_LEVELS_LEN - 2) && level < ESCAPE_LEVELS[codepoint] && (i > offset || codepoint < '0' || codepoint > '9'))
                {
                    // Note how we check whether the first char is a decimal number, in which case we have to escape it
                    continue;
                }


                /*
				 * Hyphen check: only escape when it's the first char and it's followed by '-' or a digit.
				 */
                if (codepoint == '-' && level < 3)
                {
                    if (i > offset || i + 1 >= max)
                    {
                        continue;
                    }
                    //ORIGINAL LINE: final char c1 = text.charAt(i + 1);
                    char c1 = text[i + 1];
                    if (c1 != '-' && (c1 < '0' || c1 > '9'))
                    {
                        continue;
                    }
                }


                /*
				 * Underscore check: only escape when it's the first char.
				 */
                if (codepoint == '_' && level < 3 && i > offset)
                {
                    continue;
                }


                /*
				 * Shortcut: we might not want to escape non-ASCII chars at all either.
				 */
                if (codepoint > (ESCAPE_LEVELS_LEN - 2) && level < ESCAPE_LEVELS[ESCAPE_LEVELS_LEN - 1])
                {

                    if (Character.CharCount(codepoint) > 1)
                    {
                        // This is to compensate that we are actually escaping two char[] positions with a single codepoint.
                        i++;
                    }

                    continue;

                }


                /*
				 * At this point we know for sure we will need some kind of escape, so we
				 * can increase the offset and initialize the string builder if needed, along with
				 * copying to it all the contents pending up to this point.
				 */

                if (strBuilder == null)
                {
                    strBuilder = new StringBuilder(max + 20);
                }

                if (i - readOffset > 0)
                {
                    strBuilder.Append(text, readOffset, i);
                }

                if (Character.CharCount(codepoint) > 1)
                {
                    // This is to compensate that we are actually reading two char[] positions with a single codepoint.
                    i++;
                }

                readOffset = i + 1;


                /*
				 * ------------------------------------------------------------------------------------------
				 *
				 * Perform the real escape, attending the different combinations of BACKSLASH and HEXA escapes
				 *
				 * ------------------------------------------------------------------------------------------
				 */

                if (useBackslashEscapes && codepoint < BACKSLASH_CHARS_LEN)
                {
                    // We will try to use a BACKSLASH ESCAPE

                    //ORIGINAL LINE: final char escape = BACKSLASH_CHARS[codepoint];
                    char escape = BACKSLASH_CHARS[codepoint];

                    if (escape != BACKSLASH_CHARS_NO_ESCAPE)
                    {
                        // Escape found! just Write it and go for the next char
                        strBuilder.Append(ESCAPE_PREFIX);
                        strBuilder.Append(escape);
                        continue;
                    }

                }

                /*
				 * No escape was possible, so we need hexa escape (compact or 6-digit).
				 */

                //ORIGINAL LINE: final char next = ((i + 1 < max) ? text.charAt(i + 1) : (char) 0x0);
                char next = ((i + 1 < max) ? text[i + 1] : (char)0x0);

                if (useCompactHexa)
                {
                    strBuilder.Append(ESCAPE_PREFIX);
                    strBuilder.Append(toCompactHexa(codepoint, next, level));
                    continue;
                }

                strBuilder.Append(ESCAPE_PREFIX);
                strBuilder.Append(toSixDigitHexa(codepoint, next, level));

            }


            /*
			 * -----------------------------------------------------------------------------------------------
			 * Final cleaning: return the original String object if no escape was actually needed. Otherwise
			 *                 append the remaining unescaped text to the string builder and return.
			 * -----------------------------------------------------------------------------------------------
			 */

            if (strBuilder == null)
            {
                return text;
            }

            if (max - readOffset > 0)
            {
                strBuilder.Append(text, readOffset, max);
            }

            return strBuilder.ToString();

        }



        /*
		 * Perform an escape operation, based on a Reader, according to the specified level and type and writing the
		 * result to a Writer.
		 *
		 * Note this reader is going to be Read char-by-char, so some kind of buffering might be appropriate if this
		 * is an inconvenience for the specific Reader implementation.
		 */
        //ORIGINAL LINE: static void escape(final java.io.Reader reader, final java.io.Writer writer, final CssIdentifierEscapeType escapeType, final CssIdentifierEscapeLevel escapeLevel) throws java.io.IOException
        internal static void escape(TextReader reader, TextWriter writer, CssIdentifierEscapeType escapeType, CssIdentifierEscapeLevel escapeLevel)
        {

            if (reader == null)
            {
                return;
            }

            //ORIGINAL LINE: final int level = escapeLevel.getEscapeLevel();
            int level = escapeLevel.EscapeLevel;
            //ORIGINAL LINE: final boolean useBackslashEscapes = escapeType.getUseBackslashEscapes();
            bool useBackslashEscapes = escapeType.UseBackslashEscapes;
            //ORIGINAL LINE: final boolean useCompactHexa = escapeType.getUseCompactHexa();
            bool useCompactHexa = escapeType.UseCompactHexa;

            int c0, c1, c2; // c0: last char, c1: current char, c2: next char

            c1 = -1;
            c2 = reader.Read();

            while (c2 >= 0)
            {

                c0 = c1;
                c1 = c2;
                c2 = reader.Read();

                //ORIGINAL LINE: final int codepoint = CodePointAt((char)c1, (char)c2);
                int codepoint = CodePointAt((char)c1, (char)c2);


                /*
				 * Shortcut: most characters will be ASCII/Alphanumeric, and we won't need to do anything at
				 * all for them
				 */
                if (codepoint <= (ESCAPE_LEVELS_LEN - 2) && level < ESCAPE_LEVELS[codepoint] && (c0 >= 0 || codepoint < '0' || codepoint > '9'))
                {
                    // Note how we check whether the first char is a decimal number, in which case we have to escape it
                    writer.Write(c1);
                    continue;
                }


                /*
				 * Hyphen check: only escape when it's the first char and it's followed by '-' or a digit.
				 */
                if (codepoint == '-' && level < 3)
                {
                    if (c0 >= 0 || c2 < 0)
                    { // not first or last
                        writer.Write(c1);
                        continue;
                    }
                    if (c2 != '-' && (c2 < '0' || c2 > '9'))
                    {
                        writer.Write(c1);
                        continue;
                    }
                }


                /*
				 * Underscore check: only escape when it's the first char.
				 */
                if (codepoint == '_' && level < 3 && c0 >= 0)
                {
                    writer.Write(c1);
                    continue;
                }


                /*
				 * Shortcut: we might not want to escape non-ASCII chars at all either.
				 */
                if (codepoint > (ESCAPE_LEVELS_LEN - 2) && level < ESCAPE_LEVELS[ESCAPE_LEVELS_LEN - 1])
                {

                    writer.Write(c1);

                    if (Character.CharCount(codepoint) > 1)
                    {
                        // This is to compensate that we are actually escaping two char[] positions with a single codepoint.

                        writer.Write(c2);

                        c0 = c1;
                        c1 = c2;
                        c2 = reader.Read();

                    }

                    continue;

                }


                /*
				 * We know we need to escape, so from here on we will only work with the codepoint -- we can advance
				 * the chars.
				 */

                if (Character.CharCount(codepoint) > 1)
                {
                    // This is to compensate that we are actually reading two char positions with a single codepoint.
                    c0 = c1;
                    c1 = c2;
                    c2 = reader.Read();
                }


                /*
				 * ------------------------------------------------------------------------------------------
				 *
				 * Perform the real escape, attending the different combinations of BACKSLASH and HEXA escapes
				 *
				 * ------------------------------------------------------------------------------------------
				 */

                if (useBackslashEscapes && codepoint < BACKSLASH_CHARS_LEN)
                {
                    // We will try to use a BACKSLASH ESCAPE

                    //ORIGINAL LINE: final char escape = BACKSLASH_CHARS[codepoint];
                    char escape = BACKSLASH_CHARS[codepoint];

                    if (escape != BACKSLASH_CHARS_NO_ESCAPE)
                    {
                        // Escape found! just Write it and go for the next char
                        writer.Write(ESCAPE_PREFIX);
                        writer.Write(escape);
                        continue;
                    }

                }

                /*
				 * No escape was possible, so we need hexa escape (compact or 6-digit).
				 */

                //ORIGINAL LINE: final char next = (c2 >= 0 ? (char) c2 : (char) 0x0);
                char next = (c2 >= 0 ? (char)c2 : (char)0x0);

                if (useCompactHexa)
                {
                    writer.Write(ESCAPE_PREFIX);
                    writer.Write(toCompactHexa(codepoint, next, level));
                    continue;
                }

                writer.Write(ESCAPE_PREFIX);
                writer.Write(toSixDigitHexa(codepoint, next, level));

            }

        }





        /*
		 * Perform an escape operation, based on char[], according to the specified level and type.
		 */
        //ORIGINAL LINE: static void escape(final char[] text, final int offset, final int len, final java.io.Writer writer, final CssIdentifierEscapeType escapeType, final CssIdentifierEscapeLevel escapeLevel) throws java.io.IOException
        internal static void escape(char[] text, int offset, int len, TextWriter writer, CssIdentifierEscapeType escapeType, CssIdentifierEscapeLevel escapeLevel)
        {

            if (text == null || text.Length == 0)
            {
                return;
            }

            //ORIGINAL LINE: final int level = escapeLevel.getEscapeLevel();
            int level = escapeLevel.EscapeLevel;
            //ORIGINAL LINE: final boolean useBackslashEscapes = escapeType.getUseBackslashEscapes();
            bool useBackslashEscapes = escapeType.UseBackslashEscapes;
            //ORIGINAL LINE: final boolean useCompactHexa = escapeType.getUseCompactHexa();
            bool useCompactHexa = escapeType.UseCompactHexa;

            //ORIGINAL LINE: final int max = (offset + len);
            int max = (offset + len);

            int readOffset = offset;

            for (int i = offset; i < max; i++)
            {

                //ORIGINAL LINE: final int codepoint = Character.CodePointAt(text, i);
                int codepoint = Character.CodePointAt(text, i);


                /*
				 * Shortcut: most characters will be ASCII/Alphanumeric, and we won't need to do anything at
				 * all for them
				 */
                if (codepoint <= (ESCAPE_LEVELS_LEN - 2) && level < ESCAPE_LEVELS[codepoint] && (i > offset || codepoint < '0' || codepoint > '9'))
                {
                    // Note how we check whether the first char is a decimal number, in which case we have to escape it
                    continue;
                }


                /*
				 * Hyphen check: only escape when it's the first char and it's followed by '-' or a digit.
				 */
                if (codepoint == '-' && level < 3)
                {
                    if (i > offset || i + 1 >= max)
                    {
                        continue;
                    }
                    //ORIGINAL LINE: final char c1 = text[i + 1];
                    char c1 = text[i + 1];
                    if (c1 != '-' && (c1 < '0' || c1 > '9'))
                    {
                        continue;
                    }
                }


                /*
				 * Underscore check: only escape when it's the first char.
				 */
                if (codepoint == '_' && level < 3 && i > offset)
                {
                    continue;
                }


                /*
				 * Shortcut: we might not want to escape non-ASCII chars at all either.
				 */
                if (codepoint > (ESCAPE_LEVELS_LEN - 2) && level < ESCAPE_LEVELS[ESCAPE_LEVELS_LEN - 1])
                {

                    if (Character.CharCount(codepoint) > 1)
                    {
                        // This is to compensate that we are actually escaping two char[] positions with a single codepoint.
                        i++;
                    }

                    continue;

                }


                /*
				 * At this point we know for sure we will need some kind of escape, so we
				 * can Write all the contents pending up to this point.
				 */

                if (i - readOffset > 0)
                {
                    writer.Write(text, readOffset, (i - readOffset));
                }

                if (Character.CharCount(codepoint) > 1)
                {
                    // This is to compensate that we are actually reading two char[] positions with a single codepoint.
                    i++;
                }

                readOffset = i + 1;


                /*
				 * ------------------------------------------------------------------------------------------
				 *
				 * Perform the real escape, attending the different combinations of BACKSLASH and HEXA escapes
				 *
				 * ------------------------------------------------------------------------------------------
				 */

                if (useBackslashEscapes && codepoint < BACKSLASH_CHARS_LEN)
                {
                    // We will try to use a BACKSLASH ESCAPE

                    //ORIGINAL LINE: final char escape = BACKSLASH_CHARS[codepoint];
                    char escape = BACKSLASH_CHARS[codepoint];

                    if (escape != BACKSLASH_CHARS_NO_ESCAPE)
                    {
                        // Escape found! just Write it and go for the next char
                        writer.Write(ESCAPE_PREFIX);
                        writer.Write(escape);
                        continue;
                    }

                }

                /*
				 * No escape was possible, so we need hexa escape (compact or 6-digit).
				 */

                //ORIGINAL LINE: final char next = ((i + 1 < max) ? text[i + 1] : (char) 0x0);
                char next = ((i + 1 < max) ? text[i + 1] : (char)0x0);

                if (useCompactHexa)
                {
                    writer.Write(ESCAPE_PREFIX);
                    writer.Write(toCompactHexa(codepoint, next, level));
                    continue;
                }

                writer.Write(ESCAPE_PREFIX);
                writer.Write(toSixDigitHexa(codepoint, next, level));

            }


            /*
			 * -----------------------------------------------------------------------------------------------
			 * Final cleaning: append the remaining unescaped text to the string builder and return.
			 * -----------------------------------------------------------------------------------------------
			 */

            if (max - readOffset > 0)
            {
                writer.Write(text, readOffset, (max - readOffset));
            }

        }




        //ORIGINAL LINE: private static int CodePointAt(final char c1, final char c2)
        private static int CodePointAt(char c1, char c2)
        {
            if (char.IsHighSurrogate(c1))
            {
                if (c2 >= (char)0)
                {
                    if (char.IsLowSurrogate(c2))
                    {
                        return Character.ToCodePoint(c1, c2);
                    }
                }
            }
            return c1;
        }



    }


}