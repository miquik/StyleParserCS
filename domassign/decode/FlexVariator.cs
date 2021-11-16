using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermLength = StyleParserCS.css.TermLength;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermPercent = StyleParserCS.css.TermPercent;
    using CSSProperty_FlexBasis = StyleParserCS.css.CSSProperty_FlexBasis;
    using CSSProperty_FlexGrow = StyleParserCS.css.CSSProperty_FlexGrow;
    using CSSProperty_FlexShrink = StyleParserCS.css.CSSProperty_FlexShrink;

    /// <summary>
    /// Variator for flex. Grammar:
    /// 
    /// <pre>
    /// [ <'flex-grow'> <'flex-shrink'>? || <'flex-basis'> ]
    /// | none
    /// | inherit
    /// 
    /// @author burgetr
    /// </summary>
    public class FlexVariator : Variator
    {

        public const int GROW = 0;
        public const int SHRINK = 1;
        public const int BASIS = 2;

        public FlexVariator() : base(3)
        {
            names.Add("flex-grow");
            types.Add(typeof(CSSProperty_FlexGrow));
            names.Add("flex-shrink");
            types.Add(typeof(CSSProperty_FlexShrink));
            names.Add("flex-basis");
            types.Add(typeof(CSSProperty_FlexBasis));
        }

        protected internal override bool variant(int v, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            int i = iteration.get();

            switch (v)
            {
                case GROW:
                    return genericTerm(typeof(TermNumber), terms[i], names[GROW], CSSProperty_FlexGrow.number, ValueRange.DISALLOW_NEGATIVE, properties, values) || genericTerm(typeof(TermInteger), terms[i], names[GROW], CSSProperty_FlexGrow.number, ValueRange.DISALLOW_NEGATIVE, properties, values);
                case SHRINK:
                    return genericTerm(typeof(TermNumber), terms[i], names[SHRINK], CSSProperty_FlexShrink.number, ValueRange.DISALLOW_NEGATIVE, properties, values) || genericTerm(typeof(TermInteger), terms[i], names[SHRINK], CSSProperty_FlexShrink.number, ValueRange.DISALLOW_NEGATIVE, properties, values);
                case BASIS:
                    return genericTermIdent(types[BASIS], terms[i], AVOID_INH, names[BASIS], properties) || genericTerm(typeof(TermPercent), terms[i], names[BASIS], CSSProperty_FlexBasis.percentage, ValueRange.DISALLOW_NEGATIVE, properties, values) || genericTerm(typeof(TermLength), terms[i], names[BASIS], CSSProperty_FlexBasis.length, ValueRange.DISALLOW_NEGATIVE, properties, values);
                default:
                    return false;
            }
        }

        protected internal override bool variantCondition(int variant, IntegerRef iteration)
        {
            switch (variant)
            {
                case SHRINK:
                    return variantPassed[GROW];
                default:
                    return true;
            }
        }

        public override bool vary(IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            if (terms.Count == 1 && terms[0] is TermIdent)
            {
                //check for flex: none
                if (checkInherit(ALL_VARIANTS, terms[0], properties))
                {
                    return true;
                }
                if (terms[0].Equals(tf.createIdent("none")))
                {
                    // none should compute to: 0 0 auto
                    values[names[SHRINK]] = (Term)tf.createNumber(0.0f); //override the default for shrink to 0
                    return true;
                }
            }
            bool ret = base.vary(properties, values);

            //change the default value for flex-shrink to 1 when flex: <basis> is used
            if (variantPassed[BASIS] && !variantPassed[GROW] && properties[names[BASIS]] == CSSProperty_FlexBasis.AUTO)
            {
                values[names[GROW]] = (Term)tf.createNumber(1.0f);
            }
            //change the default value for flex-basis to 0 when flex: <positive_number> is used
            if (variantPassed[GROW] && !variantPassed[BASIS])
            {
                properties[names[BASIS]] = CSSProperty_FlexBasis.length;
                values[names[BASIS]] = (Term)tf.createLength(0.0f);
            }

            return ret;
        }
    }

}