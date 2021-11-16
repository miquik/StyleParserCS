using System;
using System.Collections.Generic;

namespace StyleParserCS.domassign.decode
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using Declaration = StyleParserCS.css.Declaration;
    using SupportedCSS = StyleParserCS.css.SupportedCSS;
    using StyleParserCS.css;
    using TermIdent = StyleParserCS.css.TermIdent;
    using System.Linq;

    /// <summary>
    /// Repeats one operation on different CSS declaration duplication of code. To
    /// use, implement operation() method. Use for CSS declaration such as
    /// <code>border-width: 2px</code>
    /// 
    /// @author kapy
    /// </summary>
    public abstract class Repeater : Decoder
    {

        /// <summary>
        /// Number of times operation is repeated
        /// </summary>
        protected internal int times;

        /// <summary>
        /// Terms over which operation is repeated
        /// </summary>
        //ORIGINAL LINE: protected java.util.List<StyleParserCS.css.Term<?>> terms;
        protected internal IList<Term> terms;

        /// <summary>
        /// Property names for each iteration of repeater object
        /// </summary>
        protected internal IList<string> names;

        /// <summary>
        /// Which property is used to repeat
        /// </summary>
        protected internal Type type;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="times">
        ///            Number of iterations </param>
        public Repeater(int times)
        {
            this.times = times;
            //ORIGINAL LINE: this.terms = new java.util.ArrayList<StyleParserCS.css.Term<?>>(times);
            this.terms = new List<Term>(times);
            this.names = new List<string>(times);
        }

        /// <summary>
        /// Repeating operation
        /// </summary>
        /// <param name="iteration">
        ///            Currently passing iteration </param>
        /// <param name="properties">
        ///            Properties map where to store properties types </param>
        /// <param name="values">
        ///            Value map where to store properties values </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         elsewhere </returns>
        protected internal abstract bool operation(int iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values);

        /// <summary>
        /// Repeats operations on terms
        /// </summary>
        /// <param name="properties">
        ///            Properties map where to store properties types </param>
        /// <param name="values">
        ///            Values map where to store properties values </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         elsewhere </returns>
        public virtual bool repeat(IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            for (int i = 0; i < times; i++)
            {
                if (!operation(i, properties, values))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Construct terms array to be used by repeated object from available terms
        /// (in size 1 to 4) according to CSS rules.
        /// 
        /// Example:
        /// <para>
        /// <code>margin: 2px 5px;</code> creates virtual terms array with terms
        /// <code>2px 5px 2px 5px</code> so top and bottom; left and right contains
        /// the same margin
        /// </para>
        /// </summary>
        /// <param name="d">
        ///            Declaration with terms </param>
        /// <param name="properties">
        ///            Properties map where to store properties types </param>
        /// <param name="values">
        ///            Value map where to store properties values </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         elsewhere </returns>
        /// <exception cref="IllegalArgumentException">
        ///             In case when number of terms passed does not correspond to
        ///             iteration times </exception>
        //ORIGINAL LINE: public boolean repeatOverFourTermDeclaration(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values) throws IllegalArgumentException
        public virtual bool repeatOverFourTermDeclaration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            switch (d.Count)
            {
                case 1:
                    // one term for all value
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
                    Term term = d[0];

                    // check inherit
                    if (term is TermIdent && StyleParserCS.css.CSSProperty_Fields.INHERIT_KEYWORD.Equals(((TermIdent)term).Value, StringComparison.OrdinalIgnoreCase))
                    {
                        CSSProperty property = StyleParserCS.css.CSSProperty_Translator.createInherit(type);
                        for (int i = 0; i < times; i++)
                        {
                            properties[names[i]] = property;
                        }
                        return true;
                    }

                    assignTerms(term, term, term, term);
                    return repeat(properties, values);
                case 2:
                    // one term for bottom-top and left-right
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term1 = d.get(0);
                    Term term1 = d[0];
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term2 = d.get(1);
                    Term term2 = d[1];
                    assignTerms(term1, term2, term1, term2);
                    return repeat(properties, values);
                case 3:
                    // terms for bottom, top, left-right
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term31 = d.get(0);
                    Term term31 = d[0];
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term32 = d.get(1);
                    Term term32 = d[1];
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term33 = d.get(2);
                    Term term33 = d[2];
                    assignTerms(term31, term32, term33, term32);
                    return repeat(properties, values);
                case 4:
                    // four individual terms (or more - omitted)
                    //if (d.size() > 4)
                    //    LoggerFactory.getLogger(Repeater.class).warn("Omitting additional terms in four-term declaration");
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term41 = d.get(0);
                    Term term41 = d[0];
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term42 = d.get(1);
                    Term term42 = d[1];
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term43 = d.get(2);
                    Term term43 = d[2];
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term44 = d.get(3);
                    Term term44 = d[3];
                    assignTerms(term41, term42, term43, term44);
                    return repeat(properties, values);
                default:
                    throw new System.ArgumentException("Invalid length of terms in Repeater.");
            }
        }

        /// <summary>
        /// Assigns property names
        /// </summary>
        /// <param name="propertyNames">
        ///            Names of properties for each iteration </param>
        /// <exception cref="IllegalArgumentException">
        ///             In case when number of properties names does not correspond
        ///             with number of iterations </exception>
        //ORIGINAL LINE: public void assignPropertyNames(String... propertyNames) throws IllegalArgumentException
        public virtual void assignPropertyNames(params string[] propertyNames)
        {
            if (propertyNames.Length != times)
            {
                throw new System.ArgumentException("Invalid length of propertyNames in Repeater.");
            }
            this.names = propertyNames.ToList(); // Arrays.asList(propertyNames);
        }

        /// <summary>
        /// Assigns terms to repeater
        /// </summary>
        /// <param name="terms">
        ///            Terms to be assigned </param>
        /// <exception cref="IllegalArgumentException">
        ///             In case when number of terms does not correspond with number
        ///             of iterations </exception>
        //ORIGINAL LINE: public void assignTerms(StyleParserCS.css.Term<?>... terms) throws IllegalArgumentException
        public virtual void assignTerms(params Term[] terms)
        {
            if (terms.Length != times)
            {
                throw new System.ArgumentException("Invalid length of terms in Repeater.");
            }
            this.terms = terms.ToList(); // Arrays.asList(terms);
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

    }

}