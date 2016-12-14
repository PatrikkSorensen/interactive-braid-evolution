using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Braid
{
    public string name;
    public Vector3[] vectors;
    public List<Vector3[]> vectors2; 
    public float[] mat_values;
    public float[] rad_values;
    public int id; 

    public Braid(string newName, int id, Vector3[] newVectors, float[] newMatValues = null, float[] newRadValues = null)
    {
        this.name = newName;
        this.id = id; 
        this.vectors = newVectors;
        this.mat_values = newMatValues;
        this.rad_values = newRadValues;  
    }

    public Braid(string newName, int id, List<Vector3[]> newVectors, float[] newMatValues = null, float[] newRadValues = null)
    {
        this.name = newName;
        this.id = id;
        this.vectors2 = newVectors;
        this.mat_values = newMatValues;
        this.rad_values = newRadValues;
    }
}

