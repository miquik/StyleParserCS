using StyleParserCS.css;
using System;
using System.IO;
using System.Net;

/// 
namespace StyleParserCS.csskit
{

    /// <summary>
    /// Default implementation of the NetworkProcessor that is used when no other
    /// implementation is provided. This implementation is based on the java built-in 
    /// UriConnection mechanism.
    /// 
    /// @author burgetr
    /// </summary>
    public class DefaultNetworkProcessor : NetworkProcessor
    {

        //ORIGINAL LINE: @Override public java.io.InputStream fetch(java.net.Uri Uri) throws java.io.IOException
        public Stream fetch(Uri uri)
        {
            WebRequest con = HttpWebRequest.Create(uri);
            return con.GetResponse().GetResponseStream(); // con.GetRequestStream();
            /*
            UriConnection con = Uri.openConnection();
            Stream isv;
            if ("gzip".Equals(con.ContentEncoding, StringComparison.OrdinalIgnoreCase))
            {
                isv = new GZIPInputStream(con.InputStream);
            }
            else
            {
                isv = con.InputStream;
            }

            return isv;
            */
        }
    }

}