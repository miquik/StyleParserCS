using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;

namespace StyleParserCS.css
{
    public interface Term
    {
        Term_Operator Operator { get; }

        object Value { get; }
        Term setValue(object value);

        Term setOperator(Term_Operator op);
        Term shallowClone();
    }


    public interface Term<T> : ICloneable, Term
    {
        /// <summary>
        /// This operator is between terms in value part of CSS declaration.
        /// Typically, indistinguishable values of are shorthanded by SLASH, alternatives are 
        /// divides by COMMA and SPACE when multi-values are used 
        /// 
        /// <para>
        /// Currently, operators are only syntactic sugar, because they are not used during
        /// parsing values in current implementation
        /// </para>
        /// 
        /// @author kapy
        /// 
        /// </summary>

        /// <summary>
        /// Getter for value </summary>
        /// <returns> the value of the term </returns>
        new T Value { get; }

        /// <summary>
        /// Setter for value </summary>
        /// <param name="value"> </param>
        /// <returns> Modified object to allow chaining </returns>
        Term<T> setValue(T value);

        /// <summary>
        /// Operator between two terms. The first term is having <code>null</code> </summary>
        /// <returns> the operator </returns>
        // Term_Operator Operator { get;  }

        /// <summary>
        /// Sets operator </summary>
        /// <param name="operator"> </param>
        /// <returns> Modified object to allow chaining </returns>
        new Term<T> setOperator(Term_Operator op);

        new Term<T> shallowClone();
    }


    public sealed class Term_Operator : SmartEnum<Term_Operator, string>
    {
        public static readonly Term_Operator SPACE = new Term_Operator("SPACE", " ");
        public static readonly Term_Operator SLASH = new Term_Operator("SLASH", "/");
        public static readonly Term_Operator COMMA = new Term_Operator("COMMA", ", ");
        internal Term_Operator(string name, string value) : base(name, value)
        {
        }

        public string value()
        {
            return Name;
        }

        public override string ToString()
        {
            return Value;
        }
    }

}