using UnityEngine;
using System.Collections;

public class BraidTreeUtility : MonoBehaviour {

    public static void AttachChildren(BraidNode parent, int amount, int id)
    {
        if (amount > 5)
            Debug.Log("Children breaks normalization function"); 
        for (int i = 0; i < amount; i++)
        {
            float radius = parent.parent.data.radius / 2.0f; 
            float yValue = parent.data.vector.y + 2.0f;
            BraidNode n = new BraidNode(new BraidNodeData("ann_node" + id.ToString(), new Vector3(0.0f, yValue, 0.0f), radius));
            id++;
            parent.children.Add(n);
            parent = n;
        }
    }

    public static BraidNode CreateInputTree(int size, float radius = 1.0f)
    {
        BraidNode root = new BraidNode(new BraidNodeData("root", Vector3.zero, radius + 1.0f));
        BraidNode temp = root;
        for (int i = 0; i < size; i++)
        {
            BraidNode n = new BraidNode(new BraidNodeData("n" + i.ToString(), new Vector3(0.0f, i * 2 + 2, 0.0f), radius));
            temp.children.Add(n);
            temp = n;
        }

        return root;
    }
}
