/// <summary>
/// TermUnicodeRangeImpl.java
/// 
/// Created on 2. 11. 2017, 11:20:24 by burgetr
/// </summary>
namespace StyleParserCS.csskit
{
    using TermUnicodeRange = StyleParserCS.css.TermUnicodeRange;

    /// 
    /// <summary>
    /// @author burgetr
    /// </summary>
    public class TermUnicodeRangeImpl : TermImpl<string>, TermUnicodeRange
    {

        public override TermUnicodeRange setValue(string uri)
        {
            this.value = uri;
            return this;
        }

    }

}