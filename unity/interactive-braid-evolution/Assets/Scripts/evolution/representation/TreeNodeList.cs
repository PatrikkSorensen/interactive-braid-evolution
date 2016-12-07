using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeNodeList : List<TreeNode> {

    public TreeNode Parent; 

    public TreeNodeList (TreeNode Parent)
    {
        this.Parent = Parent; 
    }

    public new TreeNode Add(TreeNode Node)
    {
        base.Add(Node);
        Node.Parent = Parent;
        return Node; 
    }

    public TreeNode Add(string Value)
    {
        return Add(new TreeNode(Value)); 
    }

    public override string ToString()
    {
        return "Count = " + Count.ToString() + ", root name: " + this.Parent.Root.Value; 
    }

}
