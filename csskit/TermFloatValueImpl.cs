namespace StyleParserCS.csskit
{
    using StyleParserCS.css;
    using TermFloatValue = StyleParserCS.css.TermFloatValue;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermFloatValueImpl : TermNumericImpl<float>, TermFloatValue
    {

        public override TermNumeric<float> setZero()
        {
            base.setValue(0.0f);
            return this;
        }

        public override Term<float> setValue(float value)
        {
            if (value == -0.0f) //avoid negative zeroes in CSS
            {
                return base.setValue(0.0f);
            }
            else
            {
                return base.setValue(value);
            }
        }

    }

}