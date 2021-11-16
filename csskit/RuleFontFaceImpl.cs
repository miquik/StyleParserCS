using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// RuleFontFaceImpl.java
/// 
/// Created on 1.2.2013, 14:28:51 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{

    using CSSProperty_FontStyle = StyleParserCS.css.CSSProperty_FontStyle;
    using CSSProperty_FontWeight = StyleParserCS.css.CSSProperty_FontWeight;
    using Declaration = StyleParserCS.css.Declaration;
    using RuleFontFace = StyleParserCS.css.RuleFontFace;
    using StyleParserCS.css;
    using Term_Operator = StyleParserCS.css.Term_Operator;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermString = StyleParserCS.css.TermString;
    using TermURI = StyleParserCS.css.TermURI;
    using StyleParserCS.utils;

    /// <summary>
    /// Wrap of declarations bound with a font specification
    /// 
    /// @author burgetr
    /// </summary>
    public class RuleFontFaceImpl : AbstractRuleBlock<Declaration>, RuleFontFace
    {
        private const string PROPERTY_FONT_FAMILY_NAME = "font-family";
        private const string PROPERTY_SOURCE = "src";
        private const string PROPERTY_FONT_STYLE = "font-style";
        private const string PROPERTY_FONT_WEIGHT = "font-weight";
        private const string PROPERTY_UNICODE_RANGE = "unicode-range";

        protected internal RuleFontFaceImpl() : base()
        {
        }

        public virtual string FontFamily
        {
            get
            {
                return getStringValue(PROPERTY_FONT_FAMILY_NAME);
            }
        }

        public virtual IList<RuleFontFace_Source> Sources
        {
            get
            {
                Declaration decl = getDeclaration(PROPERTY_SOURCE);
                if (decl != null)
                {
                    IList<RuleFontFace_Source> ret = new List<RuleFontFace_Source>(decl.Count);
                    bool invalid = false;

                    for (int i = 0; i < decl.Count && !invalid; i++)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> val = decl.get(i);
                        Term val = decl[i];
                        if (val is TermURI)
                        {
                            //ORIGINAL LINE: final StyleParserCS.css.TermURI uri = (StyleParserCS.css.TermURI) val;
                            TermURI uri = (TermURI)val;
                            //ORIGINAL LINE: final String format = (i + 1 < decl.size()) ? checkForFormat(decl.get(i + 1)) : null;
                            string format = (i + 1 < decl.Count) ? checkForFormat(decl[i + 1]) : null;
                            if (!string.ReferenceEquals(format, null))
                            {
                                i++; //skip correct format definition
                            }
                            //ORIGINAL LINE: final StyleParserCS.css.RuleFontFace_SourceUri src = new StyleParserCS.css.RuleFontFace_SourceUri()
                            StyleParserCS.css.RuleFontFace_SourceUri src = new RuleFontFace_SourceUriAnonymousInnerClass(this, uri, format);
                            ret.Add(src);
                        }
                        else if (val is TermFunction)
                        {
                            //ORIGINAL LINE: final StyleParserCS.css.TermFunction fn = (StyleParserCS.css.TermFunction) val;
                            TermFunction fn = (TermFunction)val;
                            if (fn.FunctionName.Equals("local", StringComparison.OrdinalIgnoreCase) && fn.Count == 1 && fn[0] is TermString)
                            {
                                //ORIGINAL LINE: final String fontname = ((StyleParserCS.css.TermString) fn.get(0)).getValue();
                                string fontname = ((TermString)fn[0]).Value;
                                //ORIGINAL LINE: final StyleParserCS.css.RuleFontFace_SourceLocal src = new StyleParserCS.css.RuleFontFace_SourceLocal()
                                StyleParserCS.css.RuleFontFace_SourceLocal src = new RuleFontFace_SourceLocalAnonymousInnerClass(this, fontname);
                                ret.Add(src);
                            }
                            else
                            {
                                invalid = true;
                            }
                        }
                        else
                        {
                            invalid = true;
                        }
                        
                        if (i + 1 < decl.Count && decl[i + 1].Operator != Term_Operator.COMMA)
                        {
                            invalid = true; //some additional (invalid) terms found
                        }
                    }

                    if (!invalid)
                    {
                        return ret;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private class RuleFontFace_SourceUriAnonymousInnerClass : StyleParserCS.css.RuleFontFace_SourceUri
        {
            private readonly RuleFontFaceImpl outerInstance;

            private TermURI uri;
            private string format;

            public RuleFontFace_SourceUriAnonymousInnerClass(RuleFontFaceImpl outerInstance, TermURI uri, string format)
            {
                this.outerInstance = outerInstance;
                this.uri = uri;
                this.format = format;
            }

            public TermURI URI
            {
                get
                {
                    return uri;
                }
            }
            public string Format
            {
                get
                {
                    return format;
                }
            }
        }

        private class RuleFontFace_SourceLocalAnonymousInnerClass : StyleParserCS.css.RuleFontFace_SourceLocal
        {
            private readonly RuleFontFaceImpl outerInstance;

            private string fontname;

            public RuleFontFace_SourceLocalAnonymousInnerClass(RuleFontFaceImpl outerInstance, string fontname)
            {
                this.outerInstance = outerInstance;
                this.fontname = fontname;
            }

            public string Name
            {
                get
                {
                    return fontname;
                }
            }
        }

        private string checkForFormat(Term term)
        {
            if (term is TermFunction && ((TermFunction)term).Operator == Term_Operator.SPACE)
            {
                //ORIGINAL LINE: final StyleParserCS.css.TermFunction fn = (StyleParserCS.css.TermFunction) term;
                TermFunction fn = (TermFunction)term;
                if (fn.FunctionName.Equals("format", StringComparison.OrdinalIgnoreCase) && fn.Count == 1 && fn[0] is TermString)
                {
                    return ((TermString)fn[0]).Value;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual CSSProperty_FontStyle FontStyle
        {
            get
            {
                string strValue = getStringValue(PROPERTY_FONT_STYLE);
                if (string.ReferenceEquals(strValue, null))
                {
                    return null;
                }

                try
                {
                    return CSSProperty_FontStyle.FromName(strValue.ToUpper());
                }
                catch (System.ArgumentException)
                {
                    return null;
                }
            }
        }

        public virtual CSSProperty_FontWeight FontWeight
        {
            get
            {
                string strValue = getStringValue(PROPERTY_FONT_WEIGHT);
                if (string.ReferenceEquals(strValue, null))
                {
                    return null;
                }

                try
                {
                    return CSSProperty_FontWeight.FromName(strValue.ToUpper());
                }
                catch (System.ArgumentException)
                {
                    return null;
                }
            }
        }

        public virtual IList<string> UnicodeRanges
        {
            get
            {
                Declaration decl = getDeclaration(PROPERTY_UNICODE_RANGE);
                if (decl != null)
                {
                    IList<string> ret = new List<string>(decl.Count);
                    //ORIGINAL LINE: for (StyleParserCS.css.Term<?> term : decl)
                    foreach (Term term in decl)
                    {
                        ret.Add(term.GetValueAsString());
                        // ret.Add(term.Value.ToString());
                    }
                    return ret;
                }
                else
                {
                    return null;
                }
            }
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public virtual string ToString(int depth)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(OutputUtil.FONT_FACE_KEYWORD).Append(OutputUtil.SPACE_DELIM);

            // append declarations
            sb.Append(OutputUtil.RULE_OPENING);
            sb = OutputUtil.appendList(sb, this, OutputUtil.EMPTY_DELIM, depth + 1);
            sb.Append(OutputUtil.RULE_CLOSING);

            return sb.ToString();
        }

        private string getStringValue(string propertyName)
        {
            Declaration decl = getDeclaration(propertyName);
            if (decl == null)
            {
                return null;
            }

            //ORIGINAL LINE: StyleParserCS.css.Term<?> term= decl.get(0);
            Term term = decl[0];
            if (term == null)
            {
                return null;
            }

            if (!(term is Term<string>))
            {
                return null;
            }
            /*
            object value = term.Value;
            if (!(value is string))
            {
                return null;
            }
            */

            return ((Term<string>)term).Value;
        }

        private Declaration getDeclaration(string property)
        {
            foreach (Declaration decl in this)
            {
                if (property.Equals(decl.Property))
                {
                    return decl;
                }
            }

            return null;
        }
    }

}