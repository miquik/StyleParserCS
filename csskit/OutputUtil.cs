using System;
using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.csskit
{

    using PrettyOutput = StyleParserCS.css.PrettyOutput;
    using StyleParserCS.css;
    using TermOperator = StyleParserCS.css.TermOperator;

    /// <summary>
    /// Helper class for generation output for given CSS rules
    /// @author kapy
    /// 
    /// </summary>
    public class OutputUtil
    {

        public const string EMPTY_DELIM = "";
        public const string SPACE_DELIM = " ";
        public const string DEPTH_DELIM = "\t";
        public const string QUERY_DELIM = " AND ";
        public const string RULE_OPENING = " {\n";
        public const string RULE_CLOSING = "}\n";
        public const string MEDIA_DELIM = ", ";
        public const string SELECTOR_DELIM = ", ";
        public const string IMPORT_KEYWORD = "@import ";
        public const string Uri_OPENING = "Uri('";
        public const string Uri_CLOSING = "')";
        public const string LINE_CLOSING = ";\n";
        public const string NEW_LINE = "\n";
        public const string MEDIA_KEYWORD = "@media ";
        public const string KEYFRAMES_KEYWORD = "@keyframes ";
        public const string RULE_DELIM = "\n";
        public const string CHARSET_KEYWORD = "@charset ";
        public const string CHARSET_OPENING = "\"";
        public const string CHARSET_CLOSING = "\";\n";
        public const string PROPERTY_OPENING = ": ";
        public const string PROPERTY_CLOSING = ";\n";
        public const string IMPORTANT_KEYWORD = "!important";
        public const string PAGE_KEYWORD = "@page";
        public const string PSEUDO_OPENING = ":";
        public const string PAGE_CLOSING = "";
        public const string VIEWPORT_KEYWORD = "@viewport";
        public const string FONT_FACE_KEYWORD = "@font-face";
        public const string FUNCTION_OPENING = "(";
        public const string FUNCTION_CLOSING = ")";
        public const string STRING_OPENING = "'";
        public const string STRING_CLOSING = "'";
        public const string ATTRIBUTE_OPENING = "[";
        public const string ATTRIBUTE_CLOSING = "]";
        public const string PERCENT_SIGN = "%";
        public const string HASH_SIGN = "#";
        public const string MARGIN_AREA_OPENING = "@";
        public const string MEDIA_EXPR_OPENING = "(";
        public const string MEDIA_EXPR_CLOSING = ")";
        public const string MEDIA_FEATURE_DELIM = ": ";
        public const string CALC_KEYWORD = "calc";
        public const string RECT_KEYWORD = "rect";




        /// <summary>
        /// Appends string multiple times to buffer </summary>
        /// <param name="sb"> StringBuilder to be modified </param>
        /// <param name="append"> String to be added </param>
        /// <param name="times"> Number of times <code>append</code> is added </param>
        /// <returns> Modified StringBuilder <code>sb</code> to allow chaining </returns>
        public static StringBuilder appendTimes(StringBuilder sb, string append, int times)
        {

            for (; times > 0; times--)
            {
                sb.Append(append);
            }

            return sb;
        }

        /// <summary>
        /// Appends all elements of <code>array</code> to buffer, separated by delimiter </summary>
        /// @param <T> Type of elements stored in <code>array</code> </param>
        /// <param name="sb"> StringBuilder to be modified </param>
        /// <param name="array"> Array of elements </param>
        /// <param name="delimiter"> Delimiter to separate elements </param>
        /// <returns> Modified <code>sb</code> to allow chaining </returns>
        public static StringBuilder appendArray<T>(StringBuilder sb, T[] array, string delimiter)
        {

            bool firstRun = true;

            foreach (T elem in array)
            {
                if (!firstRun)
                {
                    sb.Append(delimiter);
                }
                else
                {
                    firstRun = false;
                }

                sb.Append(elem.ToString());
            }

            return sb;

        }

        /// <summary>
        /// Appends all elements of <code>list</code> to buffer, separated by delimiter </summary>
        /// @param <T> Type of elements stored in <code>list</code> </param>
        /// <param name="sb"> StringBuilder to be modified </param>
        /// <param name="list"> List of elements </param>
        /// <param name="delimiter"> Delimiter to separate elements </param>
        /// <returns> Modified <code>sb</code> to allow chaining </returns>
        public static StringBuilder appendList<T>(StringBuilder sb, IList<T> list, string delimiter)
        {

            bool firstRun = true;

            foreach (T elem in list)
            {
                if (!firstRun)
                {
                    sb.Append(delimiter);
                }
                else
                {
                    firstRun = false;
                }

                sb.Append(elem.ToString());
            }

            return sb;

        }

        /// <summary>
        /// Appends of elements of <code>list</code> to list, separater by delimiter.
        /// Uses depth parameter to make output nicer for each element </summary>
        /// @param <T> List of elements, which implements <code>Rule</code> </param>
        /// <param name="sb"> StringBuilder to be modified </param>
        /// <param name="list"> List of elements </param>
        /// <param name="delimiter"> Delimeter between elements </param>
        /// <param name="depth"> Depth of each element </param>
        /// <returns> Modified <code>sb</code> to allow chaining </returns>
        public static StringBuilder appendList<T>(StringBuilder sb, IList<T> list, string delimiter, int depth) where T : StyleParserCS.css.PrettyOutput
        {

            bool firstRun = true;

            foreach (T elem in list)
            {
                if (!firstRun)
                {
                    sb.Append(delimiter);
                }
                else
                {
                    firstRun = false;
                }

                sb.Append(elem.ToString(depth));
            }

            return sb;
        }

        /// <summary>
        /// Appends the calc() function arguments to a string builder. </summary>
        /// <param name="sb"> the string builder to be modified </param>
        /// <param name="args"> the calc arguments </param>
        /// <returns> Modified <code>sb</code> to allow chaining </returns>
        public static StringBuilder appendCalcArgs(StringBuilder sb, CalcArgs args)
        {
            //ORIGINAL LINE: final String astr = args.evaluate(CalcArgs.stringEvaluator);
            string astr = args.evaluate(CalcArgs.stringEvaluator);
            if (!astr.StartsWith(FUNCTION_OPENING, StringComparison.Ordinal))
            {
                sb.Append(FUNCTION_OPENING);
                sb.Append(astr);
                sb.Append(FUNCTION_CLOSING);
            }
            else
            {
                sb.Append(astr);
            }
            return sb;
        }

        /// <summary>
        /// Appends the formatted list of function arguments to a string builder. The individual
        /// arguments are separated by spaces with the exception of commas. </summary>
        /// <param name="sb"> the string builder to be modified </param>
        /// <param name="list"> the list of function arguments </param>
        /// <returns> Modified <code>sb</code> to allow chaining </returns>
        public static StringBuilder appendFunctionArgs(StringBuilder sb, IList<Term> list)
        {

            //ORIGINAL LINE: StyleParserCS.css.Term<?> prev = null, pprev = null;
            Term prev = null, pprev = null;

            //ORIGINAL LINE: for (StyleParserCS.css.Term<?> elem : list)
            foreach (Term elem in list)
            {
                bool sep = true;
                if (elem is TermOperator && ((TermOperator)elem).Value == ',')
                {
                    sep = false; //no spaces before commas
                }
                if ((prev != null && prev is TermOperator && ((TermOperator)prev).Value == '-') && (pprev == null || pprev is TermOperator)) //nothing or an operator before -
                {
                    sep = false; //no spaces after unary minus
                }
                if (prev != null && sep)
                {
                    sb.Append(SPACE_DELIM);
                }
                pprev = prev;
                prev = elem;

                sb.Append(elem.ToString());
            }

            return sb;
        }
    }

}