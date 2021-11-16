using System;
using System.Collections.Generic;

/// 
namespace StyleParserCS.domassign.decode
{

    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermIdent = StyleParserCS.css.TermIdent;
    using CSSProperty_BorderColor = StyleParserCS.css.CSSProperty_BorderColor;
    using CSSProperty_BorderStyle = StyleParserCS.css.CSSProperty_BorderStyle;
    using CSSProperty_BorderWidth = StyleParserCS.css.CSSProperty_BorderWidth;

    /// <summary>
    /// Border variator. Grammar: [ <border-width> || <border-style> ||
    /// <border-top-color> ] | inherit
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public class BorderVariator : Variator
    {

        public const int WIDTH = 0;
        public const int STYLE = 1;
        public const int COLOR = 2;

        private IList<Repeater> repeaters;

        public BorderVariator() : base(3)
        {
            types.Add(typeof(CSSProperty_BorderWidth));
            types.Add(typeof(CSSProperty_BorderStyle));
            types.Add(typeof(CSSProperty_BorderColor));
            repeaters = new List<Repeater>(variants);
            repeaters.Add(new BorderWidthRepeater());
            repeaters.Add(new BorderStyleRepeater());
            repeaters.Add(new BorderColorRepeater());
        }

        protected internal override bool variant(int variant, IntegerRef iteration, IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {

            // iteration is not modified in this function
            int i = iteration.get();
            //ORIGINAL LINE: StyleParserCS.css.Term<?> term = terms.get(i);
            Term term = terms[i];
            Repeater r;

            switch (variant)
            {
                case WIDTH:
                    r = repeaters[WIDTH];
                    r.assignTerms(term, term, term, term);
                    return r.repeat(properties, values);
                case STYLE:
                    r = repeaters[STYLE];
                    r.assignTerms(term, term, term, term);
                    return r.repeat(properties, values);
                case COLOR:
                    r = repeaters[COLOR];
                    r.assignTerms(term, term, term, term);
                    return r.repeat(properties, values);
                default:
                    return false;
            }
        }

        /// <summary>
        /// This method is overriden to use repeaters
        /// </summary>
        protected internal override bool checkInherit(int variant, Term term, IDictionary<string, CSSProperty> properties)
        {

            // check whether term equals inherit
            if (!(term is TermIdent) || !StyleParserCS.css.CSSProperty_Fields.INHERIT_KEYWORD.Equals(((TermIdent)term).Value, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (variant == ALL_VARIANTS)
            {
                for (int i = 0; i < variants; i++)
                {
                    Repeater r1 = repeaters[i];
                    r1.assignTerms(term, term, term, term);
                    r1.repeat(properties, null);
                }
                return true;
            }

            Repeater r = repeaters[variant];
            r.assignTerms(term, term, term, term);
            r.repeat(properties, null);
            return true;
        }

        public override void assignDefaults(IDictionary<string, CSSProperty> properties, IDictionary<string, Term> values)
        {
            foreach (Repeater r in repeaters)
            {
                r.assignDefaults(properties, values);
            }
        }

    }

}