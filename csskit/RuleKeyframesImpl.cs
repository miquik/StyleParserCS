using System.Text;

/// <summary>
/// RuleKeyframesImpl.java
/// 
/// Created on 31. 1. 2019, 16:37:57 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using KeyframeBlock = StyleParserCS.css.KeyframeBlock;
    using RuleKeyframes = StyleParserCS.css.RuleKeyframes;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class RuleKeyframesImpl : AbstractRuleBlock<KeyframeBlock>, RuleKeyframes
    {

        private string name;

        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        public virtual RuleKeyframes setName(string name)
        {
            this.name = name;
            return this;
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            // append medias
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.KEYFRAMES_KEYWORD);
            sb.Append(name);

            // append rules
            sb = OutputUtil.appendTimes(sb, OutputUtil.DEPTH_DELIM, depth);
            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.RULE_DELIM, depth + 1);
            sb.Append(OutputUtil.RULE_CLOSING);

            return sb.ToString();
        }
    }

}