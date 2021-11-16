using StyleParserCS.css;
using System.Collections.Generic;

namespace StyleParserCS.csskit.antlr4
{
    /// <summary>
    /// Prepares StyleSheet blocks in parser. Allows switching strategy in runtime,
    /// for example modify behavior for embedded, file and inline rules
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public interface Preparator
    {

        /// <summary>
        /// Creates base block of rules. Most usually, it will be RuleSet, but when
        /// parser is inside of imported file with medium and wrap condition is {@code true}, then
        /// RuleSet is wrapped by media into RuleMedia </summary>
        /// <param name="cslist"> List of CombinedSelector for this rule block </param>
        /// <param name="dlist"> List of Declaration for this rule block </param>
        /// <param name="wrap"> Wrap condition, it {@code true}, rule set is wrapped into rule media </param>
        /// <param name="media"> List of medias used to wrap </param>
        /// <returns> Either RuleSet containing selectors and declarations, or RuleMedia, wrapped
        /// version of RuleSet </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleSet(java.util.List<StyleParserCS.css.CombinedSelector> cslist, java.util.List<StyleParserCS.css.Declaration> dlist, boolean wrap, java.util.List<StyleParserCS.css.MediaQuery> media);
        RuleBlock prepareRuleSet(IList<CombinedSelector> cslist, IList<Declaration> dlist, bool wrap, IList<MediaQuery> media);

        /// <summary>
        /// Creates block of rules as result of parsing in-line declaration. </summary>
        /// <param name="dlist"> List of Declaration </param>
        /// <param name="pseudos"> List of pseudo page identifiers </param>
        /// <returns> RuleSet </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareInlineRuleSet(java.util.List<StyleParserCS.css.Declaration> dlist, java.util.List<StyleParserCS.css.Selector_SelectorPart> pseudos);
        RuleBlock prepareInlineRuleSet(IList<Declaration> dlist, IList<StyleParserCS.css.Selector_SelectorPart> pseudos);

        /// <summary>
        /// Creates RuleMedia, block of rules with assigned medias. Uses mark to change priority of rules,
        /// that is priority of this rule is set to mark </summary>
        /// <param name="rules"> Rules encapsulated by this RuleMedia </param>
        /// <param name="media"> List of media assigned to rule </param>
        /// <returns> RuleMedia  </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleMedia(java.util.List<StyleParserCS.css.RuleSet> rules, java.util.List<StyleParserCS.css.MediaQuery> media);
        RuleBlock prepareRuleMedia(IList<RuleSet> rules, IList<MediaQuery> media);

        /// <summary>
        /// Creates RulePage, block of rules associated with specific page </summary>
        /// <param name="declarations"> List of declarations </param>
        /// <param name="marginRules"> List of margin rules </param>
        /// <param name="name"> Name of the page </param>
        /// <param name="pseudo"> Pseudo-class of the page </param>
        /// <returns> RulePage </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRulePage(java.util.List<StyleParserCS.css.Declaration> declarations, java.util.List<StyleParserCS.css.RuleMargin> marginRules, String name, StyleParserCS.css.Selector_PseudoPage pseudo);
        RuleBlock prepareRulePage(IList<Declaration> declarations, IList<RuleMargin> marginRules, string name, StyleParserCS.css.Selector_PseudoPage pseudo);

        /// <summary>
        /// Creates RuleMargin, block of declarations associated with specific area in the page margin. </summary>
        /// <param name="area"> The margin area </param>
        /// <param name="declarations"> List of declarations </param>
        /// <returns> RuleMargin </returns>
        RuleMargin prepareRuleMargin(string area, IList<Declaration> declarations);

        /// <summary>
        /// Creates RuleViewport, block of rules associated with the viewport. </summary>
        /// <param name="decl"> List of declarations </param>
        /// <returns> RuleViewport </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleViewport(java.util.List<StyleParserCS.css.Declaration> decl);
        RuleBlock prepareRuleViewport(IList<Declaration> decl);

        /// <summary>
        /// Creates RuleFontFace, block of rules associated with specific font </summary>
        /// <param name="decl"> List of declarations </param>
        /// <returns> RuleFontFace </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleFontFace(java.util.List<StyleParserCS.css.Declaration> decl);
        RuleBlock prepareRuleFontFace(IList<Declaration> decl);

        /// <summary>
        /// Creates RuleKeyframes, block of key frames with assigned name. </summary>
        /// <param name="rules"> Rules encapsulated by this RuleKeyframes </param>
        /// <param name="name"> Keyframes name </param>
        /// <returns> RuleKeyframes </returns>
        //ORIGINAL LINE: public StyleParserCS.css.RuleBlock<?> prepareRuleKeyframes(java.util.List<StyleParserCS.css.KeyframeBlock> rules, String name);
        RuleBlock prepareRuleKeyframes(IList<KeyframeBlock> rules, string name);

    }

}