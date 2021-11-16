using System;
using System.Collections.Generic;

/// <summary>
/// GenericRadialGradient.java
/// 
/// Created on 17. 5. 2018, 13:14:05 by burgetr
/// </summary>
namespace StyleParserCS.csskit.fn
{

    using CSSFactory = StyleParserCS.css.CSSFactory;
    using StyleParserCS.css;
    using TermFactory = StyleParserCS.css.TermFactory;
    using TermIdent = StyleParserCS.css.TermIdent;
    using TermLength = StyleParserCS.css.TermLength;
    using TermLengthOrPercent = StyleParserCS.css.TermLengthOrPercent;
    using TermList = StyleParserCS.css.TermList;
    using TermPercent = StyleParserCS.css.TermPercent;
    using System.Linq;

    /// <summary>
    /// Base class for both the radial and repating-radial gradient implementations.
    /// 
    /// @author burgetr
    /// </summary>
    public class GenericRadialGradient : GenericGradient
    {

        //default property values
        private static readonly TermPercent DEFAULT_POSITION = CSSFactory.TermFactory.createPercent(50.0f);
        private static readonly TermIdent DEFAULT_SHAPE = CSSFactory.TermFactory.createIdent("ellipse");
        private static readonly TermIdent CIRCLE_SHAPE = CSSFactory.TermFactory.createIdent("circle");
        private static readonly TermIdent DEFAULT_SIZE = CSSFactory.TermFactory.createIdent("farthest-corner");

        private TermIdent shape;
        private TermLengthOrPercent[] size;
        private TermIdent sizeIdent;
        private TermLengthOrPercent[] position;

        public virtual TermIdent Shape
        {
            get
            {
                return shape;
            }
        }

        public virtual TermLengthOrPercent[] Size
        {
            get
            {
                return size;
            }
        }

        public virtual TermIdent SizeIdent
        {
            get
            {
                return sizeIdent;
            }
        }

        public virtual TermLengthOrPercent[] Position
        {
            get
            {
                return position;
            }
        }

        public override TermList setValue(IList<Term> value)
        {
            base.setValue(value);
            //ORIGINAL LINE: java.util.List<java.util.List<StyleParserCS.css.Term<?>>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            IList<IList<Term>> args = getSeparatedArgs((Term)DEFAULT_ARG_SEP);
            if (args.Count > 1)
            {
                int firstStop = 0;
                //check for shape and size
                if (decodeShape(args[0]))
                {
                    firstStop = 1;
                }
                else
                { //no shape info provided, use defaults
                    sizeIdent = DEFAULT_SIZE;
                    shape = DEFAULT_SHAPE;
                    position = new TermLengthOrPercent[2];
                    position[0] = position[1] = DEFAULT_POSITION;
                }
                //check for stops
                loadColorStops(args, firstStop);
                if (ColorStops != null)
                {
                    Valid = true;
                }
            }
            return this;
        }

