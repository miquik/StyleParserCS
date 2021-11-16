namespace StyleParserCS.csskit.fn
{
    using TermFunction = StyleParserCS.css.TermFunction;

    public class RepeatingRadialGradientImpl : GenericRadialGradient, StyleParserCS.css.TermFunction_RadialGradient
    {

        public virtual bool Repeating
        {
            get
            {
                return true;
            }
        }


    }
}