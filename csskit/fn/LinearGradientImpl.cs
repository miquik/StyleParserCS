namespace StyleParserCS.csskit.fn
{
    using TermFunction = StyleParserCS.css.TermFunction;

    public class LinearGradientImpl : GenericLinearGradient, StyleParserCS.css.TermFunction_LinearGradient
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