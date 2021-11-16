using StyleParserCS.css;
using System;
using System.Text;

namespace StyleParserCS.csskit
{

    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using Selector = StyleParserCS.css.Selector;

    /// <summary>
    /// CSS CombinedSelector with implementation of specificity
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// 
    /// </summary>
    public class CombinedSelectorImpl : AbstractRule<Selector>, CombinedSelector
    {

        protected internal CombinedSelectorImpl()
        {
        }

        //ORIGINAL LINE: public StyleParserCS.css.Selector getLastSelector() throws UnsupportedOperationException
        public virtual Selector LastSelector
        {
            get
            {
                if (Count == 0)
                {
                    throw new System.NotSupportedException("There is no \"last\" simple selector");
                }
                return this[Count - 1]; // list[list.Count - 1];
            }
        }

        public virtual StyleParserCS.css.Selector_PseudoElementType PseudoElementType
        {
            get
            {
                return LastSelector.PseudoElementType; //pseudo-elements may only be appended after the last simple selector of the selector
            }
        }

        /// <summary>
        /// Computes specificity of selector
        /// </summary>
        public virtual StyleParserCS.css.CombinedSelector_Specificity computeSpecificity()
        {
            StyleParserCS.css.CombinedSelector_Specificity spec = new SpecificityImpl();

            foreach (Selector s in this)
            {
                s.computeSpecificity(spec);
            }

            return spec;

        }


        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM);

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public class SpecificityImpl : CombinedSelector_Specificity
        {

            protected internal int[] spec = new int[Enum.GetValues(typeof(CombinedSelector_Specificity_Level)).Length]; // new int[Enum.GetValues(typeof(StyleParserCS.css.CombinedSelector_Specificity_Level)).length];

            public int CompareTo(StyleParserCS.css.CombinedSelector_Specificity o)
            {

                if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.A) > o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.A))
                {
                    return 1;
                }
                else if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.A) < o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.A))
                {
                    return -1;
                }

                if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.B) > o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.B))
                {
                    return 1;
                }
                else if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.B) < o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.B))
                {
                    return -1;
                }

                if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.C) > o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.C))
                {
                    return 1;
                }
                else if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.C) < o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.C))
                {
                    return -1;
                }

                if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.D) > o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.D))
                {
                    return 1;
                }
                else if (get(StyleParserCS.css.CombinedSelector_Specificity_Level.D) < o.get(StyleParserCS.css.CombinedSelector_Specificity_Level.D))
                {
                    return -1;
                }

                return 0;

            }

            public virtual int get(StyleParserCS.css.CombinedSelector_Specificity_Level level)
            {

                switch (level)
                {
                    case StyleParserCS.css.CombinedSelector_Specificity_Level.A:
                        return spec[0];
                    case StyleParserCS.css.CombinedSelector_Specificity_Level.B:
                        return spec[1];
                    case StyleParserCS.css.CombinedSelector_Specificity_Level.C:
                        return spec[2];
                    case StyleParserCS.css.CombinedSelector_Specificity_Level.D:
                        return spec[3];
                    default:
                        return 0;
                }
            }

            public virtual void add(StyleParserCS.css.CombinedSelector_Specificity_Level level)
            {

                switch (level)
                {
                    case CombinedSelector_Specificity_Level.A:
                        spec[0]++;
                        break;
                        // goto case B;
                    case CombinedSelector_Specificity_Level.B:
                        spec[1]++;
                        break;
                        // goto case C;
                    case CombinedSelector_Specificity_Level.C:
                        spec[2]++;
                        break;
                        // goto case D;
                    case StyleParserCS.css.CombinedSelector_Specificity_Level.D:
                        spec[3]++;
                        break;
                }
            }

            /* (non-Javadoc)
			 * @see java.lang.Object#hashCode()
			 */
            public override int GetHashCode()
            {
                const int prime = 31;
                int result = 1;
                result = prime * result + spec.GetHashCode();
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
                if (!(obj is SpecificityImpl))
                {
                    return false;
                }
                SpecificityImpl other = (SpecificityImpl)obj;                
                if (!spec.Equals(other.spec))
                {
                    return false;
                }
                return true;
            }

        }

    }

}