using AngleSharp.Dom;
using StyleParserCS.utils;

namespace StyleParserCS.domassign
{
    /// <summary>
    /// This class implements traversal of DOM tree with simplified Visitor
    /// pattern.
    /// 
    /// @author kapy
    /// 
    /// </summary>
    public abstract class Traversal<T>
    {
        protected internal object source;
        protected internal TreeWalker walker;

        public Traversal(TreeWalker walker, object source)
        {
            this.source = source;
            this.walker = walker;
        }

        public Traversal(IDocument doc, object source, int whatToShow)
        {
            /*
            if (doc is DocumentTraversal)
            {
                DocumentTraversal dt = (DocumentTraversal)doc;
                this.walker = dt.createTreeWalker(doc.DocumentElement, whatToShow, null, false);
            }
            else
            */
            {
                this.walker = new GenericTreeWalker(doc.DocumentElement, whatToShow);
            }
            this.source = source;
        }

        public virtual void listTraversal(T result)
        {

            // tree traversal as nodes are found inside
            INode current, checkpoint = null;
            current = walker.nextNode();
            while (current != null)
            {
                // this method can change position in walker
                checkpoint = walker.CurrentNode;
                processNode(result, current, source);
                walker.CurrentNode = checkpoint;
                current = walker.nextNode();
            }
        }

        public virtual void levelTraversal(T result)
        {

            // this method can change position in walker
            //ORIGINAL LINE: final org.w3c.dom.Node checkpoint = walker.getCurrentNode();
            INode checkpoint = walker.CurrentNode;
            processNode(result, checkpoint, source);
            walker.CurrentNode = checkpoint;

            // traverse children:
            for (INode n = walker.firstChild(); n != null; n = walker.nextSibling())
            {
                levelTraversal(result);
            }

            // return position to the current (level up):
            walker.CurrentNode = checkpoint;
        }

        protected internal abstract void processNode(T result, INode current, object source);

        public virtual Traversal<T> reset(TreeWalker walker, object source)
        {
            this.walker = walker;
            this.source = source;
            return this;
        }

    }

}