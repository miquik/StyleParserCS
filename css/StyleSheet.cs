namespace StyleParserCS.css
{

    /// <summary>
    /// Acts as collection of Rules. Remembers last priority used 
    /// in style sheet to allow incremental parsing. The style sheet
    /// origin is tracked as well for later rule priority evaluation.
    /// The default style sheet origin is "Author". 
    /// 
    /// @author kapy
    /// </summary>
    public interface StyleSheet : Rule<RuleBlock>
    {

        /// <summary>
        /// Sets the stylesheet origin. </summary>
        /// <param name="o"> The origin to be set </param>
        StyleSheet_Origin Origin { set; get; }


        /// <summary>
        /// The origin of the style sheet (user agent, user, or author). 
        /// @author radek
        /// </summary>
    }

    public enum StyleSheet_Origin
    {
        AUTHOR,
        AGENT,
        USER
    }

}