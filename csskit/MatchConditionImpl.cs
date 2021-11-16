/// <summary>
/// MatchConditionImpl.java
/// 
/// Created on 1.7.2013, 11:06:40 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using AngleSharp.Dom;
    using StyleParserCS.css;
    using Selector_PseudoClass = StyleParserCS.css.Selector_PseudoClass;
    using Selector_PseudoClassType = StyleParserCS.css.Selector_PseudoClassType;
    using Selector_SelectorPart = StyleParserCS.css.Selector_SelectorPart;

    /// <summary>
    /// A default match condition that matches the LINK (or other) pseudo classes to link elements.
    /// 
    /// @author burgetr
    /// </summary>
    public class MatchConditionImpl : MatchCondition
    {
        internal Selector_PseudoClassType pseudo;

        /// <summary>
        /// Creates the default condition that matches the LINK pseudo class to links.
        /// </summary>
        public MatchConditionImpl()
        {
            pseudo = Selector_PseudoClassType.LINK;
        }

        /// <summary>
        /// Creates the default condition that matches the given pseudo class to links. </summary>
        /// <param name="pseudoClass"> the pseudoClass to be matched to links </param>
        public MatchConditionImpl(Selector_PseudoClassType pseudoClass)
        {
            this.pseudo = pseudoClass;
        }

        /// <summary>
        /// Sets the pseudo class that is matched to links. </summary>
        /// <param name="pseudoClass"> the pseudoClass to be matched to links </param>
        public virtual Selector_PseudoClassType PseudoClass
        {
            set
            {
                this.pseudo = value;
            }
        }

        public virtual bool isSatisfied(IElement e, Selector_SelectorPart selpart)
        {
            if (selpart is Selector_PseudoClass)
            {
                Selector_PseudoClassType type = ((Selector_PseudoClass)selpart).Type;
                return type == pseudo && e.TagName.ToLower() == "a";
            }
            else
            {
                return false;
            }
        }

        public object Clone()
        {
            return new MatchConditionImpl(pseudo);
        }
    }

}