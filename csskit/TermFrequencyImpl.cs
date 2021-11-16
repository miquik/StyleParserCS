namespace StyleParserCS.csskit
{
    using TermFrequency = StyleParserCS.css.TermFrequency;

    public class TermFrequencyImpl : TermFloatValueImpl, TermFrequency
    {

        protected internal TermFrequencyImpl()
        {
        }

        public override TermFrequency setValue(float value)
        {
            // value is negative
            // if ((new float?(0.0f)).compareTo(value) > 0)
            if (value < 0)
            {
                throw new System.ArgumentException("Null or negative value for CSS time");
            }
            this.value = value;
            return this;
        }

    }

}