using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using CSSProperty_FlexDirection = StyleParserCS.css.CSSProperty_FlexDirection;
    using CSSProperty_FlexWrap = StyleParserCS.css.CSSProperty_FlexWrap;

    /// <summary>
    /// Variator for flex-flow. Grammar:
    /// 
    /// <pre>
    /// <'flex-direction'> || <'flex-wrap'>
    /// | inherit
    /// 
    /// @author burgetr
    /// </summary>
    public class FlexFlowVariator : Variator
    {

        public const int DIRECTION = 0;
        public const int WRAP = 1;

        public FlexFlowVariator() : base(2)
        {
            names.Add("flex-direction");
            types.Add(typeof(CSSProperty_FlexDirection));
            names.Add("flex-wrap");
            types.Add(typeof(CSSProperty_FlexWrap));
        }

        protected internal override bool variant(int v, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            int i = iteration.get();

            switch (v)
            {
                case DIRECTION:
                    return genericTermIdent(typeof(CSSProperty_FlexDirection), terms[i], AVOID_INH, names[DIRECTION], properties);
                case WRAP:
                    return genericTermIdent(typeof(CSSProperty_FlexWrap), terms[i], AVOID_INH, names[WRAP], properties);
                default:
                    return false;
            }
        }
    }

}