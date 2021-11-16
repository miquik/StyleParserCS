using System.Linq;

namespace StyleParserCS.domassign
{
    using CombinedSelector = StyleParserCS.css.CombinedSelector;
    using Declaration = StyleParserCS.css.Declaration;
    using StyleSheet = StyleParserCS.css.StyleSheet;
    using DeclarationImpl = StyleParserCS.csskit.DeclarationImpl;

    /// <summary>
    /// Adds specificity to declaration from its selector.
    /// This class shares declaration with its parent class.
    /// 
    /// @author burgetr
    /// @author kapy
    /// </summary>
    public class AssignedDeclaration : DeclarationImpl, Declaration
    {

        protected internal StyleParserCS.css.CombinedSelector_Specificity spec;
        protected internal StyleParserCS.css.StyleSheet_Origin origin;

        /// <summary>
        /// Creates assigned declaration from specificity and shallow copy of declaration </summary>
        /// <param name="d"> Declaration to be shallow-copied </param>
        /// <param name="spec"> Specificity </param>
        public AssignedDeclaration(Declaration d, StyleParserCS.css.CombinedSelector_Specificity spec, StyleParserCS.css.StyleSheet_Origin origin) : base(d)
        {
            this.spec = spec;
            this.origin = origin;
        }

        /// <summary>
        /// Creates assigned declaration from selector and shallow copy of declaration </summary>
        /// <param name="d"> Declaration to be shallow-copied </param>
        /// <param name="s"> CombinedSelector, which's specificity is computed inside </param>
        public AssignedDeclaration(Declaration d, CombinedSelector s, StyleParserCS.css.StyleSheet_Origin origin) : this(d, s.computeSpecificity(), origin)
        {
        }

        public override int CompareTo(Declaration other)
        {

            if (!(other is AssignedDeclaration))
            {
                return base.CompareTo(other);
            }

            AssignedDeclaration o = (AssignedDeclaration)other;

            int res = OriginOrder - o.OriginOrder;
            if (res == 0)
            {
                return this.spec.CompareTo(o.spec);
            }
            else
            {
                return res;
            }
        }

        /// <summary>
        /// Computes the priority order of the declaration based on its origin and importance
        /// according to the CSS specification. </summary>
        /// <returns> The priority order (1..5). </returns>
        /// <seealso cref= <a href="http://www.w3.org/TR/CSS21/cascade.html#cascading-order">http://www.w3.org/TR/CSS21/cascade.html#cascading-order</a> </seealso>
        public virtual int OriginOrder
        {
            get
            {
                if (important)
                {
                    if (origin == StyleParserCS.css.StyleSheet_Origin.AUTHOR)
                    {
                        return 4;
                    }
                    else if (origin == StyleParserCS.css.StyleSheet_Origin.AGENT)
                    {
                        return 1;
                    }
                    else
                    {
                        return 5;
                    }
                }
                else
                {
                    if (origin == StyleParserCS.css.StyleSheet_Origin.AUTHOR)
                    {
                        return 3;
                    }
                    else if (origin == StyleParserCS.css.StyleSheet_Origin.AGENT)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((spec == null) ? 0 : spec.GetHashCode());
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
            //ORIGINAL LINE: if (!super.equals(obj))
            // TOCHECK
            // if (!base.SequenceEqual(obj))
            // {
            //     return false;
            // }
            if (!(obj is AssignedDeclaration))
            {
                return false;
            }
            AssignedDeclaration other = (AssignedDeclaration)obj;
            if (spec == null)
            {
                if (other.spec != null)
                {
                    return false;
                }
            }
            else if (!spec.Equals(other.spec))
            {
                return false;
            }
            return true;
        }



    }

}