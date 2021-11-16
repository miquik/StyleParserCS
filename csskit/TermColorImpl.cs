using System;
using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.csskit
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using StyleParserCS.css;
    using TermColor = StyleParserCS.css.TermColor;
    using TermFunction = StyleParserCS.css.TermFunction;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermInteger = StyleParserCS.css.TermInteger;
    using TermNumber = StyleParserCS.css.TermNumber;
    using TermPercent = StyleParserCS.css.TermPercent;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TermColor implementation that represents a color defined by a standard color
    /// specification without usign special keywords.
    /// TODO: Clipping should be done against devices gamut
    /// @author Jan Svercl, VUT Brno, 2008
    /// 			modified by Karel Piwko, 2008
    ///          CSS3 extensions by Radek Burget, 2013
    /// </summary>
    public class TermColorImpl : TermImpl<Color>, TermColor
    {

        protected internal const string COLOR_RGB_NAME = "rgb";
        protected internal const string COLOR_RGBA_NAME = "rgba";
        protected internal const string COLOR_HSL_NAME = "hsl";
        protected internal const string COLOR_HSLA_NAME = "hsla";
        protected internal const int COLOR_PARAMS_COUNT = 3;
        protected internal const int MAX_VALUE = 255;
        protected internal const int MIN_VALUE = 0;
        protected internal const int PERCENT_CONVERSION = 100;
        protected internal const int MAX_HUE = 360;

        protected internal TermColorImpl(int r, int g, int b)
        {
            value = new Color(r, g, b);
        }

        protected internal TermColorImpl(int r, int g, int b, int a)
        {
            value = new Color(r, g, b, a);
        }

        protected internal TermColorImpl(Color value)
        {
            this.value = value;
        }

        public virtual StyleParserCS.css.TermColor_Keyword Keyword
        {
            get
            {
                return StyleParserCS.css.TermColor_Keyword.none;
            }
        }

        public virtual bool Transparent
        {
            get
            {
                return (value.Alpha == 0);
            }
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }

            if (value.Alpha == 255)
            { //use hash notation if aplha is not used
                string s = (value.RGB & 0xffffff).ToString("x");
                if (s.Length < 6)
                {
                    s = "000000".Substring(0, 6 - s.Length) + s;
                }
                sb.Append(OutputUtil.HASH_SIGN).Append(s);
            }
            else
            { //use rgba() when aplha is used
                sb.Append("rgba(");
                sb.Append(value.Red);
                sb.Append(',');
                sb.Append(value.Green);
                sb.Append(',');
                sb.Append(value.Blue);
                sb.Append(',');
                sb.Append((long)Math.Round(value.Alpha / 2.55, MidpointRounding.AwayFromZero) / 100.0);
                sb.Append(")");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks indent value against color card.
        /// If its value matches, new TermColor is returned which is 
        /// subject of replace of TermIndent afterwards </summary>
        /// <param name="ident"> Identifier possibly holding color's name </param>
        /// <returns> <code>TermColor</code> if color matches, <code>null</code> elsewhere </returns>
        public static TermColor getColorByIdent(TermIdent ident)
        {
            //ORIGINAL LINE: final StyleParserCS.css.TermColor c = ColorCard.getTermColor(ident.getValue());
            TermColor c = ColorCard.getTermColor(ident.Value);
            // copy the color card value because it may be modified later (e.g. operators added)
            if (c == null)
            {
                return null;
            }
            else if (c is TermColorKeywordImpl)
            {
                return new TermColorKeywordImpl(c.Keyword, c.Value);
            }
            else
            {
                return new TermColorImpl(c.Value);
            }
        }

        /// <summary>
        /// Creates color from string in form #ABC or #AABBCC, or
        /// just simply ABC and AABBCC.
        /// where A, B, C are hexadecimal digits. </summary>
        /// <param name="hash"> Hash string </param>
        /// <returns> Created color or <code>null</code> in case of error </returns>
        public static TermColor getColorByHash(string hash)
        {

            if (string.ReferenceEquals(hash, null))
            {
                throw new System.ArgumentException("Invalid hash value (null) for color construction");
            }

            // lowercase and remove hash character, if any
            hash = Regex.Replace(hash.ToLower(), "^#", "");

            // color written in #ABC format
            if (Regex.IsMatch(hash, "^[0-9a-f]{3}$"))
            {
                string r = hash.Substring(0, 1);
                string g = hash.Substring(1, 1);
                string b = hash.Substring(2, 1);
                return new TermColorImpl(Convert.ToInt32(r + r, 16), Convert.ToInt32(g + g, 16), Convert.ToInt32(b + b, 16));
            }
            // color written in #AABBCC format
            else if (Regex.IsMatch(hash, "^[0-9a-f]{6}$"))
            {
                string r = hash.Substring(0, 2);
                string g = hash.Substring(2, 2);
                string b = hash.Substring(4, 2);
                return new TermColorImpl(Convert.ToInt32(r, 16), Convert.ToInt32(g, 16), Convert.ToInt32(b, 16));
            }
            // invalid hash
            return null;
        }

        /// <summary>
        /// Creates color from <code>rgb()</code> function. </summary>
        /// <param name="func"> Function to be tested </param>
        /// <returns> Created color if parsing matched, <code>null</code> otherwise </returns>
        public static TermColor getColorByFunction(TermFunction func)
        {

            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = func.getSeparatedValues(StyleParserCS.css.CSSFactory.getTermFactory().createOperator(','), false);
            IList<Term> args = func.getSeparatedValues((Term)CSSFactory.TermFactory.createOperator(','), false);
            if (args != null)
            {
                if ((COLOR_RGB_NAME.Equals(func.FunctionName) && args.Count == COLOR_PARAMS_COUNT) || COLOR_RGBA_NAME.Equals(func.FunctionName) && args.Count == COLOR_PARAMS_COUNT + 1)
                {

                    bool percVals = false;
                    bool intVals = false;
                    int[] rgb = new int[COLOR_PARAMS_COUNT];
                    for (int i = 0; i < COLOR_PARAMS_COUNT; i++)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> term = args.get(i);
                        Term term = args[i];
                        // term is number and numeric
                        if (term is TermInteger)
                        {
                            rgb[i] = ((TermInteger)term).IntValue;
                            intVals = true;
                        }
                        // term is percent
                        else if (term is TermPercent)
                        {
                            //ORIGINAL LINE: final int value = ((StyleParserCS.css.TermPercent) term).getValue().intValue();
                            int value = (int)(((TermPercent)term).Value);
                            rgb[i] = (value * MAX_VALUE) / PERCENT_CONVERSION;
                            percVals = true;
                        }
                        // not valid term
                        else
                        {
                            return null;
                        }
                    }

                    if (percVals && intVals) //do not allow both percentages and int values combined
                    {
                        return null;
                    }

                    // limits
                    for (int i = 0; i < rgb.Length; i++)
                    {
                        if (rgb[i] < MIN_VALUE)
                        {
                            rgb[i] = MIN_VALUE;
                        }
                        if (rgb[i] > MAX_VALUE)
                        {
                            rgb[i] = MAX_VALUE;
                        }
                    }

                    //alpha
                    int a = MAX_VALUE;
                    if (args.Count > COLOR_PARAMS_COUNT)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> term = args.get(COLOR_PARAMS_COUNT);
                        Term term = args[COLOR_PARAMS_COUNT];
                        if (term is TermNumber || term is TermInteger)
                        {
                            float alpha = getFloatValue(term);
                            a = (int)Math.Round(alpha * MAX_VALUE, MidpointRounding.AwayFromZero);
                            if (a < MIN_VALUE)
                            {
                                a = MIN_VALUE;
                            }
                            if (a > MAX_VALUE)
                            {
                                a = MAX_VALUE;
                            }
                        }
                        else
                        {
                            return null; //unacceptable alpha value
                        }
                    }

                    return new TermColorImpl(rgb[0], rgb[1], rgb[2], a);
                }
                else if ((COLOR_HSL_NAME.Equals(func.FunctionName) && args.Count == COLOR_PARAMS_COUNT) || COLOR_HSLA_NAME.Equals(func.FunctionName) && args.Count == COLOR_PARAMS_COUNT + 1)
                {

                    float h, s, l;
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> hterm = args.get(0);
                    Term hterm = args[0];
                    if (hterm is TermNumber || hterm is TermInteger)
                    {
                        h = getFloatValue(hterm);
                        while (h >= MAX_HUE)
                        {
                            h -= MAX_HUE;
                        }
                        while (h < 0)
                        {
                            h += MAX_HUE;
                        }
                        h = h / MAX_HUE; //normalize to 0..1
                    }
                    else
                    {
                        return null;
                    }

                    //ORIGINAL LINE: StyleParserCS.css.Term<?> sterm = args.get(1);
                    Term sterm = args[1];
                    if (sterm is TermPercent)
                    {
                        int isv = (int)(((TermPercent)sterm).Value);
                        if (isv > 100)
                        {                            
                            isv = 100;
                        }
                        else if (isv < 0)
                        {
                            isv = 0;
                        }
                        s = isv / 100.0f;
                    }
                    else
                    {
                        return null;
                    }

                    //ORIGINAL LINE: StyleParserCS.css.Term<?> lterm = args.get(2);
                    Term lterm = args[2];
                    if (lterm is TermPercent)
                    {
                        int il = (int)(((TermPercent)lterm).Value);
                        if (il > 100)
                        {
                            il = 100;
                        }
                        else if (il < 0)
                        {
                            il = 0;
                        }
                        l = il / 100.0f;
                    }
                    else
                    {
                        return null;
                    }

                    int[] rgb = hslToRgb(h, s, l);

                    // alpha
                    int a = MAX_VALUE;
                    if (args.Count > COLOR_PARAMS_COUNT)
                    {
                        //ORIGINAL LINE: StyleParserCS.css.Term<?> term = args.get(3);
                        Term term = args[3];
                        if (term is TermNumber || term is TermInteger)
                        {
                            float alpha = getFloatValue(term);
                            a = (int)Math.Round(alpha * MAX_VALUE, MidpointRounding.AwayFromZero);
                            if (a < MIN_VALUE)
                            {
                                a = MIN_VALUE;
                            }
                            if (a > MAX_VALUE)
                            {
                                a = MAX_VALUE;
                            }
                        }
                        else
                        {
                            return null; // unacceptable alpha value
                        }
                    }

                    return new TermColorImpl(rgb[0], rgb[1], rgb[2], a);
                }
                // invalid function
                else
                {
                    return null;
                }
            }
            else
            {
                return null; //couldn't parse arguments
            }
        }

        private static float getFloatValue(Term term)
        {
            if (term is TermNumber)
            {
                return (float)(((TermNumber)term).Value);
            }
            else if (term is TermInteger)
            {
                return (float)(((TermInteger)term).Value);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Converts the HSL color model to RGB </summary>
        /// <param name="h"> hue normalized to 0..1 </param>
        /// <param name="s"> saturation normalized to 0..1 </param>
        /// <param name="l"> level normalized to 0..1 </param>
        /// <returns> an array of three values R, G and B in the interval 0..255 </returns>
        private static int[] hslToRgb(float h, float s, float l)
        {

            int[] ret = new int[3];

            float m2 = (l <= 0.5f) ? l * (s + 1) : l + s - l * s;
            float m1 = l * 2 - m2;
            ret[0] = (int)Math.Round(hueToRgb(m1, m2, h + 1.0f / 3.0f) * MAX_VALUE, MidpointRounding.AwayFromZero);
            ret[1] = (int)Math.Round(hueToRgb(m1, m2, h) * MAX_VALUE, MidpointRounding.AwayFromZero);
            ret[2] = (int)Math.Round(hueToRgb(m1, m2, h - 1.0f / 3.0f) * MAX_VALUE, MidpointRounding.AwayFromZero);
            return ret;
        }

        private static float hueToRgb(float m1, float m2, float h)
        {
            if (h < 0)
            {
                h += 1;
            }
            if (h > 1)
            {
                h -= 1;
            }
            if (h * 6 < 1)
            {
                return m1 + (m2 - m1) * h * 6;
            }
            if (h * 2 < 1)
            {
                return m2;
            }
            if (h * 3 < 2)
            {
                return m1 + (m2 - m1) * (2.0f / 3.0f - h) * 6;
            }
            return m1;
        }

    }

}