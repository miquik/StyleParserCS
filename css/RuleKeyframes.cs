/// 
namespace StyleParserCS.css
{
    /// <summary>
    /// Contains collection of CSS declarations specified for a keyframes rule.
    /// 
    /// @author burgetr
    /// </summary>
    public interface RuleKeyframes : RuleBlock<KeyframeBlock>, PrettyOutput
    {

        /// <summary>
        /// Gets name of the keyframe list </summary>
        /// <returns> Name of the keyframe list </returns>
        string Name { get; }

        /// <summary>
        /// Sets name of the keyframe list </summary>
        /// <param name="name"> New name of the keyframe list </param>
        /// <returns> Modified instance </returns>
        RuleKeyframes setName(string name);

    }

}