using System.Text;

/// <summary>
/// MediaQueryImpl.java
/// 
/// Created on 26. 6. 2014, 15:43:53 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using MediaExpression = StyleParserCS.css.MediaExpression;
    using MediaQuery = StyleParserCS.css.MediaQuery;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class MediaQueryImpl : AbstractRule<MediaExpression>, MediaQuery
    {
        protected internal bool negative;
        protected internal string type;

        public MediaQueryImpl()
        {
            negative = false;
            type = null;
        }

        public MediaQueryImpl(string type, bool negative)
        {
            this.negative = negative;
            this.type = type.Trim().ToLower(); // TOCHECK Locale.ENGLISH);
        }

        public virtual bool Negative
        {
            get
            {
                return negative;
            }
            set
            {
                this.negative = value;
            }
        }


        public virtual string Type
        {
            get
            {
                return type;
            }
            set
            {
                this.type = value;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (Negative)
            {
                sb.Append("NOT ");
            }

            if (!string.ReferenceEquals(Type, null))
            {
                sb.Append(CssEscape.escapeCssIdentifier(Type));                
                if (Count > 0)
                {
                    sb.Append(OutputUtil.QUERY_DELIM);
                }
            }

            sb = OutputUtil.appendList(sb, this, OutputUtil.QUERY_DELIM);

            return sb.ToString();
        }

    }

}