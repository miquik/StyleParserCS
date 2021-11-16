namespace StyleParserCS.csskit.fn
{
    using TermFunction = StyleParserCS.css.TermFunction;

    public class RadialGradientImpl : GenericRadialGradient, StyleParserCS.css.TermFunction_RadialGradient
    {

        public virtual bool Repeating
        {
            get
            {
                return false;
            }
        }

    }
}