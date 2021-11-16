using System;
using System.Text;

namespace StyleParserCS.csskit
{

    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using TermURI = StyleParserCS.css.TermURI;

    /// <summary>
    /// TermURIImpl
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public class TermURIImpl : TermImpl<string>, TermURI
    {

        protected internal Uri basev;

        protected internal TermURIImpl()
        {
        }

        public override TermURI setValue(string uri)
        {
            if (string.ReferenceEquals(uri, null))
            {
                throw new System.ArgumentException("Invalid uri for TermURI(null)");
            }

            /* this shlould be done by parser
			uri = uri.replaceAll("^Uri\\(", "")
				  .replaceAll("\\)$", "")
				  .replaceAll("^'", "")
				  .replaceAll("^\"", "")
				  .replaceAll("'$", "")
				  .replaceAll("\"$", "");
			*/

            this.value = uri;
            return this;
        }

        public virtual TermURI setBase(Uri basev)
        {
            this.basev = basev;
            return this;
        }

        public virtual Uri Base
        {
            get
            {
                return basev;
            }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            sb.Append(OutputUtil.Uri_OPENING).Append(CssEscape.escapeCssString(value)).Append(OutputUtil.Uri_CLOSING);

            return sb.ToString();
        }
    }

}