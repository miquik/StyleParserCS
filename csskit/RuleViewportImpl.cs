using System.Text;

/// <summary>
/// RuleViewportImpl.java
/// 
/// Created on 8.2.2013, 15:44:19 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using Declaration = StyleParserCS.css.Declaration;
    using RuleViewport = StyleParserCS.css.RuleViewport;

    /// <summary>
    /// Set of declarations bound with the viewport.
    /// 
    /// @author burgetr
    /// </summary>
    public class RuleViewportImpl : AbstractRuleBlock<Declaration>, RuleViewport
    {

        protected internal RuleViewportImpl() : base()
        {
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public virtual string ToString(int depth)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OutputUtil.VIEWPORT_KEYWORD).Append(OutputUtil.SPACE_DELIM);

            // append declarations
            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM, depth + 1);
            sb.Append(OutputUtil.RULE_CLOSING);

            return sb.ToString();
        }



    }

}