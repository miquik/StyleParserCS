using System;
using System.IO;

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
    ///   Utility class for performing CSS escape/unescape operations.
    /// </para>
    /// 
    /// <para>
    ///   This class supports both escaping of <strong>CSS identifiers</strong> and
    ///   <strong>CSS Strings</strong> (or <em>literals</em>).
    /// </para>
    /// 
    /// <strong><u>Configuration of escape/unescape operations</u></strong>
    /// 
    /// <para>
    ///   <strong>Escape</strong> operations can be (optionally) configured by means of:
    /// </para>
    /// <ul>
    ///   <li><em>Level</em>, which defines how deep the escape operation must be (what
    ///       chars are to be considered eligible for escaping, depending on the specific
    ///       needs of the scenario). Its values are defined by the <seealso cref="CssIdentifierEscapeLevel"/>
    ///       and <seealso cref="CssStringEscapeLevel"/> enums.</li>
    ///   <li><em>Type</em>, which defines whether escaping should be performed by means of <em>backslash escapes</em>
    ///       or by means of hexadecimal numerical escape sequences.
    ///       Its values are defined by the <seealso cref="CssIdentifierEscapeType"/>
    ///       and <seealso cref="CssStringEscapeType"/> enums.</li>
    /// </ul>
    /// <para>
    ///   <strong>Unescape</strong> operations need no configuration parameters. Unescape operations
    ///   will always perform <em>complete</em> unescape of backslash and hexadecimal escapes, including all
    ///   required <em>tweaks</em> (i.e. optional whitespace characters) needed for unescaping.
    /// </para>
    /// 
    /// <strong><u>Features</u></strong>
    /// 
    /// <para>
    ///   Specific features of the CSS escape/unescape operations performed by means of this class:
    /// </para>
    /// <ul>
    ///   <li>Complete set of CSS <em>Backslash Escapes</em> supported (e.g. <tt>&#92;+</tt>, <tt>&#92;(</tt>,
    ///       <tt>&#92;)</tt>, etc.).</li>
    ///   <li>Full set of escape syntax rules supported, both for <strong>CSS identifiers</strong> and
    ///       <strong>CSS Strings</strong> (or <em>literals</em>).</li>
    ///   <li>Non-standard tweaks supported: <tt>&#92;:</tt> not used because of lacking support in
    ///       Internet Explorer &lt; 8, <tt>&#92;_</tt> escaped at the beginning of identifiers for better
    ///       Internet Explorer 6 support, etc.</li>
    ///   <li>Hexadecimal escapes (a.k.a. <em>unicode escapes</em>) are supported both in escape
    ///       and unescape operations, and both in <em>compact</em> (<tt>&#92;E1 </tt>) and six-digit
    ///       forms (<tt>&#92;0000E1</tt>).</li>
    ///   <li>Support for the whole Unicode character set: <tt>&#92;u0000</tt> to <tt>&#92;u10FFFF</tt>, including
    ///       characters not representable by only one <tt>char</tt> in Java (<tt>&gt;&#92;uFFFF</tt>).</li>
    ///   <li>Support for unescaping unicode characters &gt; U+FFFF both when represented in standard form (one char,
    ///       <tt>&#92;20000</tt>) and non-standard (surrogate pair, <tt>&#92;D840&#92;DC00</tt>, used by older
    ///       WebKit browsers).</li>
    /// </ul>
    /// 
    /// <strong><u>Input/Output</u></strong>
    /// 
    /// <para>
    ///   There are four different input/output modes that can be used in escape/unescape operations:
    /// </para>
    /// <ul>
    ///   <li><em><tt>String</tt> input, <tt>String</tt> output</em>: Input is specified as a <tt>String</tt> object
    ///       and output is returned as another. In order to improve memory performance, all escape and unescape
    ///       operations <u>will return the exact same input object as output if no escape/unescape modifications
    ///       are required</u>.</li>
    ///   <li><em><tt>String</tt> input, <tt>java.io.TextWriter</tt> output</em>: Input will be read from a String
    ///       and output will be written into the specified <tt>java.io.TextWriter</tt>.</li>
    ///   <li><em><tt>java.io.TextReader</tt> input, <tt>java.io.TextWriter</tt> output</em>: Input will be read from a TextReader
    ///       and output will be written into the specified <tt>java.io.TextWriter</tt>.</li>
    ///   <li><em><tt>char[]</tt> input, <tt>java.io.TextWriter</tt> output</em>: Input will be read from a char array
    ///       (<tt>char[]</tt>) and output will be written into the specified <tt>java.io.TextWriter</tt>.
    ///       Two <tt>int</tt> arguments called <tt>offset</tt> and <tt>len</tt> will be
    ///       used for specifying the part of the <tt>char[]</tt> that should be escaped/unescaped. These methods
    ///       should be called with <tt>offset = 0</tt> and <tt>len = text.length</tt> in order to process
    ///       the whole <tt>char[]</tt>.</li>
    /// </ul>
    /// 
    /// <strong><u>Glossary</u></strong>
    /// 
    /// <dl>
    ///   <dt>Backslash escapes</dt>
    ///     <dd>Escape sequences performed by means of prefixing a <em>backslash</em> (<tt>&#92;</tt>) to
    ///         the escaped char: <tt>&#92;+</tt>, <tt>&#92;(</tt>, <tt>&#92;)</tt></dd>
    ///   <dt>HEXA escapes</dt>
    ///     <dd>Complete representation of unicode codepoints up to <tt>U+10FFFF</tt>, in two forms:
    ///         <ul>
    ///           <li><em>Compact</em>: non-zero-padded hexadecimal representation (<tt>&#92;E1 </tt>), followed
    ///               by an optional whitespace (<tt>U+0020</tt>), required if after the escaped character comes
    ///               a hexadecimal digit (<tt>[0-9A-Fa-f]</tt>) or another whitespace (<tt>&nbsp;</tt>).</li>
    ///           <li><em>Six-digit</em>: zero-padded hexadecimal representation (<tt>&#92;0000E1</tt>), followed
    ///               by an optional whitespace (<tt>U+0020</tt>), required if after the escaped character comes
    ///               another whitespace (<tt>&nbsp;</tt>).</li>
    ///         </ul>
    ///     </dd>
    ///   <dt>Unicode Codepoint</dt>
    ///     <dd>Each of the <tt>int</tt> values conforming the Unicode code space.
    ///         Normally corresponding to a Java <tt>char</tt> primitive value (codepoint &lt;= <tt>&#92;uFFFF</tt>),
    ///         but might be two <tt>char</tt>s for codepoints <tt>&#92;u10000</tt> to <tt>&#92;u10FFFF</tt> if the
    ///         first <tt>char</tt> is a high surrogate (<tt>&#92;uD800</tt> to <tt>&#92;uDBFF</tt>) and the
    ///         second is a low surrogate (<tt>&#92;uDC00</tt> to <tt>&#92;uDFFF</tt>).</dd>
    /// </dl>
    /// 
    /// <strong><u>References</u></strong>
    /// 
    /// <para>
    ///   The following references apply:
    /// </para>
    /// <ul>
    ///   <li><a href="http://www.w3.org/TR/CSS21/syndata.html#value-def-identifier" target="_blank">Cascading
    ///       Style Sheets Level 2 Revision 1 (CSS 2.1) Specification</a> [w3.org]</li>
    ///   <li><a href="http://mathiasbynens.be/notes/css-escapes">CSS character escape sequences</a> [mathiasbynens.be]</li>
    ///   <li><a href="http://mothereff.in/css-escapes">CSS escapes tester</a> [mothereff.in]</li>
    /// </ul>
    /// 
    /// 
    /// @author Daniel Fern&aacute;ndez
    /// 
    /// @since 1.0.0
    /// 
    /// </summary>
    public sealed class CssEscape
    {




        /// <summary>
        /// <para>
        ///   Perform a CSS String level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS String basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls <seealso cref="escapeCssString(string, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <returns> The escaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no escaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String escapeCssStringMinimal(final String text)
        public static string escapeCssStringMinimal(string text)
        {
            return escapeCssString(text, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS String level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS String basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssString(string, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <returns> The escaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no escaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String escapeCssString(final String text)
        public static string escapeCssString(string text)
        {
            return escapeCssString(text, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS String <strong>escape</strong> operation on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssStringEscapeType"/> and
        ///   <seealso cref="CssStringEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>String</tt>-based <tt>escapeCssString*(...)</tt> methods call this one with preconfigured
        ///   <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssStringEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssStringEscapeLevel"/>. </param>
        /// <returns> The escaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no escaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String escapeCssString(final String text, final CssStringEscapeType type, final CssStringEscapeLevel level)
        public static string escapeCssString(string text, CssStringEscapeType type, CssStringEscapeLevel level)
        {

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            return CssStringEscapeUtil.escape(text, type, level);

        }




        /// <summary>
        /// <para>
        ///   Perform a CSS String level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS String basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls <seealso cref="escapeCssString(string, TextWriter, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssStringMinimal(final String text, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssStringMinimal(string text, TextWriter writer)
        {
            escapeCssString(text, writer, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS String level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS String basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssString(string, TextWriter, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssString(final String text, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssString(string text, TextWriter writer)
        {
            escapeCssString(text, writer, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS String <strong>escape</strong> operation on a <tt>String</tt> input,
        ///   writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssStringEscapeType"/> and
        ///   <seealso cref="CssStringEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>String</tt>/<tt>TextWriter</tt>-based <tt>escapeCssString*(...)</tt> methods call this one
        ///   with preconfigured <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssStringEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssStringEscapeLevel"/>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssString(final String text, final java.io.TextWriter writer, final CssStringEscapeType type, final CssStringEscapeLevel level) throws java.io.IOException
        public static void escapeCssString(string text, TextWriter writer, CssStringEscapeType type, CssStringEscapeLevel level)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            CssStringEscapeUtil.escape(new InternalStringReader(text), writer, type, level);

        }




        /// <summary>
        /// <para>
        ///   Perform a CSS String level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>TextReader</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS String basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls <seealso cref="escapeCssString(TextReader, TextWriter, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssStringMinimal(final java.io.TextReader reader, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssStringMinimal(TextReader reader, TextWriter writer)
        {
            escapeCssString(reader, writer, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS String level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>TextReader</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS String basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssString(TextReader, TextWriter, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssString(final java.io.TextReader reader, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssString(TextReader reader, TextWriter writer)
        {
            escapeCssString(reader, writer, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS String <strong>escape</strong> operation on a <tt>TextReader</tt> input,
        ///   writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssStringEscapeType"/> and
        ///   <seealso cref="CssStringEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>TextReader</tt>/<tt>TextWriter</tt>-based <tt>escapeCssString*(...)</tt> methods call this one
        ///   with preconfigured <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssStringEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssStringEscapeLevel"/>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssString(final java.io.TextReader reader, final java.io.TextWriter writer, final CssStringEscapeType type, final CssStringEscapeLevel level) throws java.io.IOException
        public static void escapeCssString(TextReader reader, TextWriter writer, CssStringEscapeType type, CssStringEscapeLevel level)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            CssStringEscapeUtil.escape(reader, writer, type, level);

        }




        /// <summary>
        /// <para>
        ///   Perform a CSS String level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS String basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssString(char[], int, int, java.io.TextWriter, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be escaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the escape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void escapeCssStringMinimal(final char[] text, final int offset, final int len, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssStringMinimal(char[] text, int offset, int len, TextWriter writer)
        {
            escapeCssString(text, offset, len, writer, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS String level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS String basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>) and
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>).
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssString(char[], int, int, java.io.TextWriter, CssStringEscapeType, CssStringEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be escaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the escape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void escapeCssString(final char[] text, final int offset, final int len, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssString(char[] text, int offset, int len, TextWriter writer)
        {
            escapeCssString(text, offset, len, writer, CssStringEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssStringEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS String <strong>escape</strong> operation on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssStringEscapeType"/> and
        ///   <seealso cref="CssStringEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>char[]</tt>-based <tt>escapeCssString*(...)</tt> methods call this one with preconfigured
        ///   <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be escaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the escape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssStringEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssStringEscapeLevel"/>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void escapeCssString(final char[] text, final int offset, final int len, final java.io.TextWriter writer, final CssStringEscapeType type, final CssStringEscapeLevel level) throws java.io.IOException
        public static void escapeCssString(char[] text, int offset, int len, TextWriter writer, CssStringEscapeType type, CssStringEscapeLevel level)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            //ORIGINAL LINE: final int textLen = (text == null? 0 : text.length);
            int textLen = (text == null ? 0 : text.Length);

            if (offset < 0 || offset > textLen)
            {
                throw new System.ArgumentException("Invalid (offset, len). offset=" + offset + ", len=" + len + ", text.length=" + textLen);
            }

            if (len < 0 || (offset + len) > textLen)
            {
                throw new System.ArgumentException("Invalid (offset, len). offset=" + offset + ", len=" + len + ", text.length=" + textLen);
            }

            CssStringEscapeUtil.escape(text, offset, len, writer, type, level);

        }









        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS Identifier basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///       <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///       <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///       <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///       <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///       <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///       <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///       <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///       <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///       <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///       <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///       <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///       <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///       <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///       <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///       <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///       <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///       <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///       <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///       <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///       <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///       <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///       <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///       <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///       <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///       <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///       <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///       <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///       Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///       when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///       (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///       problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///       (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///       used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls <seealso cref="escapeCssIdentifier(string, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <returns> The escaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no escaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String escapeCssIdentifierMinimal(final String text)
        public static string escapeCssIdentifierMinimal(string text)
        {
            return escapeCssIdentifier(text, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS Identifier basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///               <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///               <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///               <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///               <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///               <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///               <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///               <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///               <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///               <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///               <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///               <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///               <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///               <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///               <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///               <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///               <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///               <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///               <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///               <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///               <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///               <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///               <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///               <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///               <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///               <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///               <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///               <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///               Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///               when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///               (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///               problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///               (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///               used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssIdentifier(string, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <returns> The escaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no escaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String escapeCssIdentifier(final String text)
        public static string escapeCssIdentifier(string text)
        {
            return escapeCssIdentifier(text, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS Identifier <strong>escape</strong> operation on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssIdentifierEscapeType"/> and
        ///   <seealso cref="CssIdentifierEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>String</tt>-based <tt>escapeCssIdentifier*(...)</tt> methods call this one with preconfigured
        ///   <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssIdentifierEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssIdentifierEscapeLevel"/>. </param>
        /// <returns> The escaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no escaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String escapeCssIdentifier(final String text, final CssIdentifierEscapeType type, final CssIdentifierEscapeLevel level)
        public static string escapeCssIdentifier(string text, CssIdentifierEscapeType type, CssIdentifierEscapeLevel level)
        {

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            return CssIdentifierEscapeUtil.escape(text, type, level);

        }




        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS Identifier basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///       <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///       <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///       <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///       <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///       <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///       <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///       <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///       <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///       <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///       <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///       <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///       <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///       <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///       <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///       <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///       <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///       <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///       <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///       <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///       <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///       <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///       <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///       <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///       <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///       <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///       <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///       <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///       Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///       when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///       (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///       problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///       (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///       used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls <seealso cref="escapeCssIdentifier(string, TextWriter, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifierMinimal(final String text, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssIdentifierMinimal(string text, TextWriter writer)
        {
            escapeCssIdentifier(text, writer, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>String</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS Identifier basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///               <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///               <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///               <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///               <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///               <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///               <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///               <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///               <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///               <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///               <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///               <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///               <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///               <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///               <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///               <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///               <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///               <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///               <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///               <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///               <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///               <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///               <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///               <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///               <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///               <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///               <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///               <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///               Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///               when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///               (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///               problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///               (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///               used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssIdentifier(string, TextWriter, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifier(final String text, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssIdentifier(string text, TextWriter writer)
        {
            escapeCssIdentifier(text, writer, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS Identifier <strong>escape</strong> operation on a <tt>String</tt> input,
        ///   writing the results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssIdentifierEscapeType"/> and
        ///   <seealso cref="CssIdentifierEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>String</tt>/<tt>TextWriter</tt>-based <tt>escapeCssIdentifier*(...)</tt> methods call this one with preconfigured
        ///   <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssIdentifierEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssIdentifierEscapeLevel"/>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifier(final String text, final java.io.TextWriter writer, final CssIdentifierEscapeType type, final CssIdentifierEscapeLevel level) throws java.io.IOException
        public static void escapeCssIdentifier(string text, TextWriter writer, CssIdentifierEscapeType type, CssIdentifierEscapeLevel level)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            CssIdentifierEscapeUtil.escape(new InternalStringReader(text), writer, type, level);

        }




        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>TextReader</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS Identifier basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///       <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///       <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///       <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///       <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///       <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///       <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///       <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///       <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///       <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///       <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///       <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///       <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///       <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///       <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///       <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///       <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///       <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///       <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///       <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///       <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///       <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///       <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///       <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///       <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///       <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///       <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///       <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///       Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///       when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///       (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///       problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///       (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///       used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls <seealso cref="escapeCssIdentifier(TextReader, TextWriter, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifierMinimal(final java.io.TextReader reader, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssIdentifierMinimal(TextReader reader, TextWriter writer)
        {
            escapeCssIdentifier(reader, writer, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>TextReader</tt> input, writing results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS Identifier basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///               <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///               <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///               <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///               <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///               <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///               <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///               <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///               <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///               <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///               <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///               <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///               <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///               <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///               <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///               <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///               <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///               <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///               <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///               <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///               <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///               <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///               <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///               <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///               <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///               <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///               <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///               <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///               Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///               when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///               (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///               problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///               (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///               used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssIdentifier(TextReader, TextWriter, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifier(final java.io.TextReader reader, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssIdentifier(TextReader reader, TextWriter writer)
        {
            escapeCssIdentifier(reader, writer, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS Identifier <strong>escape</strong> operation on a <tt>TextReader</tt> input,
        ///   writing the results to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssIdentifierEscapeType"/> and
        ///   <seealso cref="CssIdentifierEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>TextReader</tt>/<tt>TextWriter</tt>-based <tt>escapeCssIdentifier*(...)</tt> methods call this one with preconfigured
        ///   <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssIdentifierEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssIdentifierEscapeLevel"/>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs
        /// 
        /// @since 1.1.2 </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifier(final java.io.TextReader reader, final java.io.TextWriter writer, final CssIdentifierEscapeType type, final CssIdentifierEscapeLevel level) throws java.io.IOException
        public static void escapeCssIdentifier(TextReader reader, TextWriter writer, CssIdentifierEscapeType type, CssIdentifierEscapeLevel level)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            CssIdentifierEscapeUtil.escape(reader, writer, type, level);

        }




        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 1 (only basic set) <strong>escape</strong> operation
        ///   on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 1</em> means this method will only escape the CSS Identifier basic escape set:
        /// </para>
        /// <ul>
        ///   <li>The <em>Backslash Escapes</em>:
        ///       <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///       <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///       <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///       <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///       <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///       <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///       <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///       <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///       <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///       <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///       <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///       <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///       <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///       <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///       <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///       <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///       <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///       <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///       <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///       <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///       <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///       <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///       <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///       <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///       <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///       <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///       <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///       <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///       <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///       <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///       Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///       when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///       (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///       problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///       (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///       used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///   </li>
        ///   <li>
        ///       Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///       and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///   </li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssIdentifier(char[], int, int, java.io.TextWriter, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be escaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the escape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifierMinimal(final char[] text, final int offset, final int len, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssIdentifierMinimal(char[] text, int offset, int len, TextWriter writer)
        {
            escapeCssIdentifier(text, offset, len, writer, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_1_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS Identifier level 2 (basic set and all non-ASCII chars) <strong>escape</strong> operation
        ///   on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   <em>Level 2</em> means this method will escape:
        /// </para>
        /// <ul>
        ///   <li>The CSS Identifier basic escape set:
        ///         <ul>
        ///           <li>The <em>Backslash Escapes</em>:
        ///               <tt>&#92; </tt> (<tt>U+0020</tt>),
        ///               <tt>&#92;!</tt> (<tt>U+0021</tt>),
        ///               <tt>&#92;&quot;</tt> (<tt>U+0022</tt>),
        ///               <tt>&#92;#</tt> (<tt>U+0023</tt>),
        ///               <tt>&#92;$</tt> (<tt>U+0024</tt>),
        ///               <tt>&#92;%</tt> (<tt>U+0025</tt>),
        ///               <tt>&#92;&amp;</tt> (<tt>U+0026</tt>),
        ///               <tt>&#92;&#39;</tt> (<tt>U+0027</tt>),
        ///               <tt>&#92;(</tt> (<tt>U+0028</tt>),
        ///               <tt>&#92;)</tt> (<tt>U+0029</tt>),
        ///               <tt>&#92;*</tt> (<tt>U+002A</tt>),
        ///               <tt>&#92;+</tt> (<tt>U+002B</tt>),
        ///               <tt>&#92;,</tt> (<tt>U+002C</tt>),
        ///               <tt>&#92;.</tt> (<tt>U+002E</tt>),
        ///               <tt>&#92;&#47;</tt> (<tt>U+002F</tt>),
        ///               <tt>&#92;;</tt> (<tt>U+003B</tt>),
        ///               <tt>&#92;&lt;</tt> (<tt>U+003C</tt>),
        ///               <tt>&#92;=</tt> (<tt>U+003D</tt>),
        ///               <tt>&#92;&gt;</tt> (<tt>U+003E</tt>),
        ///               <tt>&#92;?</tt> (<tt>U+003F</tt>),
        ///               <tt>&#92;@</tt> (<tt>U+0040</tt>),
        ///               <tt>&#92;[</tt> (<tt>U+005B</tt>),
        ///               <tt>&#92;&#92;</tt> (<tt>U+005C</tt>),
        ///               <tt>&#92;]</tt> (<tt>U+005D</tt>),
        ///               <tt>&#92;^</tt> (<tt>U+005E</tt>),
        ///               <tt>&#92;`</tt> (<tt>U+0060</tt>),
        ///               <tt>&#92;{</tt> (<tt>U+007B</tt>),
        ///               <tt>&#92;|</tt> (<tt>U+007C</tt>),
        ///               <tt>&#92;}</tt> (<tt>U+007D</tt>) and
        ///               <tt>&#92;~</tt> (<tt>U+007E</tt>).
        ///               Note that the <tt>&#92;-</tt> (<tt>U+002D</tt>) escape sequence exists, but will only be used
        ///               when an identifier starts with two hypens or hyphen + digit. Also, the <tt>&#92;_</tt>
        ///               (<tt>U+005F</tt>) escape will only be used at the beginning of an identifier to avoid
        ///               problems with Internet Explorer 6. In the same sense, note that the <tt>&#92;:</tt>
        ///               (<tt>U+003A</tt>) escape sequence is also defined in the standard, but will not be
        ///               used for escaping as Internet Explorer &lt; 8 does not recognize it.
        ///           </li>
        ///           <li>
        ///               Two ranges of non-displayable, control characters: <tt>U+0000</tt> to <tt>U+001F</tt>
        ///               and <tt>U+007F</tt> to <tt>U+009F</tt>.
        ///           </li>
        ///         </ul>
        ///   </li>
        ///   <li>All non ASCII characters.</li>
        /// </ul>
        /// <para>
        ///   This escape will be performed by using Backslash escapes whenever possible. For escaped
        ///   characters that do not have an associated Backslash, default to <tt>&#92;FF </tt>
        ///   Hexadecimal Escapes.
        /// </para>
        /// <para>
        ///   This method calls
        ///   <seealso cref="escapeCssIdentifier(char[], int, int, java.io.TextWriter, CssIdentifierEscapeType, CssIdentifierEscapeLevel)"/>
        ///   with the following preconfigured values:
        /// </para>
        /// <ul>
        ///   <li><tt>type</tt>:
        ///       <seealso cref="CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA"/></li>
        ///   <li><tt>level</tt>:
        ///       <seealso cref="CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET"/></li>
        /// </ul>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be escaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the escape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifier(final char[] text, final int offset, final int len, final java.io.TextWriter writer) throws java.io.IOException
        public static void escapeCssIdentifier(char[] text, int offset, int len, TextWriter writer)
        {
            escapeCssIdentifier(text, offset, len, writer, CssIdentifierEscapeType.BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA, CssIdentifierEscapeLevel.LEVEL_2_ALL_NON_ASCII_PLUS_BASIC_ESCAPE_SET);
        }


        /// <summary>
        /// <para>
        ///   Perform a (configurable) CSS Identifier <strong>escape</strong> operation on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   This method will perform an escape operation according to the specified
        ///   <seealso cref="CssIdentifierEscapeType"/> and
        ///   <seealso cref="CssIdentifierEscapeLevel"/> argument values.
        /// </para>
        /// <para>
        ///   All other <tt>char[]</tt>-based <tt>escapeCssIdentifier*(...)</tt> methods call this one with preconfigured
        ///   <tt>type</tt> and <tt>level</tt> values.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be escaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the escape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be escaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the escaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <param name="type"> the type of escape operation to be performed, see
        ///             <seealso cref="CssIdentifierEscapeType"/>. </param>
        /// <param name="level"> the escape level to be applied, see <seealso cref="CssIdentifierEscapeLevel"/>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void escapeCssIdentifier(final char[] text, final int offset, final int len, final java.io.TextWriter writer, final CssIdentifierEscapeType type, final CssIdentifierEscapeLevel level) throws java.io.IOException
        public static void escapeCssIdentifier(char[] text, int offset, int len, TextWriter writer, CssIdentifierEscapeType type, CssIdentifierEscapeLevel level)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            if (type == null)
            {
                throw new System.ArgumentException("The 'type' argument cannot be null");
            }

            if (level == null)
            {
                throw new System.ArgumentException("The 'level' argument cannot be null");
            }

            //ORIGINAL LINE: final int textLen = (text == null? 0 : text.length);
            int textLen = (text == null ? 0 : text.Length);

            if (offset < 0 || offset > textLen)
            {
                throw new System.ArgumentException("Invalid (offset, len). offset=" + offset + ", len=" + len + ", text.length=" + textLen);
            }

            if (len < 0 || (offset + len) > textLen)
            {
                throw new System.ArgumentException("Invalid (offset, len). offset=" + offset + ", len=" + len + ", text.length=" + textLen);
            }

            CssIdentifierEscapeUtil.escape(text, offset, len, writer, type, level);

        }








        /// <summary>
        /// <para>
        ///   Perform a CSS <strong>unescape</strong> operation on a <tt>String</tt> input.
        /// </para>
        /// <para>
        ///   No additional configuration arguments are required. Unescape operations
        ///   will always perform <em>complete</em> CSS unescape of backslash and hexadecimal escape
        ///   sequences.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be unescaped. </param>
        /// <returns> The unescaped result <tt>String</tt>. As a memory-performance improvement, will return the exact
        ///         same object as the <tt>text</tt> input argument if no unescaping modifications were required (and
        ///         no additional <tt>String</tt> objects will be created during processing). Will
        ///         return <tt>null</tt> if input is <tt>null</tt>. </returns>
        //ORIGINAL LINE: public static String unescapeCss(final String text)
        public static string unescapeCss(string text)
        {
            if (string.ReferenceEquals(text, null))
            {
                return null;
            }
            if (text.IndexOf('\\') < 0)
            {
                // Fail fast, avoid more complex (and less JIT-table) method to execute if not needed
                return text;
            }
            return CssUnescapeUtil.unescape(text);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS <strong>unescape</strong> operation on a <tt>String</tt> input, writing results
        ///   to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   No additional configuration arguments are required. Unescape operations
        ///   will always perform <em>complete</em> CSS unescape of backslash and hexadecimal escape
        ///   sequences.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>String</tt> to be unescaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the unescaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void unescapeCss(final String text, final java.io.TextWriter writer) throws java.io.IOException
        public static void unescapeCss(string text, TextWriter writer)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }
            if (string.ReferenceEquals(text, null))
            {
                return;
            }
            if (text.IndexOf('\\') < 0)
            {
                // Fail fast, avoid more complex (and less JIT-table) method to execute if not needed
                writer.Write(text);
                return;
            }

            CssUnescapeUtil.unescape(new InternalStringReader(text), writer);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS <strong>unescape</strong> operation on a <tt>String</tt> input, writing results
        ///   to a <tt>TextWriter</tt>.
        /// </para>
        /// <para>
        ///   No additional configuration arguments are required. Unescape operations
        ///   will always perform <em>complete</em> CSS unescape of backslash and hexadecimal escape
        ///   sequences.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="reader"> the <tt>TextReader</tt> reading the text to be unescaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the unescaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void unescapeCss(final java.io.TextReader reader, final java.io.TextWriter writer) throws java.io.IOException
        public static void unescapeCss(TextReader reader, TextWriter writer)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            CssUnescapeUtil.unescape(reader, writer);
        }


        /// <summary>
        /// <para>
        ///   Perform a CSS <strong>unescape</strong> operation on a <tt>char[]</tt> input.
        /// </para>
        /// <para>
        ///   No additional configuration arguments are required. Unescape operations
        ///   will always perform <em>complete</em> CSS unescape of backslash and hexadecimal escape
        ///   sequences.
        /// </para>
        /// <para>
        ///   This method is <strong>thread-safe</strong>.
        /// </para>
        /// </summary>
        /// <param name="text"> the <tt>char[]</tt> to be unescaped. </param>
        /// <param name="offset"> the position in <tt>text</tt> at which the unescape operation should start. </param>
        /// <param name="len"> the number of characters in <tt>text</tt> that should be unescaped. </param>
        /// <param name="writer"> the <tt>java.io.TextWriter</tt> to which the unescaped result will be written. Nothing will
        ///               be written at all to this writer if input is <tt>null</tt>. </param>
        /// <exception cref="IOException"> if an input/output exception occurs </exception>
        //ORIGINAL LINE: public static void unescapeCss(final char[] text, final int offset, final int len, final java.io.TextWriter writer) throws java.io.IOException
        public static void unescapeCss(char[] text, int offset, int len, TextWriter writer)
        {

            if (writer == null)
            {
                throw new System.ArgumentException("Argument 'writer' cannot be null");
            }

            //ORIGINAL LINE: final int textLen = (text == null? 0 : text.length);
            int textLen = (text == null ? 0 : text.Length);

            if (offset < 0 || offset > textLen)
            {
                throw new System.ArgumentException("Invalid (offset, len). offset=" + offset + ", len=" + len + ", text.length=" + textLen);
            }

            if (len < 0 || (offset + len) > textLen)
            {
                throw new System.ArgumentException("Invalid (offset, len). offset=" + offset + ", len=" + len + ", text.length=" + textLen);
            }

            CssUnescapeUtil.unescape(text, offset, len, writer);

        }





        private CssEscape() : base()
        {
        }



        /*
		 * This is basically a very simplified, thread-unsafe version of StringReader that should
		 * perform better than the original StringReader by removing all synchronization structures.
		 *
		 * Note the only implemented methods are those that we know are really used from within the
		 * stream-based escape/unescape operations.
		 */
        private sealed class InternalStringReader : TextReader
        {

            internal string str;
            internal int length;
            internal int next = 0;

            //ORIGINAL LINE: public InternalStringReader(final String s)
            public InternalStringReader(string s) : base()
            {
                this.str = s;
                this.length = s.Length;
            }

            //ORIGINAL LINE: @Override public int read() throws java.io.IOException
            public override int Read()
            {
                if (this.next >= length)
                {
                    return -1;
                }
                return this.str[this.next++];
            }

            //ORIGINAL LINE: @Override public int read(final char[] cbuf, final int off, final int len) throws java.io.IOException
            public override int Read(char[] cbuf, int off, int len)
            {
                if ((off < 0) || (off > cbuf.Length) || (len < 0) || ((off + len) > cbuf.Length) || ((off + len) < 0))
                {
                    throw new System.IndexOutOfRangeException();
                }
                else if (len == 0)
                {
                    return 0;
                }
                if (this.next >= this.length)
                {
                    return -1;
                }
                int n = Math.Min(this.length - this.next, len);
                this.str.CopyTo(this.next, cbuf, off, this.next + n - this.next);
                this.next += n;
                return n;
            }

            //ORIGINAL LINE: @Override public void close() throws java.io.IOException
            public override void Close()
            {
                this.str = null; // Just set the reference to null, help the GC
            }

        }


    }


}