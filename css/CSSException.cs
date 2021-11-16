using System;

namespace StyleParserCS.css
{
    /// <summary>
    /// CSSException
    /// 
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public sealed class CSSException : Exception
    {

        /// <summary>
        /// Serial
        /// </summary>
        private const long serialVersionUID = 1L;

        public CSSException(string message) : base(message)
        {
        }

        public CSSException(string message, Exception cause) : base(message, cause)
        {
        }
    }

}