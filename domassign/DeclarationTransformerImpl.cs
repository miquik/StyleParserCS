using System;
using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.domassign
{
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using DeclarationTransformer = StyleParserCS.csskit.DeclarationTransformer;
    using StyleParserCS.domassign.decode;
    using ValueRange = StyleParserCS.domassign.decode.Decoder.ValueRange;
    using System.Linq;
    using StyleParserCS.utils;

    /// <summary>
    /// Contains methods to transform declaration into values applicable to NodeData.
    /// Uses defaults defined by CSSFactory
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class DeclarationTransformerImpl : DeclarationTransformer
    {
        /// <summary>
        /// Cache of parsing methods
        /// </summary>
        private IDictionary<string, System.Reflection.MethodInfo> methods;

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static readonly DeclarationTransformerImpl instance;

        private static readonly RuleFactory rf = CSSFactory.RuleFactory;
        private static readonly TermFactory tf = CSSFactory.TermFactory;
        private static readonly SupportedCSS css = CSSFactory.SupportedCSS;

        static DeclarationTransformerImpl()
        {
            instance = new DeclarationTransformerImpl();
        }

        /// <summary>
        /// Returns instance
        /// </summary>
        /// <returns> Singleton instance </returns>
        public static DeclarationTransformerImpl Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Converts string divided by dash ('-') characters into camelCase such as
        /// convenient for Java method names
        /// </summary>
        /// <param name="string">
        ///            String to convert </param>
        /// <returns> CamelCase version of string </returns>
        public static string camelCase(string stringv)
        {

            StringBuilder sb = new StringBuilder();

            bool upperFlag = true;

            for (int i = 0; i < stringv.Length; i++)
            {
                char ch = stringv[i];
                if (ch == '-')
                {
                    upperFlag = true;
                }
                else if (upperFlag && char.IsLetter(ch))
                {
                    sb.Append(char.ToUpper(ch));
                    upperFlag = false;
                }
                else if (!upperFlag && char.IsLetter(ch))
                {
                    sb.Append(ch);
                }
                else if (ch == '_') // vendor extension
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Core function. Parses CSS declaration into structure applicable to
        /// DataNodeImpl
        /// </summary>
        /// <param name="d">
        ///            Declaration </param>
        /// <param name="properties">
        ///            Wrap of parsed declaration's properties </param>
        /// <param name="values">
        ///            Wrap of parsed declaration's value </param>
        /// <returns> <code>true</code> in case of success, <code>false</code>
        ///         otherwise </returns>
        ///         
                       //bool parseDeclaration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, object> values);
        public virtual bool parseDeclaration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            //ORIGINAL LINE: final String propertyName = d.getProperty();
            string propertyName = d.Property;

            // no such declaration is supported or declaration is empty
            if (!css.isSupportedCSSProperty(propertyName) || d.Count == 0)
            {
                return false;
            }

            try
            {
                System.Reflection.MethodInfo m = methods.GetValue(propertyName);
                if (m != null)
                {
                    // bool result = ((bool?)m.Invoke(this, d, properties, values)).Value;
                    // TOCHECK
                    bool result = (bool)m.Invoke(this, new object[] { d, properties, values });
                    // log.debug("Parsing /{}/ {}", result, d);
                    return result;
                }
                else
                {
                    bool result = processAdditionalCSSGenericProperty(d, properties, values);
                    // log.debug("Parsing with proxy /{}/ {}", result, d);
                    return result;
                }
            }
            catch (System.ArgumentException e)
            {
                // log.warn("Illegal argument", e);
                int j = 0;
            }
            catch (Exception e)
            {
                // log.warn("Illegal access", e);
            }
            return false;
        }

        /// <summary>
        /// Sole constructor
        /// </summary>
        public DeclarationTransformerImpl()
        {
            this.methods = parsingMethods();
        }

        protected internal virtual IDictionary<string, System.Reflection.MethodInfo> parsingMethods()
        {
            Type dcit = typeof(DeclarationTransformerImpl);
            IDictionary<string, System.Reflection.MethodInfo> map = new Dictionary<string, System.Reflection.MethodInfo>(css.TotalProperties);

            foreach (string key in css.DefinedPropertyNames)
            {
                try
                {
                    // private bool processDisplay(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
                    // System.Reflection.MethodInfo m = typeof(DeclarationTransformerImpl).getDeclaredMethod(DeclarationTransformerImpl.camelCase("process-" + key), typeof(Declaration), typeof(System.Collections.IDictionary), typeof(System.Collections.IDictionary));
                    string camelCaseName = "process" + DeclarationTransformerImpl.camelCase(key);
                    // string camelCaseName = "process" + key.Substring(0, 1).ToUpper() + key.Substring(1).ToLower();
                    System.Reflection.MethodInfo m = dcit.GetMethod(camelCaseName, System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);
                    /*
                    System.Reflection.MethodInfo m = dcit.GetMethod(elelCaseName, new Type[] { 
                        typeof(Declaration), 
                        typeof(System.Collections.IDictionary), 
                        typeof(System.Collections.IDictionary) 
                    });
                    */
                    map[key] = m;
                }
                catch (Exception)
                {
                    // log.warn("Unable to find method for property {}.", key);
                    int j = 0;
                }
            }
            // log.info("Totally found {} parsing methods", map.Count);
            return map;
        }

        // =============================================================
        // processing methods

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrColor(typeof(StyleParserCS.css.CSSProperty_Color), StyleParserCS.css.CSSProperty_Color.color, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackground(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackground(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator background = new BackgroundVariator();
            return background.varyList(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundAttachment(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundAttachment(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryListOfOneTermVariant(BackgroundVariator.ATTACHMENT, d, properties, values, StyleParserCS.css.CSSProperty_BackgroundAttachment.nested_list);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryOneTermVariant(BackgroundVariator.COLOR, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundImage(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundImage(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryListOfOneTermVariant(BackgroundVariator.IMAGE, d, properties, values, StyleParserCS.css.CSSProperty_BackgroundImage.nested_list);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundRepeat(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundRepeat(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryListOfOneTermVariant(BackgroundVariator.REPEAT, d, properties, values, StyleParserCS.css.CSSProperty_BackgroundRepeat.nested_list);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundPosition(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundPosition(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryListOfMultiTermVariant(BackgroundVariator.POSITION, d, properties, values, StyleParserCS.css.CSSProperty_BackgroundPosition.nested_list);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundSize(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundSize(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryListOfMultiTermVariant(BackgroundVariator.SIZE, d, properties, values, StyleParserCS.css.CSSProperty_BackgroundSize.nested_list);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackgroundOrigin(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackgroundOrigin(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator background = new BackgroundVariator();
            Variator background = new BackgroundVariator();
            return background.tryListOfOneTermVariant(BackgroundVariator.ORIGIN, d, properties, values, StyleParserCS.css.CSSProperty_BackgroundOrigin.nested_list);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorder(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorder(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator border = new BorderVariator();
            border.assignTermsFromDeclaration(d);
            border.assignDefaults(properties, values);
            return border.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderCollapse(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderCollapse(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_BorderCollapse>(typeof(StyleParserCS.css.CSSProperty_BorderCollapse), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderTopColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderTopColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("top");
            Variator borderSide = new BorderSideVariator("top");
            return borderSide.tryOneTermVariant(BorderSideVariator.COLOR, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderRightColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderRightColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("right");
            Variator borderSide = new BorderSideVariator("right");
            return borderSide.tryOneTermVariant(BorderSideVariator.COLOR, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderBottomColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderBottomColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("bottom");
            Variator borderSide = new BorderSideVariator("bottom");
            return borderSide.tryOneTermVariant(BorderSideVariator.COLOR, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderLeftColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderLeftColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("left");
            Variator borderSide = new BorderSideVariator("left");
            return borderSide.tryOneTermVariant(BorderSideVariator.COLOR, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderTopStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderTopStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("top");
            Variator borderSide = new BorderSideVariator("top");
            return borderSide.tryOneTermVariant(BorderSideVariator.STYLE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderRightStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderRightStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("right");
            Variator borderSide = new BorderSideVariator("right");
            return borderSide.tryOneTermVariant(BorderSideVariator.STYLE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderBottomStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderBottomStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("bottom");
            Variator borderSide = new BorderSideVariator("bottom");
            return borderSide.tryOneTermVariant(BorderSideVariator.STYLE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderLeftStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderLeftStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("left");
            Variator borderSide = new BorderSideVariator("left");
            return borderSide.tryOneTermVariant(BorderSideVariator.STYLE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderSpacing(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderSpacing(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
                Term term = d[0];
                string propertyName = d.Property;
                // is it identifier or length ?
                if (Decoder.genericTermIdent<CSSProperty_BorderSpacing>(typeof(StyleParserCS.css.CSSProperty_BorderSpacing), term, Decoder.ALLOW_INH, propertyName, properties) || Decoder.genericTermLength(term, propertyName, StyleParserCS.css.CSSProperty_BorderSpacing.list_values, ValueRange.DISALLOW_NEGATIVE, properties, values))
                {
                    // one term with length was inserted, double it
                    if ((CSSProperty_BorderSpacing)properties.GetValue(propertyName) == CSSProperty_BorderSpacing.list_values)
                    {
                        TermList terms = tf.createList(2);
                        terms.Add(term);
                        terms.Add(term);
                        values[propertyName] = (Term)terms;
                    }
                    return true;
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
                if (Decoder.genericTermLength(term1, propertyName, StyleParserCS.css.CSSProperty_BorderSpacing.list_values, ValueRange.DISALLOW_NEGATIVE, properties, values) && Decoder.genericTermLength(term2, propertyName, StyleParserCS.css.CSSProperty_BorderSpacing.list_values, ValueRange.DISALLOW_NEGATIVE, properties, values))
                {
                    TermList terms = tf.createList(2);
                    terms.Add(term1);
                    terms.Add(term2);
                    values[propertyName] = (Term)terms;
                    return true;
                }
                return false;
            }
            return false;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Repeater borderColor = new BorderColorRepeater();
            return borderColor.repeatOverFourTermDeclaration(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Repeater borderStyle = new BorderStyleRepeater();
            return borderStyle.repeatOverFourTermDeclaration(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderTopWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderTopWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("top");
            Variator borderSide = new BorderSideVariator("top");
            return borderSide.tryOneTermVariant(BorderSideVariator.WIDTH, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderRightWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderRightWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("right");
            Variator borderSide = new BorderSideVariator("right");
            return borderSide.tryOneTermVariant(BorderSideVariator.WIDTH, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderBottomWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderBottomWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("bottom");
            Variator borderSide = new BorderSideVariator("bottom");
            return borderSide.tryOneTermVariant(BorderSideVariator.WIDTH, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderLeftWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderLeftWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator borderSide = new BorderSideVariator("left");
            Variator borderSide = new BorderSideVariator("left");
            return borderSide.tryOneTermVariant(BorderSideVariator.WIDTH, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Repeater borderWidth = new BorderWidthRepeater();
            return borderWidth.repeatOverFourTermDeclaration(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderTop(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderTop(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator borderSide = new BorderSideVariator("top");
            borderSide.assignTermsFromDeclaration(d);
            borderSide.assignDefaults(properties, values);
            return borderSide.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderRight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderRight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator borderSide = new BorderSideVariator("right");
            borderSide.assignTermsFromDeclaration(d);
            borderSide.assignDefaults(properties, values);
            return borderSide.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderBottom(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderBottom(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator borderSide = new BorderSideVariator("bottom");
            borderSide.assignTermsFromDeclaration(d);
            borderSide.assignDefaults(properties, values);
            return borderSide.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderLeft(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderLeft(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator borderSide = new BorderSideVariator("left");
            borderSide.assignTermsFromDeclaration(d);
            borderSide.assignDefaults(properties, values);
            return borderSide.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderTopLeftRadius(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderTopLeftRadius(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericTwoIdentsOrLengthsOrPercents(typeof(StyleParserCS.css.CSSProperty_BorderRadius), StyleParserCS.css.CSSProperty_BorderRadius.list_values, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderTopRightRadius(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderTopRightRadius(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericTwoIdentsOrLengthsOrPercents(typeof(StyleParserCS.css.CSSProperty_BorderRadius), StyleParserCS.css.CSSProperty_BorderRadius.list_values, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderBottomRightRadius(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderBottomRightRadius(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericTwoIdentsOrLengthsOrPercents(typeof(StyleParserCS.css.CSSProperty_BorderRadius), StyleParserCS.css.CSSProperty_BorderRadius.list_values, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderBottomLeftRadius(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderBottomLeftRadius(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericTwoIdentsOrLengthsOrPercents(typeof(StyleParserCS.css.CSSProperty_BorderRadius), StyleParserCS.css.CSSProperty_BorderRadius.list_values, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBorderRadius(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBorderRadius(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            BorderRadiusRepeater radius = new BorderRadiusRepeater();
            return radius.repeatOverMultiTermDeclaration(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBoxShadow(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBoxShadow(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (d.Count == 1 && Decoder.genericOneIdent<CSSProperty_BoxShadow>(typeof(StyleParserCS.css.CSSProperty_BoxShadow), d, properties))
            {
                return true;
            }
            // inset? && <length>{2,4} && <color>?
            TermList list = tf.createList();

            int lengthCount = 0;
            int lastLengthIndex = -1;
            int insetIndex = -1;
            int colorIndex = -1;

            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];

                if (t.Operator == Term_Operator.COMMA)
                {
                    if (lengthCount < 2)
                    {
                        return false;
                    }
                    lengthCount = 0;
                    lastLengthIndex = -1;
                    insetIndex = -1;
                    colorIndex = -1;
                }

                if (t is TermColor && colorIndex < 0)
                {
                    colorIndex = i;
                }
                else if (t is TermIdent && ((TermIdent)t).Value.ToLower() == "inset" && insetIndex < 0)
                {
                    insetIndex = i;
                }
                else if (t is TermLength && lastLengthIndex < 0 || (lastLengthIndex > insetIndex && lastLengthIndex > colorIndex))
                {
                    if (lengthCount >= 4)
                    {
                        return false;
                    }
                    lastLengthIndex = i;
                    lengthCount++;
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }

            if (lengthCount < 2)
            {
                return false;
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_BoxShadow.component_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBoxSizing(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBoxSizing(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_BoxSizing>(typeof(StyleParserCS.css.CSSProperty_BoxSizing), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFontFamily(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFontFamily(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator font = new FontVariator();
            Variator font = new FontVariator();
            //ORIGINAL LINE: return font.tryMultiTermVariant(FontVariator.FAMILY, properties, values, d.toArray(new StyleParserCS.css.Term<?>[0]));
            return font.tryMultiTermVariant(FontVariator.FAMILY, properties, values, d.ToArray());
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFontSize(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFontSize(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator font = new FontVariator();
            Variator font = new FontVariator();
            return font.tryOneTermVariant(FontVariator.SIZE, d, properties, values);

        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFontStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFontStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator font = new FontVariator();
            Variator font = new FontVariator();
            return font.tryOneTermVariant(FontVariator.STYLE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFontVariant(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFontVariant(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator font = new FontVariator();
            Variator font = new FontVariator();
            return font.tryOneTermVariant(FontVariator.VARIANT, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFontWeight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFontWeight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator font = new FontVariator();
            Variator font = new FontVariator();
            return font.tryOneTermVariant(FontVariator.WEIGHT, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFont(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFont(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator font = new FontVariator();
            font.assignTermsFromDeclaration(d);
            font.assignDefaults(properties, values);
            return font.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processLineHeight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processLineHeight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator font = new FontVariator();
            Variator font = new FontVariator();
            return font.tryOneTermVariant(FontVariator.LINE_HEIGHT, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTabSize(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTabSize(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericIntegerOrLength(typeof(StyleParserCS.css.CSSProperty_TabSize), StyleParserCS.css.CSSProperty_TabSize.integer, StyleParserCS.css.CSSProperty_TabSize.length, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTop(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTop(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Top), StyleParserCS.css.CSSProperty_Top.length, StyleParserCS.css.CSSProperty_Top.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processRight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processRight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Right), StyleParserCS.css.CSSProperty_Right.length, StyleParserCS.css.CSSProperty_Right.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBottom(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBottom(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Bottom), StyleParserCS.css.CSSProperty_Bottom.length, StyleParserCS.css.CSSProperty_Bottom.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processLeft(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processLeft(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Left), StyleParserCS.css.CSSProperty_Left.length, StyleParserCS.css.CSSProperty_Left.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransform(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransform(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // just a simple value (e.g. "none")
            if (d.Count == 1 && Decoder.genericOneIdent<CSSProperty_Transform>(typeof(StyleParserCS.css.CSSProperty_Transform), d, properties))
            {
                return true;
            }
            else
            {

                TermList list = tf.createList();

                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : d.asList())
                foreach (Term t in d.asList())
                {
                    if (t is StyleParserCS.css.TermFunction_TransformFunction)
                    {
                        list.Add(t);
                    }
                    else
                    {
                        return false;
                    }
                }
                // there is nothing in list after parsing
                if (list.Count == 0)
                {
                    return false;
                }

                properties["transform"] = StyleParserCS.css.CSSProperty_Transform.list_values;
                values["transform"] = (Term)list;
                return true;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransformOrigin(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransformOrigin(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1 && Decoder.genericTermIdent<CSSProperty_BorderSpacing>(typeof(StyleParserCS.css.CSSProperty_BorderSpacing), d[0], Decoder.ALLOW_INH, d.Property, properties))
            {
                return true; //must be 'inherit'
            }
            else if (d.Count >= 1 && d.Count <= 3)
            {
                TermLengthOrPercent hpos = null;
                TermLengthOrPercent vpos = null;
                TermLength zpos = null;
                //generic check and assign recognizable keywords
                for (int i = 0; i < d.Count; i++)
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(i);
                    Term term = d[i];
                    if (term is TermIdent)
                    {
                        string value = ((TermIdent)term).Value;
                        if ("top".Equals(value))
                        {
                            if (vpos == null)
                            {
                                vpos = tf.createPercent(0.0f);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if ("bottom".Equals(value))
                        {
                            if (vpos == null)
                            {
                                vpos = tf.createPercent(100.0f);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if ("left".Equals(value))
                        {
                            if (hpos == null)
                            {
                                hpos = tf.createPercent(0.0f);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if ("right".Equals(value))
                        {
                            if (hpos == null)
                            {
                                hpos = tf.createPercent(100.0f);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if ("center".Equals(value))
                        {
                            //skip for this iteration
                        }
                        else
                        {
                            return false; //unknown keyword
                        }
                    }
                    else if (term is TermLengthOrPercent)
                    {
                        if (i > 1 && ((TermLengthOrPercent)term).Percentage)
                        {
                            return false; //percentages are only allowed for arguments 1 and 2
                        }
                    }
                    else
                    {
                        return false; //invalid value (not keyword nor length nor percentage)
                    }
                }
                //assign 'center' or numeric values
                for (int i = 0; i < d.Count; i++)
                {
                    TermLengthOrPercent value = null;
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(i);
                    Term term = d[i];
                    if (i < 2) //first two arguments
                    {
                        if (term is TermIdent)
                        {
                            if ("center".Equals(((TermIdent)term).Value))
                            {
                                value = tf.createPercent(50.0f);
                            }
                        }
                        else
                        {
                            value = (TermLengthOrPercent)term;
                        }

                        if (value != null)
                        {
                            if (hpos == null)
                            {
                                hpos = value;
                            }
                            else if (vpos == null)
                            {
                                vpos = value;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else //last argument, must be length
                    {
                        zpos = (TermLength)term;
                    }
                }
                //replace null values by defaults
                if (hpos == null)
                {
                    hpos = tf.createPercent(50.0f);
                }
                if (vpos == null)
                {
                    vpos = tf.createPercent(50.0f);
                }
                if (zpos == null)
                {
                    zpos = tf.createLength(0.0f);
                }
                //publish the values
                TermList list = tf.createList();
                list.Add((Term)hpos);
                list.Add((Term)vpos);
                list.Add((Term)zpos);
                properties["transform-origin"] = StyleParserCS.css.CSSProperty_TransformOrigin.list_values;
                values["transform-origin"] = (Term)list;
                return true;
            }
            else
            {
                return false; //invalid number of arguments
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Width), StyleParserCS.css.CSSProperty_Width.length, StyleParserCS.css.CSSProperty_Width.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processHeight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processHeight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Height), StyleParserCS.css.CSSProperty_Height.length, StyleParserCS.css.CSSProperty_Height.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processCaptionSide(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processCaptionSide(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_CaptionSide>(typeof(StyleParserCS.css.CSSProperty_CaptionSide), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processClear(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processClear(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Clear>(typeof(StyleParserCS.css.CSSProperty_Clear), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processClip(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processClip(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count != 1)
            {
                return false;
            }

            //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
            Term term = d[0];
            if (term is TermIdent)
            {
                //ORIGINAL LINE: final java.util.Set<StyleParserCS.css.CSSProperty_Clip> allowedClips = java.util.EnumSet.allOf(StyleParserCS.css.CSSProperty_Clip.class);
                // ISet<StyleParserCS.css.CSSProperty_Clip> allowedClips = new HashSet<CSSProperty_Clip>(CSS EnumSet.allOf(typeof(StyleParserCS.css.CSSProperty_Clip));
                ISet<CSSProperty_Clip> allowedClips = CSSProperty_Clip.List.ToHashSet();
                StyleParserCS.css.CSSProperty_Clip clip = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_Clip), allowedClips, (TermIdent)term);
                if (clip != null)
                {
                    properties["clip"] = clip;
                    return true;
                }
                return false;
            }
            else if (term is TermRect)
            {
                return Decoder.genericTerm(typeof(TermRect), term, "clip", StyleParserCS.css.CSSProperty_Clip.shape, ValueRange.ALLOW_ALL, properties, values);
            }
            return false;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processCounterIncrement(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processCounterIncrement(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1 && Decoder.genericOneIdent<CSSProperty_CounterIncrement>(typeof(StyleParserCS.css.CSSProperty_CounterIncrement), d, properties))
            {
                return true;
            }
            // counter with increments
            else
            {
                //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> termList = decodeCounterList(d.asList(), 1);
                IList<Term> termList = decodeCounterList(d.asList(), 1);
                if (termList != null && termList.Count > 0)
                {
                    TermList list = tf.createList(termList.Count);
                    list.AddRange(termList);
                    properties["counter-increment"] = StyleParserCS.css.CSSProperty_CounterIncrement.list_values;
                    values["counter-increment"] = (Term)list;
                    return true;
                }
                return false;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processCounterReset(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processCounterReset(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1 && Decoder.genericOneIdent<CSSProperty_CounterReset>(typeof(StyleParserCS.css.CSSProperty_CounterReset), d, properties))
            {
                return true;
            }
            // counter with resets
            else
            {
                // counters are stored there
                //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> termList = decodeCounterList(d.asList(), 0);
                IList<Term> termList = decodeCounterList(d.asList(), 0);
                if (termList != null && termList.Count > 0)
                {
                    TermList list = tf.createList(termList.Count);
                    list.AddRange(termList);
                    properties["counter-reset"] = StyleParserCS.css.CSSProperty_CounterReset.list_values;
                    values["counter-reset"] = (Term)list;
                    return true;
                }
                return false;
            }
        }

        //ORIGINAL LINE: private java.util.List<StyleParserCS.css.Term<?>> decodeCounterList(java.util.List<StyleParserCS.css.Term<?>> terms, int defaultValue)
        private IList<Term> decodeCounterList(IList<Term> terms, int defaultValue)
        {
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> ret = new java.util.ArrayList<>();
            IList<Term> ret = new List<Term>();
            int i = 0;
            while (i < terms.Count)
            {
                //ORIGINAL LINE: final StyleParserCS.css.Term<?> term = terms.get(i);
                Term term = terms[i];
                if (term is TermIdent)
                {
                    //ORIGINAL LINE: final String counterName = ((StyleParserCS.css.TermIdent) term).getValue();
                    string counterName = ((TermIdent)term).Value;
                    if (i + 1 < terms.Count && terms[i + 1] is TermInteger)
                    {
                        //integer value specified after the counter name
                        int counterValue = ((TermInteger)terms[i + 1]).IntValue;
                        ret.Add((Term)tf.createPair(counterName, counterValue));
                        i += 2;
                    }
                    else
                    {
                        //only the counter name, use the default value
                        ret.Add((Term)tf.createPair(counterName, defaultValue));
                        i++;
                    }
                }
                else
                {
                    return null;
                }
            }
            return ret;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processCursor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processCursor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1 && Decoder.genericOneIdent<CSSProperty_Cursor>(typeof(StyleParserCS.css.CSSProperty_Cursor), d, properties))
            {
                return true;
            }
            else
            {

                //ORIGINAL LINE: final java.util.Set<StyleParserCS.css.CSSProperty_Cursor> allowedCursors = java.util.EnumSet.complementOf(java.util.EnumSet.of(StyleParserCS.css.CSSProperty_Cursor.INHERIT));
                // ISet<StyleParserCS.css.CSSProperty_Cursor> allowedCursors = EnumSet.complementOf(EnumSet.of(StyleParserCS.css.CSSProperty_Cursor.INHERIT));
                // TOCHECK
                ISet<CSSProperty_Cursor> allowedCursors = CSSProperty_Cursor.List.Where(x => x != CSSProperty_Cursor.INHERIT).ToHashSet();

                TermList list = tf.createList();
                StyleParserCS.css.CSSProperty_Cursor cur = null;
                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> term : d.asList())
                foreach (Term term in d.asList())
                {
                    if (term is TermURI)
                    {
                        list.Add(term);
                    }
                    else if (term is TermIdent && (cur = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_Cursor), allowedCursors, (TermIdent)term)) != null)
                    {
                        // this have to be the last cursor in sequence
                        // and only one Decoder.generic cursor is allowed
                        if (d.IndexOf(term) != d.Count - 1)
                        {
                            return false;
                        }

                        // passed as last cursor, insert into properties and values
                        properties["cursor"] = cur;
                        if (list.Count > 0)
                        {
                            values["cursor"] = (Term)list;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processDirection(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processDirection(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Direction>(typeof(StyleParserCS.css.CSSProperty_Direction), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processDisplay(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processDisplay(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Display>(typeof(StyleParserCS.css.CSSProperty_Display), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processEmptyCells(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processEmptyCells(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_EmptyCells>(typeof(StyleParserCS.css.CSSProperty_EmptyCells), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFloat(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFloat(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(CSSProperty_Floating), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processListStyleImage(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processListStyleImage(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator listStyle = new ListStyleVariator();
            Variator listStyle = new ListStyleVariator();
            return listStyle.tryOneTermVariant(ListStyleVariator.IMAGE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processListStylePosition(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processListStylePosition(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator listStyle = new ListStyleVariator();
            Variator listStyle = new ListStyleVariator();
            return listStyle.tryOneTermVariant(ListStyleVariator.POSITION, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processListStyleType(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processListStyleType(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator listStyle = new ListStyleVariator();
            Variator listStyle = new ListStyleVariator();
            return listStyle.tryOneTermVariant(ListStyleVariator.TYPE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processListStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processListStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator listStyle = new ListStyleVariator();
            listStyle.assignTermsFromDeclaration(d);
            listStyle.assignDefaults(properties, values);
            return listStyle.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMarginTop(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMarginTop(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Margin), StyleParserCS.css.CSSProperty_Margin.length, StyleParserCS.css.CSSProperty_Margin.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMarginRight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMarginRight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Margin), StyleParserCS.css.CSSProperty_Margin.length, StyleParserCS.css.CSSProperty_Margin.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMarginBottom(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMarginBottom(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Margin), StyleParserCS.css.CSSProperty_Margin.length, StyleParserCS.css.CSSProperty_Margin.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMarginLeft(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMarginLeft(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Margin), StyleParserCS.css.CSSProperty_Margin.length, StyleParserCS.css.CSSProperty_Margin.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMargin(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMargin(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Repeater margin = new MarginRepeater();
            return margin.repeatOverFourTermDeclaration(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMaxHeight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMaxHeight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_MaxHeight), StyleParserCS.css.CSSProperty_MaxHeight.length, StyleParserCS.css.CSSProperty_MaxHeight.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMaxWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMaxWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_MaxWidth), StyleParserCS.css.CSSProperty_MaxWidth.length, StyleParserCS.css.CSSProperty_MaxWidth.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMinHeight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMinHeight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_MinHeight), StyleParserCS.css.CSSProperty_MinHeight.length, StyleParserCS.css.CSSProperty_MinHeight.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processMinWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processMinWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_MinWidth), StyleParserCS.css.CSSProperty_MinWidth.length, StyleParserCS.css.CSSProperty_MinWidth.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOpacity(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOpacity(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrIntegerOrNumber(typeof(StyleParserCS.css.CSSProperty_Opacity), StyleParserCS.css.CSSProperty_Opacity.number, StyleParserCS.css.CSSProperty_Opacity.number, ValueRange.TRUNCATE_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOrphans(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOrphans(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrInteger(typeof(StyleParserCS.css.CSSProperty_Orphans), StyleParserCS.css.CSSProperty_Orphans.integer, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOutlineColor(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOutlineColor(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator outline = new OutlineVariator();
            Variator outline = new OutlineVariator();
            return outline.tryOneTermVariant(OutlineVariator.COLOR, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOutlineStyle(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOutlineStyle(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator outline = new OutlineVariator();
            Variator outline = new OutlineVariator();
            return outline.tryOneTermVariant(OutlineVariator.STYLE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOutlineWidth(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOutlineWidth(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: final Variator outline = new OutlineVariator();
            Variator outline = new OutlineVariator();
            return outline.tryOneTermVariant(OutlineVariator.WIDTH, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOutline(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOutline(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator outline = new OutlineVariator();
            outline.assignTermsFromDeclaration(d);
            outline.assignDefaults(properties, values);
            return outline.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOverflow(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOverflow(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
                Term term = d[0];
                if (term is TermIdent)
                {
                    return Decoder.genericProperty<CSSProperty_Overflow>(typeof(StyleParserCS.css.CSSProperty_Overflow), (TermIdent)term, Decoder.ALLOW_INH, properties, "overflow-x") && Decoder.genericProperty(typeof(StyleParserCS.css.CSSProperty_Overflow), (TermIdent)term, Decoder.ALLOW_INH, properties, "overflow-y");
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

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOverflowX(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOverflowX(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Overflow>(typeof(StyleParserCS.css.CSSProperty_Overflow), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOverflowY(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOverflowY(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Overflow>(typeof(StyleParserCS.css.CSSProperty_Overflow), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPaddingTop(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPaddingTop(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Padding), StyleParserCS.css.CSSProperty_Padding.length, StyleParserCS.css.CSSProperty_Padding.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPaddingRight(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPaddingRight(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Padding), StyleParserCS.css.CSSProperty_Padding.length, StyleParserCS.css.CSSProperty_Padding.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPaddingBottom(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPaddingBottom(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Padding), StyleParserCS.css.CSSProperty_Padding.length, StyleParserCS.css.CSSProperty_Padding.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPaddingLeft(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPaddingLeft(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_Padding), StyleParserCS.css.CSSProperty_Padding.length, StyleParserCS.css.CSSProperty_Padding.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPadding(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPadding(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Repeater padding = new PaddingRepeater();
            return padding.repeatOverFourTermDeclaration(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPageBreakAfter(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPageBreakAfter(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_PageBreak>(typeof(StyleParserCS.css.CSSProperty_PageBreak), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPageBreakBefore(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPageBreakBefore(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_PageBreak>(typeof(StyleParserCS.css.CSSProperty_PageBreak), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPageBreakInside(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPageBreakInside(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_PageBreakInside>(typeof(StyleParserCS.css.CSSProperty_PageBreakInside), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processPosition(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processPosition(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Position>(typeof(StyleParserCS.css.CSSProperty_Position), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processQuotes(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processQuotes(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1 && Decoder.genericTermIdent<CSSProperty_Quotes>(typeof(StyleParserCS.css.CSSProperty_Quotes), d[0], Decoder.ALLOW_INH, "quotes", properties))
            {
                return true;
            }
            else
            {
                TermList list = tf.createList();
                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> term : d.asList())
                foreach (Term term in d.asList())
                {
                    if (term is TermString)
                    {
                        list.Add(term);
                    }
                    else
                    {
                        return false;
                    }
                }

                // there are pairs of quotes
                if (list.Count > 0 && list.Count % 2 == 0)
                {
                    properties["quotes"] = StyleParserCS.css.CSSProperty_Quotes.list_values;
                    values["quotes"] = (Term)list;
                    return true;
                }
                return false;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTableLayout(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTableLayout(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_TableLayout>(typeof(StyleParserCS.css.CSSProperty_TableLayout), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTextAlign(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTextAlign(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_TextAlign>(typeof(StyleParserCS.css.CSSProperty_TextAlign), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTextDecoration(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTextDecoration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            //ORIGINAL LINE: final java.util.Set<StyleParserCS.css.CSSProperty_TextDecoration> availableDecorations = java.util.EnumSet.of(StyleParserCS.css.CSSProperty_TextDecoration.BLINK, StyleParserCS.css.CSSProperty_TextDecoration.LINE_THROUGH, StyleParserCS.css.CSSProperty_TextDecoration.OVERLINE, StyleParserCS.css.CSSProperty_TextDecoration.UNDERLINE);
            // ISet<StyleParserCS.css.CSSProperty_TextDecoration> availableDecorations = EnumSet.of(
            // StyleParserCS.css.CSSProperty_TextDecoration.BLINK,
            // StyleParserCS.css.CSSProperty_TextDecoration.LINE_THROUGH,
            // StyleParserCS.css.CSSProperty_TextDecoration.OVERLINE,
            // StyleParserCS.css.CSSProperty_TextDecoration.UNDERLINE);
            ISet<CSSProperty_TextDecoration> availableDecorations = new HashSet<CSSProperty_TextDecoration>() { 
                CSSProperty_TextDecoration.BLINK, 
                CSSProperty_TextDecoration.LINE_THROUGH, 
                CSSProperty_TextDecoration.OVERLINE, 
                CSSProperty_TextDecoration.UNDERLINE 
            };
            // it one term
            if (d.Count == 1)
            {
                return Decoder.genericOneIdent<CSSProperty_TextDecoration>(typeof(StyleParserCS.css.CSSProperty_TextDecoration), d, properties);
            }
            // there are more terms, we have to construct list
            else
            {
                TermList list = tf.createList();
                StyleParserCS.css.CSSProperty_TextDecoration dec = null;
                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> term : d.asList())
                foreach (Term term in d.asList())
                {
                    if (term is TermIdent && (dec = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_TextDecoration), availableDecorations, (TermIdent)term)) != null)
                    {
                        // construct term with value of parsed decoration
                        list.Add((Term)tf.createTerm(dec));
                    }
                    else
                    {
                        return false;
                    }
                }
                if (list.Count > 0)
                {
                    properties["text-decoration"] = StyleParserCS.css.CSSProperty_TextDecoration.list_values;
                    values["text-decoration"] = (Term)list;
                    return true;
                }
                return false;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTextIndent(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTextIndent(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_TextIndent), StyleParserCS.css.CSSProperty_TextIndent.length, StyleParserCS.css.CSSProperty_TextIndent.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTextTransform(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTextTransform(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_TextTransform>(typeof(StyleParserCS.css.CSSProperty_TextTransform), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processUnicodeBidi(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processUnicodeBidi(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_UnicodeBidi>(typeof(StyleParserCS.css.CSSProperty_UnicodeBidi), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processUnicodeRange(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processUnicodeRange(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count > 0)
            {
                TermList list = tf.createList();
                for (int i = 0; i < d.Count; i++)
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(i);
                    Term term = d[i];
                    if (term is TermUnicodeRange && ((i == 0 && term.Operator == null) || (i != 0 && term.Operator == Term_Operator.COMMA)))
                    {
                        list.Add(term);
                    }
                    else
                    {
                        return false;
                    }
                }
                properties["unicode-range"] = StyleParserCS.css.CSSProperty_UnicodeRange.list_values;
                values["unicode-range"] = (Term)list;
                return true;
            }
            else
            {
                return false;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processVerticalAlign(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processVerticalAlign(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_VerticalAlign), StyleParserCS.css.CSSProperty_VerticalAlign.length, StyleParserCS.css.CSSProperty_VerticalAlign.percentage, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processVisibility(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processVisibility(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_Visibility>(typeof(StyleParserCS.css.CSSProperty_Visibility), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processWhiteSpace(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processWhiteSpace(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent<CSSProperty_WhiteSpace>(typeof(StyleParserCS.css.CSSProperty_WhiteSpace), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processWidows(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processWidows(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrInteger(typeof(StyleParserCS.css.CSSProperty_Widows), StyleParserCS.css.CSSProperty_Widows.integer, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processWordSpacing(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processWordSpacing(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLength(typeof(StyleParserCS.css.CSSProperty_WordSpacing), StyleParserCS.css.CSSProperty_WordSpacing.length, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processLetterSpacing(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processLetterSpacing(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLength(typeof(StyleParserCS.css.CSSProperty_LetterSpacing), StyleParserCS.css.CSSProperty_LetterSpacing.length, ValueRange.ALLOW_ALL, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processZIndex(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processZIndex(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrInteger(typeof(StyleParserCS.css.CSSProperty_ZIndex), StyleParserCS.css.CSSProperty_ZIndex.integer, ValueRange.ALLOW_ALL, d, properties, values);
        }

        /// <summary>
        /// Processes an unknown property and stores its value. Unknown properties containing
        /// multiple values are ignored (the interpretation is not clear).
        /// </summary>
        /// <param name="d"> the declaration. </param>
        /// <param name="properties"> the properties. </param>
        /// <param name="values"> the values.
        /// </param>
        /// <returns> <code>true</code>, if the property has been pared successfully </returns>
        private bool processAdditionalCSSGenericProperty(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (d.Count == 1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
                Term term = d[0];

                if (term is TermIdent)
                {
                    return Decoder.genericProperty(typeof(StyleParserCS.css.CSSProperty_GenericCSSPropertyProxy), (TermIdent)term, true, properties, d.Property);
                }
                else
                {
                    return Decoder.genericTerm<CSSProperty>(typeof(TermLength), term, d.Property, null, ValueRange.ALLOW_ALL, properties, values) ||
                        Decoder.genericTerm<CSSProperty>(typeof(TermPercent), term, d.Property, null, ValueRange.ALLOW_ALL, properties, values) ||
                        Decoder.genericTerm<CSSProperty>(typeof(TermInteger), term, d.Property, null, ValueRange.ALLOW_ALL, properties, values) ||
                        Decoder.genericTermColor<CSSProperty>(term, d.Property, null, properties, values);
                }
            }
            else
            {
                // log.warn("Ignoring unsupported property " + d.Property + " with multiple values");
                return false;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlex(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlex(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator variator = new FlexVariator();
            variator.assignTermsFromDeclaration(d);
            variator.assignDefaults(properties, values);
            return variator.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlexFlow(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlexFlow(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Variator variator = new FlexFlowVariator();
            variator.assignTermsFromDeclaration(d);
            variator.assignDefaults(properties, values);
            return variator.vary(properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlexBasis(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlexBasis(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_FlexBasis), StyleParserCS.css.CSSProperty_FlexBasis.length, StyleParserCS.css.CSSProperty_FlexBasis.percentage, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlexDirection(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlexDirection(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_FlexDirection), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlexWrap(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlexWrap(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_FlexWrap), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlexGrow(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlexGrow(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrIntegerOrNumber(typeof(StyleParserCS.css.CSSProperty_FlexGrow), StyleParserCS.css.CSSProperty_FlexGrow.number, StyleParserCS.css.CSSProperty_FlexGrow.number, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFlexShrink(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFlexShrink(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrIntegerOrNumber(typeof(StyleParserCS.css.CSSProperty_FlexShrink), StyleParserCS.css.CSSProperty_FlexShrink.number, StyleParserCS.css.CSSProperty_FlexShrink.number, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processJustifyContent(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processJustifyContent(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_JustifyContent), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAlignContent(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAlignContent(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AlignContent), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAlignItems(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAlignItems(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AlignItems), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAlignSelf(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAlignSelf(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AlignSelf), d, properties);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processOrder(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processOrder(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericInteger(typeof(StyleParserCS.css.CSSProperty_Order), StyleParserCS.css.CSSProperty_Order.integer, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processContent(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processContent(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // content contains no explicit values
            if (d.Count == 1 && Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_Content), d, properties))
            {
                return true;
            }
            else
            {

                // valid term idents
                //ORIGINAL LINE: final java.util.Set<String> validTermIdents = new java.util.HashSet<String>(java.util.Arrays.asList("open-quote", "close-quote", "no-open-quote", "no-close-quote"));
                ISet<string> validTermIdents = new HashSet<string>() { "open-quote", "close-quote", "no-open-quote", "no-close-quote" };

                TermList list = tf.createList();

                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : d.asList())
                foreach (Term t in d.asList())
                {
                    // one of valid terms
                    if (t is TermIdent && validTermIdents.Contains(((TermIdent)t).Value.ToLower()))
                    {
                        list.Add(t);
                    }
                    else if (t is TermString)
                    {
                        list.Add(t);
                    }
                    else if (t is TermURI)
                    {
                        list.Add(t);
                    }
                    else if (t is StyleParserCS.css.TermFunction_CounterFunction || t is StyleParserCS.css.TermFunction_Attr)
                    {
                        list.Add(t);
                    }
                    else
                    {
                        return false;
                    }
                }
                // there is nothing in list after parsing
                if (list.Count == 0)
                {
                    return false;
                }

                properties["content"] = StyleParserCS.css.CSSProperty_Content.list_values;
                values["content"] = (Term)list;
                return true;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processFilter(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processFilter(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // single ident: none, or global ones
            if (d.Count == 1 && Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_Filter), d, properties))
            {
                return true;
            }
            else
            {
                //list of uri() or <filter-function> expected
                TermList list = tf.createList();

                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : d.asList())
                foreach (Term t in d.asList())
                {
                    if (t is StyleParserCS.css.TermFunction_FilterFunction)
                    {
                        list.Add(t);
                    }
                    else if (t is TermURI)
                    {
                        list.Add(t);
                    }
                    else
                    {
                        return false;
                    }
                }
                // there is nothing in list after parsing
                if (list.Count == 0)
                {
                    return false;
                }

                properties["filter"] = StyleParserCS.css.CSSProperty_Filter.list_values;
                values["filter"] = (Term)list;
                return true;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processBackdropFilter(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processBackdropFilter(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // single ident: none, or global ones
            if (d.Count == 1 && Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_BackdropFilter), d, properties))
            {
                return true;
            }
            else
            {
                //list of uri() or <filter-function> expected
                TermList list = tf.createList();

                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : d.asList())
                foreach (Term t in d.asList())
                {
                    if (t is StyleParserCS.css.TermFunction_FilterFunction)
                    {
                        list.Add(t);
                    }
                    else if (t is TermURI)
                    {
                        list.Add(t);
                    }
                    else
                    {
                        return false;
                    }
                }
                // there is nothing in list after parsing
                if (list.Count == 0)
                {
                    return false;
                }

                properties["backdrop-filter"] = StyleParserCS.css.CSSProperty_BackdropFilter.list_values;
                values["backdrop-filter"] = (Term)list;
                return true;
            }
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGrid(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGrid(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            // <'grid-template'> 
            // | <'grid-template-rows'> / [ auto-flow && dense? ] <'grid-auto-columns'>?
            // | [ auto-flow && dense? ] <'grid-auto-rows'>? / <'grid-template-columns'>
            Declaration templateDecl = rf.createDeclaration(d);
            templateDecl.Property = "grid-template";
            if (processGridTemplate(templateDecl, properties, values))
            {
                return true;
            }

            bool beforeSlash = true;
            bool autoFlowBeforeSlash = false;
            Declaration autoFlowDecl = (Declaration)rf.createDeclaration().unlock();
            autoFlowDecl.Property = "grid-auto-flow";
            Declaration templateRowsDecl = (Declaration)rf.createDeclaration().unlock();
            templateRowsDecl.Property = "grid-template-rows";
            Declaration autoRowsDecl = (Declaration)rf.createDeclaration().unlock();
            autoRowsDecl.Property = "grid-auto-rows";
            Declaration templateColumnsDecl = (Declaration)rf.createDeclaration().unlock();
            templateColumnsDecl.Property = "grid-template-columns";
            Declaration autoColumnsDecl = (Declaration)rf.createDeclaration().unlock();
            autoColumnsDecl.Property = "grid-auto-columns";

            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t.Operator == Term_Operator.SLASH)
                {
                    beforeSlash = false;
                }

                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_Grid), null, (TermIdent)t);
                    if (StyleParserCS.css.CSSProperty_Grid.AUTO_FLOW.Equals(property))
                    {
                        if (beforeSlash)
                        {
                            autoFlowDecl.Add((Term)tf.createIdent("row"));
                        }
                        else
                        {
                            autoFlowDecl.Add((Term)tf.createIdent("column"));
                        }
                        autoFlowBeforeSlash = beforeSlash;
                        continue;
                    }
                    else
                    {
                        property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridAutoFlow), null, (TermIdent)t);
                        if (StyleParserCS.css.CSSProperty_GridAutoFlow.DENSE.Equals(property))
                        {
                            autoFlowDecl.Add(t);
                            continue;
                        }
                    }
                }

                if (autoFlowDecl.Count == 0)
                {
                    if (beforeSlash)
                    {
                        templateRowsDecl.Add(t);
                    }
                }
                else
                {
                    if (beforeSlash)
                    {
                        autoRowsDecl.Add(t);
                    }
                    else if (autoFlowBeforeSlash)
                    {
                        templateColumnsDecl.Add(t);
                    }
                    else
                    {
                        autoColumnsDecl.Add(t);
                    }
                }
            }
            processGridAutoRows(autoRowsDecl, properties, values);
            processGridAutoColumns(autoColumnsDecl, properties, values);

            return processGridAutoFlow(autoFlowDecl, properties, values) && (processGridTemplateRows(templateRowsDecl, properties, values) || processGridTemplateColumns(templateColumnsDecl, properties, values));
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridGap(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridGap(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            //ORIGINAL LINE: StyleParserCS.css.Term<?> rowGapTerm, columnGapTerm;
            Term rowGapTerm, columnGapTerm;
            switch (d.Count)
            {
                case 1:
                    rowGapTerm = columnGapTerm = d[0];
                    break;
                case 2:
                    rowGapTerm = d[0];
                    columnGapTerm = d[1];
                    break;
                default:
                    return false;
            }
            return (Decoder.genericTermIdent(typeof(StyleParserCS.css.CSSProperty_GridGap), rowGapTerm, Decoder.ALLOW_INH, "grid-row-gap", properties) || Decoder.genericTermLength(rowGapTerm, "grid-row-gap", StyleParserCS.css.CSSProperty_GridGap.length, ValueRange.DISALLOW_NEGATIVE, properties, values) || Decoder.genericTerm(typeof(TermPercent), rowGapTerm, "grid-row-gap", StyleParserCS.css.CSSProperty_GridGap.length, ValueRange.DISALLOW_NEGATIVE, properties, values)) && (Decoder.genericTermIdent(typeof(StyleParserCS.css.CSSProperty_GridGap), columnGapTerm, Decoder.ALLOW_INH, "grid-column-gap", properties) || Decoder.genericTermLength(columnGapTerm, "grid-column-gap", StyleParserCS.css.CSSProperty_GridGap.length, ValueRange.DISALLOW_NEGATIVE, properties, values) || Decoder.genericTerm(typeof(TermPercent), columnGapTerm, "grid-column-gap", StyleParserCS.css.CSSProperty_GridGap.length, ValueRange.DISALLOW_NEGATIVE, properties, values));
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridRowGap(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridRowGap(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_GridGap), StyleParserCS.css.CSSProperty_GridGap.length, StyleParserCS.css.CSSProperty_GridGap.length, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridColumnGap(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridColumnGap(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_GridGap), StyleParserCS.css.CSSProperty_GridGap.length, StyleParserCS.css.CSSProperty_GridGap.length, ValueRange.DISALLOW_NEGATIVE, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridArea(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridArea(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processNStartEnds(4, new string[] { "grid-row-start", "grid-column-start", "grid-row-end", "grid-column-end" }, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridRow(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridRow(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processNStartEnds(2, new string[] { "grid-row-start", "grid-row-end" }, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridColumn(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridColumn(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processNStartEnds(2, new string[] { "grid-column-start", "grid-column-end" }, d, properties, values);
        }

        private bool processNStartEnds(int n, string[] propertyNames, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (n != propertyNames.Length)
            {
                return false;
            }
            TermList[] lists = new TermList[n];
            for (int i = 0; i < n; i++)
            {
                lists[i] = tf.createList();
            }
            IDictionary<string, TermList> identOnly = new Dictionary<string, TermList>();
            int listIndex = 0;
            // auto | <custom-ident> | [ <integer> && <custom-ident>? ] | [ span && [ <integer> || <custom-ident> ] ]
            int valueValue = 0;
            int valueIndex = -1;
            int spanIndex = -1;
            int identIndex = -1;
            bool autoSet = false;

            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t.Operator == Term_Operator.SLASH)
                {
                    if (!autoSet && spanIndex < 0 && valueIndex < 0)
                    {
                        identOnly[propertyNames[listIndex]] = lists[listIndex];
                    }
                    listIndex++;
                    valueIndex = -1;
                    spanIndex = -1;
                    identIndex = -1;
                    autoSet = false;
                    if (listIndex >= n)
                    {
                        return false;
                    }
                }
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridStartEnd), null, (TermIdent)t);
                    if (StyleParserCS.css.CSSProperty_GridStartEnd.AUTO.Equals(property) && lists[listIndex].Count == 0)
                    {
                        autoSet = true;
                    }
                    else if (StyleParserCS.css.CSSProperty_GridStartEnd.SPAN.Equals(property) && spanIndex < 0 && !autoSet && (valueIndex < 0 || valueValue > 0))
                    {
                        spanIndex = i;
                    }
                    else if (property == null && identIndex < 0 && (spanIndex < 0 || valueIndex < 0 || spanIndex < valueIndex) && !autoSet)
                    {
                        identIndex = i;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (t is TermInteger && ((TermInteger)t).IntValue != 0 && (spanIndex < 0 || ((TermInteger)t).IntValue > 0) && valueIndex < 0 && (identIndex < 0 || identIndex > spanIndex) && !autoSet)
                {
                    valueValue = ((TermInteger)t).IntValue;
                    valueIndex = i;
                }
                else
                {
                    return false;
                }
                lists[listIndex].Add(t);
            }
            if (!autoSet && spanIndex < 0 && valueIndex < 0)
            {
                identOnly[propertyNames[listIndex]] = lists[listIndex];
            }

            for (int i = 1; i < n; i++)
            {
                if (i <= listIndex)
                { // Property set explicitly
                    setStartEndProperties(propertyNames[i], lists[i], properties, values);
                }
                else
                {
                    switch (propertyNames[i])
                    { // Inherit indentifier from other property from declaration
                        case "grid-column-start":
                            if (identOnly.ContainsKey("grid-row-start"))
                            {
                                setStartEndProperties(propertyNames[i], identOnly.GetValue("grid-row-start"), properties, values);
                            }
                            break;
                        case "grid-row-end":
                            if (identOnly.ContainsKey("grid-row-start"))
                            {
                                setStartEndProperties(propertyNames[i], identOnly.GetValue("grid-row-start"), properties, values);
                            }
                            break;
                        case "grid-column-end":
                            if (identOnly.ContainsKey("grid-column-start"))
                            {
                                setStartEndProperties(propertyNames[i], identOnly.GetValue("grid-column-start"), properties, values);
                            }
                            else if (identOnly.ContainsKey("grid-row-start"))
                            {
                                setStartEndProperties(propertyNames[i], identOnly.GetValue("grid-row-start"), properties, values);
                            }
                            break;
                    }
                }
            }
            return setStartEndProperties(propertyNames[0], lists[0], properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridRowStart(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridRowStart(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridStartEnd(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridRowEnd(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridRowEnd(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridStartEnd(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridColumnStart(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridColumnStart(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridStartEnd(d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridColumnEnd(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridColumnEnd(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridStartEnd(d, properties, values);
        }

        private bool processGridStartEnd(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (d.Count == 0)
            {
                return false;
            }
            if (Decoder.genericOneIdentOrInteger(typeof(StyleParserCS.css.CSSProperty_GridStartEnd), StyleParserCS.css.CSSProperty_GridStartEnd.number, ValueRange.DISALLOW_ZERO, d, properties, values))
            {
                return !StyleParserCS.css.CSSProperty_GridStartEnd.SPAN.Equals(properties.GetValue(d.Property));
            }
            // auto | <custom-ident> | [ <integer> && <custom-ident>? ] | [ span && [ <integer> || <custom-ident> ] ]
            int valueValue = 0;
            int valueIndex = -1;
            int spanIndex = -1;
            int identIndex = -1;
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridStartEnd), null, (TermIdent)t);
                    if (StyleParserCS.css.CSSProperty_GridStartEnd.SPAN.Equals(property) && spanIndex < 0 && (valueIndex < 0 || valueValue > 0))
                    {
                        spanIndex = i;
                    }
                    else if (property == null && identIndex < 0 && (spanIndex < 0 || valueIndex < 0 || spanIndex < valueIndex))
                    {
                        identIndex = i;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (t is TermInteger && ((TermInteger)t).IntValue != 0 && (spanIndex < 0 || ((TermInteger)t).IntValue > 0) && valueIndex < 0 && (identIndex < 0 || identIndex > spanIndex))
                {
                    valueValue = ((TermInteger)t).IntValue;
                    valueIndex = i;
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            return setStartEndProperties(d.Property, list, properties, values);
        }

        private bool setStartEndProperties(string propertyName, TermList list, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            switch (list.Count)
            {
                case 0:
                    return false;
                case 1:
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> single = list.get(0);
                    Term single = list[0];
                    CSSProperty property;
                    if (single is TermIdent)
                    {
                        CSSProperty identProperty = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridStartEnd), null, (TermIdent)single);
                        if (StyleParserCS.css.CSSProperty_GridStartEnd.SPAN.Equals(identProperty))
                        {
                            return false;
                        }
                        else if ((CSSProperty_GridStartEnd)identProperty == StyleParserCS.css.CSSProperty_GridStartEnd.AUTO)
                        {
                            property = identProperty;
                        }
                        else
                        {
                            property = StyleParserCS.css.CSSProperty_GridStartEnd.identificator;
                        }
                    }
                    else if (single is TermInteger)
                    {
                        property = StyleParserCS.css.CSSProperty_GridStartEnd.number;
                    }
                    else
                    {
                        return false;
                    }
                    properties[propertyName] = property;
                    values[propertyName] = single;
                    break;
                default:
                    properties[propertyName] = StyleParserCS.css.CSSProperty_GridStartEnd.component_values;
                    values[propertyName] = (Term)list;
                    break;
            }
            return true;
        }

        private bool processGridTemplate(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            d.Property = "grid-template-areas";
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_GridTemplateAreas), d, properties))
            {
                return true;
            }
            // none 
            // | grid-template-rows / grid-template-columns
            // | (<areaString> <rowLenght>?)+ (/ <colLenght>+)?
            Declaration rowsDecl = (Declaration)rf.createDeclaration().unlock();
            rowsDecl.Property = "grid-template-rows";
            Declaration columnsDecl = (Declaration)rf.createDeclaration().unlock();
            columnsDecl.Property = "grid-template-columns";
            bool beforeSlash = true;
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t.Operator == Term_Operator.SLASH)
                {
                    beforeSlash = false;
                }
                if (beforeSlash)
                {
                    rowsDecl.Add(t);
                }
                else
                {
                    columnsDecl.Add(t);
                }
            }
            if (processGridTemplateRows(rowsDecl, properties, values) && processGridTemplateColumns(columnsDecl, properties, values))
            {
                return true;
            }

            TermList areasTerms = tf.createList();
            TermList rowsTerms = tf.createList();
            TermList columnsTerms = tf.createList();
            beforeSlash = true;
            bool bracketedIdentUsed = false;
            bool rowLengthSet = false;
            int areasInRow = 0;
            IList<string[]> map = new List<string[]>();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t.Operator == Term_Operator.SLASH)
                {
                    bracketedIdentUsed = false;
                    beforeSlash = false;
                }
                if (t is TermString)
                {
                    string[] rowAreas = ValidationUtils.getAreas(((TermString)t).Value);
                    if (rowAreas.Length == 0 || (map.Count > 0 && rowAreas.Length != areasInRow) || !beforeSlash)
                    {
                        return false;
                    }
                    areasInRow = rowAreas.Length;
                    map.Add(rowAreas);
                    rowLengthSet = false;
                    areasTerms.Add(t);
                }
                else if (t is TermBracketedIdents)
                {
                    if (bracketedIdentUsed)
                    {
                        return false;
                    }
                    else
                    {
                        bracketedIdentUsed = true;
                        if (beforeSlash)
                        {
                            rowsTerms.Add(t);
                        }
                        else
                        {
                            columnsTerms.Add(t);
                        }
                    }
                }
                else if (isTermTrackBreadth(t))
                {
                    bracketedIdentUsed = false;
                    if (beforeSlash)
                    {
                        if (rowLengthSet)
                        {
                            return false;
                        }
                        else
                        {
                            rowLengthSet = true;
                            rowsTerms.Add(t);
                        }
                    }
                    else
                    {
                        columnsTerms.Add(t);
                    }
                }
                else
                {
                    return false;
                }
            }
            if (!ValidationUtils.containsRectangles(((List<string[]>)map).ToArray()))
            {
                return false;
            }
            properties["grid-template-areas"] = StyleParserCS.css.CSSProperty_GridTemplateAreas.list_values;
            values["grid-template-areas"] = (Term)areasTerms;
            if (rowsTerms.Count > 0)
            {
                properties["grid-template-rows"] = StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.list_values;
                values["grid-template-rows"] = (Term)rowsTerms;
            }
            if (columnsTerms.Count > 0)
            {
                properties["grid-template-columns"] = StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.list_values;
                values["grid-template-columns"] = (Term)columnsTerms;
            }
            return true;
        }

        /// <summary>
        /// <track-breadth> = <length> | <percentage> | <flex> | min-content |
        /// max-content | auto
        /// </summary>
        private bool isTermTrackBreadth(Term t)
        {
            if (t is TermLengthOrPercent)
            {
                return true;
            }
            else if (t is TermIdent)
            {
                CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridTemplateRowsColumns), null, (TermIdent)t);
                CSSProperty_GridTemplateRowsColumns colProperty = (CSSProperty_GridTemplateRowsColumns)property;
                return colProperty == StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.AUTO ||
                    colProperty == StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.MIN_CONTENT ||
                    colProperty == StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.MAX_CONTENT;
            }
            return false;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processGridTemplateAreas(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processGridTemplateAreas(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_GridTemplateAreas), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            int areasInRow = 0;
            string[][] map = new string[d.Count][];
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t is TermString)
                {
                    map[i] = ValidationUtils.getAreas(((TermString)t).Value);
                    if (map[i].Length == 0 || (i > 0 && map[i].Length != areasInRow))
                    {
                        return false;
                    }
                    areasInRow = map[i].Length;
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            if (!ValidationUtils.containsRectangles(map))
            {
                return false;
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_GridTemplateAreas.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        private bool processGridTemplateRows(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridTemplateRowsColumns(d, properties, values);
        }

        private bool processGridTemplateColumns(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridTemplateRowsColumns(d, properties, values);
        }

        private bool processGridTemplateRowsColumns(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (d.Count == 0)
            {
                return false;
            }
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_GridTemplateRowsColumns), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            bool bracketedIdentUsed = false;
            bool repeatUsed = false;
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridTemplateRowsColumns), null, (TermIdent)t);
                    if (property == null || (CSSProperty_GridTemplateRowsColumns)property == StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.NONE)
                    {
                        return false;
                    }
                }
                else if (t is TermBracketedIdents)
                {
                    if (bracketedIdentUsed)
                    {
                        return false;
                    }
                    else
                    {
                        bracketedIdentUsed = true;
                        list.Add(t);
                        continue;
                    }
                }
                else if (t is StyleParserCS.css.TermFunction_Repeat && !repeatUsed)
                {
                    repeatUsed = true;
                }
                else if (!(t is TermLengthOrPercent) && !(t is StyleParserCS.css.TermFunction_MinMax) && !(t is StyleParserCS.css.TermFunction_FitContent))
                {
                    return false;
                }
                list.Add(t);
                bracketedIdentUsed = false;
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_GridTemplateRowsColumns.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        private bool processGridAutoFlow(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_GridAutoFlow), d, properties))
            {
                return !StyleParserCS.css.CSSProperty_GridAutoFlow.DENSE.Equals(properties.GetValue(d.Property));
            }
            bool autoFlowSet = false;
            bool denseSet = false;
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridAutoFlow), null, (TermIdent)t);
                    if ((StyleParserCS.css.CSSProperty_GridAutoFlow.ROW.Equals(property) || StyleParserCS.css.CSSProperty_GridAutoFlow.COLUMN.Equals(property)) && !autoFlowSet)
                    {
                        autoFlowSet = true;
                    }
                    else if (StyleParserCS.css.CSSProperty_GridAutoFlow.DENSE.Equals(property) && !denseSet)
                    {
                        denseSet = true;
                    }
                    else
                    {
                        return false;
                    }
                    list.Add(t);
                }
                else
                {
                    return false;
                }
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_GridAutoFlow.component_values;
            values[d.Property] = (Term)list;
            return true;
        }

        private bool processGridAutoRows(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridAutoRowsOrColumns(d, properties, values);
        }

        private bool processGridAutoColumns(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processGridAutoRowsOrColumns(d, properties, values);
        }

        private bool processGridAutoRowsOrColumns(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (d.Count == 0)
            {
                return false;
            }
            if (Decoder.genericOneIdentOrLengthOrPercent(typeof(StyleParserCS.css.CSSProperty_GridAutoRowsColumns), StyleParserCS.css.CSSProperty_GridAutoRowsColumns.length, StyleParserCS.css.CSSProperty_GridAutoRowsColumns.length, ValueRange.DISALLOW_NEGATIVE, d, properties, values))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_GridAutoRowsColumns), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else if (t is TermLengthOrPercent)
                {
                    if (!isPositive(t))
                    {
                        return false;
                    }
                }
                else if (t is StyleParserCS.css.TermFunction_MinMax)
                {
                    StyleParserCS.css.TermFunction_MinMax f = (StyleParserCS.css.TermFunction_MinMax)t;
                    if (f.Min.Lenght != null)
                    {
                        if (!isPositive((Term)f.Min.Lenght))
                        {
                            return false;
                        }
                    }
                    if (f.Max.Lenght != null)
                    {
                        if (!isPositive((Term)f.Max.Lenght))
                        {
                            return false;
                        }
                    }
                }
                else if (t is StyleParserCS.css.TermFunction_FitContent)
                {
                    StyleParserCS.css.TermFunction_FitContent f = (StyleParserCS.css.TermFunction_FitContent)t;
                    if (!isPositive((Term)f.Maximum))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_GridAutoRowsColumns.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        private static bool isPositive(Term t)
        {
            if (t is TermLengthOrPercent)
            {
                if (((TermLengthOrPercent)t).Value < 0)
                {
                    return false;
                }
            }
            else if (t is TermFloatValue)
            {
                if (((TermFloatValue)t).Value < 0)
                {
                    return false;
                }
            }
            else if (t is TermTime)
            {
                if (((TermTime)t).Value < 0)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimation(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimation(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processPropertiesInList(new string[] { "animation-duration", "animation-timing-function", "animation-delay", "animation-iteration-count", "animation-direction", "animation-fill-mode", "animation-play-state", "animation-name" }, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationDelay(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationDelay(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericTime(typeof(StyleParserCS.css.CSSProperty_AnimationDelay), StyleParserCS.css.CSSProperty_AnimationDelay.time, ValueRange.DISALLOW_NEGATIVE, d, properties, values))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermTime)
                {
                    if (!isPositive(t))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationDelay.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationDirection(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationDirection(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AnimationDirection), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_AnimationDirection), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationDirection.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationDuration(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationDuration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericTime(typeof(StyleParserCS.css.CSSProperty_AnimationDuration), StyleParserCS.css.CSSProperty_AnimationDuration.time, ValueRange.DISALLOW_NEGATIVE, d, properties, values))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermTime)
                {
                    if (!isPositive(t))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationDuration.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationFillMode(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationFillMode(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AnimationFillMode), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_AnimationFillMode), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationFillMode.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationIterationCount(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationIterationCount(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdentOrInteger(typeof(StyleParserCS.css.CSSProperty_AnimationIterationCount), StyleParserCS.css.CSSProperty_AnimationIterationCount.number, ValueRange.DISALLOW_NEGATIVE, d, properties, values))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (i > 0 && t.Operator != Term_Operator.COMMA)
                {
                    return false;
                }
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_AnimationIterationCount), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else if (t is TermFloatValue)
                {
                    if (!isPositive(t))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationIterationCount.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationName(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationName(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AnimationName), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (i > 0 && t.Operator != Term_Operator.COMMA || !(t is TermIdent))
                {
                    return false;
                }
                list.Add(t);
            }
            if (list.Count == 1)
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationName.custom_ident;
                values[d.Property] = list[0];
            }
            else
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationName.list_values;
                values[d.Property] = (Term)list;
            }
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationPlayState(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationPlayState(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AnimationPlayState), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_AnimationPlayState), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationPlayState.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processAnimationTimingFunction(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processAnimationTimingFunction(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_AnimationTimingFunction), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (i > 0 && t.Operator != Term_Operator.COMMA)
                {
                    return false;
                }
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_AnimationTimingFunction), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else if (!(t is StyleParserCS.css.TermFunction_TimingFunction))
                {
                    return false;
                }
                list.Add(t);
            }
            if (list.Count == 1)
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationTimingFunction.timing_function;
                values[d.Property] = list[0];
            }
            else
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_AnimationTimingFunction.list_values;
                values[d.Property] = (Term)list;
            }
            return true;
        }

        private bool processPropertiesInList(string[] propertyList, Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            Declaration subDeclaration = (Declaration)rf.createDeclaration().unlock();
            TermList[] termLists = new TermList[propertyList.Length];
            for (int i = 0; i < termLists.Length; i++)
            {
                termLists[i] = tf.createList();
            }
            bool[] propertySet = new bool[propertyList.Length];
            Array.Fill(propertySet, false);

            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                subDeclaration.Add(t);
                if (t.Operator == Term_Operator.COMMA)
                {
                    Array.Fill(propertySet, false);
                }
                for (int propertyIndex = 0; propertyIndex <= propertyList.Length; propertyIndex++)
                {
                    if (propertyIndex == propertyList.Length)
                    {
                        return false;
                    }
                    if (propertySet[propertyIndex])
                    {
                        continue;
                    }
                    subDeclaration.Property = propertyList[propertyIndex];
                    if (parseDeclaration(subDeclaration, properties, values))
                    {
                        propertySet[propertyIndex] = true;
                        t.setOperator(termLists[propertyIndex].Count == 0 ? null : Term_Operator.COMMA);
                        termLists[propertyIndex].Add(t);
                        break;
                    }
                }
                subDeclaration.Clear();
            }

            for (int propertyIndex = 0; propertyIndex < propertyList.Length; propertyIndex++)
            {
                subDeclaration.Property = propertyList[propertyIndex];
                subDeclaration.AddRange(termLists[propertyIndex]);
                if (subDeclaration.Count > 0 && !parseDeclaration(subDeclaration, properties, values))
                {
                    return false;
                }
                subDeclaration.Clear();
            }
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransition(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransition(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return processPropertiesInList(new string[] { "transition-duration", "transition-delay", "transition-timing-function", "transition-property" }, d, properties, values);
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransitionDelay(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransitionDelay(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericTime(typeof(StyleParserCS.css.CSSProperty_TransitionDelay), StyleParserCS.css.CSSProperty_TransitionDelay.time, ValueRange.DISALLOW_NEGATIVE, d, properties, values))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermTime)
                {
                    if (!isPositive(t))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_TransitionDelay.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransitionDuration(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransitionDuration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericTime(typeof(StyleParserCS.css.CSSProperty_TransitionDuration), StyleParserCS.css.CSSProperty_TransitionDuration.time, ValueRange.DISALLOW_NEGATIVE, d, properties, values))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermTime)
                {
                    if (!isPositive(t))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            properties[d.Property] = StyleParserCS.css.CSSProperty_TransitionDuration.list_values;
            values[d.Property] = (Term)list;
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransitionProperty(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransitionProperty(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_TransitionProperty), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if ((i == 0 || t.Operator == Term_Operator.COMMA) && t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_TransitionProperty), null, (TermIdent)t);
                    if ((CSSProperty_TransitionProperty)property == StyleParserCS.css.CSSProperty_TransitionProperty.NONE)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                list.Add(t);
            }
            if (list.Count == 1)
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_TransitionProperty.custom_ident;
                values[d.Property] = list[0];
            }
            else
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_TransitionProperty.list_values;
                values[d.Property] = (Term)list;
            }
            return true;
        }

        //ORIGINAL LINE: @SuppressWarnings("unused") private boolean processTransitionTimingFunction(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values)
        private bool processTransitionTimingFunction(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (Decoder.genericOneIdent(typeof(StyleParserCS.css.CSSProperty_TransitionTimingFunction), d, properties))
            {
                return true;
            }
            TermList list = tf.createList();
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> t = d.get(i);
                Term t = d[i];
                if (i > 0 && t.Operator != Term_Operator.COMMA)
                {
                    return false;
                }
                if (t is TermIdent)
                {
                    CSSProperty property = Decoder.genericPropertyRaw(typeof(StyleParserCS.css.CSSProperty_TransitionTimingFunction), null, (TermIdent)t);
                    if (property == null)
                    {
                        return false;
                    }
                }
                else if (!(t is StyleParserCS.css.TermFunction_TimingFunction))
                {
                    return false;
                }
                list.Add(t);
            }
            if (list.Count == 1)
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_TransitionTimingFunction.timing_function;
                values[d.Property] = list[0];
            }
            else
            {
                properties[d.Property] = StyleParserCS.css.CSSProperty_TransitionTimingFunction.list_values;
                values[d.Property] = (Term)list;
            }
            return true;
        }

    }

}