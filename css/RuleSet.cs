using System.Collections.Generic;

namespace StyleParserCS.css
{

    /// <summary>
    /// Holds set of CSS declarations for specified selectors.
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public interface RuleSet : RuleBlock<Declaration>, PrettyOutput
    {

        /// <summary>
        /// Gets selectors of given declaration
        /// </summary>
        /// <returns> Selectors for this rule </returns>
        CombinedSelector[] getSelectors();

        /// <summary>
        /// Sets selectors for this CSS declarations
        /// </summary>
        /// <param name="selectors">
        ///            Selectors to be set </param>
        /// <returns> Modified instance </returns>
        RuleSet setSelectors(IList<CombinedSelector> selectors);

    }

}