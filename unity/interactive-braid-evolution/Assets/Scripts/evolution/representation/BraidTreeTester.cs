using UnityEngine;
using System.Collections;

public class BraidTreeTester : MonoBehaviour {
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            CreateTreeTests();
    }

    void CreateTreeTests()
    {
        Test1();
    }

    void Test1()
    {
        Debug.Log("Test 1: ");
        BraidNodeData data = new BraidNodeData("root", Vector3.zero, 1.5f); 
        BraidNode root = new BraidNode(data);

        data = new BraidNodeData("n1", Vector3.left, 1.0f); 
        BraidNode child1 = new BraidNode(new BraidNodeData("n1", Vector3.right, 1.0f));
        BraidNode child2 = new BraidNode(new BraidNodeData("n2", Vector3.right, 1.0f));
        BraidNode child3 = new BraidNode(new BraidNodeData("n3", Vector3.right, 1.0f));

        root.children.Add(child1);
        root.children.Add(child1);
        root.children.Add(child1);

        root.PrintTree(); 
    }

}
