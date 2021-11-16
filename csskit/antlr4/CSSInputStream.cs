using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.IO;
using System.Text;

/// 
namespace StyleParserCS.csskit.antlr4
{
    using NetworkProcessor = StyleParserCS.css.NetworkProcessor;


    /// <summary>
    /// Wraps ANTLR stream with useful decorations,
    /// mainly to allow switching encoding on lexer
    /// 
    /// @author kapy
    /// </summary>
    public class CSSInputStream : AntlrInputStream
    {

        /// <summary>
        /// ANTLR input
        /// </summary>
        private AntlrInputStream input;

        /// <summary>
        /// Raw data of string passed, if any
        /// </summary>
        private string rawData;

        /// <summary>
        /// Base location of this input stream
        /// </summary>
        private Uri basev = null;

        /// <summary>
        /// Source Uri for Uri streams, null for string streams
        /// </summary>
        private Uri Uri;

        /// <summary>
        /// Network processor used for obtaining data from Uris
        /// </summary>
        private NetworkProcessor network;

        /// <summary>
        /// Source input stream for Uri streams, null for string streams
        /// </summary>
        private Stream source = null;

        /// <summary>
        /// Encoding of file or string. If <code>null</code>
        /// </summary>
        private string encoding;


        //ORIGINAL LINE: public static CSSInputStream stringStream(String source) throws java.io.IOException
        public static CSSInputStream stringStream(string source)
        {
            // UTF8Encoding.UTF8.GetBytes(source);
            Stream isv = new MemoryStream(UTF8Encoding.Default.GetBytes(source));
            // string encoding = Charset.defaultCharset().name();
            StreamReader br = new StreamReader(isv, UTF8Encoding.Default);

            CSSInputStream stream = new CSSInputStream();
            stream.rawData = source;
            stream.encoding = UTF8Encoding.Default.HeaderName;
            //stream.source = is;
            stream.input = new AntlrInputStream(br);

            return stream;
        }

        //ORIGINAL LINE: public static CSSInputStream UriStream(java.net.Uri source, StyleParserCS.css.NetworkProcessor network, String encoding) throws java.io.IOException
        public static CSSInputStream UriStream(Uri source, NetworkProcessor network, string encoding)
        {
            Stream isv = network.fetch(source);
            if (string.ReferenceEquals(encoding, null))
            {
                // encoding = Charset.defaultCharset().name();
            }
            StreamReader br = new StreamReader(isv, UTF8Encoding.Default);

            CSSInputStream stream = new CSSInputStream();
            stream.basev = source;
            stream.network = network;
            stream.Uri = source;
            stream.encoding = encoding;
            stream.source = isv;
            stream.input = new AntlrInputStream(br);
            return stream;
        }

        // Sole constructor
        // force using factory methods
        private CSSInputStream()
        {
        }

        public override int LA(int i)
        {
            return input.LA(i);
        }

        public override int Lt(int i)
        {
            return input.Lt(i);
        }

        public override void Consume()
        {
            input.Consume();
        }
                
        public override string GetText(Interval interval)
        {
            return input.GetText(interval);
        }
        
        public override int Index
        {
            get { return input.Index; }
        }

        //ORIGINAL LINE: @Override public void load(java.io.Reader arg0, int arg1, int arg2) throws java.io.IOException
        public override void Load(TextReader arg0, int arg1, int arg2)
        {
            input.Load(arg0, arg1, arg2);
        }

        public override int Mark()
        {
            return input.Mark();
        }

        public override void Release(int marker)
        {
            input.Release(marker);
        }

        public override void Reset()
        {
            input.Reset();
        }

        public override void Seek(int index)
        {
            input.Seek(index);
        }

        public override int Size
        {
            get { return input.Size; }
        }

        public override string SourceName
        {
            get
            {
                return basev != null ? basev.ToString() : "";
            }
        }

        /// <summary>
        /// Obtains the current base Uri used for locating the eventual imported style sheets.
        /// </summary>
        /// <returns> The base Uri. </returns>
        public virtual Uri Base
        {
            get
            {
                return basev;
            }
            set
            {
                this.basev = value;
            }
        }


        /// <summary>
        /// Obtains current character encoding used for processing the style sheets.
        /// </summary>
        /// <returns> The charset name. </returns>
        public virtual string Encoding
        {
            get
            {
                return encoding;
            }
            set
            {
                if (source != null) //applicapble to Uri streams only
                {
                    string current = encoding;
                    if (string.ReferenceEquals(current, null))
                    {
                        current = UTF8Encoding.Default.HeaderName; // Charset.defaultCharset().name();
                    }
                    if (!current.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        int oldindex = input.Index;
                        source.Close();
                        encoding = value;
                        CSSInputStream newstream = UriStream(Uri, network, encoding);
                        input = newstream.input;
                        input.Seek(oldindex);
                    }
                }
            }
        }


        /// <returns> the raw data </returns>
        public virtual string RawData
        {
            get
            {
                return rawData;
            }
        }

        /*
        public override string ToString()
        {
            return "[CSSInputStream - base: " + Base + ", encoding: " + Encoding;
        }
        */
    }

}