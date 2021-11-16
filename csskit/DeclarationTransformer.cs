using System.Collections.Generic;

namespace StyleParserCS.csskit
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using Declaration = StyleParserCS.css.Declaration;
    using StyleParserCS.css;

    public interface DeclarationTransformer
    {
        //ORIGINAL LINE: boolean parseDeclaration(StyleParserCS.css.Declaration d, java.util.Map<String, StyleParserCS.css.CSSProperty> properties, java.util.Map<String, StyleParserCS.css.Term<?>> values);
        bool parseDeclaration(Declaration d, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values);
    }

}