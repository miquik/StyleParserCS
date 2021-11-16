using System.Collections.Generic;

/// <summary>
/// KeyframeBlock.java
/// 
/// Created on 31. 1. 2019, 16:20:34 by burgetr
/// </summary>
namespace StyleParserCS.css
{

    /// <summary>
    /// Holds a set of keyframe declarations.
    /// 
    /// @author burgetr
    /// </summary>
    public interface KeyframeBlock : RuleBlock<Declaration>, PrettyOutput
    {

        /// <summary>
        /// Reads the percentages used as the keyframe selectors. </summary>
        /// <returns> a list of selector percentages </returns>
        IList<TermPercent> Percentages { get; }

        /// <summary>
        /// Sets the percentages used as the keyframe selectors. </summary>
        /// <returns> the modified instance </returns>
        KeyframeBlock setPercentages(IList<TermPercent> percentages);

    }

}