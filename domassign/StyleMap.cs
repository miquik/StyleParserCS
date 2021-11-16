/// <summary>
/// StyleMap.java
/// 
/// Created on 22.1.2010, 16:06:07 by burgetr
/// </summary>
namespace StyleParserCS.domassign
{
    using AngleSharp.Dom;
    using CSSFactory = StyleParserCS.css.CSSFactory;
    using NodeData = StyleParserCS.css.NodeData;
    using Selector_PseudoElementType = StyleParserCS.css.Selector_PseudoElementType;

    /// <summary>
    /// This is a map that assigns a style to a particular elements and moreover, it
    /// gathers the information about the pseudo elements. 
    /// 
    /// @author burgetr
    /// </summary>
    public class StyleMap : MultiMap<IElement, Selector_PseudoElementType, NodeData>
    {

        public StyleMap(int size) : base(size)
        {
        }

        protected internal override NodeData createDataInstance()
        {
            return CSSFactory.createNodeData();
        }

    }

}