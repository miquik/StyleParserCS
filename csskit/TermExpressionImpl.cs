using System.Text;

namespace StyleParserCS.csskit
{
    using TermExpression = StyleParserCS.css.TermExpression;

    /// <summary>
    /// TermExpression
    /// 
    /// @author Radek Burget, 2009
    /// </summary>
    public class TermExpressionImpl : TermImpl<string>, TermExpression
    {

        protected internal TermExpressionImpl()
        {
        }

        public virtual TermExpression setValue(string value)
        {
            if (string.ReferenceEquals(value, null))
            {
                throw new System.ArgumentException("Invalid value for TermExpression(null)");
            }
            this.value = value;
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            sb.Append("expression(").Append(value).Append(")");
            return sb.ToString();
        }
    }

}