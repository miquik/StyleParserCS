namespace StyleParserCS.css
{
    public interface RuleBlock
    {
        /// <summary>
        /// Sets the owner style sheet for this rule. </summary>
        /// <param name="sheet"> The stylesheet where this rule is contained. </param>
        StyleSheet StyleSheet { set; get; }
    }


    /// <summary>
    /// Special case of rule, where rule is meant to be comparable
    /// with other rules to determine priority of CSS declarations
    /// @author kapy
    /// </summary>
    /// @param <T> Internal content of rule </param>
    public interface RuleBlock<T> : Rule<T>, RuleBlock
    {
    }

}