using AngleSharp.Dom;
using System;

namespace StyleParserCS.css
{
    /// <summary>
    /// Creates rules, declarations and selectors,
    /// that is the most of CSS grammar elements
    /// @author kapy
    /// 
    /// </summary>
    public interface RuleFactory
    {

        /// <summary>
        /// Creates CSS declaration </summary>
        /// <returns> New CSS declaration </returns>
        Declaration createDeclaration();

        /// <summary>
        /// Creates CSS declaration by shallow cloning </summary>
        /// <param name="clone"> Source </param>
        /// <returns> New CSS declaration </returns>
        Declaration createDeclaration(Declaration clone);

        /// <summary>
        /// Creates CSS import rule </summary>
        /// <returns> New CSS import rule </returns>
        [Obsolete]
        RuleImport createImport();

        /// <summary>
        /// Creates CSS rule set, that is collection of CSS declarations
        /// with collection of CSS combined selectors. 
        /// 
        /// In current implementation of parser they are used to pass 
        /// integer value by parser to preserve rule ordering according
        /// to their occurrence in CSS style sheet.
        /// </summary>
        /// <returns> New CSS rule set </returns>
        RuleSet createSet();

        /// <summary>
        /// Creates CSS media page </summary>
        /// <returns> New CSS media page </returns>
        RuleMedia createMedia();

        /// <summary>
        /// Creates a CSS media query </summary>
        /// <returns> New CSS media query </returns>
        MediaQuery createMediaQuery();

        /// <summary>
        /// Creates a new CSS media query expression. </summary>
        /// <returns> The new expression </returns>
        MediaExpression createMediaExpression();

        /// <summary>
        /// Creates a new keyframes rule </summary>
        /// <returns> The new keyframes rule </returns>
        RuleKeyframes createKeyframes();

        /// <summary>
        /// Creates a new keyframe block </summary>
        /// <returns> The new keyframe block </returns>
        KeyframeBlock createKeyframeBlock();

        /// <summary>
        /// Creates CSS named page </summary>
        /// <returns> New CSS page </returns>
        RulePage createPage();

        /// <summary>
        /// Creates CSS margin rule </summary>
        /// <param name="area"> Margin area </param>
        /// <returns> New CSS margin rule </returns>
        RuleMargin createMargin(string area);

        /// <summary>
        /// Creates CSS viewport rule. </summary>
        /// <returns> New CSS viewport rule </returns>
        RuleViewport createViewport();

        /// <summary>
        /// Creates CSS named font </summary>
        /// <returns> New CSS font </returns>
        RuleFontFace createFontFace();

        /// <summary>
        /// Creates CSS combined selector, collection of (simple) selectors </summary>
        /// <returns> New CSS combined selector </returns>
        CombinedSelector createCombinedSelector();

        /// <summary>
        /// Creates CSS selector </summary>
        /// <returns> New CSS selector </returns>
        Selector createSelector();

        /// <summary>
        /// Creates CSS selector part, element DOM node </summary>
        /// <param name="e"> Element node </param>
        /// <param name="inlinePriority"> true means that the selector has an inline priority </param>
        /// <returns> New CSS element DOM selector part </returns>
        Selector_ElementDOM createElementDOM(IElement e, bool inlinePriority);

        /// <summary>
        /// Creates CSS selector part, element name </summary>
        /// <param name="elementName"> Name of element </param>
        /// <returns> New CSS element name selector part </returns>
        Selector_ElementName createElement(string elementName);

        /// <summary>
        /// Creates CSS selector part, element attribute </summary>
        /// <param name="value"> Value of attribute </param>
        /// <param name="isStringValue"> Value given is string or identifier </param>
        /// <param name="operator"> Operator between value and attribute </param>
        /// <param name="attribute"> Name of attribute </param>
        /// <returns> New CSS element attribute selector part </returns>
        Selector_ElementAttribute createAttribute(string value, bool isStringValue, Selector_Operator operatorv, string attribute);

        /// <summary>
        /// Creates CSS selector part, element class </summary>
        /// <param name="className"> Name of class </param>
        /// <returns> New CSS element class selector part </returns>
        Selector_ElementClass createClass(string className);

        /// <summary>
        /// Creates CSS selector part, element id </summary>
        /// <param name="id"> ID of element </param>
        /// <returns> New CSS element ID selector part </returns>
        Selector_ElementID createID(string id);

        /// <summary>
        /// Creates CSS selector part, page at-rule pseudo-class </summary>
        /// <param name="name"> Name of pseudo-class </param>
        /// <returns> New CSS page pseudo-class </returns>
        Selector_PseudoPage createPseudoPage(string name);

        /// <summary>
        /// Creates CSS selector part, pseudo-element </summary>
        /// <param name="name"> Name of pseudo-element </param>
        /// <returns> New CSS pseudo-element </returns>
        Selector_PseudoElement createPseudoElement(string name);

        /// <summary>
        /// Creates CSS selector part, pseudo-element </summary>
        /// <param name="name"> Name of pseudo-element </param>
        /// <param name="functionValue"> Value of its function argument </param>
        /// <returns> New CSS pseudo-element </returns>
        Selector_PseudoElement createPseudoElement(string name, string functionValue);

        /// <summary>
        /// Creates CSS selector part, pseudo-element </summary>
        /// <param name="name"> Name of pseudo-element </param>
        /// <param name="nestedSelector"> Selector in its function argument </param>
        /// <returns> New CSS pseudo-element </returns>
        Selector_PseudoElement createPseudoElement(string name, Selector nestedSelector);

        /// <summary>
        /// Creates CSS selector part, pseudo-class </summary>
        /// <param name="name"> Name of pseudo-class </param>
        /// <returns> New CSS pseudo-class </returns>
        Selector_PseudoClass createPseudoClass(string name);

        /// <summary>
        /// Creates CSS selector part, pseudo-class </summary>
        /// <param name="name"> Name of pseudo-class </param>
        /// <param name="functionValue"> Value of its function argument </param>
        /// <returns> New CSS pseudo-class </returns>
        Selector_PseudoClass createPseudoClass(string name, string functionValue);

        /// <summary>
        /// Creates CSS selector part, pseudo-class </summary>
        /// <param name="name"> Name of pseudo-class </param>
        /// <param name="nestedSelector"> Selector in its function argument </param>
        /// <returns> New CSS pseudo-class </returns>
        Selector_PseudoClass createPseudoClass(string name, Selector nestedSelector);

        /// <summary>
        /// Creates CSS author style sheet </summary>
        /// <returns> The new style sheet. </returns>
        StyleSheet createStyleSheet();

        /// <summary>
        /// Creates CSS author style sheet with the given origin. </summary>
        /// <param name="origin"> the origin of the style sheet. </param>
        /// <returns> The new style sheet. </returns>
        StyleSheet createStyleSheet(StyleSheet_Origin origin);

    }

}