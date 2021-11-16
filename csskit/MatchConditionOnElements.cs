using AngleSharp.Dom;
using StyleParserCS.css;
using System.Collections.Generic;

/// <summary>
/// MatchConditionOnElements.java
/// 
/// Created on 1.7.2013, 11:26:35 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using Selector_PseudoClass = StyleParserCS.css.Selector_PseudoClass;
    using Selector_PseudoClassType = StyleParserCS.css.Selector_PseudoClassType;
    using Selector_SelectorPart = StyleParserCS.css.Selector_SelectorPart;

    /// <summary>
    /// A match condition for matching the pseudo classes to particular elements. It allows to assign
    /// pseudoclasses to the individual elements in the DOM tree and to the element names. Multiple pseudo classes
    /// may be assigned to a single element or element name. When testing the condition, the exact element is
    /// tested first. If no pseudo class is defined for that element, the element name is tested.
    /// 
    /// @author burgetr
    /// </summary>
    public class MatchConditionOnElements : MatchCondition
    {
        private IDictionary<IElement, ISet<Selector_PseudoClassType>> elements;
        private IDictionary<string, ISet<Selector_PseudoClassType>> names;

        /// <summary>
        /// Creates the condition with an empty set of assigned elements and element names.
        /// </summary>
        public MatchConditionOnElements()
        {
            elements = null;
            names = null;
        }

        /// <summary>
        /// Creates the condition and assigns a pseudo class to a given element. </summary>
        /// <param name="e"> the element </param>
        /// <param name="pseudoClass"> the pseudo class to be assigned </param>
        public MatchConditionOnElements(IElement e, Selector_PseudoClassType pseudoClass)
        {
            addMatch(e, pseudoClass);
        }

        /// <summary>
        /// Creates the condition and assigns a pseudo class to a given element name. IElement names are case-insensitive. </summary>
        /// <param name="name"> the element name </param>
        /// <param name="pseudoClass"> the pseudo class to be assigned </param>
        public MatchConditionOnElements(string name, Selector_PseudoClassType pseudoClass)
        {
            addMatch(name, pseudoClass);
        }

        /// <summary>
        /// Assigns a pseudo class to the given element. Multiple pseudo classes may be assigned to a single element. </summary>
        /// <param name="e"> the DOM element </param>
        /// <param name="pseudoClass"> the pseudo class to be assigned </param>
        public virtual void addMatch(IElement e, Selector_PseudoClassType pseudoClass)
        {
            if (elements == null)
            {
                elements = new Dictionary<IElement, ISet<Selector_PseudoClassType>>();
            }

            ISet<Selector_PseudoClassType> classes = elements[e];
            if (classes == null)
            {
                classes = new HashSet<Selector_PseudoClassType>(2);
                elements[e] = classes;
            }
            classes.Add(pseudoClass);
        }

        /// <summary>
        /// Removes the pseudo class from the given element. </summary>
        /// <param name="e"> the DOM element </param>
        /// <param name="pseudoClass"> the pseudo class to be removed </param>
        public virtual void removeMatch(IElement e, Selector_PseudoClassType pseudoClass)
        {
            if (elements != null)
            {
                ISet<Selector_PseudoClassType> classes = elements[e];
                if (classes != null)
                {
                    classes.Remove(pseudoClass);
                }
            }
        }

        /// <summary>
        /// Assigns a pseudo class to the given element name. IElement names are case-insensitive.
        /// Multiple pseudo classes may be assigned to a single element name. </summary>
        /// <param name="name"> the element name </param>
        /// <param name="pseudoClass"> the pseudo class to be assigned </param>
        public virtual void addMatch(string name, Selector_PseudoClassType pseudoClass)
        {
            if (names == null)
            {
                names = new Dictionary<string, ISet<Selector_PseudoClassType>>();
            }

            ISet<Selector_PseudoClassType> classes = names[name];
            if (classes == null)
            {
                classes = new HashSet<Selector_PseudoClassType>(2);
                names[name] = classes;
            }
            classes.Add(pseudoClass);
        }

        /// <summary>
        /// Removes the pseudo class from the given element name. IElement names are case-insensitive. </summary>
        /// <param name="name"> the element name </param>
        /// <param name="pseudoClass"> the pseudo class to be removed </param>
        public virtual void removeMatch(string name, Selector_PseudoClassType pseudoClass)
        {
            if (names != null)
            {
                ISet<Selector_PseudoClassType> classes = names[name];
                if (classes != null)
                {
                    classes.Remove(pseudoClass);
                }
            }
        }

        public virtual bool isSatisfied(IElement e, Selector_SelectorPart selpart)
        {
            if (selpart is Selector_PseudoClass)
            {
                Selector_PseudoClassType required = ((Selector_PseudoClass)selpart).Type;

                if (elements != null)
                {
                    ISet<Selector_PseudoClassType> pseudos = elements[e];
                    if (pseudos != null)
                    {
                        return pseudos.Contains(required);
                    }
                }

                if (names != null)
                {
                    ISet<Selector_PseudoClassType> pseudos = names[e.TagName.ToLower()];
                    if (pseudos != null)
                    {
                        return pseudos.Contains(required);
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        public object Clone()
        {
            //ORIGINAL LINE: final MatchConditionOnElements clone = new MatchConditionOnElements();
            MatchConditionOnElements clone = new MatchConditionOnElements();
            if (elements != null)
            {
                clone.elements = new Dictionary<IElement, ISet<Selector_PseudoClassType>>();
                foreach (IElement e in elements.Keys)
                {
                    //ORIGINAL LINE: final java.util.HashSet<StyleParserCS.css.Selector_PseudoClassType> clonedDeclarations = new java.util.HashSet<StyleParserCS.css.Selector_PseudoClassType>(elements.get(e));
                    HashSet<Selector_PseudoClassType> clonedDeclarations = new HashSet<Selector_PseudoClassType>(elements[e]);
                    clone.elements[e] = clonedDeclarations;
                }
            }
            if (names != null)
            {
                clone.names = new Dictionary<string, ISet<Selector_PseudoClassType>>();
                foreach (string n in names.Keys)
                {
                    //ORIGINAL LINE: final java.util.HashSet<StyleParserCS.css.Selector_PseudoClassType> clonedDeclarations = new java.util.HashSet<StyleParserCS.css.Selector_PseudoClassType>(names.get(n));
                    HashSet<Selector_PseudoClassType> clonedDeclarations = new HashSet<Selector_PseudoClassType>(names[n]);
                    clone.names[n] = clonedDeclarations;
                }
            }
            return clone;
        }
    }

}