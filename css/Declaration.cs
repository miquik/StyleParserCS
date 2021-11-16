using System;

namespace StyleParserCS.css
{

    /// <summary>
    /// Basic CSS declaration consisting of list of terms.
    /// Implements comparable to allow set declaration with bigger priority,
    /// either by its important! flag or by its selector's specificity
    /// 
    /// @author burgetr
    /// @author kapy
    /// @author Jan Svercl, VUT Brno, 2008
    /// </summary>
    public interface Declaration : Rule<Term>, PrettyOutput, IComparable<Declaration>
    {

        bool Important { get; set; }


        /// <summary>
        /// Obtains the property name. </summary>
        /// <returns> The property name (always lowercase) </returns>
        string Property { get; set; }


        Declaration_Source Source { get; set; }


        //==================================================================================================

        /// <summary>
        /// The declaration source information. 
        /// @author burgetr
        /// </summary>

    }

    public class Declaration_Source
    {

        internal Uri uri;
        internal int line;
        internal int position;

        public Declaration_Source(Uri uri, int line, int position)
        {
            this.uri = uri;
            this.line = line;
            this.position = position;
        }

        public Declaration_Source(Declaration_Source other)
        {
            this.uri = other.uri;
            this.line = other.line;
            this.position = other.position;
        }

        public virtual Uri Uri
        {
            get
            {
                return uri;
            }
            set
            {
                this.uri = value;
            }
        }


        public virtual int Line
        {
            get
            {
                return line;
            }
            set
            {
                this.line = value;
            }
        }


        public virtual int Position
        {
            get
            {
                return position;
            }
            set
            {
                this.position = value;
            }
        }


        public override string ToString()
        {
            return ((Uri == null) ? "<internal>" : Uri.ToString()) + ":" + line + ":" + position;
        }
    }

}