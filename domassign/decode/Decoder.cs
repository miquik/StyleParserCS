using System;
using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using Declaration = StyleParserCS.css.Declaration;
    using RuleFactory = StyleParserCS.css.RuleFactory;
    using StyleParserCS.css;
    using Term_Operator = StyleParserCS.css.Term_Operator;
    using TermColor = StyleParserCS.css.TermColor;
    using TermFactory = StyleParserCS.css.TermFactory;
    using TermFloatValue = StyleParserCS.css.TermFloatValue;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermPercent = StyleParserCS.css.TermPercent;
    using TermTime = StyleParserCS.css.TermTime;

    /// <summary>
    /// A base class for repeaters and variators.
    /// 
    /// @author burgetr
    /// </summary>
    public class Decoder
    {

        /// <summary>
        /// A hint about the allowed value range when processing numeric values. 
        /// </summary>
        public enum ValueRange
        {
            /// <summary>
            /// Allow all values </summary>
            ALLOW_ALL,
            /// <summary>
            /// Treat negative values as invalid </summary>
            DISALLOW_NEGATIVE,
            /// <summary>
            /// Truncate negative values to zero </summary>
            TRUNCATE_NEGATIVE,
            /// <summary>
            /// Treat zero as invalid </summary>
            DISALLOW_ZERO
        }

        /// <summary>
        /// Inherit acceptance flags
        /// </summary>
        public const bool AVOID_INH = true;
        public const bool ALLOW_INH = false;

        public static readonly RuleFactory rf = CSSFactory.RuleFactory;
        public static readonly TermFactory tf = CSSFactory.TermFactory;


        /// <summary>
        /// Converts TermIdent into CSSProperty using intersection set.
        /// ICssValue is used.
        /// </summary>
        /// @param <T>
        ///            Subclass of CSSProperty to be returned </param>
        /// <param name="type">
        ///            Class of property to be used to retrieve value </param>
        /// <param name="intersection">
        ///            Intersection set or <code>null</code> if no intersection is
        ///            used </param>
        /// <param name="term">
        ///            TermIdent to be transferred to property </param>
        /// <returns> CSSProperty of type &lt;T&gt; or <code>null</code> </returns>
        public static T genericPropertyRaw<T>(Type type, ISet<T> intersection, TermIdent term) where T : StyleParserCS.css.CSSProperty
        {
            try
            {
                string name = term.Value.Replace("-", "_").ToUpper();
                // TOCHECK: type check
                T property = (T)StyleParserCS.css.CSSProperty_Translator.valueOf(type, name);
                if (intersection != null && intersection.Contains(property))
                {
                    return property;
                }
                return property;
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public static CSSProperty genericPropertyRaw(Type type, ISet<CSSProperty> intersection, TermIdent term)
        {
            try
            {
                string name = term.Value.Replace("-", "_").ToUpper();
                // TOCHECK: type check
                CSSProperty property = CSSProperty_Translator.valueOf(type, name);
                if (intersection != null && intersection.Contains(property))
                {
                    return property;
                }
                return property;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts TermIdent into value of enum of given class and stores it into
        /// properties map under key property
        /// </summary>
        /// @param <T>
        ///            Enum &amp; CSSProperty limitation </param>
        /// <param name="type">
        ///            Type of enum which instance is retrieved </param>
        /// <param name="term">
        ///            Term with value to be converted </param>
        /// <param name="avoidInherit">
        ///            If <code>true</code> inherit value is not considered valid </param>
        /// <param name="properties">
        ///            Properties map where to store value </param>
        /// <param name="propertyName">
        ///            Name under which property is stored in map </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        public static bool genericProperty<T>(Type type, TermIdent term, bool avoidInherit, IDictionary<string, CSSProperty> properties, string propertyName) where T : StyleParserCS.css.CSSProperty
        {
            T property = genericPropertyRaw<T>(type, null, term);
            if (property == null || (avoidInherit && property.equalsInherit()))
            {
                return false;
            }

            properties[propertyName] = property;
            return true;
        }

        public static bool genericProperty(Type type, TermIdent term, bool avoidInherit, IDictionary<string, CSSProperty> properties, string propertyName)
        {
            CSSProperty property = genericPropertyRaw(type, null, term);
            if (property == null || (avoidInherit && property.equalsInherit()))
            {
                return false;
            }

            properties[propertyName] = property;
            return true;
        }

        /// <summary>
        /// Converts TermIdent into value of CSSProperty for given class
        /// 
        /// </summary>
        public static bool genericTermIdent<T>(Type type, Term term, bool avoidInherit, string propertyName, IDictionary<string, CSSProperty> properties) where T : StyleParserCS.css.CSSProperty
        {
            if (term is TermIdent)
            {
                return genericProperty<T>(type, (TermIdent)term, avoidInherit, properties, propertyName);
            }
            return false;
        }

        public static bool genericTermIdent(Type type, Term term, bool avoidInherit, string propertyName, IDictionary<string, CSSProperty> properties)
        {
            if (term is TermIdent)
            {
                return genericProperty(type, (TermIdent)term, avoidInherit, properties, propertyName);
            }
            return false;
        }

        /// <summary>
        /// Converts term into Color and stored values and types in maps
        /// </summary>
        /// @param <T>
        ///            CSSProperty </param>
        /// <param name="term">
        ///            Term to be parsed </param>
        /// <param name="propertyName">
        ///            How to store colorIdentificiton </param>
        /// <param name="colorIdentification">
        ///            What to store under propertyName </param>
        /// <param name="properties">
        ///            Map to store property types </param>
        /// <param name="values">
        ///            Map to store property values </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        public static bool genericTermColor<T>(Term term, string propertyName, T colorIdentification, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {
            if (term is TermColor)
            {
                properties[propertyName] = colorIdentification;
                values[propertyName] = term;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts term into TermLength and stores values and types in maps
        /// </summary>
        /// @param <T>
        ///            CSSProperty </param>
        /// <param name="term">
        ///            Term to be parsed </param>
        /// <param name="propertyName">
        ///            How to store colorIdentificiton </param>
        /// <param name="lengthIdentification">
        ///            What to store under propertyName </param>
        /// <param name="properties">
        ///            Map to store property types </param>
        /// <param name="values">
        ///            Map to store property values </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        public static bool genericTermLength<T>(Term term, string propertyName, T lengthIdentification, ValueRange range, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {
            if (term is TermInteger && ((TermInteger)term).Unit.Equals(TermLength_Unit.none))
            {
                if (CSSFactory.ImplyPixelLength || ((TermInteger)term).Value == 0)
                { //0 is always allowed with no units
                  // convert to length with units of px
                    TermLength tl = tf.createLength(((TermInteger)term).Value, TermLength_Unit.px);
                    return genericTerm<T>(typeof(TermLength), (Term)tl, propertyName, lengthIdentification, range, properties, values);
                }
                else
                {
                    return false;
                }
            }
            else if (term is TermLength)
            {
                return genericTerm<T>(typeof(TermLength), term, propertyName, lengthIdentification, range, properties, values);
            }

            return false;
        }

        /// <summary>
        /// Check whether given declaration contains one term of given type. It is
        /// able to check even whether is above zero for numeric values
        /// </summary>
        /// @param <T>
        ///            Class of CSSProperty to be used for result </param>
        /// <param name="termType">
        ///            Supposed type of term </param>
        /// <param name="term">
        ///            Term of which is supposed to be of type <code>termType</code>,
        ///            that is input data </param>
        /// <param name="propertyName">
        ///            Name under which property's value and type is stored in maps </param>
        /// <param name="typeIdentification">
        ///            How this type of term is described in type T </param>
        /// <param name="sanify">
        ///            Check if value is positive </param>
        /// <param name="properties">
        ///            Where to store property type </param>
        /// <param name="values">
        ///            Where to store property value </param>
        /// <returns> <code>true</code> if succeeded in recognition, <code>false</code>
        ///         otherwise </returns>
        public static bool genericTerm<T>(Type termType, Term term, string propertyName, T typeIdentification, ValueRange range, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            // check type
            if (termType.IsInstanceOfType(term))
            {
                // sanity check
                if (range != ValueRange.ALLOW_ALL)
                {
                    // check for integer
                    // TOCHECK!!!
                    if (term is Term<int>)
                    {
                        //ORIGINAL LINE: final System.Nullable<int> zero = 0;
                        // TOCHECK
                        int zero = 0;
                        int result = zero.CompareTo(((Term<int>)term).Value);
                        if (result > 0)
                        {
                            // return false is also possibility
                            // but we will change to zero
                            if (range == ValueRange.TRUNCATE_NEGATIVE)
                            {
                                ((TermInteger)term).setZero();
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (result == 0)
                        {
                            if (range == ValueRange.DISALLOW_ZERO)
                            {
                                return false;
                            }
                        }
                    }
                    // check for float
                    else if (term is Term<float>)
                    {
                        //ORIGINAL LINE: final System.Nullable<float> zero = 0f;
                        // TOCHECK
                        float zero = 0f;
                        int result = zero.CompareTo(((Term<float>)term).Value);
                        if (result > 0)
                        {
                            // return false is also possibility
                            // but we will change to zero
                            if (range == ValueRange.TRUNCATE_NEGATIVE)
                            {
                                ((TermFloatValue)term).setZero();
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (result == 0)
                        {
                            if (range == ValueRange.DISALLOW_ZERO)
                            {
                                return false;
                            }
                        }
                    }
                }
                // passed both type check and range check,
                // store
                properties[propertyName] = typeIdentification;
                values[propertyName] = term;
                return true;

            }
            return false;

        }

        public static bool genericOneIdent(Type type, Declaration d, IDictionary<string, CSSProperty> properties)
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent(type, d[0], ALLOW_INH, d.Property, properties);
        }

        /// <summary>
        /// Processes declaration which is supposed to contain one identification
        /// term
        /// </summary>
        /// @param <T>
        ///            Type of CSSProperty </param>
        /// <param name="type">
        ///            Class of CSSProperty to be stored </param>
        /// <param name="d">
        ///            Declaration to be parsed </param>
        /// <param name="properties">
        ///            Properties map where to store enum </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         elsewhere </returns>
        public static bool genericOneIdent<T>(Type type, Declaration d, IDictionary<string, CSSProperty> properties) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent<T>(type, d[0], ALLOW_INH, d.Property, properties);
        }

        /// <summary>
        /// Processes declaration which is supposed to contain one identification
        /// term or one TermColor
        /// </summary>
        /// @param <T>
        ///            Type of CSSProperty </param>
        /// <param name="type">
        ///            Class of enum to be stored </param>
        /// <param name="colorIdentification">
        ///            Instance of CSSProperty stored into properties to indicate
        ///            that additional value of type TermColor is stored in values </param>
        /// <param name="d">
        ///            Declaration to be parsed </param>
        /// <param name="properties">
        ///            Properties map where to store enum </param>
        /// <param name="values"> </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         elsewhere </returns>
        public static bool genericOneIdentOrColor<T>(Type type, T colorIdentification, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent<T>(type, d[0], ALLOW_INH, d.Property, properties) || genericTermColor(d[0], d.Property, colorIdentification, properties, values);
        }

        public static bool genericOneIdentOrInteger<T>(Type type, T integerIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent<T>(type, d[0], ALLOW_INH, d.Property, properties) || genericTerm(typeof(TermInteger), d[0], d.Property, integerIdentification, range, properties, values);
        }

        public static bool genericOneIdentOrIntegerOrNumber<T>(Type type, T integerIdentification, T numberIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent<T>(type, d[0], ALLOW_INH, d.Property, properties) || genericTerm(typeof(TermInteger), d[0], d.Property, integerIdentification, range, properties, values) || genericTerm(typeof(TermNumber), d[0], d.Property, numberIdentification, range, properties, values);
        }

        public static bool genericOneIdentOrLength<T>(Type type, T lengthIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent<T>(type, d[0], ALLOW_INH, d.Property, properties) || genericTermLength(d[0], d.Property, lengthIdentification, range, properties, values);
        }

        public static bool genericTime<T>(Type type, T integerIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {
            if (d.Count != 1)
            {
                return false;
            }

            //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
            Term term = d[0];
            if (term is TermIdent)
            {
                T property = genericPropertyRaw<T>(type, null, (TermIdent)term);
                if (!property.equalsInherit())
                {
                    return false;
                }
                else
                {
                    properties[d.Property] = property;
                    return true;
                }
            }
            return genericTerm(typeof(TermTime), term, d.Property, integerIdentification, range, properties, values);
        }

        public static bool genericInteger<T>(Type type, T integerIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
            Term term = d[0];
            if (term is TermIdent)
            {
                T property = genericPropertyRaw<T>(type, null, (TermIdent)term);
                if (!property.equalsInherit())
                {
                    return false;
                }
                else
                {
                    properties[d.Property] = property;
                    return true;
                }
            }
            else
            {
                return genericTerm(typeof(TermInteger), term, d.Property, integerIdentification, range, properties, values);
            }
        }

        public static bool genericIntegerOrLength<T>(Type type, T integerIdentification, T lengthIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
            Term term = d[0];
            if (term is TermIdent)
            {
                T property = genericPropertyRaw<T>(type, null, (TermIdent)term);
                if (!property.equalsInherit())
                {
                    return false;
                }
                else
                {
                    properties[d.Property] = property;
                    return true;
                }
            }
            else
            {
                return genericTerm(typeof(TermInteger), term, d.Property, integerIdentification, range, properties, values) || genericTermLength(term, d.Property, lengthIdentification, range, properties, values);
            }
        }

        // TOCHECK!!! removed enum
        public static bool genericOneIdentOrLengthOrPercent<T>(Type type, T lengthIdentification, T percentIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count != 1)
            {
                return false;
            }

            return genericTermIdent<T>(type, d[0], ALLOW_INH, d.Property, properties) || genericTermLength(d[0], d.Property, lengthIdentification, range, properties, values) || genericTerm(typeof(TermPercent), d[0], d.Property, percentIdentification, range, properties, values);
        }

        // TOCHECK: ATTENTION, can change everything!!!!!!!!!!!
        public static bool genericTwoIdentsOrLengthsOrPercents<T>(Type type, T listIdentification, ValueRange range, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values) where T : StyleParserCS.css.CSSProperty
        {

            if (d.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
                Term term = d[0];
                string propertyName = d.Property;
                // is it identifier or length ?
                if (genericTermIdent<T>(type, term, ALLOW_INH, propertyName, properties) || genericTermLength(term, propertyName, listIdentification, range, properties, values) || genericTerm(typeof(TermPercent), term, propertyName, listIdentification, range, properties, values))
                {
                    // one term with length was inserted, double it
                    if (properties[propertyName] == (CSSProperty)listIdentification)
                    {
                        TermList terms = tf.createList(2);
                        terms.Add(term);
                        terms.Add(term);
                        values[propertyName] = (Term)terms;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // two numerical values
            else if (d.Count == 2)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term1 = d.get(0);
                Term term1 = d[0];
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term2 = d.get(1);
                Term term2 = d[1];
                string propertyName = d.Property;
                // two lengths ?
                if ((genericTermLength(term1, propertyName, listIdentification, range, properties, values) || genericTerm(typeof(TermPercent), term1, propertyName, listIdentification, range, properties, values)) && (genericTermLength(term2, propertyName, listIdentification, range, properties, values) || genericTerm(typeof(TermPercent), term2, propertyName, listIdentification, range, properties, values)))
                {
                    TermList terms = tf.createList(2);
                    terms.Add(term1);
                    terms.Add(term2);
                    values[propertyName] = (Term)terms;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Splits a declaration to multiple declarations by a separating term. </summary>
        /// <param name="src"> the source declarations </param>
        /// <param name="separator"> separating operator </param>
        /// <returns> a list of declarations where each of them contains a sub-list of terms of the source declaration </returns>
        public static IList<Declaration> splitDeclarations(Declaration src, Term_Operator sepOperator)
        {
            //ORIGINAL LINE: final java.util.List<StyleParserCS.css.Declaration> ret = new java.util.ArrayList<>();
            IList<Declaration> ret = new List<Declaration>();
            Declaration current = rf.createDeclaration();
            current.unlock();
            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : src.asList())
            foreach (Term t in src.asList())
            {
                if (t.Operator == sepOperator)
                {
                    ret.Add(current);
                    current = rf.createDeclaration();
                    current.unlock();
                }
                current.Add(t);
            }
            ret.Add(current);
            return ret;
        }


    }

}