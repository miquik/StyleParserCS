namespace StyleParserCS.csskit
{
    using TermLength = StyleParserCS.css.TermLength;

    public class TermLengthImpl : TermFloatValueImpl, TermLength
    {

        protected internal TermLengthImpl()
        {
        }

        public virtual bool Percentage
        {
            get
            {
                return false;
            }
        }

    }

}