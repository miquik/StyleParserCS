using Ardalis.SmartEnum;
using System.Collections.Generic;

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
    ///   Types of escape operations to be performed on CSS strings:
    /// </para>
    /// 
    /// <ul>
    ///     <li><tt><strong>BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA</strong></tt>: Use
    ///         backslash escapes whenever possible (depending on the specified
    ///         <seealso cref="CssStringEscapeLevel"/>). For escaped characters that do
    ///         not have an associated backslash escape, default to using
    ///         <tt>&#92;FF*</tt> variable-length hexadecimal escapes.</li>
    ///     <li><tt><strong>BACKSLASH_ESCAPES_DEFAULT_TO_SIX_DIGIT_HEXA</strong></tt>: Use
    ///         backslash escapes whenever possible (depending on the specified
    ///         <seealso cref="CssStringEscapeLevel"/>). For escaped characters that do
    ///         not have an associated backslash escape, default to using
    ///         <tt>&#92;FFFFFF</tt> 6-digit hexadecimal escapes.</li>
    ///     <li><tt><strong>COMPACT_HEXA</strong></tt>: Replace escaped characters with
    ///         <tt>&#92;FF*</tt> variable-length hexadecimal escapes.</li>
    ///     <li><tt><strong>SIX_DIGIT_HEXA</strong></tt>: Replace escaped characters with
    ///         <tt>&#92;FFFFFF</tt> 6-digit hexadecimal escapes.</li>
    /// </ul>
    /// 
    /// <para>
    ///   For further information, see the <em>Glossary</em> and the <em>References</em> sections at the
    ///   documentation for the <seealso cref="CssEscape"/> class.
    /// </para>
    /// 
    /// @author Daniel Fern&aacute;ndez
    /// 
    /// @since 1.0.0
    /// 
    /// </summary>
    public sealed class CssStringEscapeType : SmartEnum<CssStringEscapeType>
    {

        /// <summary>
        /// Use backslash escapes if possible, default to &#92;FF* variable-length hexadecimal escapes.
        /// </summary>
        public static readonly CssStringEscapeType BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA = new CssStringEscapeType("BACKSLASH_ESCAPES_DEFAULT_TO_COMPACT_HEXA", 1, true, true);

        /// <summary>
        /// Use backslash escapes if possible, default to &#92;FFFFFF 6-digit hexadecimal escapes.
        /// </summary>
        public static readonly CssStringEscapeType BACKSLASH_ESCAPES_DEFAULT_TO_SIX_DIGIT_HEXA = new CssStringEscapeType("BACKSLASH_ESCAPES_DEFAULT_TO_SIX_DIGIT_HEXA", 2, true, false);

        /// <summary>
        /// Always use &#92;FF* variable-length hexadecimal escapes.
        /// </summary>
        public static readonly CssStringEscapeType COMPACT_HEXA = new CssStringEscapeType("COMPACT_HEXA", 3, false, true);

        /// <summary>
        /// Always use &#92;FFFFFF 6-digit hexadecimal escapes.
        /// </summary>
        public static readonly CssStringEscapeType SIX_DIGIT_HEXA = new CssStringEscapeType("SIX_DIGIT_HEXA", 4, false, false);

        private readonly bool useBackslashEscapes;
        private readonly bool useCompactHexa;

        //ORIGINAL LINE: CssStringEscapeType(final boolean useBackslashEscapes, final boolean useCompactHexa)
        internal CssStringEscapeType(string name, int value, bool useBackslashEscapes, bool useCompactHexa) : base(name, value)
        {
            this.useBackslashEscapes = useBackslashEscapes;
            this.useCompactHexa = useCompactHexa;
        }

        public bool UseBackslashEscapes
        {
            get
            {
                return useBackslashEscapes;
            }
        }

        public bool UseCompactHexa
        {
            get
            {
                return useCompactHexa;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

}