using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermColor = StyleParserCS.css.TermColor;
    using CSSProperty_BorderColor = StyleParserCS.css.CSSProperty_BorderColor;

    /// <summary>
    /// Border color repeater
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class BorderColorRepeater : Repeater
    {

        public BorderColorRepeater() : base(4)
        {
            this.type = typeof(CSSProperty_BorderColor);
            names.Add("border-top-color");
            names.Add("border-right-color");
            names.Add("border-bottom-color");
            names.Add("border-left-color");
        }

        protected internal override bool operation(int i, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            return genericTermIdent(type, terms[i], ALLOW_INH, names[i], properties) ||
                genericTerm(typeof(TermColor), terms[i], names[i], CSSProperty_BorderColor.color, ValueRange.ALLOW_ALL, properties, values);
        }
    }

}