using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using CSSProperty_BorderStyle = StyleParserCS.css.CSSProperty_BorderStyle;

    /// <summary>
    /// Border style repeater
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class BorderStyleRepeater : Repeater
    {

        public BorderStyleRepeater() : base(4)
        {
            this.type = typeof(CSSProperty_BorderStyle);
            names.Add("border-top-style");
            names.Add("border-right-style");
            names.Add("border-bottom-style");
            names.Add("border-left-style");
        }

        protected internal override bool operation(int i, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            return genericTermIdent(typeof(CSSProperty_BorderStyle), terms[i], ALLOW_INH, names[i], properties);
        }
    }

}