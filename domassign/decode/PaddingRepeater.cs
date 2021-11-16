using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermPercent = StyleParserCS.css.TermPercent;
    using CSSProperty_Padding = StyleParserCS.css.CSSProperty_Padding;

    /// <summary>
    /// Padding repeater
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class PaddingRepeater : Repeater
    {

        public PaddingRepeater() : base(4)
        {
            names.Add("padding-top");
            names.Add("padding-right");
            names.Add("padding-bottom");
            names.Add("padding-left");
            this.type = typeof(CSSProperty_Padding);
        }

        protected internal override bool operation(int i, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            return genericTermIdent(type, terms[i], AVOID_INH, names[i], properties) ||
                genericTermLength(terms[i], names[i], CSSProperty_Padding.length, ValueRange.DISALLOW_NEGATIVE, properties, values) || 
                genericTerm(typeof(TermPercent), terms[i], names[i], CSSProperty_Padding.percentage, ValueRange.DISALLOW_NEGATIVE, properties, values);
        }
    }

}