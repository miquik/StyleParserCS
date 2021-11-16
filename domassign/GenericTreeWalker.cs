/*
 * The Apache Software License, Version 1.1
 *
 *
 * Copyright (c) 1999 The Apache Software Foundation.  All rights 
 * reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer. 
 *
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 *
 * 3. The end-user documentation included with the redistribution,
 *    if any, must include the following acknowledgment:  
 *       "This product includes software developed by the
 *        Apache Software Foundation (http://www.apache.org/)."
 *    Alternately, this acknowledgment may appear in the software itself,
 *    if and wherever such third-party acknowledgments normally appear.
 *
 * 4. The names "Xerces" and "Apache Software Foundation" must
 *    not be used to endorse or promote products derived from this
 *    software without prior written permission. For written 
 *    permission, please contact apache@apache.org.
 *
 * 5. Products derived from this software may not be called "Apache",
 *    nor may "Apache" appear in their name, without prior written
 *    permission of the Apache Software Foundation.
 *
 * THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED.  IN NO EVENT SHALL THE APACHE SOFTWARE FOUNDATION OR
 * ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF
 * USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
 * OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 * ====================================================================
 *
 * This software consists of voluntary contributions made by many
 * individuals on behalf of the Apache Software Foundation and was
 * originally based on software copyright (c) 1999, International
 * Business Machines, Inc., http://www.apache.org.  For more
 * information on the Apache Software Foundation, please see
 * <http://www.apache.org/>.
 * 
 * This class was used as inspiration
 * Modified by Karel Piwko, 2008
 * 
 */

using AngleSharp.Dom;
using StyleParserCS.utils;

namespace StyleParserCS.domassign
{
    /// <summary>
    /// A fallback implementation of the TreeWalker interface. It is used when the used DOM
    /// parser does not support the Traversal extension.
    /// </summary>
    public class GenericTreeWalker : TreeWalker
    {

        /// <summary>
        /// The whatToShow mask. </summary>
        internal int whatToShow;

        /// <summary>
        /// The current INode. </summary>
        internal INode currentNode;

        /// <summary>
        /// The root INode. </summary>
        internal INode root;

        public GenericTreeWalker(INode root, int whatToShow)
        {
            this.root = root;
            this.currentNode = root;
            this.whatToShow = whatToShow;
        }

        public virtual INode Root
        {
            get
            {
                return root;
            }
        }

        /// <summary>
        /// Return the whatToShow value </summary>
        public virtual int WhatToShow
        {
            get
            {
                return whatToShow;
            }
        }

        /// <summary>
        /// Return the FilterResult </summary>
        public virtual FilterResult Filter
        {
            get
            {
                return FilterResult.Accept;
            }
        }

        /// <summary>
        /// Return whether children entity references are included in the iterator. </summary>
        public virtual bool ExpandEntityReferences
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Return the current INode. </summary>
        public virtual INode CurrentNode
        {
            get
            {
                return currentNode;
            }
            set
            {
                currentNode = value;
            }
        }


        /// <summary>
        /// Return the parent INode from the current node, after applying filter,
        /// whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode parentNode()
        {

            if (currentNode == null)
            {
                return null;
            }

            INode node = getParentNode(currentNode);

            if (node != null)
            {
                currentNode = node;
            }

            return node;

        }

        /// <summary>
        /// Return the first child INode from the current node, after applying filter,
        /// whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode firstChild()
        {

            if (currentNode == null)
            {
                return null;
            }

            INode node = getFirstChild(currentNode);

            if (node != null)
            {
                currentNode = node;
            }

            return node;
        }

        /// <summary>
        /// Return the last child INode from the current node, after applying filter,
        /// whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode lastChild()
        {

            if (currentNode == null)
            {
                return null;
            }

            INode node = getLastChild(currentNode);

            if (node != null)
            {
                currentNode = node;
            }

            return node;
        }

        /// <summary>
        /// Return the previous sibling INode from the current node, after applying
        /// filter, whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode previousSibling()
        {

            if (currentNode == null)
            {
                return null;
            }

            INode node = getPreviousSibling(currentNode);
            if (node != null)
            {
                currentNode = node;
            }

            return node;
        }

        /// <summary>
        /// Return the next sibling INode from the current node, after applying
        /// filter, whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode nextSibling()
        {

            if (currentNode == null)
            {
                return null;
            }

            INode node = getNextSibling(currentNode);
            if (node != null)
            {
                currentNode = node;
            }

            return node;
        }

        /// <summary>
        /// Return the previous INode from the current node, after applying filter,
        /// whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode previousNode()
        {

            if (currentNode == null)
            {
                return null;
            }

            // get sibling
            INode result = getPreviousSibling(currentNode);
            if (result == null)
            {
                result = getParentNode(currentNode);
                if (result != null)
                {
                    currentNode = result;
                    return result;
                }
                return null;
            }

            // get the lastChild of result.
            INode lastChild = getLastChild(result);

            INode prev = lastChild;
            while (lastChild != null)
            {
                prev = lastChild;
                lastChild = getLastChild(prev);
            }

            lastChild = prev;

            // if there is a lastChild which passes filters return it.
            if (lastChild != null)
            {
                currentNode = lastChild;
                return lastChild;
            }

            // otherwise return the previous sibling.
            currentNode = result;
            return result;
        }

