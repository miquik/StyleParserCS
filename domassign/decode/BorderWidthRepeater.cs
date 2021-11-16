using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using CSSProperty_BorderWidth = StyleParserCS.css.CSSProperty_BorderWidth;

    /// <summary>
    /// Border width repeater
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class BorderWidthRepeater : Repeater
    {

        public BorderWidthRepeater() : base(4)
        {
            this.type = typeof(CSSProperty_BorderWidth);
            names.Add("border-top-width");
            names.Add("border-right-width");
            names.Add("border-bottom-width");
            names.Add("border-left-width");
        }

        protected internal override bool operation(int i, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            return genericTermIdent(type, terms[i], ALLOW_INH, names[i], properties) || 
                genericTermLength(terms[i], names[i], CSSProperty_BorderWidth.length, ValueRange.DISALLOW_NEGATIVE, properties, values);
        }
    }

}