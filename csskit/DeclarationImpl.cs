using System.Text;

namespace StyleParserCS.csskit
{
    using Declaration = StyleParserCS.css.Declaration;
    using StyleParserCS.css;

    /// <summary>
    /// CSS Declaration
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public class DeclarationImpl : AbstractRule<Term>, Declaration
    {

        protected internal string property;
        protected internal bool important;
        protected internal StyleParserCS.css.Declaration_Source source;

        protected internal DeclarationImpl()
        {
            this.property = "";
            this.important = false;
            this.source = null;
        }

        /// <summary>
        /// Shallow copy constructor </summary>
        /// <param name="clone"> Declaration to share term values with </param>
        protected internal DeclarationImpl(Declaration clone)
        {
            this.property = clone.Property;
            this.important = clone.Important;
            this.source = new StyleParserCS.css.Declaration_Source(clone.Source);
            this.replaceAll(clone.asList());
        }

        /// <summary>
        /// This declaration type is never inherited </summary>
        /// <returns> <code>false</code> </returns>
        public virtual bool isInherited(int level)
        {
            return false;
        }

        public virtual int InheritanceLevel
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// This declaration type is not about to be compared
        /// using precise conditions
        /// </summary>
        public virtual int CompareTo(Declaration o)
        {

            if (this.Important && !o.Important)
            {
                return 1;
            }
            else if (o.Important && !this.Important)
            {
                return -1;
            }

            return 0;
        }

        /// <returns> the property </returns>
        public virtual string Property
        {
            get
            {
                return property;
            }
            set
            {
                this.property = value.ToLower();
            }
        }





        /// <returns> the important </returns>
        public virtual bool Important
        {
            get
            {
                return important;
            }
            set
            {
                this.important = value;
            }
        }


        public virtual StyleParserCS.css.Declaration_Source Source
        {
            get
            {
                return source;
            }
            set
            {
                this.source = value;
            }
        }


        public override string ToString()
        {
            return this.ToString(0);
        }


        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            // add property
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(property).Append(OutputUtil.PROPERTY_OPENING);

            // add terms
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM);

            // importance flag
            if (important)
            {
                sb.Append(OutputUtil.SPACE_DELIM).Append(OutputUtil.IMPORTANT_KEYWORD);
            }

            sb.Append(OutputUtil.PROPERTY_CLOSING);

            return sb.ToString();
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + (important ? 1231 : 1237);
            result = prime * result + ((string.ReferenceEquals(property, null)) ? 0 : property.GetHashCode());
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
            if (!(obj is DeclarationImpl))
            {
                return false;
            }
            DeclarationImpl other = (DeclarationImpl)obj;
            if (important != other.important)
            {
                return false;
            }
            if (string.ReferenceEquals(property, null))
            {
                if (!string.ReferenceEquals(other.property, null))
                {
                    return false;
                }
            }
            else if (!property.Equals(other.property))
            {
                return false;
            }
            return true;
        }

    }

}