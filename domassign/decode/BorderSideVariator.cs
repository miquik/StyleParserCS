using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using CSSProperty_BorderColor = StyleParserCS.css.CSSProperty_BorderColor;
    using CSSProperty_BorderStyle = StyleParserCS.css.CSSProperty_BorderStyle;
    using CSSProperty_BorderWidth = StyleParserCS.css.CSSProperty_BorderWidth;

    /// <summary>
    /// Variator for border side.
    /// Grammar:
    /// <pre>
    /// [ <border-width> || <border-style> || <'border-top-color'> ] 
    /// | inherit
    /// </pre>
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class BorderSideVariator : Variator
    {

        public const int COLOR = 0;
        public const int STYLE = 1;
        public const int WIDTH = 2;

        public BorderSideVariator(string side) : base(3)
        {
            names.Add("border-" + side + "-color");
            types.Add(typeof(CSSProperty_BorderColor));
            names.Add("border-" + side + "-style");
            types.Add(typeof(CSSProperty_BorderStyle));
            names.Add("border-" + side + "-width");
            types.Add(typeof(CSSProperty_BorderWidth));
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
                        genericTermColor(terms[i], names[COLOR], CSSProperty_BorderColor.color, properties, values);
                case STYLE:
                    // process style
                    return genericTermIdent(types[STYLE], terms[i], AVOID_INH, names[STYLE], properties);
                case WIDTH:
                    // process width
                    return genericTermIdent(types[WIDTH], terms[i], AVOID_INH, names[WIDTH], properties) || 
                        genericTermLength(terms[i], names[WIDTH], CSSProperty_BorderWidth.length, ValueRange.DISALLOW_NEGATIVE, properties, values);
                default:
                    return false;
            }

        }
    }

}