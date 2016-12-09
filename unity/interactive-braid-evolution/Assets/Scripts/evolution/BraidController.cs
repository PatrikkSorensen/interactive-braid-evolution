﻿using UnityEngine;
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

    // Braid node tree specific variables
    List<Vector3> vects = new List<Vector3>();
    List<BraidNode> brancedNodes = new List<BraidNode>();
    List<Vector3[]> braidVectors = new List<Vector3[]>();

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
        BraidNode tree = BraidTreeUtility.CreateInputTree(5);
        _nodeid = 0; 
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
        res = NormalizeOutput(res); 

        parentNode.data.vector += new Vector3(res.x, res.y, res.z); 

        // check if we should branch and add new node with attached children
        if (res.w > 0.5f && layer < 1.0f) {
            layer += 0.5f;
            Vector4 v = CreateOutput(parentNode.data.vector.y, layer);
            NormalizeOutput(v);
            float radius = parentNode.data.radius / 2; 
            BraidNodeData data = new BraidNodeData("ann_node_", parentNode.data.vector + new Vector3(v.x, v.y, v.z), radius, _nodeid++);
            BraidNode b = new BraidNode(data);
            parentNode.children.Add(b);
            BraidTreeUtility.AttachChildren(b, 5 - b.Depth, _nodeid++); 
        }

        // recursion for the win 
        foreach (BraidNode subNode in parentNode.children)
            ActivateCPPNSplit(subNode, layer);
    }



    Vector4 CreateOutput(float input, float layer)
    {
        ISignalArray inputArr = neat.InputSignalArray;
        inputArr[0] = NormalizeInput(input, 0.0f, 10.0f);
        inputArr[1] = layer;
        neat.Activate();

        ISignalArray outputArr = neat.OutputSignalArray;
        float x = (float) outputArr[0];
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
