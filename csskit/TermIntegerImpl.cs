using System;

namespace StyleParserCS.csskit
{
    using TermInteger = StyleParserCS.css.TermInteger;

    public class TermIntegerImpl : TermLengthImpl, TermInteger
    {

        protected internal TermIntegerImpl()
        {
            setUnit(StyleParserCS.css.TermLength_Unit.none);
        }

        public virtual int IntValue
        {
            get
            {
                return (int)Value;
            }
        }

        public virtual TermInteger setValue(int value)
        {
            setValue(Convert.ToSingle(value));
            return this;
        }

        public override string ToString()
        {
            if (operatorv != null)
            {
                return operatorv.value() + IntValue;
            }
            return IntValue.ToString();
        }

    }

}