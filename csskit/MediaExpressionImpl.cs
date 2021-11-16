using System.Text;

/// <summary>
/// MediaExpresionImpl.java
/// 
/// Created on 26. 6. 2014, 15:55:01 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using CssEscape = StyleParserCS.unbescape.CssEscape;

    using MediaExpression = StyleParserCS.css.MediaExpression;
    using StyleParserCS.css;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class MediaExpressionImpl : AbstractRule<Term>, MediaExpression
    {
        protected internal string feature;

        public virtual string Feature
        {
            get
            {
                return feature;
            }
            set
            {
                this.feature = value.Trim().ToLower(); // TOCHECK Locale.ENGLISH);
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OutputUtil.MEDIA_EXPR_OPENING);
            sb.Append(CssEscape.escapeCssIdentifier(Feature)).Append(OutputUtil.MEDIA_FEATURE_DELIM);
            sb = OutputUtil.appendList(sb, this, OutputUtil.SPACE_DELIM);
            sb.Append(OutputUtil.MEDIA_EXPR_CLOSING);

            return sb.ToString();
        }

    }

}