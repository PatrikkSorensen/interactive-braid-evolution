using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BraidNodeList : List<BraidNode> {

    public BraidNode parent;

    public BraidNodeList(BraidNode parent)
    {
        this.parent = parent;
    }

    public new BraidNode Add(BraidNode node)
    {
        base.Add(node);
        node.parent = parent;
        return node;
    }

    public BraidNode Add(BraidNodeData data)
    {
        return Add(new BraidNode(data));
    }

    public override string ToString()
    {
        return "Count = " + Count.ToString() + ", root name: " + this.parent.root.data;
    }
}
