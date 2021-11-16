using System;
using System.Collections.Generic;

namespace StyleParserCS.domassign.decode
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using CSSProperty_ValueType = StyleParserCS.css.CSSProperty_ValueType;
    using Declaration = StyleParserCS.css.Declaration;
    using SupportedCSS = StyleParserCS.css.SupportedCSS;
    using StyleParserCS.css;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermList = StyleParserCS.css.TermList;
    using TermPropertyValue = StyleParserCS.css.TermPropertyValue;
    using Term_Operator = StyleParserCS.css.Term_Operator;
    using System.Linq;

    /// <summary>
    /// Selects appropriate variant when parsing content of CSS declaration. Allows
    /// easy parsing of CSS declaration multi-values such as
    /// <code>border: blue 1px</code>
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public abstract class Variator : Decoder
    {

        /// <summary>
        /// All variants flag
        /// </summary>
        protected internal const int ALL_VARIANTS = -1;

        /// <summary>
        /// Total variants available
        /// </summary>
        protected internal int variants;

        /// <summary>
        /// Results of variants. Each variant is allowed to be passed only once in
        /// case of multi-value declaration, so this array is used to show that
        /// currently passed variant was already successfully passed in past
        /// </summary>
        protected internal bool[] variantPassed;

        /// <summary>
        /// Property names according to each variant
        /// </summary>
        protected internal IList<string> names;

        protected internal IList<Type> types;

        /// <summary>
        /// Terms over which variants are tested
        /// </summary>
        //ORIGINAL LINE: protected java.util.List<StyleParserCS.css.Term<?>> terms;
        protected internal IList<Term> terms;

        /// <summary>
        /// Creates variator which contains <code>variants</code> variants to be
        /// tested
        /// </summary>
        /// <param name="variants"> </param>
        public Variator(int variants)
        {
            this.variants = variants;
            this.variantPassed = new bool[variants];
            this.names = new List<string>(variants);
            this.types = new List<Type>(variants);
            reset();
        }

        /// <summary>
        /// Resets the variator to its initial state.
        /// </summary>
        public virtual void reset()
        {
            for (int i = 0; i < variants; i++)
            {
                variantPassed[i] = false;
            }
        }

        /// <summary>
        /// This function contains parsing block for variants
        /// </summary>
        /// <param name="variant">
        ///            Tested variant </param>
        /// <param name="iteration">
        ///            Number of iteration, that is term to be tested.
        ///            This number may be changed internally in function 
        ///            to inform that more than one term was used for variant </param>
        /// <param name="properties">
        ///            Properties map where to store properties types </param>
        /// <param name="values">
        ///            Values map where to store properties values </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        protected internal abstract bool variant(int variant, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values);

        /// <summary>
        /// Solves variant which leads to <code>inherit</code> CSS Property value.
        /// This overrides all other possible variants and no other informations are
        /// allowed per CSS Declaration.
        /// 
        /// This method is called before check for variants or before variant itself
        /// is called in one shot way.
        /// 
        /// Example: <code>margin: inherit</code> is valid value and leads to setting
        /// of
        /// <ul>
        /// <li><code>margin-top: inherit</code></li>
        /// <li><code>margin-right: inherit</code></li>
        /// <li><code>margin-bottom: inherit</code></li>
        /// <li><code>margin-left: inherit</code></li>
        /// </ul>
        /// 
        /// <code>margin: 0px inherit</code> is invalid value.
        /// </summary>
        /// <param name="variant">
        ///            Number of variant or identifier of all variants
        ///            <code>VARIANT_ALL</code> </param>
        /// <param name="properties">
        ///            Properties map where to store properties types </param>
        /// <param name="term">
        ///            Term to be checked </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        protected internal virtual bool checkInherit(int variant, Term term, IDictionary<string, CSSProperty> properties)
        {

            // check whether term equals inherit
            if (!(term is TermIdent) || !StyleParserCS.css.CSSProperty_Fields.INHERIT_KEYWORD.Equals(((TermIdent)term).Value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (variant == ALL_VARIANTS)
            {
                for (int i = 0; i < variants; i++)
                {
                    properties[names[i]] = createInherit(i);
                }
                return true;
            }

            properties[names[variant]] = createInherit(variant);

            return true;
        }

        /// <summary>
        /// Creates INHERIT value of given class
        /// </summary>
        /// <param name="i">
        ///            Ordinal in list of types </param>
        /// <returns> Created CSSProperty with value inherit </returns>
        /// <exception cref="UnsupportedOperationException">
        ///             If class does not provide INHERIT or is not implementation of
        ///             CSSProperty </exception>
        private CSSProperty createInherit(int i)
        {

            try
            {
                Type clazz = types[i];
                CSSProperty property = StyleParserCS.css.CSSProperty_Translator.createInherit(clazz);
                if (property != null)
                {
                    return property;
                }

                throw new Exception("No inherit value for: " + clazz.FullName);
            }
            catch (Exception e)
            {
                throw new System.NotSupportedException("Unable to create inherit value", e);
            }

        }

        /// <summary>
        /// Check if variant, which was passed is able to be located in place where it was 
        /// found.
        /// 
        /// Example:
        /// We have declaration:
        /// <code>font: 12px/14px sans-serif</code>
        /// Then according to grammar:
        /// <pre>
        /// 	[ 
        /// 		[ &lt;'font-style'&gt; || &lt;'font-variant'&gt; || &lt;'font-weight'&gt; ]? 
        /// 		&lt;'font-size'&gt; 
        /// 		[ / &lt;'line-height'&gt; ]? 
        /// 		&lt;'font-family'&gt; 
        ///  ] 
        ///  | caption | icon | menu | message-box | 
        ///  small-caption | status-bar | inherit
        /// </pre> 
        /// <ol>
        /// <li><code>12px</code> is assigned to <i>font-size</i></li>
        /// <li><code>14px</code> is checked to have SLASH operator before 
        /// and check to whether <i>font-size</i> was defined before it</li>
        /// <li><code>sans-serif</code> is tested to have at least 
        /// definition of <i>font-size</i> before itself</li>
        /// <li>declaration passes</li>
        /// </ol>
        /// </summary>
        /// <param name="variant"> Identification of current variant which passed test </param>
        /// <param name="term"> Position in term list of terms which passed test, for multiple
        /// value term allow to change it </param>
        /// <returns> <code>true</code> in case of success, <code>false</code> elsewhere </returns>
        /// <seealso cref= Term.Operator </seealso>
        protected internal virtual bool variantCondition(int variant, IntegerRef term)
        {
            return true;
        }

        /// <summary>
        /// Test all terms
        /// 
        /// </summary>
        public virtual bool vary(IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // try inherit variant
            if (terms.Count == 1 && checkInherit(ALL_VARIANTS, terms[0], properties))
            {
                return true;
            }

            // for all terms
            for (IntegerRef i = new IntegerRef(0); i.get() < terms.Count; i.inc())
            {

                bool passed = false;

                // check all variants
                for (int v = 0; v < variants; v++)
                {
                    // check and if variant was already found
                    // signalize error by discarding complete declaration
                    // have to check variant condition firstly to avoid false
                    // positive
                    // variantPassed
                    if (!variantCondition(v, i))
                    {
                        continue;
                    }
                    //if this variant already passed, do not try again
                    //TODO: check if we shouldn't try better combination of terms
                    if (variantPassed[v])
                    {
                        continue;
                    }
                    //check if this term corresponds to this variant
                    passed = variant(v, i, properties, values);
                    if (passed)
                    {
                        // mark occurrence of variant
                        variantPassed[v] = true;
                        // we have found, skip evaluation
                        break;
                    }
                }
                // no variant could be assigned
                if (!passed)
                {
                    return false;
                }
            }
            // all terms passed
            return true;
        }

        /// <summary>
        /// Uses variator functionality to test selected variant on term
        /// </summary>
        /// <param name="variant">
        ///            Which variant will be tested </param>
        /// <param name="d">
        ///            The declaration on which variant will be tested </param>
        /// <param name="properties">
        ///            Properties map where to store property type </param>
        /// <param name="values">
        ///            Values map where to store property value </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        public virtual bool tryOneTermVariant(int var, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // only one term is allowed
            if (d.Count != 1)
            {
                return false;
            }

            // try inherit variant
            if (checkInherit(var, d[0], properties))
            {
                return true;
            }

            //ORIGINAL LINE: this.terms = new java.util.ArrayList<StyleParserCS.css.Term<?>>();
            this.terms = new List<Term>();
            this.terms.Add(d[0]);

            return variant(var, new IntegerRef(0), properties, values);
        }

        /// <summary>
        /// Uses variator functionality to test selected variant on more terms. This
        /// is used when variant is represented by more terms. Since usually only one
        /// term per variant is used, only one multiple variant is allowed per
        /// variator and should be placed as the last one
        /// </summary>
        /// <param name="variant">
        ///            Number of variant (last variant in variator) </param>
        /// <param name="properties">
        ///            Properties map where to store property type </param>
        /// <param name="values">
        ///            Values map where to store property value </param>
        /// <param name="terms">
        ///            Array of terms used for variant </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        //ORIGINAL LINE: public boolean tryMultiTermVariant(int variant, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values, StyleParserCS.css.Term<?>... terms)
        public virtual bool tryMultiTermVariant(int var, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values, params Term[] terms)
        {

            this.terms = terms.ToList(); // Arrays.asList(terms);

            // try inherit variant
            if (this.terms.Count == 1 && checkInherit(var, this.terms[0], properties))
            {
                return true;
            }

            return variant(var, new IntegerRef(0), properties, values);
        }

        /// <summary>
        /// Assigns property names for each variant
        /// </summary>
        /// <param name="variantPropertyNames">
        ///            List of property names </param>
        public virtual void assignVariantPropertyNames(params string[] variantPropertyNames)
        {
            this.names = variantPropertyNames.ToList(); // Arrays.asList(variantPropertyNames);
        }

        /// <summary>
        /// Assigns terms to be checked by variator
        /// </summary>
        /// <param name="terms">
        ///            Terms to be assigned </param>
        //ORIGINAL LINE: public void assignTerms(StyleParserCS.css.Term<?>... terms)
        public virtual void assignTerms(params Term[] terms)
        {
            this.terms = terms.ToList(); //  Arrays.asList(terms);
        }

        /// <summary>
        /// Assigns terms from declaration
        /// </summary>
        /// <param name="d">
        ///            Declaration which contains terms </param>
        public virtual void assignTermsFromDeclaration(Declaration d)
        {
            this.terms = d.asList();
        }


        /// <summary>
        /// Assigns the default values to all the properties. </summary>
        /// <param name="properties"> </param>
        /// <param name="values"> </param>
        public virtual void assignDefaults(IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            SupportedCSS css = CSSFactory.SupportedCSS;
            foreach (string name in names)
            {
                CSSProperty dp = css.getDefaultProperty(name);
                if (dp != null)
                {
                    properties[name] = dp;
                }
                //ORIGINAL LINE: StyleParserCS.css.Term<?> dv = css.getDefaultValue(name);
                Term dv = css.getDefaultValue(name);
                if (dv != null)
                {
                    values[name] = dv;
                }
            }
        }


        //=========================================================================
        // Nested list creation

        /// <summary>
        /// Tries a single variant that may consist of a term or a list of comma-separated terms.
        /// </summary>
        /// <param name="variant"> the variant to be tried </param>
        /// <param name="d"> the declaration to be processed </param>
        /// <param name="properties"> target property map </param>
        /// <param name="values"> target value map </param>
        /// <param name="listValue"> the list_values value to be used for the property value </param>
        /// <returns> {@code true} when parsed successfully </returns>
        public virtual bool tryListOfOneTermVariant(int var, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values, CSSProperty listValue)
        {

            // try inherit variant
            if (d.Count == 1 && checkInherit(var, d[0], properties))
            {
                return true;
            }
            //scan the list
            TermList list = tf.createList();
            //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.CSSProperty> p = new java.util.HashMap<>();
            IDictionary<string, CSSProperty> p = new Dictionary<string, CSSProperty>();
            //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.Term<?>> v = new java.util.HashMap<>();
            IDictionary<string, Term> v = new Dictionary<string, Term>();
            bool first = true;
            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : d.asList())
            foreach (Term t in d.asList())
            {
                //terms must be separated by commas
                if ((first && t.Operator != null) || (!first && t.Operator != Term_Operator.COMMA))
                {
                    return false;
                }
                //process the variant for a single term
                p.Clear();
                v.Clear();
                //ORIGINAL LINE: this.terms = new java.util.ArrayList<StyleParserCS.css.Term<?>>();
                this.terms = new List<Term>();
                this.terms.Add(t);
                if (!variant(var, new IntegerRef(0), p, v))
                {
                    return false;
                }
                //collect the resulting term
                //ORIGINAL LINE: final StyleParserCS.css.CSSProperty prop = p.values().iterator().next();
                // TOCHECK!!!
                // CSSProperty prop = p.Values.GetEnumerator().next();
                var enumer = p.Values.GetEnumerator();
                enumer.MoveNext();
                CSSProperty prop = enumer.Current; // p.Values.GetEnumerator().next();
                //ORIGINAL LINE: final StyleParserCS.css.Term<?> val = (v.values().isEmpty()) ? null : v.values().iterator().next();
                // Term val = (v.Values.Count == 0) ? null : v.Values.GetEnumerator().next();
                Term val = null;
                if (v.Values.Count > 0)
                {
                    var enumer2 = v.Values.GetEnumerator();
                    enumer2.MoveNext();
                    val = enumer2.Current;
                }
                //ORIGINAL LINE: final StyleParserCS.css.TermPropertyValue pval = tf.createPropertyValue(prop, val);
                TermPropertyValue pval = tf.createPropertyValue(prop, val);
                if (!first)
                {
                    pval.setOperator(Term_Operator.COMMA);
                }
                list.Add((Term)pval);
                first = false;
            }
            //store the result
            properties[names[var]] = listValue;
            values[names[var]] = (Term)list;
            return true;
        }

        /// <summary>
        /// Tries a single variant that may consist of space-separated terms or a comma-separated list
        /// of space-separated lists.
        /// </summary>
        /// <param name="variant"> the variant to be tried </param>
        /// <param name="d"> the declaration to be processed </param>
        /// <param name="properties"> target property map </param>
        /// <param name="values"> target value map </param>
        /// <param name="listValue"> the list_values value to be used for the property value </param>
        /// <returns> {@code true} when parsed successfully </returns>
        public virtual bool tryListOfMultiTermVariant(int var, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values, CSSProperty listValue)
        {

            // try inherit variant
            if (d.Count == 1 && checkInherit(var, d[0], properties))
            {
                return true;
            }
            // for all sub-declarations
            TermList list = tf.createList();
            //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.CSSProperty> p = new java.util.HashMap<>();
            IDictionary<string, CSSProperty> p = new Dictionary<string, CSSProperty>();
            //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.Term<?>> v = new java.util.HashMap<>();
            IDictionary<string, Term> v = new Dictionary<string, Term>();
            bool first = true;
            IList<Declaration> subs = splitDeclarations(d, Term_Operator.COMMA);
            foreach (Declaration sub in subs)
            {
                p.Clear();
                v.Clear();
                assignTermsFromDeclaration(sub);
                if (!variant(var, new IntegerRef(0), p, v))
                {
                    return false;
                }
                //ORIGINAL LINE: final StyleParserCS.css.CSSProperty prop = p.values().iterator().next();
                var enumer = p.Values.GetEnumerator();
                enumer.MoveNext();
                CSSProperty prop = enumer.Current; // p.Values.GetEnumerator().next();
                // CSSProperty prop = p.Values.GetEnumerator().next();
                //ORIGINAL LINE: final StyleParserCS.css.Term<?> val = (v.values().isEmpty()) ? null : v.values().iterator().next();
                // Term val = (v.Values.Count == 0) ? null : v.Values.GetEnumerator().next();
                Term val = null;
                if (v.Values.Count > 0)
                {
                    var enumer2 = v.Values.GetEnumerator();
                    enumer2.MoveNext();
                    val = enumer2.Current;
                }
                //ORIGINAL LINE: final StyleParserCS.css.TermPropertyValue pval = tf.createPropertyValue(prop, val);
                TermPropertyValue pval = tf.createPropertyValue(prop, val);
                if (!first)
                {
                    pval.setOperator(Term_Operator.COMMA);
                }
                list.Add((Term)pval);
                first = false;
            }
            //store the result
            properties[names[var]] = listValue;
            values[names[var]] = (Term)list;
            return true;
        }

        /// <summary>
        /// Varies over a comma-separated list of layers where each layer defines all the variants.
        /// </summary>
        /// <param name="d"> </param>
        /// <param name="properties"> </param>
        /// <param name="values">
        /// @return </param>
        public virtual bool varyList(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // try inherit variant
            if (d.Count == 1 && checkInherit(ALL_VARIANTS, d[0], properties))
            {
                return true;
            }

            // temporary result of the whole list which will be used after final validation
            //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.CSSProperty> destProps = new java.util.HashMap<>();
            IDictionary<string, CSSProperty> destProps = new Dictionary<string, CSSProperty>();
            //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.Term<?>> destVals = new java.util.HashMap<>();
            IDictionary<string, Term> destVals = new Dictionary<string, Term>();

            // for all sub-declarations
            int listIndex = 0;
            IList<Declaration> subs = splitDeclarations(d, Term_Operator.COMMA);
            foreach (Declaration sub in subs)
            {
                reset();
                //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.CSSProperty> props = new java.util.HashMap<>();
                IDictionary<string, CSSProperty> props = new Dictionary<string, CSSProperty>();
                //ORIGINAL LINE: final java.util.Map<String, StyleParserCS.css.Term<?>> vals = new java.util.HashMap<>();
                IDictionary<string, Term> vals = new Dictionary<string, Term>();
                assignDefaults(props, vals);
                assignTermsFromDeclaration(sub);
                // for all terms
                for (IntegerRef i = new IntegerRef(0); i.get() < terms.Count; i.inc())
                {

                    bool passed = false;

                    // check all variants
                    for (int v = 0; v < variants; v++)
                    {
                        // check and if variant was already found
                        // signalize error by discarding complete declaration
                        // have to check variant condition firstly to avoid false
                        // positive
                        // variantPassed
                        if (!variantCondition(v, i))
                        {
                            continue;
                        }
                        //if this variant already passed, do not try again
                        //TODO: check if we shouldn't try better combination of terms
                        if (variantPassed[v])
                        {
                            continue;
                        }
                        //check if this term corresponds to this variant
                        passed = variant(v, i, props, vals);
                        if (passed)
                        {
                            // mark occurrence of variant
                            variantPassed[v] = true;
                            // we have found, skip evaluation
                            break;
                        }
                    }
                    // no variant could be assigned
                    if (!passed)
                    {
                        return false;
                    }
                }
                if (!validateListItem(listIndex, subs.Count, props, vals))
                {
                    return false; //validation failed
                }
                // all terms passed
                foreach (string key in props.Keys)
                {
                    //ORIGINAL LINE: final StyleParserCS.css.CSSProperty p = props.get(key);
                    CSSProperty p = props[key];
                    //ORIGINAL LINE: final StyleParserCS.css.Term<?> v = vals.get(key);
                    Term v = vals[key];
                    if (p.ValueType == CSSProperty_ValueType.LIST)
                    {
                        addToMap(destVals, key, p, v, (listIndex == 0));
                        destProps[key] = StyleParserCS.css.CSSProperty_Translator.createNestedListValue(p.GetType());
                    }
                    else
                    {
                        destProps[key] = p;
                        destVals[key] = v;
                    }
                }
                listIndex++;
            }

            //validate and store the whole list
            if (!validateList(subs.Count, destProps, destVals))
            {
                return false;
            }
            // properties.putAll(destProps);
            foreach (var dp in destProps)
            {
                properties.Add(dp);
            }

            // values.putAll(destVals);
            foreach (var dp in destVals)
            {
                values.Add(dp);
            }
            return true;
        }

        /// <summary>
        /// Adds a property-value pair to a nested list in the destination value map. Creates a new
        /// list when the corresponding value is not set or it is not a list. </summary>
        /// <param name="dest"> the destination value map </param>
        /// <param name="key"> the key to use (property name) </param>
        /// <param name="property"> the property value to set </param>
        /// <param name="value"> an optional Term value to set </param>
        /// <param name="first"> {@code true} for the first value in the list (for generating separators properly) </param>
        private void addToMap(IDictionary<string, Term> dest, string key, CSSProperty property, Term value, bool first)
        {
            //ORIGINAL LINE: final StyleParserCS.css.Term<?> cur = dest.get(key);
            Term cur = dest[key];

            TermList list;
            if (cur is TermList)
            {
                list = (TermList)cur;
            }
            else
            {
                list = tf.createList();
                dest[key] = (Term)list;
            }

            //make a copy and remove the original operator from the value
            //ORIGINAL LINE: StyleParserCS.css.Term<?> vvalue = (value == null) ? null : value.shallowClone();
            Term vvalue = (value == null) ? null : value.shallowClone();
            if (vvalue != null)
            {
                vvalue.setOperator(null);
            }
            //add the pair
            //ORIGINAL LINE: final StyleParserCS.css.TermPropertyValue pval = tf.createPropertyValue(property, vvalue);
            TermPropertyValue pval = tf.createPropertyValue(property, vvalue);
            if (!first)
            {
                pval.setOperator(Term_Operator.COMMA);
            }
            list.Add((Term)pval);
        }

        protected internal virtual bool validateListItem(int listIndex, int listSize, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return true; //no validation is performed by default, this may be overriden in subclasses
        }

        protected internal virtual bool validateList(int listSize, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return true; //no validation is performed by default, this may be overriden in subclasses
        }

        //=========================================================================

        /// <summary>
        /// Reference to integer
        /// @author kapy
        /// 
        /// </summary>
        protected internal class IntegerRef
        {

            internal int i;

            public IntegerRef(int i)
            {
                this.i = i;
            }

            /// <returns> the i </returns>
            public virtual int get()
            {
                return i;
            }

            /// <param name="i"> the i to set </param>
            public virtual void set(int i)
            {
                this.i = i;
            }

            public virtual void inc()
            {
                this.i++;
            }

        }
    }

}