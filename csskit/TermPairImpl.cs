using System.Text;

namespace StyleParserCS.csskit
{
    using StyleParserCS.css;

    public class TermPairImpl<K, V> : TermImpl<V>, TermPair<K, V>
    {

        protected internal K key;

        protected internal TermPairImpl()
        {
        }

        public virtual K Key
        {
            get
            {
                return key;
            }
        }

        public virtual TermPair<K, V> setKey(K key)
        {
            this.key = key;
            return this;
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }
            if (key != null)
            {
                sb.Append(key);
            }
            if (value != null)
            {
                sb.Append(OutputUtil.SPACE_DELIM).Append(value);
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
            result = prime * result + ((key == null) ? 0 : key.GetHashCode());
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
            //ORIGINAL LINE: if (!(obj instanceof TermPairImpl<?, ?>))
            // TOCHECK: type check???
            // if (!(obj is TermPairImpl < object, ?>))
            // {
            //     return false;
            // }
            //ORIGINAL LINE: @SuppressWarnings("unchecked") TermPairImpl<K,V> other = (TermPairImpl<K,V>) obj;
            TermPairImpl<K, V> other = (TermPairImpl<K, V>)obj;
            if (key == null)
            {
                if (other.key != null)
                {
                    return false;
                }
            }
            else if (!key.Equals(other.key))
            {
                return false;
            }
            return true;
        }



    }

}