        private bool decodeShape(IList<Term> arglist)
        {
            //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> args = new java.util.ArrayList<>(arglist);
            IList<Term> args = new List<Term>(arglist);
            //determine the 'at' position
            int atpos = -1;
            for (int i = 0; i < args.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> arg = args.get(i);
                Term arg = args[i];
                if (arg is TermIdent && ((TermIdent)arg).Value.ToLower() == "at")
                {
                    atpos = i;
                    break;
                }
            }
            if (atpos != -1)
            {
                //ORIGINAL LINE: java.util.List<StyleParserCS.css.Term<?>> posList = args.subList(atpos + 1, args.size());
                // TOCHECK
                IList<Term> posList = args.Skip(atpos + 1).Take(args.Count - atpos - 1).ToList(); // args.subList(atpos + 1, args.Count);
                if (!decodePosition(posList))
                {
                    return false;
                }
                // TOCHECK
                args = args.Take(atpos).ToList();   // subList(0, atpos);
            }
            else
            { //no position, use the default (center)
                position = new TermLengthOrPercent[2];
                position[0] = position[1] = DEFAULT_POSITION;
            }
            //determine the shape
            bool isCircle = false;
            //ORIGINAL LINE: for (java.util.Iterator<StyleParserCS.css.Term<?>> it = args.iterator(); it.hasNext();)
            for (int i = 0; i < args.Count; i++)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> arg = it.Current;
                Term arg = args[i];
                string idval = (arg is TermIdent) ? ((TermIdent)arg).Value : null;
                if (!string.ReferenceEquals(idval, null) && (idval.Equals("circle", StringComparison.OrdinalIgnoreCase) || idval.Equals("ellipse", StringComparison.OrdinalIgnoreCase)))
                {
                    shape = (TermIdent)arg;
                    isCircle = idval.Equals("circle", StringComparison.OrdinalIgnoreCase);
                    args.RemoveAt(i--);
                    break;
                }
            }
            /*
            for (IEnumerator<Term> it = args.GetEnumerator(); it.MoveNext();)
            {
                //ORIGINAL LINE: StyleParserCS.css.Term<?> arg = it.Current;
                Term arg = it.Current;
                string idval = (arg is TermIdent) ? ((TermIdent)arg).Value : null;
                if (!string.ReferenceEquals(idval, null) && (idval.Equals("circle", StringComparison.OrdinalIgnoreCase) || idval.Equals("ellipse", StringComparison.OrdinalIgnoreCase)))
                {
                    shape = (TermIdent)arg;
                    isCircle = idval.Equals("circle", StringComparison.OrdinalIgnoreCase);
                    it.remove();
                    break;
                }
            }
            */
            //determine the size
            if (shape == null)
            { //shape not given
                if (args.Count == 0)
                {
                    sizeIdent = DEFAULT_SIZE;
                    shape = DEFAULT_SHAPE;
                }
                else if (args.Count == 1)
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> arg = args.get(0);
                    Term arg = args[0];
                    if (isExtentKeyword(arg))
                    {
                        sizeIdent = (TermIdent)arg;
                        shape = DEFAULT_SHAPE; //see https://drafts.csswg.org/css-images-3/#radial-gradients
                    }
                    else if (arg is TermLength)
                    {
                        size = new TermLengthOrPercent[1];
                        size[0] = (TermLength)arg;
                        shape = CIRCLE_SHAPE;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (args.Count == 2)
                {
                    size = new TermLengthOrPercent[2];
                    int i = 0;
                    //ORIGINAL LINE: for (StyleParserCS.css.Term<?> arg : args)
                    foreach (Term arg in args)
                    {
                        if (arg is TermLengthOrPercent)
                        {
                            size[i++] = (TermLengthOrPercent)arg;
                        }
                        else
                        {
                            size = null;
                            return false;
                        }
                    }
                    shape = DEFAULT_SHAPE;
                }
                else
                {
                    return false;
                }
            }
            else if (!isCircle)
            { //ellipse
                if (args.Count == 0)
                {
                    sizeIdent = DEFAULT_SIZE;
                }
                else if (args.Count == 1)
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> arg = args.get(0);
                    Term arg = args[0];
                    if (isExtentKeyword(arg))
                    {
                        sizeIdent = (TermIdent)arg;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (args.Count == 2)
                {
                    size = new TermLengthOrPercent[2];
                    int i = 0;
                    //ORIGINAL LINE: for (StyleParserCS.css.Term<?> arg : args)
                    foreach (Term arg in args)
                    {
                        if (arg is TermLengthOrPercent)
                        {
                            size[i++] = (TermLengthOrPercent)arg;
                        }
                        else
                        {
                            size = null;
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            { //circle
                if (args.Count == 0)
                {
                    sizeIdent = DEFAULT_SIZE;
                }
                else if (args.Count == 1)
                {
                    //ORIGINAL LINE: StyleParserCS.css.Term<?> arg = args.get(0);
                    Term arg = args[0];
                    if (isExtentKeyword(arg))
                    {
                        sizeIdent = (TermIdent)arg;
                    }
                    else if (arg is TermLength)
                    {
                        size = new TermLengthOrPercent[1];
                        size[0] = (TermLength)arg;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            //parsed ok
            return true;
        }

        private bool decodePosition(IList<Term> arglist)
        {
            if (arglist.Count == 1 || arglist.Count == 2)
            {

                position = new TermLengthOrPercent[2];
                //distribute terms to position
                //ORIGINAL LINE: for (StyleParserCS.css.Term<?> term : arglist)
                foreach (Term term in arglist)
                {
                    if (term is TermIdent || term is TermLengthOrPercent)
                    {
                        // storeBackgroundPosition(position.Cast<Term>().ToArray(), term);
                        // OK: storeBackgroundPosition(position.Cast<Term>().ToArray(), term);
                        // TOCHECK
                        storeBackgroundPosition((Term[])((object)position), term);
                    }
                    else
                    {
                        return false;
                    }
                }
                //check validity
                int assigned = 0;
                int valid = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (position[i] == null)
                    {
                        position[i] = DEFAULT_POSITION;
                        valid++;
                    }
                    else if (position[i] is TermLengthOrPercent)
                    {
                        assigned++;
                        valid++;
                    }
                }
                return (assigned > 0 && valid == 2);

            }
            else
            {
                return false;
            }
        }

        private void storeBackgroundPosition(Term[] storage, Term term)
        {

            if (term is TermIdent)
            {
                string idval = ((TermIdent)term).Value;
                TermFactory tf = CSSFactory.TermFactory;
                if (idval.Equals("left", StringComparison.OrdinalIgnoreCase))
                {
                    setPositionValue(storage, 0, (Term)tf.createPercent(0.0f));
                }
                else if (idval.Equals("right", StringComparison.OrdinalIgnoreCase))
                {
                    setPositionValue(storage, 0, (Term)tf.createPercent(100.0f));
                }
                else if (idval.Equals("top", StringComparison.OrdinalIgnoreCase))
                {
                    setPositionValue(storage, 1, (Term)tf.createPercent(0.0f));
                }
                else if (idval.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                {
                    setPositionValue(storage, 1, (Term)tf.createPercent(100.0f));
                }
                else if (idval.Equals("center", StringComparison.OrdinalIgnoreCase))
                {
                    setPositionValue(storage, -1, (Term)tf.createPercent(50.0f));
                }
            }
            else
            {
                setPositionValue(storage, -1, term);
            }
        }

        private void setPositionValue(Term[] s, int index, Term term)
        {
            switch (index)
            {
                case -1:
                    if (s[0] == null) //any position - use the free position
                    {
                        s[0] = term;
                    }
                    else
                    {
                        s[1] = term;
                    }
                    break;
                case 0:
                    if (s[0] != null) //if the position is occupied, move the old value
                    {
                        s[1] = s[0];
                    }
                    s[0] = term;
                    break;
                case 1:
                    if (s[1] != null)
                    {
                        s[0] = s[1];
                    }
                    s[1] = term;
                    break;
            }
        }

        private bool isExtentKeyword(Term term)
        {
            if (term is TermIdent)
            {
                switch (((TermIdent)term).Value)
                {
                    case "closest-corner":
                    case "closest-side":
                    case "farthest-corner":
                    case "farthest-side":
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

}