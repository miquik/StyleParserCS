/// <summary>
/// MediaSpecType.java
/// 
/// Created on 8. 7. 2014, 11:01:23 by burgetr
/// </summary>
namespace StyleParserCS.css
{
    /// <summary>
    /// A special case of media specification that matches all media queries with the given media type
    /// regardless on the remaining media features. This specification <b>should be used with care</b> because
    /// it matches even the disjunct queries. E.g. when used for {@code screen} media, it matches
    /// both {@code screen AND (min-width: 1000px)} and {@code screen AND (max-width: 999px)}.
    /// 
    /// @author burgetr
    /// </summary>
    public class MediaSpecType : MediaSpec
    {

        /// <summary>
        /// Creates a new media specification that matches all media queries with the given media type. </summary>
        /// <param name="type"> </param>
        public MediaSpecType(string type) : base(type)
        {
        }

        public override bool matches(MediaExpression e)
        {
            return true; //we match all the expressions
        }

        public override string ToString()
        {
            return type + "[*]";
        }

    }

}