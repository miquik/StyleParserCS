using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermURI = StyleParserCS.css.TermURI;
    using CSSProperty_ListStyleImage = StyleParserCS.css.CSSProperty_ListStyleImage;
    using CSSProperty_ListStylePosition = StyleParserCS.css.CSSProperty_ListStylePosition;
    using CSSProperty_ListStyleType = StyleParserCS.css.CSSProperty_ListStyleType;

    /// <summary>
    /// Variator for list style. Grammar:
    /// 
    /// <pre>
    /// [ 'list-style-type' || 'list-style-position' || 'list-style-image' ]
    /// | inherit
    /// 
    /// @author kapy
    /// </summary>
    public class ListStyleVariator : Variator
    {

        public const int TYPE = 0;
        public const int POSITION = 1;
        public const int IMAGE = 2;

        /*
		 * protected String[] names = { "list-style-image", "list-style-type",
		 * "list-style-position" };
		 */
        public ListStyleVariator() : base(3)
        {
            names.Add("list-style-type");
            types.Add(typeof(CSSProperty_ListStyleType));
            names.Add("list-style-position");
            types.Add(typeof(CSSProperty_ListStylePosition));
            names.Add("list-style-image");
            types.Add(typeof(CSSProperty_ListStyleImage));
        }

        protected internal override bool variant(int v, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // we won't use multivalue functionallity
            int i = iteration.get();

            switch (v)
            {
                case TYPE:
                    // list style type
                    return genericTermIdent(typeof(CSSProperty_ListStyleType), terms[i], AVOID_INH, names[TYPE], properties);
                case POSITION:
                    // list style position
                    return genericTermIdent(typeof(CSSProperty_ListStylePosition), terms[i], AVOID_INH, names[POSITION], properties);
                case IMAGE:
                    // list style image
                    return genericTermIdent(types[IMAGE], terms[i], AVOID_INH, names[IMAGE], properties) || genericTerm(typeof(TermURI), terms[i], names[IMAGE], CSSProperty_ListStyleImage.uri, ValueRange.ALLOW_ALL, properties, values);
                default:
                    return false;
            }
        }
    }

}