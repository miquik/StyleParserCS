using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using CSSProperty_OutlineColor = StyleParserCS.css.CSSProperty_OutlineColor;
    using CSSProperty_OutlineStyle = StyleParserCS.css.CSSProperty_OutlineStyle;
    using CSSProperty_OutlineWidth = StyleParserCS.css.CSSProperty_OutlineWidth;

    /// <summary>
    /// Outline variator Grammar:
    /// 
    /// <pre>
    /// [ 'outline-color' || 'outline-style' || 'outline-width' ] 
    /// | inherit
    /// </pre>
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class OutlineVariator : Variator
    {

        public const int COLOR = 0;
        public const int STYLE = 1;
        public const int WIDTH = 2;

        public OutlineVariator() : base(3)
        {
            names.Add("outline-color");
            types.Add(typeof(CSSProperty_OutlineColor));
            names.Add("outline-style");
            types.Add(typeof(CSSProperty_OutlineStyle));
            names.Add("outline-width");
            types.Add(typeof(CSSProperty_OutlineWidth));
        }

        protected internal override bool variant(int v, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // we won't use multivalue functionallity
            int i = iteration.get();

            switch (v)
            {
                case COLOR:
                    // process color
                    return genericTermIdent(types[COLOR], terms[i], AVOID_INH, names[COLOR], properties) || 
                        genericTermColor(terms[i], names[COLOR], CSSProperty_OutlineColor.color, properties, values);
                case STYLE:
                    // process style
                    return genericTermIdent(types[STYLE], terms[i], AVOID_INH, names[STYLE], properties);
                case WIDTH:
                    // process width
                    return genericTermIdent(types[WIDTH], terms[i], AVOID_INH, names[WIDTH], properties) || 
                        genericTermLength(terms[i], names[WIDTH], CSSProperty_OutlineWidth.length, ValueRange.DISALLOW_NEGATIVE, properties, values);
                default:
                    return false;
            }
        }
    }

}