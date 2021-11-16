using System.Collections.Generic;

namespace StyleParserCS.css
{

    /// <summary>
    /// Contains CSS rules associated with medias. 
    /// Externally provides view of collection of defined RuleSet 
    /// with additional media information.
    /// 
    /// @author burgetr
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008,
    /// 
    /// </summary>
    public interface RuleMedia : RuleBlock<RuleSet>, PrettyOutput
    {

        /// <summary>
        /// Returns list of all media associated with this rule </summary>
        /// <returns> List of media </returns>
        IList<MediaQuery> MediaQueries { get; }

        /// <summary>
        /// Sets media associated with rules </summary>
        /// <param name="media"> Media associated </param>
        /// <returns> Modified instance </returns>
        RuleMedia setMediaQueries(IList<MediaQuery> media);


    }

}