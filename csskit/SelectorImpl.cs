using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.csskit
{
    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using ElementMatcher = StyleParserCS.css.ElementMatcher;
    using MatchCondition = StyleParserCS.css.MatchCondition;
    using StyleParserCS.css;
    using Selector = StyleParserCS.css.Selector;
    using CombinedSelector_Specificity = StyleParserCS.css.CombinedSelector_Specificity;
    using CombinedSelector_Specificity_Level = StyleParserCS.css.CombinedSelector_Specificity_Level;
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// Encapsulates one selector for CSS declaration.
    /// CombinedSelector can contain classes, attributes, ids, pseudo attributes,
    /// and element name, together with combinator according to next placed selectors
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008   	    
    /// </summary>
    public class SelectorImpl : AbstractRule<StyleParserCS.css.Selector_SelectorPart>, Selector
    {

        protected internal StyleParserCS.css.Selector_Combinator combinator;
        protected internal StyleParserCS.css.Selector_PseudoElementType pseudoElementType; //the pseudo element if defined (only a single pseudo element may be used in the selector)

        public override Rule<StyleParserCS.css.Selector_SelectorPart> replaceAll(IList<StyleParserCS.css.Selector_SelectorPart> replacement)
        {
            foreach (StyleParserCS.css.Selector_SelectorPart item in replacement)
            {
                checkPseudoElement(item);
            }
            return base.replaceAll(replacement);
        }

        protected override void SetItem(int index, Selector_SelectorPart item)
        {
            hash = 0;
            checkPseudoElement(item);
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, Selector_SelectorPart item)
        {
            hash = 0;
            checkPseudoElement(item);
            base.InsertItem(index, item);
        }

        /*
        public virtual StyleParserCS.css.Selector_SelectorPart setJavaToDotNetIndexer(int index, StyleParserCS.css.Selector_SelectorPart element)
        {
            checkPseudoElement(element);
            return base.set(index, element);
        }

        public override void insert(int index, StyleParserCS.css.Selector_SelectorPart element)
        {
            checkPseudoElement(element);
            // base.add(index, element);
            base.insert(index, element);
        }                

        public override bool add(StyleParserCS.css.Selector_SelectorPart o)
        {
            checkPseudoElement(o);
            return base.add(o);
        }
        */

        /// <returns> the combinator </returns>
        public virtual StyleParserCS.css.Selector_Combinator Combinator
        {
            get
            {
                return combinator;
            }
        }

        /// <summary>
        /// Registers the pseudo element if the given selector part is a pseudo element.
        /// </summary>
        private void checkPseudoElement(StyleParserCS.css.Selector_SelectorPart item)
        {
            if (item is StyleParserCS.css.Selector_PseudoElement)
            {
                pseudoElementType = ((StyleParserCS.css.Selector_PseudoElement)item).Type;
            }
        }

        /// <param name="combinator"> the combinator to set </param>
        public virtual Selector setCombinator(StyleParserCS.css.Selector_Combinator combinator)
        {
            this.combinator = combinator;
            return this;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            if (combinator != null)
            {
                sb.Append(combinator.value());
            }
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM);

            return sb.ToString();
        }


        public virtual string ClassName
        {
            get
            {
                string className = null;
                foreach (StyleParserCS.css.Selector_SelectorPart item in this)
                {
                    if (item is StyleParserCS.css.Selector_ElementClass)
                    {
                        className = ((StyleParserCS.css.Selector_ElementClass)item).ClassName;
                    }
                }
                return className;
            }
        }


        public virtual string IDName
        {
            get
            {
                string idName = null;
                foreach (StyleParserCS.css.Selector_SelectorPart item in this)
                {
                    if (item is StyleParserCS.css.Selector_ElementID)
                    {
                        idName = ((StyleParserCS.css.Selector_ElementID)item).ID;
                    }
                }
                return idName;
            }
        }

        public virtual string ElementName
        {
            get
            {
                string elementName = null;
                foreach (StyleParserCS.css.Selector_SelectorPart item in this)
                {
                    if (item is StyleParserCS.css.Selector_ElementName)
                    {
                        elementName = ((StyleParserCS.css.Selector_ElementName)item).Name;
                    }
                }
                return elementName;
            }
        }

        public virtual StyleParserCS.css.Selector_PseudoElementType PseudoElementType
        {
            get
            {
                return pseudoElementType;
            }
        }

        //ORIGINAL LINE: @Override public boolean hasPseudoClass(final StyleParserCS.css.Selector_PseudoClassType pct)
        public virtual bool hasPseudoClass(StyleParserCS.css.Selector_PseudoClassType pct)
        {
            foreach (StyleParserCS.css.Selector_SelectorPart item in this)
            {
                if (item is StyleParserCS.css.Selector_PseudoClass && ((StyleParserCS.css.Selector_PseudoClass)item).Type == pct)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool matches(IElement e)
        {

            // check other items of simple selector
            foreach (StyleParserCS.css.Selector_SelectorPart item in this)
            {
                if (item == null || !item.matches(e, CSSFactory.ElementMatcher, CSSFactory.DefaultMatchCondition)) //null in case of syntax error (missing term)
                {
                    return false;
                }
            }

            // we passed checking
            return true;
        }

        public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
        {

            // check other items of simple selector
            foreach (StyleParserCS.css.Selector_SelectorPart item in this)
            {
                if (item == null || !item.matches(e, matcher, cond)) //null in case of syntax error (missing term)
                {
                    return false;
                }
            }
            // we passed checking
            return true;
        }

        /// <summary>
        /// Computes specificity of this selector
        /// </summary>
        public virtual void computeSpecificity(CombinedSelector_Specificity spec)
        {
            foreach (StyleParserCS.css.Selector_SelectorPart item in this)
            {
                item.computeSpecificity(spec);
            }
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((combinator == null) ? 0 : combinator.GetHashCode());
            return result;
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#equals(java.lang.Object)
		 */
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!base.Equals(obj))
            {
                return false;
            }
            if (!(obj is SelectorImpl))
            {
                return false;
            }
            SelectorImpl other = (SelectorImpl)obj;
            if (combinator == null)
            {
                if (other.combinator != null)
                {
                    return false;
                }
            }
            else if (!combinator.Equals(other.combinator))
            {
                return false;
            }
            return true;
        }


        // ============================================================
        // implementation of intern classes	

        /// <summary>
        /// IElement name
        /// @author kapy
        /// </summary>
        public class ElementNameImpl : StyleParserCS.css.Selector_ElementName
        {

            internal string name;

            protected internal ElementNameImpl(string name)
            {
                setName(name);
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                if (!StyleParserCS.css.Selector_ElementName_Fields.WILDCARD.Equals(name))
                {
                    spec.add(CombinedSelector_Specificity_Level.D);
                }
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                if (!string.ReferenceEquals(name, null) && StyleParserCS.css.Selector_ElementName_Fields.WILDCARD.Equals(name))
                {
                    return true;
                }
                return matcher.matchesName(e, name);
            }

            public virtual string Name
            {
                get
                {
                    return name;
                }
            }

            public virtual StyleParserCS.css.Selector_ElementName setName(string name)
            {
                if (string.ReferenceEquals(name, null))
                {
                    throw new System.ArgumentException("Invalid element name (null)");
                }

                this.name = name;
                return this;
            }

            public override string ToString()
            {
                if (StyleParserCS.css.Selector_ElementName_Fields.WILDCARD.Equals(name))
                {
                    return name;
                }
                else
                {
                    return CssEscape.escapeCssIdentifier(name);
                }
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#hashCode()
			 */
            public override int GetHashCode()
            {
                const int prime = 31;
                int result = 1;
                result = prime * result + ((string.ReferenceEquals(name, null)) ? 0 : name.GetHashCode());
                return result;
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#equals(java.lang.Object)
			 */
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ElementNameImpl))
                {
                    return false;
                }
                ElementNameImpl other = (ElementNameImpl)obj;
                if (string.ReferenceEquals(name, null))
                {
                    if (!string.ReferenceEquals(other.name, null))
                    {
                        return false;
                    }
                }
                else if (!name.Equals(other.name))
                {
                    return false;
                }
                return true;
            }

        }

        /// <summary>
        /// IElement class
        /// @author kapy
        /// 
        /// </summary>
        public class ElementClassImpl : StyleParserCS.css.Selector_ElementClass
        {

            internal string className;

            protected internal ElementClassImpl(string className)
            {
                setClassName(className);
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                spec.add(CombinedSelector_Specificity_Level.C);
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                return matcher.matchesClass(e, className);
            }

            public virtual string ClassName
            {
                get
                {
                    return className;
                }
            }

            public virtual StyleParserCS.css.Selector_ElementClass setClassName(string className)
            {
                if (string.ReferenceEquals(className, null))
                {
                    throw new System.ArgumentException("Invalid element class (null)");
                }

                this.className = className;
                return this;
            }

            public override string ToString()
            {
                return "." + CssEscape.escapeCssIdentifier(className);
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#hashCode()
			 */
            public override int GetHashCode()
            {
                const int prime = 31;
                int result = 1;
                result = prime * result + ((string.ReferenceEquals(className, null)) ? 0 : className.GetHashCode());
                return result;
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#equals(java.lang.Object)
			 */
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ElementClassImpl))
                {
                    return false;
                }
                ElementClassImpl other = (ElementClassImpl)obj;
                if (string.ReferenceEquals(className, null))
                {
                    if (!string.ReferenceEquals(other.className, null))
                    {
                        return false;
                    }
                }
                else if (!className.Equals(other.className))
                {
                    return false;
                }
                return true;
            }
        }

        public class PseudoPageImpl : StyleParserCS.css.Selector_PseudoPage
        {

            internal readonly string name;
            internal readonly StyleParserCS.css.Selector_PseudoPageType type;

            protected internal PseudoPageImpl(string name)
            {
                this.name = name;
                type = StyleParserCS.css.Selector_PseudoPageType.ForNameLookup(name);
            }

            public virtual string Name
            {
                get
                {
                    return name;
                }
            }

            public virtual StyleParserCS.css.Selector_PseudoPageType Type
            {
                get
                {
                    return type;
                }
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                spec.add(CombinedSelector_Specificity_Level.C);
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                // TODO: check this implementation
                if (type == null)
                {
                    return false; // unknown or unimplemented pseudo
                }
                return cond.isSatisfied(e, this);
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(OutputUtil.PSEUDO_OPENING);

                if (!string.ReferenceEquals(name, null))
                {
                    sb.Append(name);
                }

                return sb.ToString();
            }

            public override int GetHashCode()
            {
                int hash = 7;
                hash = 23 * hash + this.name.GetHashCode(); // Objects.hashCode(this.name);
                hash = 23 * hash + this.type.GetHashCode(); // Objects.hashCode(this.type);
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (this.GetType() != obj.GetType())
                {
                    return false;
                }
                //ORIGINAL LINE: final PseudoPageImpl other = (PseudoPageImpl) obj;
                PseudoPageImpl other = (PseudoPageImpl)obj;
                if (!(this.name.Equals(other.name)))
                {
                    return false;
                }
                if (this.type != other.type)
                {
                    return false;
                }
                return true;
            }

        }

        public class PseudoClassImpl : StyleParserCS.css.Selector_PseudoClass
        {

            internal readonly string name;
            internal readonly string functionValue;
            internal readonly StyleParserCS.css.Selector_PseudoClassType type;
            internal Selector nestedSelector; // for :not(sel)
            internal int[] elementIndex; // decoded element index for nth-XXXX properties -- values a and b in the an+b specification

            internal PseudoClassImpl(string name, string functionValue, Selector nestedSelector)
            {
                this.name = name;
                type = StyleParserCS.css.Selector_PseudoClassType.ForNameLookup(name);
                this.functionValue = functionValue;
                this.nestedSelector = nestedSelector;

                // Type-specific initialization
                if (type != null)
                {
                    switch (type.Name)
                    {
                        case nameof(Selector_PseudoClassType.NOT):
                            if (nestedSelector == null && !string.ReferenceEquals(functionValue, null))
                            {
                                nestedSelector = new SelectorImpl();
                                nestedSelector.unlock();
                                nestedSelector.Add(new ElementNameImpl(functionValue));
                            }
                            break;
                        case nameof(Selector_PseudoClassType.NTH_CHILD):
                        case nameof(Selector_PseudoClassType.NTH_LAST_CHILD):
                        case nameof(Selector_PseudoClassType.NTH_OF_TYPE):
                        case nameof(Selector_PseudoClassType.NTH_LAST_OF_TYPE):
                            elementIndex = decodeIndex(functionValue);
                            break;
                        default:
                            break;
                    }
                }
            }

            protected internal PseudoClassImpl(string name) : this(name, null, null)
            {
            }

            protected internal PseudoClassImpl(string name, string functionValue) : this(name, functionValue, null)
            {
            }

            protected internal PseudoClassImpl(string name, Selector nestedSelector) : this(name, null, nestedSelector)
            {
            }

            public virtual string Name
            {
                get
                {
                    return name;
                }
            }

            public virtual string FunctionValue
            {
                get
                {
                    return functionValue;
                }
            }

            public virtual StyleParserCS.css.Selector_PseudoClassType Type
            {
                get
                {
                    return type;
                }
            }

            public virtual Selector NestedSelector
            {
                get
                {
                    return nestedSelector;
                }
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                spec.add(CombinedSelector_Specificity_Level.C);
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                if (type == null)
                {
                    return false; // unknown or unimplemented pseudo
                }

                switch (type.Name)
                {
                    case nameof(Selector_PseudoClassType.FIRST_CHILD):
                    case nameof(Selector_PseudoClassType.LAST_CHILD):
                    case nameof(Selector_PseudoClassType.ONLY_CHILD):
                        if (e.Parent.NodeType == NodeType.Element)
                        {
                            bool first = false;
                            bool last = false;
                            if (type != Selector_PseudoClassType.LAST_CHILD)
                            {
                                INode prev = e;
                                do
                                {
                                    prev = prev.PreviousSibling;
                                    if (prev == null)
                                    {
                                        first = true;
                                        break;
                                    }
                                } while (prev.NodeType != NodeType.Element);
                            }
                            if (type != Selector_PseudoClassType.FIRST_CHILD)
                            {
                                INode next = e;
                                do
                                {
                                    next = next.NextSibling;
                                    if (next == null)
                                    {
                                        last = true;
                                        break;
                                    }
                                } while (next.NodeType != NodeType.Element);
                            }
                            switch (type.Name)
                            {
                                case nameof(Selector_PseudoClassType.FIRST_CHILD):
                                    return first;
                                case nameof(Selector_PseudoClassType.LAST_CHILD):
                                    return last;
                                default:
                                    return first && last; //ONLY_CHILD
                            }
                        }
                        else
                        {
                            return false;
                        }
                    case nameof(Selector_PseudoClassType.FIRST_OF_TYPE):
                    case nameof(Selector_PseudoClassType.LAST_OF_TYPE):
                    case nameof(Selector_PseudoClassType.ONLY_OF_TYPE):
                        if (e.Parent.NodeType == NodeType.Element)
                        {
                            bool firstt = false;
                            bool lastt = false;
                            if (type != Selector_PseudoClassType.LAST_OF_TYPE)
                            {
                                INode prev = e;
                                firstt = true;
                                do
                                {
                                    prev = prev.PreviousSibling;
                                    if (prev != null && prev.NodeType == NodeType.Element && isSameElementType(e, (IElement)prev))
                                    {
                                        firstt = false;
                                    }
                                } while (prev != null && firstt);
                            }
                            if (type != Selector_PseudoClassType.FIRST_OF_TYPE)
                            {
                                INode next = e;
                                lastt = true;
                                do
                                {
                                    next = next.NextSibling;
                                    if (next != null && next.NodeType == NodeType.Element && isSameElementType(e, (IElement)next))
                                    {
                                        lastt = false;
                                    }
                                } while (next != null && lastt);
                            }
                            switch (type.Name)
                            {
                                case nameof(Selector_PseudoClassType.FIRST_OF_TYPE):
                                    return firstt;
                                case nameof(Selector_PseudoClassType.LAST_OF_TYPE):
                                    return lastt;
                                default:
                                    return firstt && lastt; //ONLY_OF_TYPE
                            }
                        }
                        else
                        {
                            return false;
                        }
                    case nameof(Selector_PseudoClassType.NTH_CHILD):
                        return positionMatches(countSiblingsBefore(e, false) + 1, elementIndex);
                    case nameof(Selector_PseudoClassType.NTH_LAST_CHILD):
                        return positionMatches(countSiblingsAfter(e, false) + 1, elementIndex);
                    case nameof(Selector_PseudoClassType.NTH_OF_TYPE):
                        return positionMatches(countSiblingsBefore(e, true) + 1, elementIndex);
                    case nameof(Selector_PseudoClassType.NTH_LAST_OF_TYPE):
                        return positionMatches(countSiblingsAfter(e, true) + 1, elementIndex);
                    case nameof(Selector_PseudoClassType.ROOT):
                        return e.Parent.NodeType == NodeType.Document;
                    case nameof(Selector_PseudoClassType.EMPTY):
                        INodeList elist = e.ChildNodes;
                        for (int i = 0; i < elist.Length; i++)
                        {
                            NodeType t = elist[i].NodeType;
                            if (t == NodeType.Element || t == NodeType.Text || t == NodeType.CharacterData || t == NodeType.EntityReference)
                            {
                                return false;
                            }
                        }
                        return true;
                    case nameof(Selector_PseudoClassType.NOT):
                        return nestedSelector != null && !nestedSelector.matches(e, matcher, cond);
                    default:
                        // match all pseudo classes specified by an additional condition (usually used for using LINK pseudo class for links)
                        return cond.isSatisfied(e, this);
                }
            }

            /// <summary>
            /// Checks whether two elements have the same name. </summary>
            /// <param name="e1"> the first element </param>
            /// <param name="e2"> the second element </param>
            /// <returns> <code>true</code> when the elements have the same names </returns>
            protected internal virtual bool isSameElementType(IElement e1, IElement e2)
            {
                return e1.NodeName.ToLower() == e2.NodeName;
            }

            /// <summary>
            /// Checks whether the element position matches a <code>an+b</code> index specification. </summary>
            /// <param name="pos"> The element position according to some counting criteria. </param>
            /// <param name="n"> The index specification <code>an+b</code> - <code>a</code> and <code>b</code> values in array int[2]. </param>
            /// <returns> <code>true</code> when the position matches the index. </returns>
            protected internal virtual bool positionMatches(int pos, int[] n)
            {
                if (n != null)
                {
                    try
                    {
                        int an = pos - n[1];
                        if (n[0] == 0)
                        {
                            return an == 0;
                        }
                        else
                        {
                            return an * n[0] >= 0 && an % n[0] == 0;
                        }
                    }
                    catch (System.FormatException)
                    {
                        return false;
                    }
                }
                else //no indices specified (syntax error or missing values)
                {
                    return false;
                }
            }

            /// <summary>
            /// Computes the count of element siblings before the given element in the DOM tree. </summary>
            /// <param name="e"> The element to be examined </param>
            /// <param name="sameType"> when set to <code>true</code> only the element with the same type are considered.
            ///                 Otherwise, all elements are considered. </param>
            /// <returns> the number of preceding siblings </returns>
            protected internal virtual int countSiblingsBefore(IElement e, bool sameType)
            {
                int cnt = 0;
                INode prev = e;
                do
                {
                    prev = prev.PreviousSibling;
                    if (prev != null && prev.NodeType == NodeType.Element)
                    {
                        if (!sameType || isSameElementType(e, (IElement)prev))
                        {
                            cnt++;
                        }
                    }
                } while (prev != null);

                return cnt;
            }

            /// <summary>
            /// Computes the count of element siblings after the given element in the DOM tree. </summary>
            /// <param name="e"> The element to be examined </param>
            /// <param name="sameType"> when set to <code>true</code> only the element with the same type are considered.
            ///                 Otherwise, all elements are considered. </param>
            /// <returns> the number of following siblings </returns>
            protected internal virtual int countSiblingsAfter(IElement e, bool sameType)
            {
                int cnt = 0;
                INode next = e;
                do
                {
                    next = next.NextSibling;
                    if (next != null && next.NodeType == NodeType.Element)
                    {
                        if (!sameType || isSameElementType(e, (IElement)next))
                        {
                            cnt++;
                        }
                    }
                } while (next != null);

                return cnt;
            }

            /// <summary>
            /// Decodes the element index in the <code>an+b</code> form. </summary>
            /// <param name="index"> the element index string </param>
            /// <returns> an array of two integers <code>a</code> and <code>b</code>, or null if the index string is malformatted </returns>
            protected internal virtual int[] decodeIndex(string index)
            {
                if (string.ReferenceEquals(index, null))
                {
                    return null;
                }
                string s = index.ToLower().Trim();
                if (s.Equals("odd"))
                {
                    return new int[] { 2, 1 };
                }
                if (s.Equals("even"))
                {
                    return new int[] { 2, 0 };
                }
                int[] ret = new int[] { 0, 0 };
                int n = s.IndexOf('n');
                if (n != -1)
                {
                    string sa = s.Substring(0, n).Trim();
                    if (sa.Length == 0)
                    {
                        ret[0] = 1;
                    }
                    else if (sa.Equals("-"))
                    {
                        ret[0] = -1;
                    }
                    else
                    {
                        if (int.TryParse(sa, out ret[0]) == false)
                        {
                            return null;
                        }
                        // ret[0] = int.Parse(sa);
                    }

                    n++;
                    StringBuilder sb = new StringBuilder();
                    while (n < s.Length)
                    {
                        char ch = s[n];
                        if (ch != '+' && !char.IsWhiteSpace(ch))
                        {
                            sb.Append(ch);
                        }
                        n++;
                    }
                    if (sb.Length > 0)
                    {
                        if (int.TryParse(sb.ToString(), out ret[1]) == false)
                        {
                            return null;
                        }
                        // ret[1] = int.Parse(sb.ToString());
                    }
                }
                else
                {
                    if (int.TryParse(s, out ret[1]) == false)
                    {
                        return null;
                    }
                    // ret[1] = int.Parse(s);
                }

                return ret;
                /*
                try
                {
                    int[] ret = new int[] { 0, 0 };
                    int n = s.IndexOf('n');
                    if (n != -1)
                    {
                        string sa = s.Substring(0, n).Trim();
                        if (sa.Length == 0)
                        {
                            ret[0] = 1;
                        }
                        else if (sa.Equals("-"))
                        {
                            ret[0] = -1;
                        }
                        else
                        {
                            if (int.TryParse(sa, out ret[0]) == false)
                            {
                                return null;
                            }
                            // ret[0] = int.Parse(sa);
                        }

                        n++;
                        StringBuilder sb = new StringBuilder();
                        while (n < s.Length)
                        {
                            char ch = s[n];
                            if (ch != '+' && !char.IsWhiteSpace(ch))
                            {
                                sb.Append(ch);
                            }
                            n++;
                        }
                        if (sb.Length > 0)
                        {
                            if (int.TryParse(sb.ToString(), out ret[1]) == false)
                            {
                                return null;
                            }
                            // ret[1] = int.Parse(sb.ToString());
                        }
                    }
                    else
                    {
                        if (int.TryParse(s, out ret[1]) == false)
                        {
                            return null;
                        }
                        // ret[1] = int.Parse(s);
                    }

                    return ret;
                }
                catch (System.FormatException)
                {
                    return null;
                }
                */
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(OutputUtil.PSEUDO_OPENING);

                if (!string.ReferenceEquals(name, null))
                {
                    sb.Append(name);
                }

                if (nestedSelector != null)
                {
                    sb.Append(OutputUtil.FUNCTION_OPENING).Append(nestedSelector.ToString()).Append(OutputUtil.FUNCTION_CLOSING);
                }
                else if (!string.ReferenceEquals(functionValue, null))
                {
                    sb.Append(OutputUtil.FUNCTION_OPENING).Append(functionValue).Append(OutputUtil.FUNCTION_CLOSING);
                }

                return sb.ToString();
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash = 43 * hash + this.name.GetHashCode(); // Objects.hashCode(this.name);
                hash = 43 * hash + this.functionValue.GetHashCode(); // Objects.hashCode(this.functionValue);
                hash = 43 * hash + this.type.GetHashCode(); // Objects.hashCode(this.type);
                hash = 43 * hash + this.nestedSelector.GetHashCode(); // Objects.hashCode(this.nestedSelector);
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (this.GetType() != obj.GetType())
                {
                    return false;
                }
                //ORIGINAL LINE: final PseudoClassImpl other = (PseudoClassImpl) obj;
                PseudoClassImpl other = (PseudoClassImpl)obj;
                if (this.type != other.type)
                {
                    return false;
                }
                if (!Object.Equals(this.name, other.name))
                {
                    return false;
                }
                if (!Object.Equals(this.functionValue, other.functionValue))
                {
                    return false;
                }
                if (!Object.Equals(this.nestedSelector, other.nestedSelector))
                {
                    return false;
                }
                return true;
            }
        }

        public class PseudoElementImpl : StyleParserCS.css.Selector_PseudoElement
        {

            internal readonly string name;
            internal readonly string functionValue;
            internal readonly StyleParserCS.css.Selector_PseudoElementType type;
            internal Selector nestedSelector; // for ::cue(sel)

            internal PseudoElementImpl(string name, string functionValue, Selector nestedSelector)
            {
                this.name = name;
                type = StyleParserCS.css.Selector_PseudoElementType.ForNameLookup(name);
                this.functionValue = functionValue;
                this.nestedSelector = nestedSelector;

                // Type-specific initialization
                if (type != null)
                {
                    switch (type.Name)
                    {
                        case nameof(Selector_PseudoElementType.CUE):
                            if (nestedSelector == null && !string.ReferenceEquals(functionValue, null))
                            {
                                nestedSelector = new SelectorImpl();
                                nestedSelector.Add(new ElementNameImpl(functionValue));
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            protected internal PseudoElementImpl(string name) : this(name, null, null)
            {
            }

            protected internal PseudoElementImpl(string name, string functionValue) : this(name, functionValue, null)
            {
            }

            protected internal PseudoElementImpl(string name, Selector nestedSelector) : this(name, null, nestedSelector)
            {
            }

            public virtual string Name
            {
                get
                {
                    return name;
                }
            }

            public virtual string FunctionValue
            {
                get
                {
                    return functionValue;
                }
            }

            public virtual StyleParserCS.css.Selector_PseudoElementType Type
            {
                get
                {
                    return type;
                }
            }

            public virtual Selector NestedSelector
            {
                get
                {
                    return nestedSelector;
                }
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                spec.add(CombinedSelector_Specificity_Level.D);
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                // TODO: check
                if (type == null)
                {
                    return false; // Unknown or unimplemented pseudo-element
                }
                return true; // All pseudo-elements match
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(OutputUtil.PSEUDO_OPENING).Append(OutputUtil.PSEUDO_OPENING);

                if (!string.ReferenceEquals(name, null))
                {
                    sb.Append(name);
                }

                if (nestedSelector != null)
                {
                    sb.Append(OutputUtil.FUNCTION_OPENING).Append(nestedSelector.ToString()).Append(OutputUtil.FUNCTION_CLOSING);
                }
                else if (!string.ReferenceEquals(functionValue, null))
                {
                    sb.Append(OutputUtil.FUNCTION_OPENING).Append(functionValue).Append(OutputUtil.FUNCTION_CLOSING);
                }

                return sb.ToString();
            }

            public override int GetHashCode()
            {
                int hash = 3;
                hash = 53 * hash + this.name.GetHashCode(); // Object.GetHashCode(this.name);
                hash = 53 * hash + this.functionValue.GetHashCode(); // Objects.hashCode(this.functionValue);
                hash = 53 * hash + this.type.GetHashCode(); // Objects.hashCode(this.type);
                hash = 53 * hash + this.nestedSelector.GetHashCode(); // Objects.hashCode(this.nestedSelector);
                return hash;
            }

            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (this.GetType() != obj.GetType())
                {
                    return false;
                }
                //ORIGINAL LINE: final PseudoElementImpl other = (PseudoElementImpl) obj;
                PseudoElementImpl other = (PseudoElementImpl)obj;
                if (this.type != other.type)
                {
                    return false;
                }
                if (!Object.Equals(this.name, other.name))
                {
                    return false;
                }
                if (!Object.Equals(this.functionValue, other.functionValue))
                {
                    return false;
                }
                if (!Object.Equals(this.nestedSelector, other.nestedSelector))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// IElement ID
        /// @author kapy
        /// 
        /// </summary>
        public class ElementIDImpl : StyleParserCS.css.Selector_ElementID
        {

            internal string id;

            protected internal ElementIDImpl(string value)
            {
                setID(value);
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                spec.add(CombinedSelector_Specificity_Level.B);
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                return matcher.matchesID(e, id);
            }

            public virtual StyleParserCS.css.Selector_ElementID setID(string id)
            {
                if (string.ReferenceEquals(id, null))
                {
                    throw new System.ArgumentException("Invalid element ID (null)");
                }

                this.id = id;
                return this;
            }

            public virtual string ID
            {
                get
                {
                    return id;
                }
            }

            public override string ToString()
            {
                return "#" + CssEscape.escapeCssIdentifier(id);
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#hashCode()
			 */
            public override int GetHashCode()
            {
                const int prime = 31;
                int result = 1;
                result = prime * result + ((string.ReferenceEquals(id, null)) ? 0 : id.GetHashCode());
                return result;
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#equals(java.lang.Object)
			 */
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ElementIDImpl))
                {
                    return false;
                }
                ElementIDImpl other = (ElementIDImpl)obj;
                if (string.ReferenceEquals(id, null))
                {
                    if (!string.ReferenceEquals(other.id, null))
                    {
                        return false;
                    }
                }
                else if (!id.Equals(other.id))
                {
                    return false;
                }
                return true;
            }


        }

        /// <summary>
        /// Attribute holder
        /// @author kapy
        /// 
        /// </summary>
        public class ElementAttributeImpl : StyleParserCS.css.Selector_ElementAttribute
        {

            /// <summary>
            /// Operator between attribute and value </summary>
            internal StyleParserCS.css.Selector_Operator operatorv;

            internal string attribute;
            internal string value;
            internal bool isStringValue;

            protected internal ElementAttributeImpl(string value, bool isStringValue, StyleParserCS.css.Selector_Operator operatorv, string attribute)
            {
                this.isStringValue = isStringValue;
                this.operatorv = operatorv;
                this.attribute = attribute;
                setValue(value);
            }

            /// <returns> the operator </returns>
            public virtual StyleParserCS.css.Selector_Operator Operator
            {
                get
                {
                    return operatorv;
                }
                set
                {
                    this.operatorv = value;
                }
            }




            /// <returns> the attribute </returns>
            public virtual string Attribute
            {
                get
                {
                    return attribute;
                }
            }



            /// <param name="name"> the attribute to set </param>
            public virtual StyleParserCS.css.Selector_ElementAttribute setAttribute(string name)
            {
                this.attribute = name;
                return this;
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                spec.add(CombinedSelector_Specificity_Level.C);
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                return matcher.matchesAttribute(e, attribute, value, operatorv);
            }

            public virtual string Value
            {
                get
                {
                    return value;
                }
            }

            public virtual StyleParserCS.css.Selector_ElementAttribute setValue(string value)
            {
                this.value = value;
                return this;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(OutputUtil.ATTRIBUTE_OPENING).Append(attribute);
                sb.Append(operatorv.value());

                if (!string.ReferenceEquals(value, null))
                {
                    if (isStringValue)
                    {
                        sb.Append(OutputUtil.STRING_OPENING);
                        sb.Append(CssEscape.escapeCssString(value));
                        sb.Append(OutputUtil.STRING_CLOSING);
                    }
                    else
                    {
                        sb.Append(CssEscape.escapeCssIdentifier(value));
                    }
                }

                sb.Append(OutputUtil.ATTRIBUTE_CLOSING);

                return sb.ToString();
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#hashCode()
			 */
            public override int GetHashCode()
            {
                const int prime = 31;
                int result = 1;
                result = prime * result + ((string.ReferenceEquals(attribute, null)) ? 0 : attribute.GetHashCode());
                result = prime * result + (isStringValue ? 1231 : 1237);
                result = prime * result + ((operatorv == null) ? 0 : operatorv.GetHashCode());
                result = prime * result + ((string.ReferenceEquals(value, null)) ? 0 : value.GetHashCode());
                return result;
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#equals(java.lang.Object)
			 */
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ElementAttributeImpl))
                {
                    return false;
                }
                ElementAttributeImpl other = (ElementAttributeImpl)obj;
                if (string.ReferenceEquals(attribute, null))
                {
                    if (!string.ReferenceEquals(other.attribute, null))
                    {
                        return false;
                    }
                }
                else if (!attribute.Equals(other.attribute))
                {
                    return false;
                }
                if (isStringValue != other.isStringValue)
                {
                    return false;
                }
                if (operatorv == null)
                {
                    if (other.operatorv != null)
                    {
                        return false;
                    }
                }
                else if (!operatorv.Equals(other.operatorv))
                {
                    return false;
                }
                if (string.ReferenceEquals(value, null))
                {
                    if (!string.ReferenceEquals(other.value, null))
                    {
                        return false;
                    }
                }
                else if (!value.Equals(other.value))
                {
                    return false;
                }
                return true;
            }

        }

        public class ElementDOMImpl : StyleParserCS.css.Selector_ElementDOM
        {

            /// <summary>
            /// The element used as the selector </summary>
            internal IElement elem;
            /// <summary>
            /// When set to true, the selector has a maximal specificity (inline). Otherwise, it has a minimal specificity. </summary>
            internal bool inlinePriority;

            protected internal ElementDOMImpl(IElement e, bool inlinePriority)
            {
                this.elem = e;
                this.inlinePriority = inlinePriority;
            }

            public virtual IElement IElement
            {
                get
                {
                    return elem;
                }
            }

            public virtual StyleParserCS.css.Selector_ElementDOM setElement(IElement e)
            {
                this.elem = e;
                return this;
            }

            public virtual void computeSpecificity(CombinedSelector_Specificity spec)
            {
                if (inlinePriority)
                {
                    spec.add(CombinedSelector_Specificity_Level.A);
                }
            }

            public virtual bool matches(IElement e, ElementMatcher matcher, MatchCondition cond)
            {
                return elem.Equals(e);
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#hashCode()
			 */
            public override int GetHashCode()
            {
                const int prime = 31;
                int result = 1;
                result = prime * result + ((elem == null) ? 0 : elem.GetHashCode());
                return result;
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#equals(java.lang.Object)
			 */
            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null)
                {
                    return false;
                }
                if (!(obj is ElementDOMImpl))
                {
                    return false;
                }
                ElementDOMImpl other = (ElementDOMImpl)obj;
                if (elem == null)
                {
                    if (other.elem != null)
                    {
                        return false;
                    }
                }
                else if (!elem.Equals(other.elem))
                {
                    return false;
                }
                return true;
            }



        }

    }

}