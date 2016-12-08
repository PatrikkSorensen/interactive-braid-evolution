using UnityEngine;
using System.Collections;

public class BraidTreeUtility : MonoBehaviour {

    public static void AttachChildren(BraidNode parent, int amount, int id)
    {
        BraidNode temp = parent;
        for (int i = 0; i < amount; i++)
        {
            BraidNode n = new BraidNode(new BraidNodeData("ann_node" + id.ToString(), parent.data.vector + new Vector3(0.0f, i * 2 + 2, 0.0f)));
            id++;
            parent.children.Add(n);
            parent = n;
        }
    }

    public static BraidNode CreateInputTree(int size)
    {
        BraidNode root = new BraidNode(new BraidNodeData("root", Vector3.zero, 1.5f));
        BraidNode temp = root;
        for (int i = 0; i < size; i++)
        {
            BraidNode n = new BraidNode(new BraidNodeData("n" + i.ToString(), new Vector3(0.0f, i * 2 + 2, 0.0f)));
            temp.children.Add(n);
            temp = n;
        }

        return root;
    }
}
