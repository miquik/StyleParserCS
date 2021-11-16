using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StyleParserCS.csskit
{

    using Declaration = StyleParserCS.css.Declaration;
    using RuleSet = StyleParserCS.css.RuleSet;
    using CombinedSelector = StyleParserCS.css.CombinedSelector;

    /// <summary>
    /// Basic holder of declarations with CSS selectors
    /// 
    /// @author kapy
    /// </summary>
    public class RuleSetImpl : AbstractRuleBlock<Declaration>, RuleSet
    {

        protected internal CombinedSelector[] selectors;

        protected internal RuleSetImpl() : base()
        {
            this.selectors = new CombinedSelector[0];
        }

        public RuleSetImpl(CombinedSelector[] selectors) : base()
        {
            this.selectors = selectors;
        }

        /// <summary>
        /// Shallow copy constructor </summary>
        /// <param name="rs"> RuleSet to share selectors and declarations with  </param>
        protected internal RuleSetImpl(RuleSet rs) : base()
        {
            this.selectors = rs.getSelectors();
            this.replaceAll(rs.asList());
        }


        /// <returns> the selectors </returns>
        public virtual CombinedSelector[] getSelectors()
        {
            return selectors;
        }

        /// <param name="selectors"> the selectors to set </param>
        /// <returns> Modified instance </returns>
        public virtual RuleSet setSelectors(IList<CombinedSelector> selectors)
        {
            this.selectors = ((List<CombinedSelector>)selectors).ToArray();
            return this;
        }

        public override string ToString()
        {
            return this.ToString(0);
        }


        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            // append selectors
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb = OutputUtil.appendArray(sb, selectors, OutputUtil.SELECTOR_DELIM);

            // append rules (declarations)
            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM, depth + 1);
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.RULE_CLOSING);

            return sb.ToString();
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((selectors == null) ? 0 : selectors.GetHashCode());
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
            if (!(obj is RuleSetImpl))
            {
                return false;
            }
            RuleSetImpl other = (RuleSetImpl)obj;
            if (selectors == null)
            {
                if (other.selectors != null)
                {
                    return false;
                }
            }
            else if (!selectors.Equals(other.selectors))
            {
                return false;
            }
            return true;
        }





    }

}