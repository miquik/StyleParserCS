using System;
using System.Text;
using System.Text.RegularExpressions;

namespace StyleParserCS.csskit
{
    using RuleImport = StyleParserCS.css.RuleImport;

    /// <summary>
    /// URI with style sheet to be imported.
    /// Since this is never used in new parser where imports 
    /// are directly included, this is obsolete
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    [Obsolete]
    public class RuleImportImpl : AbstractRuleBlock<string>, RuleImport
    {

        /// <summary>
        /// URI of file to be imported </summary>
        protected internal string uri;

        /// <summary>
        /// Creates empty RuleImport instance
        /// </summary>
        protected internal RuleImportImpl() : base()
        {
            this.uri = "";
        }

        public virtual string URI
        {
            get
            {
                return uri;
            }
        }

        public virtual RuleImport setURI(string uri)
        {

            // sanity check
            if (string.ReferenceEquals(uri, null))
            {
                return this;
            }
            string temp = Regex.Replace(uri, "^Uri\\(", "");
            temp = Regex.Replace(temp, "\\)$", "");
            temp = Regex.Replace(temp, "^'", "");
            temp = Regex.Replace(temp, "^\"", "");
            temp = Regex.Replace(temp, "'$", "");
            this.uri = Regex.Replace(temp, "\"$", "");

            // this.uri = uri.replaceAll("^Uri\\(", "").replaceAll("\\)$", "").replaceAll("^'", "").replaceAll("^\"", "").replaceAll("'$", "").replaceAll("\"$", "");
            return this;
        }

        public virtual string ToString(int depth)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(OutputUtil.IMPORT_KEYWORD).Append(OutputUtil.Uri_OPENING).Append(uri).Append(OutputUtil.Uri_CLOSING);

            // append medias
            if (Count != 0)
            {
                sb.Append(OutputUtil.SPACE_DELIM);
            }
            sb = OutputUtil.appendList(sb, this, OutputUtil.MEDIA_DELIM);
            sb.Append(OutputUtil.LINE_CLOSING);

            return sb.ToString();

        }

        public override string ToString()
        {
            return ToString(0);
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((string.ReferenceEquals(uri, null)) ? 0 : uri.GetHashCode());
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
            if (!(obj is RuleImportImpl))
            {
                return false;
            }
            RuleImportImpl other = (RuleImportImpl)obj;
            if (string.ReferenceEquals(uri, null))
            {
                if (!string.ReferenceEquals(other.uri, null))
                {
                    return false;
                }
            }
            else if (!uri.Equals(other.uri))
            {
                return false;
            }
            return true;
        }

    }

}