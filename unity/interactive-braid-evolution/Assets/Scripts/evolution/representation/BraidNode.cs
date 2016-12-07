using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BraidNode
{
    public BraidNode(BraidNodeData data)
    {
        this._data = data;
        parent = null;
        children = new BraidNodeList(this);
    }

    public BraidNode(BraidNodeData data, BraidNode parent)
    {
        this.data = data;
        this.parent = parent;
        children = new BraidNodeList(this);
    }

    private BraidNode _parent;
    public BraidNode parent
    {
        get { return _parent; }
        set
        {
            if (value == _parent)
            {
                return;
            }
            if (_parent != null)
            {
                _parent.children.Remove(this);
            }
            if (value != null && !value.children.Contains(this))
            {
                value.children.Add(this);
            }
            _parent = value;
        }
    }

    public BraidNode root
    {
        get
        {
            BraidNode node = this;
            while (node.parent != null)
            {
                node = node.parent;
            }
            return node;
        }
    }

    public int Depth
    {
        get
        {
            int depth = 0;
            BraidNode node = this;
            while (node.parent != null)
            {
                node = node.parent;
                depth++;
            }
            return depth;
        }
    }

    private BraidNodeList _children;
    public BraidNodeList children
    {
        get { return _children; }
        private set { _children = value; }
    }

    private BraidNodeData _data;
    public BraidNodeData data
    {
        get { return _data; }
        set { _data = value; }
    }

    public override string ToString()
    {
        if (this.parent != null)
            return "Node data: " + data.ToString() + ", parent = " + this.parent.data.name;
        else
            return "Root, Node val = " + data;
    }

    public void PrintTree()
    {
        Debug.Log("Depth: " + this.Depth);

        BraidNode root = this.root;
        List<BraidNode> firstStack = new List<BraidNode>();
        firstStack.Add(root);

        List<List<BraidNode>> childListStack = new List<List<BraidNode>>();
        childListStack.Add(firstStack);

        while (childListStack.Count > 0)
        {
            List<BraidNode> childStack = childListStack[childListStack.Count - 1];

            if (childStack.Count == 0)
            {
                childListStack.RemoveAt(childListStack.Count - 1);
            }
            else
            {
                root = childStack[0];
                childStack.RemoveAt(0);

                string indent = "";
                for (int i = 0; i < childListStack.Count - 1; i++)
                {
                    indent += (childListStack[i].Count > 0) ? "|  " : "   ";
                }

                Debug.Log(indent + "+- " + root.ToString());

                if (root.children.Count > 0)
                {
                    childListStack.Add(new List<BraidNode>(root.children));
                }
            }
        }
    }
}
