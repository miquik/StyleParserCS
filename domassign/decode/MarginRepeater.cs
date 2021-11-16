using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermPercent = StyleParserCS.css.TermPercent;
    using CSSProperty_Margin = StyleParserCS.css.CSSProperty_Margin;

    /// <summary>
    /// Margin repeater
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class MarginRepeater : Repeater
    {

        public MarginRepeater() : base(4)
        {
            this.type = typeof(CSSProperty_Margin);
            names.Add("margin-top");
            names.Add("margin-right");
            names.Add("margin-bottom");
            names.Add("margin-left");

        }

        protected internal override bool operation(int i, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            return genericTermIdent(type, terms[i], AVOID_INH, names[i], properties) || 
                genericTermLength(terms[i], names[i], CSSProperty_Margin.length, ValueRange.ALLOW_ALL, properties, values) || 
                genericTerm(typeof(TermPercent), terms[i], names[i], CSSProperty_Margin.percentage, ValueRange.ALLOW_ALL, properties, values);
        }
    }

}