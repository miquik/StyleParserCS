using System.Collections.Generic;
using System.Text;

/// <summary>
/// TermRectImpl.java
/// 
/// Created on 22. 3. 2017, 16:06:36 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{

    using StyleParserCS.css;
    using TermLength = StyleParserCS.css.TermLength;
    using TermRect = StyleParserCS.css.TermRect;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermRectImpl : TermImpl<IList<TermLength>>, TermRect
    {

        public TermRectImpl(TermLength a, TermLength b, TermLength c, TermLength d)
        {
            value = new List<TermLength>(4);
            value.Add(a);
            value.Add(b);
            value.Add(c);
            value.Add(d);
        }

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(OutputUtil.RECT_KEYWORD);
            ret.Append(OutputUtil.FUNCTION_OPENING);
            for (int i = 0; i < 4; i++)
            {
                if (i != 0)
                {
                    ret.Append(OutputUtil.SPACE_DELIM);
                }
                //ORIGINAL LINE: StyleParserCS.css.Term<?> v = value.get(i);
                TermLength v = value[i];
                ret.Append((v == null) ? "auto" : v.ToString());
            }
            ret.Append(OutputUtil.FUNCTION_CLOSING);
            return ret.ToString();
        }

    }

}