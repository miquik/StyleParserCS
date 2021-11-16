using System;
using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using Declaration = StyleParserCS.css.Declaration;
    using StyleParserCS.css;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;
    using TermPercent = StyleParserCS.css.TermPercent;
    using CSSProperty_BorderRadius = StyleParserCS.css.CSSProperty_BorderRadius;
    using Term_Operator = StyleParserCS.css.Term_Operator;

    /// <summary>
    /// Border radius repeater
    /// 
    /// @author burgetr
    /// 
    /// </summary>
    public class BorderRadiusRepeater : Repeater
    {

        public BorderRadiusRepeater() : base(4)
        {
            this.type = typeof(CSSProperty_BorderRadius);
            names.Add("border-top-left-radius");
            names.Add("border-top-right-radius");
            names.Add("border-bottom-right-radius");
            names.Add("border-bottom-left-radius");
        }

        protected internal override bool operation(int i, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            //ORIGINAL LINE: StyleParserCS.css.Term<?> term = terms.get(i);
            Term term = terms[i];
            string name = names[i];

            if (genericTermIdent(type, terms[i], AVOID_INH, names[i], properties))
            {
                return true;
            }
            else if (term is TermList)
            {
                properties[name] = CSSProperty_BorderRadius.list_values;
                values[name] = term;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Decodes the complicated border-radius declaration into four term pairs </summary>
        //ORIGINAL LINE: public boolean repeatOverMultiTermDeclaration(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values) throws IllegalArgumentException
        public virtual bool repeatOverMultiTermDeclaration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (d.Count == 1) //one value - check for inherit
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(0);
                Term term = d[0];
                if (term is TermIdent && StyleParserCS.css.CSSProperty_Fields.INHERIT_KEYWORD.Equals(((TermIdent)term).Value, StringComparison.OrdinalIgnoreCase))
                {
                    CSSProperty property = StyleParserCS.css.CSSProperty_Translator.createInherit(type);
                    for (int i = 0; i < times; i++)
                    {
                        properties[names[i]] = property;
                    }
                    return true;
                }
            }

            //find the slash (if any)
            int slash = -1;
            for (int i = 0; i < d.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> term = d.get(i);
                Term term = d[i];
                if (term.Operator == Term_Operator.SLASH)
                {
                    slash = i;
                    break;
                }
            }
            if (slash == -1)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?>[] sterms = createFourTerms(d, 0, d.size());
                Term[] sterms = createFourTerms(d, 0, d.Count);
                for (int i = 0; i < 4; i++)
                {
                    TermList list = tf.createList(2);
                    list.Add(sterms[i]);
                    list.Add(sterms[i]);
                    terms.Add((Term)list);
                }
            }
            else
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?>[] sterms1 = createFourTerms(d, 0, slash);
                Term[] sterms1 = createFourTerms(d, 0, slash);
                //ORIGINAL LINE: StyleParserCS.css.Term<?>[] sterms2 = createFourTerms(d, slash, d.size());
                Term[] sterms2 = createFourTerms(d, slash, d.Count);
                for (int i = 0; i < 4; i++)
                {
                    TermList list = tf.createList(2);
                    list.Add(sterms1[i]);
                    list.Add(sterms2[i]);
                    terms.Add((Term)list);
                }
            }
            return repeat(properties, values);
        }

        //ORIGINAL LINE: private StyleParserCS.css.Term<?>[] createFourTerms(StyleParserCS.css.Declaration d, int fromIndex, int toIndex) throws IllegalArgumentException
        private Term[] createFourTerms(Declaration d, int fromIndex, int toIndex)
        {
            int size = toIndex - fromIndex;
            //ORIGINAL LINE: StyleParserCS.css.Term<?>[] ret = new StyleParserCS.css.Term<?>[4];
            Term[] ret = new Term[4];
            switch (size)
            {
                case 1:
                    // one term for all value
                    ret[0] = ret[1] = ret[2] = ret[3] = d[fromIndex];
                    break;
                case 2:
                    ret[0] = ret[2] = d[fromIndex];
                    ret[1] = ret[3] = d[fromIndex + 1];
                    break;
                case 3:
                    ret[0] = d[fromIndex];
                    ret[1] = ret[3] = d[fromIndex + 1];
                    ret[2] = d[fromIndex + 2];
                    break;
                case 4:
                    for (int i = 0; i < 4; i++)
                    {
                        ret[i] = d[fromIndex + i];
                    }
                    break;
                default:
                    throw new System.ArgumentException("Invalid length of terms in Repeater.");
            }

            //when started by a slash, remove the slash from the terms
            if (fromIndex != 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (ret[i].Operator == Term_Operator.SLASH)
                    {
                        ret[i] = stripSlash(ret[i]);
                    }
                }
            }

            return ret;
        }

        //ORIGINAL LINE: private StyleParserCS.css.Term<?> stripSlash(StyleParserCS.css.Term<?> src)
        private Term stripSlash(Term src)
        {
            if (src.Operator == Term_Operator.SLASH)
            {
                if (src is TermLength)
                {
                    return (Term)tf.createLength(((Term<float>)src).Value, ((TermLength)src).Unit);
                }
                else if (src is TermPercent)
                {
                    return (Term)tf.createPercent(((Term<float>)src).Value);
                }
                else
                {
                    return src;
                }
            }
            else
            {
                return src;
            }
        }

    }

}