        /// <summary>
        /// Return the next INode from the current node, after applying filter,
        /// whatToshow. If result is not null, set the current INode.
        /// </summary>
        public virtual INode nextNode()
        {

            if (currentNode == null)
            {
                return null;
            }

            INode result = getFirstChild(currentNode);

            if (result != null)
            {
                currentNode = result;
                return result;
            }

            result = getNextSibling(currentNode);

            if (result != null)
            {
                currentNode = result;
                return result;
            }

            // return parent's 1st sibling.
            INode parent = getParentNode(currentNode);
            while (parent != null)
            {
                result = getNextSibling(parent);
                if (result != null)
                {
                    currentNode = result;
                    return result;
                }
                else
                {
                    parent = getParentNode(parent);
                }
            }

            // end , return null
            return null;
        }

        /// <summary>
        /// Internal function. Return the parent INode, from the input node after
        /// applying filter, whatToshow. The current node is not consulted or set.
        /// </summary>
        private INode getParentNode(INode node)
        {

            if (node == null || node == root)
            {
                return null;
            }

            INode newNode = node.Parent;
            if (newNode == null)
            {
                return null;
            }

            int accept = acceptNode(newNode);

            if (accept == (int)FilterResult.Accept)
            {
                return newNode;
            }
            else
            {
                // if (accept == FilterResult.SKIP_NODE)
                // and REJECT too.
                return getParentNode(newNode);
            }

        }

        /// <summary>
        /// Internal function. Return the nextSibling INode, from the input node after
        /// applying filter, whatToshow. The current node is not consulted or set.
        /// </summary>
        private INode getNextSibling(INode node)
        {

            if (node == null || node == root)
            {
                return null;
            }

            INode newNode = node.NextSibling;
            if (newNode == null)
            {
                newNode = node.Parent;
                if (newNode == null || node == root)
                {
                    return null;
                }
                int parentAccept = acceptNode(newNode);
                if (parentAccept == (int)FilterResult.Skip)
                {
                    return getNextSibling(newNode);
                }
                return null;
            }

            int accept = acceptNode(newNode);

            if (accept == (int)FilterResult.Accept)
            {
                return newNode;
            }
            else if (accept == (int)FilterResult.Skip)
            {
                INode fChild = getFirstChild(newNode);
                if (fChild == null)
                {
                    return getNextSibling(newNode);
                }

                return fChild;
            }
            else
            {
                // if (accept == FilterResult.REJECT_NODE)
                return getNextSibling(newNode);
            }

        }

        /// <summary>
        /// Internal function. Return the previous sibling INode, from the input node
        /// after applying filter, whatToshow. The current node is not consulted or
        /// set.
        /// </summary>
        private INode getPreviousSibling(INode node)
        {

            if (node == null || node == root)
            {
                return null;
            }

            INode newNode = node.PreviousSibling;
            if (newNode == null)
            {
                newNode = node.Parent;
                if (newNode == null || node == root)
                {
                    return null;
                }
                int parentAccept = acceptNode(newNode);
                if (parentAccept == (int)FilterResult.Skip)
                {
                    return getPreviousSibling(newNode);
                }

                return null;
            }

            int accept = acceptNode(newNode);

            if (accept == (int)FilterResult.Accept)
            {
                return newNode;
            }
            else if (accept == (int)FilterResult.Skip)
            {
                INode fChild = getLastChild(newNode);
                if (fChild == null)
                {
                    return getPreviousSibling(newNode);
                }

                return fChild;
            }
            else
            {
                // if (accept == FilterResult.REJECT_NODE)
                return getPreviousSibling(newNode);
            }

        }

        /// <summary>
        /// Internal function. Return the first child INode, from the input node after
        /// applying filter, whatToshow. The current node is not consulted or set.
        /// </summary>
        private INode getFirstChild(INode node)
        {

            if (node == null)
            {
                return null;
            }

            INode newNode = node.FirstChild;
            if (newNode == null)
            {
                return null;
            }

            int accept = acceptNode(newNode);
            if (accept == (int)FilterResult.Accept)
            {
                return newNode;
            }
            else if (accept == (int)FilterResult.Skip && newNode.HasChildNodes)
            {
                return getFirstChild(newNode);
            }

            // if (accept == FilterResult.REJECT_NODE)
            return getNextSibling(newNode);
        }

        /// <summary>
        /// Internal function. Return the last child INode, from the input node after
        /// applying filter, whatToshow. The current node is not consulted or set.
        /// </summary>
        private INode getLastChild(INode node)
        {

            if (node == null)
            {
                return null;
            }

            INode newNode = node.LastChild;
            if (newNode == null)
            {
                return null;
            }

            int accept = acceptNode(newNode);
            if (accept == (int)FilterResult.Accept)
            {
                return newNode;
            }
            else if (accept == (int)FilterResult.Skip && newNode.HasChildNodes)
            {
                return getLastChild(newNode);
            }

            // if (accept == FilterResult.REJECT_NODE)
            return getPreviousSibling(newNode);

        }

        /// <summary>
        /// Internal function. The node whatToShow and the filter are combined into
        /// one result.
        /// </summary>
        private short acceptNode(INode node)
        {
            /// <summary>
            ///*
            /// 7.1.2.4. Filters and whatToShow flags
            /// 
            /// Iterator and TreeWalker apply whatToShow flags before applying
            /// Filters. If a node is rejected by the active whatToShow flags, a
            /// Filter will not be called to evaluate that node. When a node is
            /// rejected by the active whatToShow flags, children of that node will
            /// still be considered, and Filters may be called to evaluate them.
            /// **
            /// </summary>

            if ((whatToShow & (1 << (int)node.NodeType - 1)) != 0)
            {
                return (int)FilterResult.Accept;
            }
            else
            {
                return (int)FilterResult.Skip;
            }
        }

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

            public Traversal(Document doc, object source, int whatToShow)
            {
                this.walker = new GenericTreeWalker(doc.DocumentElement, whatToShow);
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
                INode current, checkpoint = null;
                current = checkpoint = walker.CurrentNode;
                processNode(result, current, source);
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

}