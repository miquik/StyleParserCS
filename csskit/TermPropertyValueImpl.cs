using System.Text;

/// 
namespace StyleParserCS.csskit
{
    using CSSProperty = StyleParserCS.css.CSSProperty;
    using StyleParserCS.css;
    using TermPropertyValue = StyleParserCS.css.TermPropertyValue;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermPropertyValueImpl : TermPairImpl<CSSProperty, Term>, TermPropertyValue
    {

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();
            if (operatorv != null)
            {
                sb.Append(operatorv.value());
            }

            if (value != null)
            {
                sb.Append(value);
            }
            else
            {
                sb.Append(key);
            }

            return sb.ToString();
        }


    }

}