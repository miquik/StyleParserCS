namespace StyleParserCS.css
{

    /// <summary>
    /// Contains collection of CSS declarations specified for a page rule.
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// @author Bert Frees, 2012
    /// </summary>
    public interface RulePage : RuleBlock<Rule>, PrettyOutput
    {

        /// <summary>
        /// Gets name of the page </summary>
        /// <returns> Name of the page </returns>
        string Name { get; }

        /// <summary>
        /// Sets name of the page </summary>
        /// <param name="name"> New name of the page </param>
        /// <returns> Modified instance </returns>
        RulePage setName(string name);

        /// <summary>
        /// Gets pseudo-class of the page </summary>
        /// <returns> Pseudo-class of the page </returns>
        Selector_PseudoPage Pseudo { get; }

        /// <summary>
        /// Sets pseudo-class of the page </summary>
        /// <param name="pseudo"> New pseudo-class of the page </param>
        /// <returns> Modified instance </returns>
        RulePage setPseudo(Selector_PseudoPage pseudo);

    }

}