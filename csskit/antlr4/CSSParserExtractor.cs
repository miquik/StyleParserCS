using System.Collections.Generic;

namespace StyleParserCS.csskit.antlr4
{
    using MediaQuery = StyleParserCS.css.MediaQuery;
    using RuleList = StyleParserCS.css.RuleList;

    /// <summary>
    /// CSS Parser extractor interface
    /// should be implemented by ANTLR listener or visitor extractor
    /// </summary>
    public interface CSSParserExtractor
    {
        IList<string> ImportPaths { get; }

        IList<IList<MediaQuery>> ImportMedia { get; }

        RuleList Rules { get; }

        IList<MediaQuery> Media { get; }
    }

}