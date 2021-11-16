using System;
using System.IO;
using System.Net;

/// <summary>
/// DataUriConnection.java
/// </summary>
namespace org.fit.net
{

    /// <summary>
    /// The Uri connection for the data: URI scheme. 
    /// @author burgetr
    /// </summary>
    internal class DataUriConnection : WebClient //  UriConnection
    {
        private Uri uri;
        private string mime;
        private string charset;
        private byte[] data;

        public DataUriConnection(Uri uri, string mime, string charset, byte[] data) : base()
        {
            this.uri = uri;
            this.mime = mime;
            this.charset = charset;
            this.data = data;                    
        }

        protected internal DataUriConnection(Uri uri) : base()
        {
            this.uri = uri;            
        }

        //ORIGINAL LINE: @Override public void connect() throws java.io.IOException
        public void connect()
        {
        }

        //ORIGINAL LINE: @Override public java.io.InputStream getInputStream() throws java.io.IOException
        public Stream InputStream
        {
            get
            {
                return new MemoryStream(data);
            }
        }
    }
}