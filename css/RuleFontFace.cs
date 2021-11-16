using System.Collections.Generic;

/// <summary>
/// RuleFontFace.java
/// 
/// Created on 1.2.2013, 14:27:04 by burgetr
/// </summary>
namespace StyleParserCS.css
{


    /// <summary>
    /// Contains collection of CSS declarations specified for a given font.
    /// 
    /// @author burgetr
    /// </summary>
    public interface RuleFontFace : RuleBlock<Declaration>, PrettyOutput
    {
        /// <summary>
        /// Gets the font family name </summary>
        /// <returns> Font family name </returns>
        string FontFamily { get; }

        /// <summary>
        /// Gets the declared font sources </summary>
        /// <returns> The list of sources or {@code null} when no valid source declaration is present. </returns>
        IList<RuleFontFace_Source> Sources { get; }

        /// <summary>
        /// Gets the font style </summary>
        /// <returns> Font style </returns>
        CSSProperty_FontStyle FontStyle { get; }

        /// <summary>
        /// Gets the font weight </summary>
        /// <returns> Font weight </returns>
        CSSProperty_FontWeight FontWeight { get; }

        /// <summary>
        /// Gets the unicode ranges </summary>
        /// <returns> List of unicode ranges </returns>
        IList<string> UnicodeRanges { get; }

        //=================================================================================

        /// <summary>
        /// A generic font source.
        /// @author burgetr
        /// </summary>

        /// <summary>
        /// A local font (src: local()). 
        /// @author burgetr
        /// </summary>

        /// <summary>
        /// A remote source indentified by its Uri (src: Uri()). 
        /// @author burgetr
        /// </summary>

    }

    public interface RuleFontFace_Source
    {
    }

    public interface RuleFontFace_SourceLocal : RuleFontFace_Source
    {
        /// <summary>
        /// The local font name </summary>
        string Name { get; }
    }

    public interface RuleFontFace_SourceUri : RuleFontFace_Source
    {
        /// <summary>
        /// The font URI specification </summary>
        TermURI URI { get; }
        /// <summary>
        /// Format specification or {@code null} if not present </summary>
        string Format { get; }
    }

}