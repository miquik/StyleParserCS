using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StyleParserCS.css;
using System.Collections.ObjectModel;

/// <summary>
/// TermBracketedIdentImpl.java
/// 
/// Created on 30. 11. 2018, 10:28:47 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermBracketedIdentsImpl : Collection<TermIdent>, TermBracketedIdents
    {

        protected internal IList<TermIdent> value;
        protected internal Term_Operator operatorv;

        protected internal TermBracketedIdentsImpl()
        {
            this.value = new List<TermIdent>();
        }

        protected internal TermBracketedIdentsImpl(int initialSize)
        {
            this.value = new List<TermIdent>(initialSize);
        }

        /// <returns> the value </returns>
        public virtual IList<TermIdent> Value
        {
            get
            {
                return value;
            }
        }

        /// <param name="value"> the value to set </param>
        public virtual Term<IList<TermIdent>> setValue(IList<TermIdent> value)
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
        public virtual Term<IList<TermIdent>> setOperator(Term_Operator operatorv)
        {
            this.operatorv = operatorv;
            return this;
        }

        /*
        public virtual TermIdent this[int arg0]
        {
            get
            {
                return value[arg0];
            }
        }

        public virtual void Insert(int index, TermIdent element)
        {
            value.Insert(index, element);
        }

        public override TermIdent remove(int index)
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

        public virtual IEnumerator<TermIdent> GetEnumerator()
        {
            return value.GetEnumerator();
        }
        
        public override bool add(TermIdent o)
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
            sb.Append('[');
            OutputUtil.appendList(sb, value, OutputUtil.SPACE_DELIM);
            sb.Append(']');
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
            if (!(obj is TermBracketedIdentsImpl))
            {
                return false;
            }
            TermBracketedIdentsImpl other = (TermBracketedIdentsImpl)obj;
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

        public virtual Term<IList<TermIdent>> shallowClone()
        {
            try
            {
                // return (TermBracketedIdents)Clone();
                // tocheck!
                return (TermBracketedIdents)MemberwiseClone();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public object Clone()
        {
            return null;
            // TODO throw new NotImplementedException();
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