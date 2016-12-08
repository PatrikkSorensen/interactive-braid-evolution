using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BraidTreeTester : MonoBehaviour {

    UDPSender sender;
    List<Vector3> vects = new List<Vector3>();
    List<BraidNode> brancedNodes = new List<BraidNode>();
    List<Vector3[]> braidVectors = new List<Vector3[]>(); 
    

    private void Start()
    {
        sender = FindObjectOfType<UDPSender>(); 
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            CreateTreeTests();
    }

    void CreateTreeTests()
    {
        //Test1();
        //Test2();
        //Test3();
        Test4();
    }



    void Test1()
    {
        BraidNodeData data = new BraidNodeData("root", Vector3.zero, 1.5f); 
        BraidNode root = new BraidNode(data);

        data = new BraidNodeData("n1", Vector3.left, 1.0f); 
        BraidNode child1 = new BraidNode(new BraidNodeData("n1", Vector3.right, 1.0f));
        BraidNode child2 = new BraidNode(new BraidNodeData("n2", Vector3.right, 1.0f));
        BraidNode child3 = new BraidNode(new BraidNodeData("n3", Vector3.right, 1.0f));

        root.children.Add(child1);
        root.children.Add(child2);
        root.children.Add(child3);

        root.PrintTree();

        JsonHelper.CreateJSONFromDataTree(root);
    }

    void Test2 ()
    {
        BraidNode root = new BraidNode(new BraidNodeData("root", Vector3.zero, 1.5f));

        BraidNode temp = root; 
        for(int i = 0; i < 5; i++)
        {
            BraidNode n = new BraidNode(new BraidNodeData("n" + i.ToString(), new Vector3(0.0f, i * 2 + 2, 0.0f)));
            temp.children.Add(n);
            temp = n;  
        }

        root.PrintTree();

        string message = JsonHelper.CreateJSONFromDataTree(root);
        sender.SendString(message); 
    }

    void Test3()
    {

        Debug.Log("Test 3");
        BraidNode root = new BraidNode(new BraidNodeData("root", Vector3.zero, 1.5f));

        BraidNode n1 = new BraidNode(new BraidNodeData("n1", new Vector3(0.0f, 2.0f, 0.0f), 1.0f));
        BraidNode n2 = new BraidNode(new BraidNodeData("n2", new Vector3(-2.0f, 2.0f, 0.0f), 1.0f));
        BraidNode n3 = new BraidNode(new BraidNodeData("n3", new Vector3(2.0f, 2.0f, 0.0f), 1.0f));
        BraidNode n4 = new BraidNode(new BraidNodeData("n4", new Vector3(-2.0f, 4.0f, 0.0f), 1.0f));
        BraidNode n5 = new BraidNode(new BraidNodeData("n5", new Vector3(2.0f, 4.0f, 0.0f), 1.0f));

        root.children.Add(n1);
        n1.children.Add(n2);
        n1.children.Add(n3);
        n2.children.Add(n4);
        n3.children.Add(n5);

        List<BraidNode> firstStack = new List<BraidNode>();
        firstStack.Add(root);

        CreateBraidVectorsFromTree(root, 0);

        string message = JsonHelper.CreateJSONFromVectors(braidVectors);
        Debug.Log(message); 
        sender.SendString(message); 
    }

    private void Test4()
    {
        Debug.Log("Performing test 4");
        BraidNode root = new BraidNode(new BraidNodeData("root", Vector3.zero, 1.5f));

        // left branch
        BraidNode n1 = new BraidNode(new BraidNodeData("n1", new Vector3(0.0f, 2.0f, 0.0f), 1.0f));
        BraidNode n2 = new BraidNode(new BraidNodeData("n2", new Vector3(-2.0f, 2.0f, 0.0f), 1.0f));
        BraidNode n4 = new BraidNode(new BraidNodeData("n4", new Vector3(-2.0f, 4.0f, 0.0f), 1.0f));
        BraidNode n6 = new BraidNode(new BraidNodeData("n6", new Vector3(-2.0f, 6.0f, 0.0f), 1.0f));
        BraidNode n9 = new BraidNode(new BraidNodeData("n9", new Vector3(0.0f, 8.0f, 0.0f), 1.0f));

        // right branch
        BraidNode n3 = new BraidNode(new BraidNodeData("n3", new Vector3(2.0f, 2.0f, 0.0f), 1.0f));
        BraidNode n5 = new BraidNode(new BraidNodeData("n5", new Vector3(2.0f, 4.0f, 0.0f), 1.0f));
        BraidNode n7 = new BraidNode(new BraidNodeData("n7", new Vector3(1.0f, 6.0f, 0.0f), 1.0f));
        BraidNode n8 = new BraidNode(new BraidNodeData("n8", new Vector3(4.0f, 8.0f, 0.0f), 1.0f));
        BraidNode n10 = new BraidNode(new BraidNodeData("n10", new Vector3(6.0f, 10.0f, 0.0f), 1.0f));

        root.children.Add(n1);
        n1.children.Add(n2);
        n1.children.Add(n3);

        // left branch
        n2.children.Add(n4);
        n4.children.Add(n6);
        n6.children.Add(n9);

        // right branch
        n3.children.Add(n5);
        n5.children.Add(n7);
        n5.children.Add(n8);
        n8.children.Add(n10); 

        root.PrintTree(); 

        List<BraidNode> firstStack = new List<BraidNode>();
        firstStack.Add(root);

        CreateBraidVectorsFromTree(root, 0); 

        string message = JsonHelper.CreateJSONFromVectors(braidVectors);
        Debug.Log(message);
        sender.SendString(message);


    }

    void CreateBraidVectorsFromTree(BraidNode parentNode, int layer)
    {
        // add reference branch point to list
        if (parentNode.children.Count > 1)
            brancedNodes.Add(parentNode);

        vects.Add(parentNode.data.vector);

        // add reference node vect value to list
        if (parentNode.children.Count == 0 && brancedNodes.Count != 0)
        {
            // get back to last reference point
            int i = brancedNodes.Count - 1;
            braidVectors.Add(vects.ToArray());
            vects.Clear();

            vects.Add(brancedNodes[i].data.vector);
            brancedNodes.RemoveAt(i);
        } else if (parentNode.children.Count == 0 && brancedNodes.Count == 0)
            braidVectors.Add(vects.ToArray()); 


        foreach (BraidNode subNode in parentNode.children)
            CreateBraidVectorsFromTree(subNode, layer + 1);
    }

}
