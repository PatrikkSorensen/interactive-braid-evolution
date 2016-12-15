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
    public float MAIN_RADIUS; 

    // Braid node tree specific variables
    List<Vector3> vects = new List<Vector3>();
    List<BraidNode> brancedNodes = new List<BraidNode>();
    List<Vector3[]> braidVectors = new List<Vector3[]>();
    List<float> radiusValues = new List<float>(); 

    int _nodeid;

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
        ActivateBraidController(); 
    }

    protected void ActivateBraidController()
    {
        Debug.Log("Creating braid with this controller");
        TREE_SIZE = 5;
        MAIN_RADIUS = 2.5f; 
        _nodeid = 0;

        BraidNode tree = BraidTreeUtility.CreateInputTree(TREE_SIZE, MAIN_RADIUS);
        ActivateCPPNSplit(tree, -1.0f);

        //tree.PrintTree();
        CreateBraidVectorsFromTree(tree, 0);

        Braid b = new Braid("braid_", braidId, braidVectors, null, radiusValues.ToArray());

        messenger.AddBraid(b, braidId);
        BraidSimulationManager.vectorArraysMade++; 
    }

    void ActivateCPPNSplit(BraidNode parentNode, float layer)
    {
        Vector4 res = CreateOutput(parentNode.data.vector.y, layer);
        res = NormalizeOutput(res); 

        // check if we should branch and add new node with attached children
        if (res.w > 0.5f && layer < 1.0f)
        {
            layer += 0.5f;
            BraidNode b = CreateNewNode(parentNode.data.vector.y, layer); 
            parentNode.children.Add(b);
            BraidTreeUtility.AttachChildren(b, TREE_SIZE - b.Depth, _nodeid++);
        }

        parentNode.data.vector += new Vector3(res.x, res.y, res.z);

        // recursion for the win 
        foreach (BraidNode subNode in parentNode.children)
            ActivateCPPNSplit(subNode, layer);
    }


    BraidNode CreateNewNode(float yValue, float layer)
    {
        Vector4 v = CreateOutput(yValue, layer);
        NormalizeOutput(v);
        Vector3 segment_vector = new Vector3(0.0f, yValue, 0.0f) + new Vector3(v.x, v.y, v.z); 
        BraidNodeData data = new BraidNodeData("ann_node_", new Vector3(0.0f, yValue, 0.0f) , 1.0f, _nodeid++);
        BraidNode b = new BraidNode(data);

        return b;
    }


    Vector4 CreateOutput(float input, float layer)
    {
        ISignalArray inputArr = neat.InputSignalArray;
        double yVal = NormalizeInput(input, 0.0f, 10.0f);

        if(layer > 1.0)
            layer = 1.0f;

        inputArr[0] = yVal; 
        inputArr[1] = layer;
        neat.Activate();

        ISignalArray outputArr = neat.OutputSignalArray;
        float x = (float)outputArr[0];
        float y = (float) outputArr[1];
        float z = (float) outputArr[2];
        float branch = (float) outputArr[3];

        return new Vector4(x, y, z, branch); 
    }

    Vector4 NormalizeOutput(Vector4 output)
    {
        Vector4 newVect = output; 
        newVect.x *= 10.0f;
        newVect.y *= 10.0f;
        newVect.z *= 10.0f;

        return newVect; 
    }

    float NormalizeInput (float value, float min, float max)
    {
        value = (value + Mathf.Abs(min)) / (max - min);
        value *= 2;
        value -= 1;
        return value;  
    }

    void CreateBraidVectorsFromTree(BraidNode parentNode, int layer)
    {
        // add reference branch point to list
        if (parentNode.children.Count > 1)
            brancedNodes.Add(parentNode);

        radiusValues.Add(parentNode.data.radius); 
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

    public void SetFitness(float newFitness) { fitness = newFitness; }

    public override float GetFitness() { return fitness; }

    public override void Stop() { Debug.Log("Stop braidController called"); }
}
