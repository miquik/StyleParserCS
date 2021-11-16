namespace StyleParserCS.css
{
    /// <summary>
    /// Forces human readable variant of ToString() method.
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public interface PrettyOutput
    {

        /// <summary>
        /// Pretty output of CSS definition using indentation </summary>
        /// <param name="depth"> Number of times output is indented </param>
        /// <returns> String with given rule </returns>
        /// <seealso cref= StyleParserCS.csskit.OutputUtil </seealso>
        string ToString(int depth);

    }

}