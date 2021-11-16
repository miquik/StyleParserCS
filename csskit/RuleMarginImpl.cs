using System.Text;

namespace StyleParserCS.csskit
{
    using Declaration = StyleParserCS.css.Declaration;
    using RuleMargin = StyleParserCS.css.RuleMargin;

    /// <summary>
    /// Implementation of RuleMargin
    /// 
    /// @author Bert Frees, 2012
    /// </summary>
    public class RuleMarginImpl : AbstractRuleBlock<Declaration>, RuleMargin
    {

        private StyleParserCS.css.RuleMargin_MarginArea marginArea;

        protected internal RuleMarginImpl(string area)
        {
            foreach (StyleParserCS.css.RuleMargin_MarginArea a in StyleParserCS.css.RuleMargin_MarginArea.List)
            {
                if (a.Value.Equals(area))
                {
                    marginArea = a;
                    break;
                }
            }
            if (marginArea == null)
            {
                throw new System.ArgumentException("Illegal value for margin area: " + area);
            }
        }

        public virtual StyleParserCS.css.RuleMargin_MarginArea MarginArea
        {
            get
            {
                return marginArea;
            }
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.MARGIN_AREA_OPENING).Append(marginArea.Value);

            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.RULE_DELIM, depth + 1);
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
            result = prime * result + ((marginArea == null) ? 0 : marginArea.GetHashCode());
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
            if (!(obj is RuleMarginImpl))
            {
                return false;
            }
            RuleMarginImpl other = (RuleMarginImpl)obj;
            if (marginArea == null)
            {
                if (other.marginArea != null)
                {
                    return false;
                }
            }
            else if (!marginArea.Equals(other.marginArea))
            {
                return false;
            }
            return true;
        }
    }

}