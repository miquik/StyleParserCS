namespace StyleParserCS.csskit.fn
{
    using TermFunction = StyleParserCS.css.TermFunction;

    public class RepeatingLinearGradientImpl : GenericLinearGradient, StyleParserCS.css.TermFunction_LinearGradient
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