using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;

namespace StyleParserCS.csskit.antlr4
{
    /// <summary>
    /// Token with encapsulation of LexerState during parse.
    /// Models view at token text by removing syntactic sugar
    /// from tokens with contains it,
    /// e.g. STRING, URI, FUNCTION
    /// @author kapy
    /// 
    /// </summary>
    public class CSSToken : CommonToken
    {

        /// <summary>
        /// Extended with EOF_TOKEN
        /// </summary>
        private const long serialVersionUID = 3L;

        /// <summary>
        /// Current lexer state
        /// </summary>
        protected internal CSSLexerState ls;

        /// <summary>
        /// Base Uri for URIs </summary>
        protected internal Uri basev;

        protected internal bool valid = true;

        // token types
        public const int FUNCTION = 1;
        public const int URI = 2;
        public const int STRING = 3;
        public const int CLASSKEYWORD = 4;
        public const int HASH = 5;
        public const int UNCLOSED_STRING = 6;
        public const int UNCLOSED_URI = 7;

        private readonly TypeMapper typeMapper;

        /// <summary>
        /// Creates CSSToken, this is base {@code emit()} constructor </summary>
        /// <param name="input"> Input stream </param>
        /// <param name="type"> Type of token </param>
        /// <param name="channel"> Channel of token </param>
        /// <param name="start"> Start position in stream </param>
        /// <param name="stop"> End position in stream </param>
        public CSSToken(Tuple<ITokenSource, ICharStream> input, int type, int channel, int start, int stop, TypeMapper typeMapper) : base(input, type, channel, start, stop)
        {
            this.typeMapper = typeMapper;
        }

        /// <summary>
        /// Creates CSSToken of given type with cloning lexer state
        /// automatically </summary>
        /// <param name="type"> Type of token </param>
        /// <param name="state"> State of lexer, which will be copied </param>
        public CSSToken(int type, CSSLexerState state, TypeMapper typeMapper) : this(type, state, 0, 0, typeMapper)
        {
        }

        /// <summary>
        /// Creates CSSToken of given type with cloning lexer state
        /// automatically, allows to set text boundaries in input stream </summary>
        /// <param name="type"> Type of token </param>
        /// <param name="state"> State of lexer, which will be copied </param>
        /// <param name="start"> Start position in stream </param>
        /// <param name="stop"> End position in stream </param>
        public CSSToken(int type, CSSLexerState state, int start, int stop, TypeMapper typeMapper) :
            this(new Tuple<ITokenSource, ICharStream>(null, null), type, TokenConstants.DefaultChannel, start, stop, typeMapper)
        {
            this.ls = new CSSLexerState(state);
        }

        /// <summary>
        /// Sets lexer state for current token </summary>
        /// <param name="state"> Current lexer state </param>
        /// <returns> Modified CSSToken </returns>
        public virtual CSSToken setLexerState(CSSLexerState state)
        {
            this.ls = state;
            return this;
        }

        /// <summary>
        /// Gets lexer state at creation of token </summary>
        /// <returns> the lexer state </returns>
        public virtual CSSLexerState LexerState
        {
            get
            {
                return ls;
            }
        }

        /// <summary>
        /// Obtains the base Uri used for the relative URIs </summary>
        /// <returns> the Uri or null if not set </returns>
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


        public virtual bool Valid
        {
            get
            {
                return valid;
            }
            set
            {
                this.valid = value;
            }
        }

        public int CharPositionInLine
        {
            get => charPositionInLine;
            set => charPositionInLine = value;
        }


        /// <summary>
        /// Considers text as content of STRING token,
        /// and models view at this text as an common string,
        /// that is one character removed from the both beginning
        /// and the end. </summary>
        /// <param name="string"> Content of STRING token </param>
        /// <returns> String with trimmed quotation marks </returns>
        public static string extractSTRING(string stringv)
        {
            return stringv.Substring(1, (stringv.Length - 1) - 1);
        }

        public static string extractUNCLOSEDSTRING(string stringv)
        {
            return stringv.Substring(1, stringv.Length - 1);
        }
        /// <summary>
        /// Considers text as content of URI token,
        /// and models view at this text as an common string,
        /// that is removed {@code 'Uri('} from the beginning
        /// and {@code ')'} from the and. If result of this operation
        /// is STRING, remove even quotation marks </summary>
        /// <param name="uri"> Content of URI token </param>
        /// <returns> String with trimmed URI syntax sugar and
        /// optionally quotation marks </returns>
        public static string extractURI(string uri)
        {
            string ret = uri.Substring(4, (uri.Length - 1) - 4).Trim();
            // trim string
            if (ret.Length > 0 && (ret[0] == '\'' || ret[0] == '"'))
            {
                ret = ret.Substring(1, (ret.Length - 1) - 1);
            }

            return ret;
        }

