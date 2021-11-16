using System.Collections.Generic;
using System.Linq;

namespace StyleParserCS.csskit
{

    using StyleParserCS.css;
    using System.Collections;
    using System.Collections.ObjectModel;


    public class AbstractRule<T> : Collection<T>, Rule<T>, Rule
    {
        protected internal int hash = 0;

        public IList<T> asList()
        {
            return Items;
        }
        public virtual Rule<T> replaceAll(IList<T> replacement)
        {
            hash = 0;
            Clear();
            foreach (T item in replacement)
            {
                Add(item);
            }
            return this;
        }
        public Rule<T> unlock()
        {
            hash = 0;
            Clear();
            return this;
        }


        // NONGENERIC INTERFACE IMPLEMENTATION!!!
        public Rule replaceAll(IList replacement)
        {
            return replaceAll((IList<T>)replacement);
        }
        Rule Rule.unlock()
        {
            return unlock();
        }
        IList Rule.asList()
        {
            return (IList)asList();
        }

        public override int GetHashCode()
        {
            if (hash == 0)
            {
                const int prime = 31;
                int result = base.GetHashCode();
                result = prime * result + GetHashCode();
                hash = result;
            }
            return hash;
        }
    }


    /*
    public class AbstractRule<T> : Collection<T>, Rule<T>, Rule
    {

        // protected internal IList<T> list = null;
        protected internal int hash = 0;

        public virtual IList<T> asList()
        {
            return this.ToList();
        }

        public virtual Rule<T> replaceAll(IList<T> replacement)
        {
            hash = 0;
            Clear();
            foreach(T item in replacement)
            {
                Add(item);
            }
            // this.list = replacement;
            return this;
        }

        public virtual Rule<T> unlock()
        {
            hash = 0;
            Clear();
            return this;
        }

        public virtual T set(int index, T element)
        {
            hash = 0;
            this[index] = element;
            return this[index];
        }

        public virtual bool add(T element)
        {
            hash = 0;
            Add(element);
            return true;
            // Insert(index, element);
            // list.add(index, element);
        }

        public virtual void insert(int index, T element)
        {
            hash = 0;
            Insert(index, element);
            // list.add(index, element);
        }

        public override int GetHashCode()
        {
            if (hash == 0)
            {
                const int prime = 31;
                int result = base.GetHashCode();
                result = prime * result + GetHashCode();
                hash = result;
            }
            return hash;
        }

        // NONGENERIC INTERFACE IMPLEMENTATION!!!
        public Rule replaceAll(IList replacement)
        {
            return replaceAll((IList<T>)replacement);
        }

        Rule Rule.unlock()
        {
            return unlock();
        }

        IList Rule.asList()
        {
            return (IList)asList();
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            //ORIGINAL LINE: if (!super.equals(obj))
            if (!base.SequenceEqual(obj))
            {
                return false;
            }
            //ORIGINAL LINE: if (!(obj instanceof AbstractRule<?>))
            if (!(obj is AbstractRule))
            {
                return false;
            }
            //ORIGINAL LINE: AbstractRule<?> other = (AbstractRule<?>) obj;
            AbstractRule other = (AbstractRule)obj;
            if (list == null)
            {
                if (other.list != null)
                {
                    return false;
                }
            }
            //ORIGINAL LINE: else if (!list.equals(other.list))
            else if (!list.SequenceEqual(other.list))
            {
                return false;
            }
            return true;
        }
    }
    */
}