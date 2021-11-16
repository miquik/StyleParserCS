using System;
using System.Text;

namespace StyleParserCS.csskit
{
    using StyleParserCS.css;

    /// <summary>
    /// Term
    /// 
    /// @author Karel Piwko, 2008
    /// </summary>
    public class TermImpl<T> : Term<T>
    {

        protected internal T value;
        protected internal Term_Operator operatorv = null;

        protected internal TermImpl()
        {
        }

        public virtual StyleParserCS.css.Term_Operator Operator
        {
            get
            {
                return operatorv;
            }
        }

        public virtual Term<T> setOperator(StyleParserCS.css.Term_Operator operatorv)
        {
            this.operatorv = operatorv;
            return this;
        }

        /// <returns> the value </returns>
        public virtual T Value
        {
            get
            {
                return value;
            }
        }

        /// <param name="value">
        ///            the value to set </param>
        public virtual Term<T> setValue(T value)
        {
            this.value = value;
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
                sb.Append(value.ToString());
            }

            return sb.ToString();
        }

        /* (non-Javadoc)
		 * @see java.lang.Object#hashCode()
		 */
        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((operatorv == null) ? 0 : operatorv.GetHashCode());
            result = prime * result + ((value == null) ? 0 : value.GetHashCode());
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
            if (obj == null)
            {
                return false;
            }
            if (!(obj is TermImpl<T>))
            {
                return false;
            }
            //ORIGINAL LINE: @SuppressWarnings("unchecked") TermImpl<T> other = (TermImpl<T>) obj;
            TermImpl<T> other = (TermImpl<T>)obj;
            if (operatorv == null)
            {
                if (other.operatorv != null)
                {
                    return false;
                }
            }
            else if (!operatorv.Equals(other.operatorv))
            {
                return false;
            }
            if (value == null)
            {
                if (other.value != null)
                {
                    return false;
                }
            }
            else if (!value.Equals(other.value))
            {
                return false;
            }
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unchecked") public StyleParserCS.css.Term<T> shallowClone()
        public virtual Term<T> shallowClone()
        {
            try
            {
                // return (Term<T>)Clone();
                // tocheck!
                return (Term<T>)MemberwiseClone();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        // NONGENERIC INTERFACE IMPLEMENTATION!!!
        object Term.Value => Value;

        public Term setValue(object value)
        {
            return setValue((T)value);
        }

        Term Term.setOperator(Term_Operator op)
        {
            return setOperator(op);
        }

        Term Term.shallowClone()
        {
            return shallowClone();
        }
    }

}