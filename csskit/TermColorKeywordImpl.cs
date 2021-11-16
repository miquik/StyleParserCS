/// <summary>
/// TermColorKeywordImpl.java
/// Created on 25.12.2016 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using TermColor = StyleParserCS.css.TermColor;

    /// <summary>
    /// A TermColor implementation that represents a color created using a special
    /// keyword.
    /// @author Radek Burget
    /// </summary>
    public class TermColorKeywordImpl : TermImpl<Color>, TermColor
    {

        private StyleParserCS.css.TermColor_Keyword keyword;

        protected internal TermColorKeywordImpl(StyleParserCS.css.TermColor_Keyword keyword, int r, int g, int b, int a)
        {
            this.keyword = keyword;
            this.value = new Color(r, g, b, a);
        }

        protected internal TermColorKeywordImpl(StyleParserCS.css.TermColor_Keyword keyword, Color value)
        {
            this.keyword = keyword;
            this.value = value;
        }

        public virtual StyleParserCS.css.TermColor_Keyword Keyword
        {
            get
            {
                return keyword;
            }
        }

        public virtual bool Transparent
        {
            get
            {
                return (keyword == StyleParserCS.css.TermColor_Keyword.TRANSPARENT) || (value.Alpha == 0);
            }
        }

        public override string ToString()
        {
            if (operatorv != null)
            {
                return operatorv.value() + keyword.ToString();
            }
            else
            {
                return keyword.ToString();
            }
        }

    }

}