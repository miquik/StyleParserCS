using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace StyleParserCS.csskit
{

    using StyleParserCS.css;
    using System.Collections.ObjectModel;

    public class TermListImpl : Collection<Term>, TermList
    {

        //ORIGINAL LINE: protected java.util.List<StyleParserCS.css.Term<?>> value;
        protected internal IList<Term> value;
        protected internal Term_Operator operatorv;

        protected internal TermListImpl()
        {
            //ORIGINAL LINE: this.value = new java.util.ArrayList<StyleParserCS.css.Term<?>>();
            this.value = new List<Term>();
        }

        protected internal TermListImpl(int initialSize)
        {
            //ORIGINAL LINE: this.value = new java.util.ArrayList<StyleParserCS.css.Term<?>>(initialSize);
            this.value = new List<Term>(initialSize);
        }

        /// <returns> the value </returns>
        //ORIGINAL LINE: public java.util.List<StyleParserCS.css.Term<?>> getValue()
        public virtual IList<Term> Value
        {
            get
            {
                return value;
            }
        }

        /// <param name="value"> the value to set </param>
        public virtual Term<IList<Term>> setValue(IList<Term> value)
        {
            this.value = value;
            return this;
        }
        /// <returns> the operator </returns>
        public virtual Term_Operator Operator
        {
            get
            {
                return operatorv;
            }
        }

        /// <param name="operator"> the operator to set </param>
        public virtual Term<IList<Term>> setOperator(Term_Operator operatorv)
        {
            this.operatorv = operatorv;
            return this;
        }

        /*
        //ORIGINAL LINE: @Override public StyleParserCS.css.Term<?> get(int arg0)
        public virtual Term this[int arg0]
        {
            get
            {
                return value[arg0];
            }
        }

        public virtual void add(int index, Term element)
        {
            value.Insert(index, element);
        }

        //ORIGINAL LINE: @Override public StyleParserCS.css.Term<?> remove(int index)
        public override Term remove(int index)
        {
            return value.RemoveAt(index);
        }

        public virtual int Count
        {
            get
            {
                return value.Count;
            }
        }

        //ORIGINAL LINE: @Override public java.util.Iterator<StyleParserCS.css.Term<?>> iterator()
        public virtual IEnumerator<Term> GetEnumerator()
        {
            return value.GetEnumerator();
        }

        public override bool add(Term o)
        {
            return value.Add(o);
        }
        */
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            // append operator
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            OutputUtil.appendList(sb, value, OutputUtil.SPACE_DELIM);
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
            if (!(obj is TermListImpl))
            {
                return false;
            }
            TermListImpl other = (TermListImpl)obj;
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
            //ORIGINAL LINE: else if (!value.equals(other.value))
            else if (!value.SequenceEqual(other.value))
            {
                return false;
            }
            return true;
        }

        public virtual Term<IList<Term>> shallowClone()
        {
            try
            {
                return (TermList)Clone();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }


        // NONGENERIC INTERFACE IMPLEMENTATION!!!
        object Term.Value => Value;

        public Term setValue(object value)
        {
            return setValue((IList<Term>)value);
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