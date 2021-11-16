using System.Text;

namespace StyleParserCS.csskit
{
    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using TermIdent = StyleParserCS.css.TermIdent;

    /// <summary>
    /// TermIdent
    /// @author Jan Svercl, VUT Brno, 2008
    /// 			modified by Karel Piwko, 2008
    /// </summary>
    public class TermIdentImpl : TermImpl<string>, TermIdent
    {

        protected internal TermIdentImpl()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            if (!string.ReferenceEquals(value, null))
            {
                sb.Append(CssEscape.escapeCssIdentifier(value));
            }

            return sb.ToString();
        }

    }

}