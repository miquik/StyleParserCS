using System.Text;

namespace StyleParserCS.csskit
{
    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using TermString = StyleParserCS.css.TermString;

    /// <summary>
    /// TermString
    /// 
    /// @author Jan Svercl, VUT Brno, 2008
    /// 			modified by Karel Piwko, 2008
    /// </summary>
    public class TermStringImpl : TermImpl<string>, TermString
    {

        protected internal TermStringImpl()
        {
        }

        public virtual TermString setValue(string value)
        {
            if (string.ReferenceEquals(value, null))
            {
                throw new System.ArgumentException("Invalid value for TermString(null)");
            }
            /* This should be done by parser
			value = value.replaceAll("^'", "")
				.replaceAll("^\"", "")
				.replaceAll("'$", "")
				.replaceAll("\"$", "");
			*/
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
            sb.Append(OutputUtil.STRING_OPENING).Append(CssEscape.escapeCssString(value)).Append(OutputUtil.STRING_CLOSING);

            return sb.ToString();
        }
    }

}