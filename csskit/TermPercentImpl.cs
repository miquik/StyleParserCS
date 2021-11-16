using System.Text;

namespace StyleParserCS.csskit
{
    using TermPercent = StyleParserCS.css.TermPercent;

    /// <summary>
    /// TermPercent
    /// @author burgetr
    /// @author Jan Svercl, VUT Brno, 2008
    /// 			modified by Karel Piwko
    /// @version 1.0 * Construction moved to parser
    /// 				 * Rewritten ToString() method
    /// </summary>
    public class TermPercentImpl : TermFloatValueImpl, TermPercent
    {

        protected internal TermPercentImpl()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            sb.Append(value).Append(OutputUtil.PERCENT_SIGN);

            return sb.ToString();
        }

        public virtual bool Percentage
        {
            get
            {
                return true;
            }
        }

    }

}