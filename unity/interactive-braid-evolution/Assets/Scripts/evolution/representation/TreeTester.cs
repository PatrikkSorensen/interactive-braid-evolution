using UnityEngine;
using System.Collections;

public class TreeTester : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            CreateTreeTests(); 
    }

    void CreateTreeTests()
    {
        //TreeTest1();
        TreeTest2();
        TreeTest3();
        TreeTest4();
    }

    void TreeTest1()
    {
        Debug.Log("Test 1: ");
        TreeNode root = new TreeNode("Root");
        TreeNodeList l = new TreeNodeList(root);

        TreeNode child1 = new TreeNode("child1");
        TreeNode child2 = new TreeNode("child2");
        TreeNode child3 = new TreeNode("child3");

        l.Add(child1);
        l.Add(child2);
        l.Add(child3);
    }

    void TreeTest2()
    {
        Debug.Log("Test 2: ");
        TreeNode root = new TreeNode("Root");

        TreeNode node1 = new TreeNode("child1", root);
        TreeNode node2 = new TreeNode("child2", node1);
        TreeNode node3 = new TreeNode("child3", node2);

        root.PrintTree();
    }

    void TreeTest3()
    {
        Debug.Log("Test 3: ");
        TreeNode root = new TreeNode("Root");
        TreeNodeList l = new TreeNodeList(root);

        TreeNode node1 = new TreeNode("n1", root);
        TreeNode node2 = new TreeNode("n2", root);
        TreeNode node3 = new TreeNode("n3", node1);
        TreeNode node4 = new TreeNode("n4", node3);

        root.PrintTree();

    }

    void TreeTest4()
    {
        Debug.Log("Test 4: ");
        TreeNode root = new TreeNode("Root");
        TreeNode node1 = new TreeNode("n1", root);

        // this splits so we create a list
        TreeNodeList l = new TreeNodeList(node1);
        l.Add(new TreeNode("n2"));
        l.Add(new TreeNode("n3"));

        // then we continue for each node on the new list
        int id = 4; 
        foreach (TreeNode n in l.ToArray())
        {
            string val = "n" + id; 
            TreeNode newNode = new TreeNode(val);
            newNode.Parent = n; 
            id++; 
        }

        // then we add a new child to the rightmost node
        TreeNode rightMostNode = l[l.Count - 1];
        rightMostNode.Children.Add("n" + id.ToString());
        id++; 

        // and then we finally add a list of three nodes to this new node
        TreeNode tempNode = rightMostNode.Children[0];
        tempNode.Children.Add("n" + id.ToString());
        id++;
        tempNode.Children.Add("n" + id.ToString());
        id++;
        tempNode.Children.Add("n" + id.ToString());

        root.PrintTree(); 
    }

}
