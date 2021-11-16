using AngleSharp.Dom;
using System;

/// 
namespace StyleParserCS.csskit
{
    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using Declaration = StyleParserCS.css.Declaration;
    using KeyframeBlock = StyleParserCS.css.KeyframeBlock;
    using MediaExpression = StyleParserCS.css.MediaExpression;
    using MediaQuery = StyleParserCS.css.MediaQuery;
    using RuleFactory = StyleParserCS.css.RuleFactory;
    using RuleFontFace = StyleParserCS.css.RuleFontFace;
    using RuleImport = StyleParserCS.css.RuleImport;
    using RuleKeyframes = StyleParserCS.css.RuleKeyframes;
    using RuleMargin = StyleParserCS.css.RuleMargin;
    using RuleMedia = StyleParserCS.css.RuleMedia;
    using RulePage = StyleParserCS.css.RulePage;
    using RuleSet = StyleParserCS.css.RuleSet;
    using RuleViewport = StyleParserCS.css.RuleViewport;
    using Selector = StyleParserCS.css.Selector;
    using StyleSheet = StyleParserCS.css.StyleSheet;
    using Selector_ElementAttribute = StyleParserCS.css.Selector_ElementAttribute;
    using Selector_ElementClass = StyleParserCS.css.Selector_ElementClass;
    using Selector_ElementDOM = StyleParserCS.css.Selector_ElementDOM;
    using Selector_ElementID = StyleParserCS.css.Selector_ElementID;
    using Selector_ElementName = StyleParserCS.css.Selector_ElementName;
    using Selector_Operator = StyleParserCS.css.Selector_Operator;

    /// <summary>
    /// @author kapy
    /// 
    /// </summary>
    //ORIGINAL LINE: @SuppressWarnings("deprecation") public class RuleFactoryImpl implements StyleParserCS.css.RuleFactory
    public class RuleFactoryImpl : RuleFactory
    {

        private static RuleFactory instance;

        static RuleFactoryImpl()
        {
            instance = new RuleFactoryImpl();
        }

        public RuleFactoryImpl()
        {
        }

        public static RuleFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createDeclaration()
		 */
        public virtual Declaration createDeclaration()
        {
            return new DeclarationImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createDeclaration(StyleParserCS.css.Declaration)
		 */
        public virtual Declaration createDeclaration(Declaration clone)
        {
            return new DeclarationImpl(clone);
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createImport()
		 */
        [Obsolete]
        public virtual RuleImport createImport()
        {
            return new RuleImportImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createMedia()
		 */
        public virtual RuleMedia createMedia()
        {
            return new RuleMediaImpl();
        }

        public virtual MediaQuery createMediaQuery()
        {
            return new MediaQueryImpl();
        }

        public virtual MediaExpression createMediaExpression()
        {
            return new MediaExpressionImpl();
        }

        public virtual RuleKeyframes createKeyframes()
        {
            return new RuleKeyframesImpl();
        }

        public virtual KeyframeBlock createKeyframeBlock()
        {
            return new KeyframeBlockImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createPage()
		 */
        public virtual RulePage createPage()
        {
            return new RulePageImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createMargin()
		 */
        public virtual RuleMargin createMargin(string area)
        {
            return new RuleMarginImpl(area);
        }

        public virtual RuleViewport createViewport()
        {
            return new RuleViewportImpl();
        }

        public virtual RuleFontFace createFontFace()
        {
            return new RuleFontFaceImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createCombinedSelector()
		 */
        public virtual CombinedSelector createCombinedSelector()
        {
            return new CombinedSelectorImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createSet()
		 */
        public virtual RuleSet createSet()
        {
            return new RuleSetImpl();
        }

        /* (non-Javadoc)
		 * @see StyleParserCS.css.RuleFactory#createSelector()
		 */
        public virtual Selector createSelector()
        {
            return new SelectorImpl();
        }

        public virtual Selector_ElementAttribute createAttribute(string value, bool isStringValue, Selector_Operator operatorv, string attribute)
        {
            return new SelectorImpl.ElementAttributeImpl(value, isStringValue, operatorv, attribute);
        }

        public virtual Selector_ElementClass createClass(string className)
        {
            return new SelectorImpl.ElementClassImpl(className);
        }

        public virtual Selector_ElementName createElement(string elementName)
        {
            return new SelectorImpl.ElementNameImpl(elementName);
        }

        public virtual Selector_ElementDOM createElementDOM(IElement e, bool inlinePriority)
        {
            return new SelectorImpl.ElementDOMImpl(e, inlinePriority);
        }


        public virtual Selector_ElementID createID(string id)
        {
            return new SelectorImpl.ElementIDImpl(id);
        }

        public virtual StyleParserCS.css.Selector_PseudoPage createPseudoPage(string name)
        {
            return new SelectorImpl.PseudoPageImpl(name);
        }

        public virtual StyleParserCS.css.Selector_PseudoElement createPseudoElement(string name)
        {
            return new SelectorImpl.PseudoElementImpl(name);
        }

        public virtual StyleParserCS.css.Selector_PseudoElement createPseudoElement(string name, string functionValue)
        {
            return new SelectorImpl.PseudoElementImpl(name, functionValue);
        }

        public virtual StyleParserCS.css.Selector_PseudoElement createPseudoElement(string name, Selector nestedSelector)
        {
            return new SelectorImpl.PseudoElementImpl(name, nestedSelector);
        }

        public virtual StyleParserCS.css.Selector_PseudoClass createPseudoClass(string name)
        {
            return new SelectorImpl.PseudoClassImpl(name);
        }

        public virtual StyleParserCS.css.Selector_PseudoClass createPseudoClass(string name, string functionValue)
        {
            return new SelectorImpl.PseudoClassImpl(name, functionValue);
        }

        public virtual StyleParserCS.css.Selector_PseudoClass createPseudoClass(string name, Selector nestedSelector)
        {
            return new SelectorImpl.PseudoClassImpl(name, nestedSelector);
        }

        public virtual StyleSheet createStyleSheet()
        {
            return new StyleSheetImpl();
        }

        public virtual StyleSheet createStyleSheet(StyleParserCS.css.StyleSheet_Origin origin)
        {
            StyleSheet ret = new StyleSheetImpl();
            ret.Origin = origin;
            return ret;
        }
    }

}