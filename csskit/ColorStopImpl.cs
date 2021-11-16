namespace StyleParserCS.csskit
{
    using TermColor = StyleParserCS.css.TermColor;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;

    public class ColorStopImpl : StyleParserCS.css.TermFunction_Gradient_ColorStop
    {
        private TermColor color;
        private TermLengthOrPercent length;

        public ColorStopImpl(TermColor color, TermLengthOrPercent length)
        {
            this.color = color;
            this.length = length;
        }

        public virtual TermColor Color
        {
            get
            {
                return color;
            }
        }

        public virtual TermLengthOrPercent Length
        {
            get
            {
                return length;
            }
        }
    }
}