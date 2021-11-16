using AngleSharp.Dom;
using System.Collections.Generic;

/// <summary>
/// SimpleAnalyzer.java
/// 
/// Created on 6.6.2012, 13:57:00 by burgetr
/// </summary>
namespace StyleParserCS.domassign
{
    using MediaSpec = StyleParserCS.css.MediaSpec;
    using NodeData = StyleParserCS.css.NodeData;
    using Selector_PseudoElementType = StyleParserCS.css.Selector_PseudoElementType;
    using StyleSheet = StyleParserCS.css.StyleSheet;

    /// <summary>
    /// A simple ananalyzer that computes a style for the individual DOM nodes with no mapping and caching.
    /// This analyzer is suitable for obtaining the style of individual elements without computing the style
    /// for the whole DOM tree. However, in larger scale, the performance of the individual computation
    /// is significantly worse.  
    /// 
    /// @author burgetr
    /// </summary>
    public class DirectAnalyzer : Analyzer
    {
        /// <summary>
        /// Creates the analyzer for a single style sheet. </summary>
        /// <param name="sheet"> The stylesheet that will be used as the source of rules. </param>
        public DirectAnalyzer(StyleSheet sheet) : base(sheet)
        {
        }

        /// <summary>
        /// Creates the analyzer for multiple style sheets. </summary>
        /// <param name="sheets"> A list of stylesheets that will be used as the source of rules. </param>
        public DirectAnalyzer(IList<StyleSheet> sheets) : base(sheets)
        {
        }

        /// <summary>
        /// Computes the style of an element with an eventual pseudo element for the given media. </summary>
        /// <param name="el"> The DOM element. </param>
        /// <param name="pseudo"> A pseudo element that should be used for style computation or <code>null</code> if no pseudo element should be used (e.g. :after). </param>
        /// <param name="media"> Used media specification. </param>
        /// <returns> The relevant declarations from the registered style sheets. </returns>
        public virtual NodeData getElementStyle(IElement el, Selector_PseudoElementType pseudo, MediaSpec media)
        {
            //ORIGINAL LINE: final OrderedRule[] applicableRules = AnalyzerUtil.getApplicableRules(sheets, el, media);
            OrderedRule[] applicableRules = AnalyzerUtil.getApplicableRules(sheets, el, media);
            return AnalyzerUtil.getElementStyle(el, pseudo, ElementMatcher, MatchCondition, applicableRules);
        }

        /// <summary>
        /// Computes the style of an element with an eventual pseudo element for the given media. </summary>
        /// <param name="el"> The DOM element. </param>
        /// <param name="pseudo"> A pseudo element that should be used for style computation or <code>null</code> if no pseudo element should be used (e.g. :after). </param>
        /// <param name="media"> Used media name (e.g. "screen" or "all") </param>
        /// <returns> The relevant declarations from the registered style sheets. </returns>
        public virtual NodeData getElementStyle(IElement el, Selector_PseudoElementType pseudo, string media)
        {
            return getElementStyle(el, pseudo, new MediaSpec(media));
        }

        //==========================================================================================

    }

}