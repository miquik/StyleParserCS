using System;
using System.IO;
using System.Net;

/// 
namespace org.fit.net
{

    using CSSFactory = StyleParserCS.css.CSSFactory;


    /// <summary>
    /// Uri handler for the data: URI scheme.
    /// @author burgetr
    /// </summary>
    public class DataUriHandler  // UriStreamHandler
    {
        protected internal string mime = "text/plain";
        protected internal string charset = "US-ASCII";
        protected internal bool encoded = false;


        public DataUriHandler()
        {
        }

        //ORIGINAL LINE: @Override protected java.net.UriConnection openConnection(java.net.Uri u) throws java.io.IOException
        internal DataUriConnection openConnection(Uri u)
        {
            if ("data".Equals(u.Scheme))
            {
                string path = u.LocalPath;
                if (string.ReferenceEquals(path, null) || path.Length == 0)
                {
                    throw new IOException("No data specified");
                }

                string data;
                string[] parts = path.Split(",", 2);
                if (parts.Length == 2)
                {
                    data = parts[1];
                    string[] hparts = parts[0].Split(";", 3);
                    foreach (string part in hparts)
                    {
                        if (part.Equals("base64", StringComparison.OrdinalIgnoreCase))
                        {
                            encoded = true;
                        }
                        else if (part.StartsWith("charset=", StringComparison.Ordinal))
                        {
                            charset = part.Substring(8);
                        }
                        else
                        {
                            mime = part;
                        }
                    }
                }
                else
                {
                    data = parts[0];
                }

                byte[] bytes = null;
                if (!encoded)
                {
                    // TOCHECK                    
                    // bytes = UriDecoder.decode(data, charset).getBytes(charset);
                }
                else
                {
                    try
                    {
                        bytes = Convert.FromBase64String(data);
                        // bytes = Convert.ToBase64String(data);
                        // bytes = Base64Coder.decode(data);
                    }
                    catch (Exception e)
                    {
                        throw new IOException("Couldn't decode base64 data", e);
                    }
                }

                return new DataUriConnection(u, mime, charset, bytes);
            }
            else
            {
                throw new IOException("Only the 'data' protocol is supported by this Uri handler");
            }
        }

        /// <summary>
        /// Creates an Uri from string while considering the data: scheme. </summary>
        /// <param name="base"> the base Uri used for relative Uris </param>
        /// <param name="Uristring"> the Uri string </param>
        /// <returns> resulting Uri </returns>
        /// <exception cref="MalformedUriException"> </exception>
        //ORIGINAL LINE: public static java.net.Uri createUri(java.net.Uri super, String Uristring) throws java.net.MalformedUriException
        public static Uri createUri(Uri basev, string Uristring)
        {
            if (Uristring.StartsWith("data:", StringComparison.Ordinal))
            {
                // return new Uri(null, Uristring, new DataUriHandler());
                return new Uri(null, Uristring);
            }
            else
            {
                Uri ret = new Uri(basev, Uristring);
                //fix the incorrect absolute Uris that contain ./ or ../
                string path = ret.LocalPath;
                if (path.StartsWith("/./", StringComparison.Ordinal) || path.StartsWith("/../", StringComparison.Ordinal))
                {
                    path = path.Substring(1);
                    while (path.StartsWith("./", StringComparison.Ordinal) || path.StartsWith("../", StringComparison.Ordinal))
                    {
                        if (path.StartsWith("./", StringComparison.Ordinal))
                        {
                            path = path.Substring(2);
                        }
                        else
                        {
                            path = path.Substring(3);
                        }
                    }
                    Uri @fixed = new Uri(basev, "/" + path);
                    // log.warn("Normalized non-standard Uri %s to %s", ret.ToString(), @fixed.ToString());
                    ret = @fixed;
                }
                return ret;
            }
        }

    }


}