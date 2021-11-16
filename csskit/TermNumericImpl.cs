using System.Text;

namespace StyleParserCS.csskit
{
    using StyleParserCS.css;
    using System;

    public abstract class TermNumericImpl<T> : TermImpl<T>, TermNumeric<T>
    {
        public abstract TermNumeric<T> setZero();

        protected internal StyleParserCS.css.TermLength_Unit unit;

        /// <returns> the unit </returns>
        public virtual StyleParserCS.css.TermLength_Unit Unit
        {
            get
            {
                return unit;
            }
        }

        /// <param name="unit"> the unit to set </param>
        public virtual TermNumeric<T> setUnit(StyleParserCS.css.TermLength_Unit unit)
        {
            this.unit = unit;
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            if (value != null)
            {
                // TOCHECK: type check!!!
                double doubleValue = Convert.ToDouble(value);
                double intValue = (double)Convert.ToInt32(value);
                if (intValue == doubleValue)
                {
                    sb.Append(intValue);
                }
                else
                {
                    sb.Append(value);
                }
            }
            if (unit != null)
            {
                sb.Append(unit.value());
            }
            return sb.ToString();
        }


        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = base.GetHashCode();
            result = prime * result + ((unit == null) ? 0 : unit.GetHashCode());
            return result;
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#equals(java.lang.Object)
		 */
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!base.Equals(obj))
            {
                return false;
            }
            //ORIGINAL LINE: if (!(obj instanceof TermNumericImpl<?>))
            // TOCHECK!!!!
            if (!(obj is TermNumericImpl<object>))
            {
                return false;
            }
            //ORIGINAL LINE: @SuppressWarnings("unchecked") TermNumericImpl<T> other = (TermNumericImpl<T>) obj;
            TermNumericImpl<T> other = (TermNumericImpl<T>)obj;
            if (unit == null)
            {
                if (other.unit != null)
                {
                    return false;
                }
            }
            else if (!unit.Equals(other.unit))
            {
                return false;
            }
            return true;
        }

        // NONGENERIC INTERFACE IMPLEMENTATION!!!
        TermNumeric TermNumeric.setUnit(TermLength_Unit unit)
        {
            return setUnit(unit);
        }

        TermNumeric TermNumeric.setZero()
        {
            return setZero();
        }
    }

}