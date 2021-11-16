using System.Collections.Generic;
using System.Text;
using System.Linq;

/// <summary>
/// KeyframeBlockImpl.java
/// 
/// Created on 31. 1. 2019, 16:25:42 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{

    using Declaration = StyleParserCS.css.Declaration;
    using KeyframeBlock = StyleParserCS.css.KeyframeBlock;
    using TermPercent = StyleParserCS.css.TermPercent;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class KeyframeBlockImpl : AbstractRuleBlock<Declaration>, KeyframeBlock
    {

        private IList<TermPercent> percentages;

        public KeyframeBlockImpl() : base()
        {
            percentages = new List<TermPercent>();
        }

        public KeyframeBlockImpl(IList<TermPercent> percentages) : base()
        {
            percentages = new List<TermPercent>(percentages);
        }

        public virtual IList<TermPercent> Percentages
        {
            get
            {
                return percentages;
            }
        }

        public virtual KeyframeBlock setPercentages(IList<TermPercent> percentages)
        {
            this.percentages = new List<TermPercent>(percentages);
            return this;
        }

        public virtual string ToString(int depth)
        {
            StringBuilder sb = new StringBuilder();

            // append selectors
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb = OutputUtil.appendList(sb, percentages, OutputUtil.SELECTOR_DELIM);

            // append rules (declarations)
            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM, depth + 1);
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.RULE_CLOSING);

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((percentages == null) ? 0 : percentages.GetHashCode());
            return result;
        }

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
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            KeyframeBlockImpl other = (KeyframeBlockImpl)obj;
            if (percentages == null)
            {
                if (other.percentages != null)
                {
                    return false;
                }
            }
            //ORIGINAL LINE: else if (!percentages.equals(other.percentages))
            else if (!percentages.SequenceEqual(other.percentages))
            {
                return false;
            }
            return true;
        }

    }

}