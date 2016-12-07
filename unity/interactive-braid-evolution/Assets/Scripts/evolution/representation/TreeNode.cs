using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TreeNode {

    public TreeNode(string Value)
    {
        this.Value = Value;
        Parent = null;
        Children = new TreeNodeList(this); 
    }

    public TreeNode(string Value, TreeNode Parent)
    {
        this.Value = Value;
        this.Parent = Parent;
        Children = new TreeNodeList(this); 
    }

    private TreeNode _Parent; 
    public TreeNode Parent
    {
        get { return _Parent; }
        set
        {
            if(value == _Parent)
            {
                return;
            }
            if(_Parent != null)
            {
                _Parent.Children.Remove(this); 
            }
            if(value != null && !value.Children.Contains(this))
            {
                value.Children.Add(this); 
            }
            _Parent = value; 
        }
    }

    public TreeNode Root
    {
        get
        {
            TreeNode node = this; 
            while(node.Parent != null)
            {
                node = node.Parent; 
            }
            return node; 
        }
    }

    public int Depth
    {
        get
        {
            int depth = 0;
            TreeNode node = this;
            while (node.Parent != null)
            {
                node = node.Parent;
                depth++;
            }
            return depth;
        }
    }

    private TreeNodeList _Children;
    public TreeNodeList Children
    {
        get { return _Children; }
        private set { _Children = value; }
    }

    private string _Value; 
    public string Value
    {
        get { return _Value; }
        set { _Value = value; } 
    }

    public override string ToString()
    {
        if(this.Parent != null)
            return "Node val = " + Value + ", parent = " + this.Parent.Value; 
        else 
            return "Node val = " + Value;
    }

    public void PrintTree()
    {
        Debug.Log("Depth: " + this.Depth);
         
        TreeNode root = this.Root; 
        List<TreeNode> firstStack = new List<TreeNode>();
        firstStack.Add(root);

        List<List<TreeNode>> childListStack = new List<List<TreeNode>>();
        childListStack.Add(firstStack);

        while (childListStack.Count > 0)
        {
            List<TreeNode> childStack = childListStack[childListStack.Count - 1];

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

                if (root.Children.Count > 0)
                {
                    childListStack.Add(new List<TreeNode>(root.Children));
                }
            }
        }
    }
}
