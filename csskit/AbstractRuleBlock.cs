namespace StyleParserCS.csskit
{
    using StyleParserCS.css;
    using StyleSheet = StyleParserCS.css.StyleSheet;

    public class AbstractRuleBlock<T> : AbstractRule<T>, RuleBlock<T>, RuleBlock
    {

        protected internal StyleSheet stylesheet;

        public virtual StyleSheet StyleSheet
        {
            get
            {
                return stylesheet;
            }
            set
            {
                this.stylesheet = value;
            }
        }


        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result;
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
            //ORIGINAL LINE: if (!(obj instanceof AbstractRuleBlock<?>))
            /*
             * TOCHECK!!
            if (!(obj is AbstractRuleBlock))
            {
                return false;
            }
            */
            return true;
        }


    }

}