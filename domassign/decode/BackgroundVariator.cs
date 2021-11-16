using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermLength = StyleParserCS.css.TermLength;
    using TermList = StyleParserCS.css.TermList;
    using TermPercent = StyleParserCS.css.TermPercent;
    using TermURI = StyleParserCS.css.TermURI;
    using CSSProperty_BackgroundAttachment = StyleParserCS.css.CSSProperty_BackgroundAttachment;
    using CSSProperty_BackgroundColor = StyleParserCS.css.CSSProperty_BackgroundColor;
    using CSSProperty_BackgroundImage = StyleParserCS.css.CSSProperty_BackgroundImage;
    using CSSProperty_BackgroundOrigin = StyleParserCS.css.CSSProperty_BackgroundOrigin;
    using CSSProperty_BackgroundPosition = StyleParserCS.css.CSSProperty_BackgroundPosition;
    using CSSProperty_BackgroundRepeat = StyleParserCS.css.CSSProperty_BackgroundRepeat;
    using CSSProperty_BackgroundSize = StyleParserCS.css.CSSProperty_BackgroundSize;
    using Term_Operator = StyleParserCS.css.Term_Operator;
    using System.Linq;

    /// <summary>
    /// Background variator.
    /// Grammar:
    /// <pre>
    /// [ <'background-color'> || <'background-image'> 
    ///      || <'background-repeat'> || <'background-attachment'> 
    ///      || <'background-position'> [ / <background-size> ]?
    /// ] 
    /// | inherit
    /// </pre>
    /// 
    /// @author kapy
    /// </summary>
    public class BackgroundVariator : Variator
    {

        public const int COLOR = 0;
        public const int IMAGE = 1;
        public const int REPEAT = 2;
        public const int ATTACHMENT = 3;
        public const int POSITION = 4;
        public const int SIZE = 5;
        public const int ORIGIN = 6;

        public BackgroundVariator() : base(7)
        {
            names.Add("background-color");
            types.Add(typeof(CSSProperty_BackgroundColor));
            names.Add("background-image");
            types.Add(typeof(CSSProperty_BackgroundImage));
            names.Add("background-repeat");
            types.Add(typeof(CSSProperty_BackgroundRepeat));
            names.Add("background-attachment");
            types.Add(typeof(CSSProperty_BackgroundAttachment));
            names.Add("background-position");
            types.Add(typeof(CSSProperty_BackgroundPosition));
            names.Add("background-size");
            types.Add(typeof(CSSProperty_BackgroundSize));
            names.Add("background-origin");
            types.Add(typeof(CSSProperty_BackgroundOrigin));
        }

        protected internal override bool variant(int v, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // we will use multi value functionality in
            // POSITION branch
            int i = iteration.get();

            switch (v)
            {
                case COLOR:
                    return genericTermIdent(types[COLOR], terms[i], AVOID_INH, names[COLOR], properties) || genericTermColor(terms[i], names[COLOR], CSSProperty_BackgroundColor.color, properties, values);
                case IMAGE:
                    return genericTermIdent(types[IMAGE], terms[i], AVOID_INH, names[IMAGE], properties) || genericTerm(typeof(TermURI), terms[i], names[IMAGE], CSSProperty_BackgroundImage.uri, ValueRange.ALLOW_ALL, properties, values) || genericTerm(typeof(StyleParserCS.css.TermFunction_Gradient), terms[i], names[IMAGE], CSSProperty_BackgroundImage.gradient, ValueRange.ALLOW_ALL, properties, values);
                case REPEAT:
                    return genericTermIdent(types[REPEAT], terms[i], AVOID_INH, names[REPEAT], properties);
                case ORIGIN:
                    return genericTermIdent(types[ORIGIN], terms[i], AVOID_INH, names[ORIGIN], properties);
                case ATTACHMENT:
                    return genericTermIdent(types[ATTACHMENT], terms[i], AVOID_INH, names[ATTACHMENT], properties);
                case POSITION:

                    //ORIGINAL LINE: final java.util.EnumSet<StyleParserCS.css.CSSProperty_BackgroundPosition> allowedBackground = java.util.EnumSet.complementOf(java.util.EnumSet.of(StyleParserCS.css.CSSProperty_BackgroundPosition.list_values, StyleParserCS.css.CSSProperty_BackgroundPosition.INHERIT));
                    // EnumSet<CSSProperty_BackgroundPosition> allowedBackground = EnumSet.complementOf(EnumSet.of(CSSProperty_BackgroundPosition.list_values, CSSProperty_BackgroundPosition.INHERIT));
                    ISet<CSSProperty_BackgroundPosition> allowedBackground =
                        CSSProperty_BackgroundPosition
                        .List
                        .Where(x => x != CSSProperty_BackgroundPosition.list_values && x != CSSProperty_BackgroundPosition.INHERIT).ToHashSet();

                    // try this and next term, but consider terms size
                    CSSProperty_BackgroundPosition bp = null;
                    //ORIGINAL LINE: StyleParserCS.css.Term<?>[] vv = {null, null};
                    Term[] vv = new Term[] { null, null }; //horizontal and vertical position
                    for (; i < terms.Count; i++)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> term = terms.get(i);
                        Term term = terms[i];
                        if (term.Operator != Term_Operator.SLASH)
                        {
                            if (term is TermIdent)
                            {
                                bp = genericPropertyRaw(typeof(CSSProperty_BackgroundPosition), allowedBackground, (TermIdent)term);
                                if (bp != null)
                                {
                                    storeBackgroundPosition(vv, bp, term);
                                }
                            }
                            else if (term is TermPercent)
                            {
                                storeBackgroundPosition(vv, null, term);
                            }
                            else if (term is TermLength)
                            {
                                storeBackgroundPosition(vv, null, term);
                            }
                            else //not recognized value
                            {
                                break;
                            }
                        }
                        else //slash found - this value belongs to size rather than position
                        {
                            break;
                        }
                    }

                    //create term list from the values, replace unspecified values by center
                    int assigned = 0;
                    TermList list = tf.createList(2);
                    for (int j = 0; j < 2; j++)
                    {
                        if (vv[j] == null)
                        {
                            list.Add((Term)tf.createPercent(50.0f));
                        }
                        else
                        {
                            list.Add(vv[j]);
                            assigned++;
                        }
                    }

                    // no values could be used
                    if (assigned == 0)
                    {
                        return false;
                    }
                    // if used two elements, inform master
                    else if (assigned == 2)
                    {
                        iteration.inc();
                    }

                    // store list
                    properties[names[POSITION]] = CSSProperty_BackgroundPosition.list_values;
                    values[names[POSITION]] = (Term)list;
                    return true;

                case SIZE:

                    //ORIGINAL LINE: final java.util.EnumSet<StyleParserCS.css.CSSProperty_BackgroundSize> allowedSize = java.util.EnumSet.complementOf(java.util.EnumSet.of(StyleParserCS.css.CSSProperty_BackgroundSize.list_values, StyleParserCS.css.CSSProperty_BackgroundSize.INHERIT));
                    // EnumSet<CSSProperty_BackgroundSize> allowedSize = EnumSet.complementOf(EnumSet.of(CSSProperty_BackgroundSize.list_values, CSSProperty_BackgroundSize.INHERIT));
                    ISet<CSSProperty_BackgroundSize> allowedSize =
                        CSSProperty_BackgroundSize
                        .List
                        .Where(x => x != CSSProperty_BackgroundSize.list_values && x != CSSProperty_BackgroundSize.INHERIT).ToHashSet();


                    // try this and next term, but consider terms size
                    CSSProperty_BackgroundSize bs = null;
                    //ORIGINAL LINE: StyleParserCS.css.Term<?>[] sz = {null, null};
                    Term[] sz = new Term[] { null, null }; //horizontal and vertical size
                    int vi = 0; //current value index
                    for (; i < terms.Count && vi < 2; i++)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> term = terms.get(i);
                        Term term = terms[i];
                        if (term is TermIdent)
                        {
                            bs = genericPropertyRaw(typeof(CSSProperty_BackgroundSize), allowedSize, (TermIdent)term);
                            if (bs != null)
                            {
                                //contain and cover have only one occurence
                                properties[names[SIZE]] = bs;
                                values.Remove(names[SIZE]); //only keyword, no value
                                return true;
                            }
                            else if (((TermIdent)term).Value.Equals("auto"))
                            {
                                sz[vi++] = term;
                            }
                        }
                        else if (term is TermPercent || term is TermLength)
                        {
                            //TODO this allows integers with no unit as lengths
                            sz[vi++] = term;
                        }
                        else
                        {
                            break; //something that cannot be assigned
                        }
                    }

                    //check the number of values
                    if (sz[0] == null)
                    {
                        return false; //no values set
                    }
                    else if (sz[1] == null)
                    {
                        sz[1] = (Term)tf.createIdent("auto");
                    }
                    else //if used two elements, inform master
                    {
                        iteration.inc();
                    }

                    //create term list from the values, replace unspecified values by center
                    TermList szlist = tf.createList(2);
                    szlist.Add(sz[0]);
                    szlist.Add(sz[1]);

                    // store list
                    properties[names[SIZE]] = CSSProperty_BackgroundSize.list_values;
                    values[names[SIZE]] = (Term)szlist;
                    return true;

                default:
                    return false;
            }
        }

        // private void storeBackgroundPosition(Term<?>[] storage, BackgroundPosition bp, Term<?> term)
        private void storeBackgroundPosition(Term[] storage, CSSProperty_BackgroundPosition bp, Term term)
        {
            if (bp == CSSProperty_BackgroundPosition.LEFT)
            {
                setPositionValue(storage, 0, (Term)tf.createPercent(0.0f));
            }
            else if (bp == CSSProperty_BackgroundPosition.RIGHT)
            {
                setPositionValue(storage, 0, (Term)tf.createPercent(100.0f));
            }
            else if (bp == CSSProperty_BackgroundPosition.TOP)
            {
                setPositionValue(storage, 1, (Term)tf.createPercent(0.0f));
            }
            else if (bp == CSSProperty_BackgroundPosition.BOTTOM)
            {
                setPositionValue(storage, 1, (Term)tf.createPercent(100.0f));
            }
            else if (bp == CSSProperty_BackgroundPosition.CENTER)
            {
                setPositionValue(storage, -1, (Term)tf.createPercent(50.0f));
            }
            else
            {
                setPositionValue(storage, -1, term);
            }
        }

        private void setPositionValue(Term[] s, int index, Term term)
        {
            switch (index)
            {
                case -1:
                    if (s[0] == null) //any position - use the free position
                    {
                        s[0] = term;
                    }
                    else
                    {
                        s[1] = term;
                    }
                    break;
                case 0:
                    if (s[0] != null) //if the position is occupied, move the old value
                    {
                        s[1] = s[0];
                    }
                    s[0] = term;
                    break;
                case 1:
                    if (s[1] != null)
                    {
                        s[0] = s[1];
                    }
                    s[1] = term;
                    break;
            }
        }

        protected internal override bool variantCondition(int variant, IntegerRef iteration)
        {
            switch (variant)
            {
                case POSITION:
                    if (variantPassed[SIZE])
                    {
                        return false;
                    }
                    return terms[iteration.get()].Operator != Term_Operator.SLASH;
                case SIZE:
                    if (!variantPassed[POSITION])
                    {
                        return false;
                    }
                    return terms[iteration.get()].Operator == Term_Operator.SLASH;
                default:
                    return true;
            }
        }

        protected internal override bool validateListItem(int listIndex, int listSize, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            if (listIndex == listSize - 1)
            {
                return true; //everything is allowed in the last layer
            }
            else
            {
                return !variantPassed[COLOR]; //color is only allowed in the last layer
            }
        }

    }

}