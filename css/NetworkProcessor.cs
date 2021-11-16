using System;
using System.IO;

/// 
namespace StyleParserCS.css
{

    /// <summary>
    /// A network processor that is able to obtain an input stream from a Uri.
    /// 
    /// @author burgetr
    /// </summary>
    public interface NetworkProcessor
    {

        /// <summary>
        /// Fetches the resource with the given Uri and creates a stream containing
        /// the resource contents.
        /// </summary>
        /// <param name="Uri"> Resource Uri. </param>
        /// <returns> input stream that reads resource contents </returns>
        /// <exception cref="IOException"> when the stream cannot be obtained for any reason </exception>
        //ORIGINAL LINE: public java.io.InputStream fetch(java.net.Uri Uri) throws java.io.IOException;
        Stream fetch(Uri uri);

    }

}