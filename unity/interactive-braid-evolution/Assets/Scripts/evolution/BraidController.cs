using UnityEngine;
using System.Collections;
using SharpNeat.Phenomes;
using System;
using ExperimentTypes;
using System.Collections.Generic;

public class BraidController : UnitController
{

    // Evolution specific variables 
    public int CURRENT_GENERATION; 
    public int TREE_SIZE;
    //public int NUM_INPUTS;
    //public int NUM_OUTPUTS;

    //double[] INPUT_ARRAY;
    //double[] OUTPUT_ARRAY;
    //double[] DELTA_ARRAY;
    //double[] VECTOR_ARRAY;
    //double[] RADIUS_ARRAY;

    // new approach
    List<Vector3> vects = new List<Vector3>();
    List<BraidNode> brancedNodes = new List<BraidNode>();
    List<Vector3[]> braidVectors = new List<Vector3[]>();

    int id;

    protected IBlackBox neat;
    protected float fitness = 0.0f;

    // Message variables 
    protected int braidId;
    ModelMessenger messenger;
    public Vector3[] BraidVectors; 

    // Braid specific variables
    public int BraidId {
        get { return braidId; }
        set { braidId = value; }
    }

    public override void Activate(IBlackBox box)
    {
        neat = box;
        messenger = GameObject.FindObjectOfType<ModelMessenger>();
        //SetupCPPNController();
        ActivateBraidController(); 
    }

    protected void ActivateBraidController()
    {
        Debug.Log("Creating braid with this controller"); 
        BraidNode tree = BraidTreeUtility.CreateInputTree(5);
        id = 0; 
        ActivateCPPNSplit(tree, -1.0f);

        tree.PrintTree();
        CreateBraidVectorsFromTree(tree, 0);

        JsonHelper.CreateJSONFromVectors(braidVectors);
        Braid b = new Braid("braid_" + braidId.ToString(), braidVectors);

        messenger.AddBraid(b, braidId);
        BraidSimulationManager.vectorArraysMade++; 
    }

    void ActivateCPPNSplit(BraidNode parentNode, float layer)
    {
        Vector4 res = CreateOutput(parentNode.data.vector.y, layer); 
        parentNode.data.vector += new Vector3(res.x, res.y, res.z); 

        // check if we should branch and add new node with attached children
        if (res.w < 0.8f && layer < 1.0f) {
            layer += 1.5f;
            Vector4 v = CreateOutput(parentNode.data.vector.y, layer); 
            BraidNodeData data = new BraidNodeData("ann_node_" + id.ToString(), parentNode.data.vector + new Vector3(v.x, v.y, v.z));
            BraidNode b = new BraidNode(data);
            id++;
            parentNode.children.Add(b);
            BraidTreeUtility.AttachChildren(b, 5 - b.Depth, id++); 
        }

        // recursion for the win 
        foreach (BraidNode subNode in parentNode.children)
            ActivateCPPNSplit(subNode, layer);
    }

    Vector4 CreateOutput(float input, float layer)
    {
        ISignalArray inputArr = neat.InputSignalArray;
        inputArr[0] = input;
        inputArr[1] = layer;
        neat.Activate();
        ISignalArray outputArr = neat.OutputSignalArray;
        float x = (float) outputArr[0];
        float y = (float) outputArr[1];
        float z = (float) outputArr[2];
        float branch = (float) outputArr[3];

        return new Vector4(x, y, z, branch); 
    }

    //void AttachChildren(BraidNode parent, int amount)
    //{
    //    BraidNode temp = parent;
    //    for (int i = 0; i < amount; i++)
    //    {
    //        BraidNode n = new BraidNode(new BraidNodeData("ann_node" + id.ToString(), parent.data.vector + new Vector3(0.0f, i * 2 + 2, 0.0f)));
    //        id++;
    //        parent.children.Add(n);
    //        parent = n;
    //    }
    //}

    //BraidNode CreateInputTree()
    //{
    //    BraidNode root = new BraidNode(new BraidNodeData("root", Vector3.zero, 1.5f));
    //    BraidNode temp = root;
    //    for (int i = 0; i < 5; i++)
    //    {
    //        BraidNode n = new BraidNode(new BraidNodeData("n" + id.ToString(), new Vector3(0.0f, i * 2 + 2, 0.0f)));
    //        id++;
    //        temp.children.Add(n);
    //        temp = n;
    //    }

    //    return root; 
    //}

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
        }
        else if (parentNode.children.Count == 0 && brancedNodes.Count == 0)
            braidVectors.Add(vects.ToArray());


        foreach (BraidNode subNode in parentNode.children)
            CreateBraidVectorsFromTree(subNode, layer + 1);
    }


    void ActivateCPPN()
    {
        //ISignalArray inputArr = neat.InputSignalArray;
        //int i, j;
        //for (i = j = 0; i < INPUT_ARRAY.Length; i += 3, j++)
        //{
        //    Vector3 v = new Vector3((float) INPUT_ARRAY[i], (float) INPUT_ARRAY[i + 1], (float) INPUT_ARRAY[i + 2]); 
        //    double distance = UtilityHelper.GetDistanceFromCenter(v, 10.0f, -10.0f);

        //    inputArr[0] = INPUT_ARRAY[i];      // x
        //    inputArr[1] = INPUT_ARRAY[i + 1];  // y
        //    inputArr[2] = INPUT_ARRAY[i + 2];  // z
        //    inputArr[3] = distance; 

        //    neat.Activate();
        //    ISignalArray outputArr = neat.OutputSignalArray;

        //    DELTA_ARRAY[i] += outputArr[0]; 
        //    DELTA_ARRAY[i + 1] += outputArr[1]; 
        //    DELTA_ARRAY[i + 2] += outputArr[2];
        //    RADIUS_ARRAY[j] = outputArr[4]; 
        //}

        //VECTOR_ARRAY = UtilityHelper.MergeArraysFromVectorANN(INPUT_ARRAY, DELTA_ARRAY);
    }

    //void SetupCPPNController()
    //{
    //    SetupANNStructure(5, 4, 5);

    //    //INPUT_ARRAY = UtilityHelper.CreateInputDoubleArray(VECTOR_ARRAY_SIZE, -10, 10); 
    //    INPUT_ARRAY = UtilityHelper.NormalizeInputVector3Array(INPUT_ARRAY, -10, VECTOR_ARRAY_SIZE * 2);
    //    RADIUS_ARRAY = new double[VECTOR_ARRAY_SIZE];
    //}

    //void SetupANNStructure(int vectorSize, int inputSize, int outputSize)
    //{
        
    //    VECTOR_ARRAY_SIZE = vectorSize;
    //    NUM_INPUTS = inputSize;
    //    NUM_OUTPUTS = outputSize;
    //    OUTPUT_ARRAY = new double[VECTOR_ARRAY_SIZE * NUM_OUTPUTS];
    //    DELTA_ARRAY = new double[VECTOR_ARRAY_SIZE * 3];
    //}

    public void SetFitness(float newFitness) { fitness = newFitness; }

    public override float GetFitness() { return fitness; }

    public override void Stop() { Debug.Log("Stop braidController called"); }
}