        public static string extractUNCLOSEDURI(string uri)
        {
            string ret = uri.Substring(4).Trim();
            // trim quotes (if any)
            if (ret.Length > 0)
            {
                //ORIGINAL LINE: final char fc = ret.charAt(0);
                char fc = ret[0];
                if (fc == '\'' || fc == '"')
                {
                    //ORIGINAL LINE: final char lc = (ret.length() > 1) ? ret.charAt(ret.length() - 1) : ' ';
                    char lc = (ret.Length > 1) ? ret[ret.Length - 1] : ' ';
                    if (fc == lc)
                    {
                        ret = ret.Substring(1, (ret.Length - 1) - 1); //both quotes (finished string)
                    }
                    else
                    {
                        ret = ret.Substring(1, ret.Length - 1); //left quote only (unfinished string)
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Considers text as content of FUNCTION token,
        /// and models view at this text as an common string,
        /// that is removed {@code '('} from the end of string </summary>
        /// <param name="function"> Content of FUNCTION token </param>
        /// <returns> String with trimmed FUNCTION open parenthesis </returns>
        public static string extractFUNCTION(string function)
        {
            return function.Substring(0, function.Length - 1);
        }

        /// <summary>
        /// Considers text as content of HASH token,
        /// and models view at this text as an common string,
        /// that is removed {@code '#'} from the beginning of string </summary>
        /// <param name="hash"> Content of HASH token </param>
        /// <returns> String with trimmed HASH # character </returns>
        public static string extractHASH(string hash)
        {
            return hash.Substring(1, hash.Length - 1);
        }

        /// <summary>
        /// Considers text as content of CLASSKEYWORD token,
        /// and models view at this text as an common string,
        /// that is removed {@code '.'} from the beginning of string </summary>
        /// <param name="className"> Content of CLASSKEYWORD token </param>
        /// <returns> String with trimmed CLASSKEYWORD dot </returns>
        public static string extractCLASSKEYWORD(string className)
        {
            return className.Substring(1, className.Length - 1);
        }

        /// <summary>
        /// Returns common text stored in token. Content is not modified. </summary>
        /// <returns> Model view of text in token </returns>
        public override string Text
        {
            get
            {
                // sets text from input if not text directly available                
                string _text = base.Text;
                // Text = base.Text;

                int? t = typeMapper.inverse().get(Type);
                if (t == null)
                {
                    return base.Text;
                }
                /*
                try
                {
                    t = typeMapper.inverse().get(Type);
                }
                catch (KeyNotFoundException)
                {
                    return base.Text;
                }
                */
                switch (t.Value)
                {
                    case FUNCTION:
                        return _text.Substring(0, _text.Length - 1);
                    case URI:
                        return extractURI(_text);
                    case UNCLOSED_URI:
                        return extractUNCLOSEDURI(_text);
                    case STRING:
                        return extractSTRING(_text);
                    case UNCLOSED_STRING:
                        return extractUNCLOSEDSTRING(_text);
                    case CLASSKEYWORD:
                        return extractCLASSKEYWORD(_text);
                    case HASH:
                        return extractHASH(_text);
                    default:
                        return _text;
                }

            }
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("/").Append(ls).Append("/").Append(base.ToString());

            return sb.ToString();
        }

        /// <summary>
        /// Convert between type values defined in two classes.
        /// </summary>
        public class TypeMapper
        {
            internal readonly IDictionary<int, int> map;
            internal readonly TypeMapper inverse_Conflict;
            internal TypeMapper(IDictionary<int, int> map, TypeMapper inverse)
            {
                this.map = map;
                this.inverse_Conflict = inverse;
            }
            public TypeMapper(Type classA, Type classB, params string[] fieldNames)
            {
                map = new SortedDictionary<int, int>();
                IDictionary<int, int> inverseMap = new SortedDictionary<int, int>();
                foreach (string f in fieldNames)
                {
                    try
                    {
                        int a = (int)classA.GetField(f).GetValue(null);
                        int b = (int)classB.GetField(f).GetValue(null);
                        map[a] = b;
                        inverseMap[b] = a;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                }
                inverse_Conflict = new TypeMapper(inverseMap, this);
            }
            //ORIGINAL LINE: public int get(int type) throws NullPointerException
            public virtual int? get(int type)
            {
                if (map.ContainsKey(type) == false)
                {
                    return null;
                }
                return map[type];
            }
            public virtual TypeMapper inverse()
            {
                return inverse_Conflict;
            }
        }

        public static TypeMapper createDefaultTypeMapper(Type lexerClass)
        {
            return new TypeMapper(typeof(CSSToken), lexerClass, "FUNCTION", "URI", "STRING", "CLASSKEYWORD", "HASH", "UNCLOSED_STRING", "UNCLOSED_URI");
        }


        /// <summary>
        /// extract charset value from CHARSET token
        /// </summary>
        public static string extractCHARSET(string charset)
        {
            //ORIGINAL LINE: final String arg = charset.replace("@charset","").replace(";","").trim();
            string arg = charset.Replace("@charset", "").Replace(";", "").Trim();
            if (arg.Length > 2)
            {
                return extractSTRING(arg);
            }
            else
            {
                return "";
            }
        }
    }

}