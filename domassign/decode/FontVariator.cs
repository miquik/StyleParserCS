using System.Collections.Generic;
using System.Text;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermPercent = StyleParserCS.css.TermPercent;
    using TermString = StyleParserCS.css.TermString;
    using CSSProperty_Font = StyleParserCS.css.CSSProperty_Font;
    using CSSProperty_FontFamily = StyleParserCS.css.CSSProperty_FontFamily;
    using CSSProperty_FontSize = StyleParserCS.css.CSSProperty_FontSize;
    using CSSProperty_FontStyle = StyleParserCS.css.CSSProperty_FontStyle;
    using CSSProperty_FontVariant = StyleParserCS.css.CSSProperty_FontVariant;
    using CSSProperty_FontWeight = StyleParserCS.css.CSSProperty_FontWeight;
    using CSSProperty_LineHeight = StyleParserCS.css.CSSProperty_LineHeight;
    using Term_Operator = StyleParserCS.css.Term_Operator;
    using System.Linq;

    /// <summary>
    /// Font variator:
    /// Grammar:
    /// <pre>
    ///  [ 
    ///      [ <'font-style'> || <'font-variant'> || <'font-weight'> ]? 
    ///      <'font-size'> 
    ///      [ / <'line-height'> ]? 
    ///      <'font-family'> 
    ///  ] 
    ///  | caption | icon | menu | message-box 
    ///  | small-caption | status-bar | inherit
    /// </pre>
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class FontVariator : Variator
    {

        public const int STYLE = 0;
        public const int VARIANT = 1;
        public const int WEIGHT = 2;
        public const int SIZE = 3;
        public const int LINE_HEIGHT = 4;
        public const int FAMILY = 5;

        public FontVariator() : base(6)
        {
            names.Add("font-style");
            types.Add(typeof(CSSProperty_FontStyle));
            names.Add("font-variant");
            types.Add(typeof(CSSProperty_FontVariant));
            names.Add("font-weight");
            types.Add(typeof(CSSProperty_FontWeight));
            names.Add("font-size");
            types.Add(typeof(CSSProperty_FontSize));
            names.Add("line-height");
            types.Add(typeof(CSSProperty_LineHeight));
            names.Add("font-family");
            types.Add(typeof(CSSProperty_FontFamily));
        }

        protected internal override bool variant(int v, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // we will use multi value functionality in
            // FAMILY branch
            int i = iteration.get();

            switch (v)
            {
                case STYLE:
                    // process font style
                    return genericTermIdent(types[STYLE], terms[i], AVOID_INH, names[STYLE], properties);
                case VARIANT:
                    // process font variant
                    return genericTermIdent(types[VARIANT], terms[i], AVOID_INH, names[VARIANT], properties);
                case WEIGHT:
                    // process font weight
                    // test against numeric values
                    //ORIGINAL LINE: final System.Nullable<int>[] fontWeight = new System.Nullable<int>[] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };
                    int?[] fontWeight = new int?[] { 100, 200, 300, 400, 500, 600, 700, 800, 900 };

                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = terms.get(i);
                    Term term = terms[i];

                    if (term is TermIdent)
                    {
                        return genericProperty(types[WEIGHT], (TermIdent)term, AVOID_INH, properties, names[WEIGHT]);
                    }
                    else if (term is TermInteger)
                    {
                        int value = ((TermInteger)term).IntValue;
                        foreach (int test in fontWeight)
                        {
                            int result = value.CompareTo(test);
                            // not found if value is smaller than currently compared
                            if (result < 0)
                            {
                                break;
                            }

                            // match
                            // construct according CSSProperty name
                            if (result == 0)
                            {
                                CSSProperty property = StyleParserCS.css.CSSProperty_Translator.valueOf(types[WEIGHT], "numeric_" + (int)value);
                                if (property == null)
                                {
                                    //log.warn("Not found numeric values for FontWeight: " + "numeric_ " + value.intValue());
                                    return false;
                                }
                                properties[names[WEIGHT]] = property;
                                return true;
                            }
                        }
                    }
                    return false;
                case SIZE:
                    return genericTermIdent(types[SIZE], terms[i], AVOID_INH, names[SIZE], properties) || genericTermLength(terms[i], names[SIZE], CSSProperty_FontSize.length, ValueRange.DISALLOW_NEGATIVE, properties, values) || genericTerm(typeof(TermPercent), terms[i], names[SIZE], CSSProperty_FontSize.percentage, ValueRange.DISALLOW_NEGATIVE, properties, values);
                case LINE_HEIGHT:
                    return genericTermIdent(types[LINE_HEIGHT], terms[i], AVOID_INH, names[LINE_HEIGHT], properties) ||
                        genericTerm(typeof(TermNumber), terms[i], names[LINE_HEIGHT], CSSProperty_LineHeight.number, ValueRange.DISALLOW_NEGATIVE, properties, values) ||
                        genericTerm(typeof(TermInteger), terms[i], names[LINE_HEIGHT], CSSProperty_LineHeight.number, ValueRange.DISALLOW_NEGATIVE, properties, values) ||
                        genericTerm(typeof(TermPercent), terms[i], names[LINE_HEIGHT], CSSProperty_LineHeight.percentage, ValueRange.DISALLOW_NEGATIVE, properties, values) ||
                        genericTerm(typeof(TermLength), terms[i], names[LINE_HEIGHT], CSSProperty_LineHeight.length, ValueRange.DISALLOW_NEGATIVE, properties, values);
                case FAMILY:
                    // all values parsed
                    TermList list = tf.createList();
                    // current font name
                    StringBuilder sb = new StringBuilder();
                    // font name was composed from multiple parts
                    bool composed = false;
                    //ORIGINAL LINE: for (StyleParserCS.css.Term<?> t : terms.subList(i, terms.size()))
                    foreach (Term t in terms.Skip(i).Take(terms.Count - i)) //subList(i, terms.Count))
                    {
                        // first item
                        if (t is TermIdent && sb.Length == 0)
                        {
                            sb.Append(((TermIdent)t).Value);
                            composed = false;
                        }
                        // next item
                        else if (t is TermIdent && sb.Length != 0 && t.Operator != Term_Operator.COMMA && t.Operator != Term_Operator.SLASH)
                        {
                            sb.Append(" ").Append(((TermIdent)t).Value);
                            composed = true;
                        }
                        // store current state
                        else if (t is TermString || (t is TermIdent && t.Operator == Term_Operator.COMMA))
                        {
                            storeFamilyName(list, sb.ToString(), composed);
                            sb = new StringBuilder();
                            composed = false;
                            // store next
                            if (t is TermString)
                            {
                                storeFamilyName(list, ((TermString)t).Value, true);
                            }
                            else
                            {
                                sb.Append(((TermString)t).Value);
                            }
                        }
                        // invalid term
                        else
                        {
                            return false;
                        }
                    }
                    // add last item
                    storeFamilyName(list, sb.ToString(), composed);

                    if (list.Count == 0)
                    {
                        return false;
                    }

                    // when only generic family is stored, there is no need to have
                    // list with one value
                    //ORIGINAL LINE: if (list.size() == 1 && (list.toArray(new StyleParserCS.css.Term<?>[0])[0] instanceof StyleParserCS.css.TermString) == false)
                    // if (list.Count == 1 && (list.ToArray(typeof(Term))[0] is TermString) == false)
                    if (list.Count == 1 && (list[0] is TermString) == false)
                    {
                        //ORIGINAL LINE: properties.put(names.get(FAMILY), (StyleParserCS.css.CSSProperty_FontFamily)(list.toArray(new StyleParserCS.css.Term<?>[0])[0]).getValue());
                        properties[names[FAMILY]] = (CSSProperty_FontFamily)(list[0]).Value;
                        return true;
                    }

                    properties[names[FAMILY]] = CSSProperty_FontFamily.list_values;
                    values[names[FAMILY]] = (Term)list;
                    // modify reference to the last element
                    iteration.set(terms.Count);
                    return true;
                default:
                    return false;
            }
        }

        protected internal override bool variantCondition(int variant, IntegerRef iteration)
        {

            switch (variant)
            {
                case STYLE:
                case VARIANT:
                case WEIGHT:
                    // must be within 3 first terms
                    return iteration.get() < 3;
                case SIZE:
                    // no condition
                    return true;
                case LINE_HEIGHT:
                    if (!variantPassed[SIZE])
                    {
                        return false;
                    }
                    return terms[iteration.get()].Operator == Term_Operator.SLASH;
                case FAMILY:
                    // requires passed size
                    return variantPassed[SIZE];
                default:
                    return false;
            }
        }

        public override bool vary(IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // special check for user interface values
            // such as "caption", "icon" or "menu"
            // this will break inheritance division into distint categories,
            // so it must be reconstructed later
            if (terms.Count == 1 && terms[0] is TermIdent)
            {

                if (checkInherit(ALL_VARIANTS, terms[0], properties))
                {
                    return true;
                }

                return genericTermIdent(typeof(CSSProperty_Font), terms[0], AVOID_INH, "font", properties);
            }
            // follow basic control flow
            return base.vary(properties, values);
        }

        private void storeFamilyName(TermList storage, string name, bool composed)
        {

            //ORIGINAL LINE: final java.util.Set<StyleParserCS.css.CSSProperty_FontFamily> allowedFamilies = java.util.EnumSet.complementOf(java.util.EnumSet.of(StyleParserCS.css.CSSProperty_FontFamily.INHERIT, StyleParserCS.css.CSSProperty_FontFamily.list_values));
            // ISet<CSSProperty_FontFamily> allowedFamilies = EnumSet.complementOf(EnumSet.of(CSSProperty_FontFamily.INHERIT, CSSProperty_FontFamily.list_values));
            ISet<CSSProperty_FontFamily> allowedFamilies = CSSProperty_FontFamily
                .List
                .Where(x => x != CSSProperty_FontFamily.INHERIT && x != CSSProperty_FontFamily.list_values)
                .ToHashSet();

            if (string.ReferenceEquals(name, null) || "".Equals(name) || name.Length == 0)
            {
                return;
            }

            // trim spaces
            name = name.Trim();

            // if composed, store directly as family name
            if (composed)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = tf.createString(name);
                TermString term = (TermString)tf.createString(name);
                if (storage.Count > 0)
                {
                    term.setOperator(Term_Operator.COMMA);
                }
                storage.Add(term);
            }
            // try to find generic name
            else
            {
                CSSProperty_FontFamily generic = genericPropertyRaw(typeof(CSSProperty_FontFamily), allowedFamilies, tf.createIdent(name));
                // generic name found,
                // store in term which value is generic font name FontFamily
                // we have to append even operator
                if (generic != null)
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = tf.createTerm(generic);
                    // TOSOLVE:
                    Term term = (Term)tf.createTerm(generic);
                    if (storage.Count > 0)
                    {
                        term.setOperator(Term_Operator.COMMA);
                    }
                    storage.Add(term);
                }
                // generic name not found, store as family name
                // we have to append even operator
                else
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> term = tf.createString(name);
                    TermString term = (TermString)tf.createString(name);
                    if (storage.Count > 0)
                    {
                        term.setOperator(Term_Operator.COMMA);
                    }
                    storage.Add(term);
                }
            }
        }

    }

}