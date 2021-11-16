using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StyleParserCS.utils
{
    public interface TreeWalker
    {
        /**
         * The <code>root</code> node of the <code>TreeWalker</code>, as specified
         * when it was created.
         */
        public INode Root { get; }

        /**
         * This attribute determines which node types are presented via the
         * <code>TreeWalker</code>. The available set of constants is defined in
         * the <code>NodeFilter</code> interface.  Nodes not accepted by
         * <code>whatToShow</code> will be skipped, but their children may still
         * be considered. Note that this skip takes precedence over the filter,
         * if any.
         */
        public int WhatToShow { get; }

        /**
         * The filter used to screen nodes.
         */
        public FilterResult Filter { get; }

        /**
         * The value of this flag determines whether the children of entity
         * reference nodes are visible to the <code>TreeWalker</code>. If false,
         * these children  and their descendants will be rejected. Note that
         * this rejection takes precedence over <code>whatToShow</code> and the
         * filter, if any.
         * <br> To produce a view of the document that has entity references
         * expanded and does not expose the entity reference node itself, use
         * the <code>whatToShow</code> flags to hide the entity reference node
         * and set <code>expandEntityReferences</code> to true when creating the
         * <code>TreeWalker</code>. To produce a view of the document that has
         * entity reference nodes but no entity expansion, use the
         * <code>whatToShow</code> flags to show the entity reference node and
         * set <code>expandEntityReferences</code> to false.
         */
        public bool ExpandEntityReferences { get; }

        /**
         * The node at which the <code>TreeWalker</code> is currently positioned.
         * <br>Alterations to the DOM tree may cause the current node to no longer
         * be accepted by the <code>TreeWalker</code>'s associated filter.
         * <code>currentNode</code> may also be explicitly set to any node,
         * whether or not it is within the subtree specified by the
         * <code>root</code> node or would be accepted by the filter and
         * <code>whatToShow</code> flags. Further traversal occurs relative to
         * <code>currentNode</code> even if it is not part of the current view,
         * by applying the filters in the requested direction; if no traversal
         * is possible, <code>currentNode</code> is not changed.
         */
        public INode CurrentNode { get; set; }


        /**
         * Moves to and returns the closest visible ancestor node of the current
         * node. If the search for <code>parentNode</code> attempts to step
         * upward from the <code>TreeWalker</code>'s <code>root</code> node, or
         * if it fails to find a visible ancestor node, this method retains the
         * current position and returns <code>null</code>.
         * @return The new parent node, or <code>null</code> if the current node
         *   has no parent  in the <code>TreeWalker</code>'s logical view.
         */
        public INode parentNode();

        /**
         * Moves the <code>TreeWalker</code> to the first visible child of the
         * current node, and returns the new node. If the current node has no
         * visible children, returns <code>null</code>, and retains the current
         * node.
         * @return The new node, or <code>null</code> if the current node has no
         *   visible children  in the <code>TreeWalker</code>'s logical view.
         */
        public INode firstChild();

        /**
         * Moves the <code>TreeWalker</code> to the last visible child of the
         * current node, and returns the new node. If the current node has no
         * visible children, returns <code>null</code>, and retains the current
         * node.
         * @return The new node, or <code>null</code> if the current node has no
         *   children  in the <code>TreeWalker</code>'s logical view.
         */
        public INode lastChild();

        /**
         * Moves the <code>TreeWalker</code> to the previous sibling of the
         * current node, and returns the new node. If the current node has no
         * visible previous sibling, returns <code>null</code>, and retains the
         * current node.
         * @return The new node, or <code>null</code> if the current node has no
         *   previous sibling.  in the <code>TreeWalker</code>'s logical view.
         */
        public INode previousSibling();

        /**
         * Moves the <code>TreeWalker</code> to the next sibling of the current
         * node, and returns the new node. If the current node has no visible
         * next sibling, returns <code>null</code>, and retains the current node.
         * @return The new node, or <code>null</code> if the current node has no
         *   next sibling.  in the <code>TreeWalker</code>'s logical view.
         */
        public INode nextSibling();

        /**
         * Moves the <code>TreeWalker</code> to the previous visible node in
         * document order relative to the current node, and returns the new
         * node. If the current node has no previous node,  or if the search for
         * <code>previousNode</code> attempts to step upward from the
         * <code>TreeWalker</code>'s <code>root</code> node,  returns
         * <code>null</code>, and retains the current node.
         * @return The new node, or <code>null</code> if the current node has no
         *   previous node  in the <code>TreeWalker</code>'s logical view.
         */
        public INode previousNode();

        /**
         * Moves the <code>TreeWalker</code> to the next visible node in document
         * order relative to the current node, and returns the new node. If the
         * current node has no next node, or if the search for nextNode attempts
         * to step upward from the <code>TreeWalker</code>'s <code>root</code>
         * node, returns <code>null</code>, and retains the current node.
         * @return The new node, or <code>null</code> if the current node has no
         *   next node  in the <code>TreeWalker</code>'s logical view.
         */
        public INode nextNode();

    }
}
