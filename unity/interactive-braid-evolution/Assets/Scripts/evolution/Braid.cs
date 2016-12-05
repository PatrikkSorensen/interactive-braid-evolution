using UnityEngine;
using System.Collections;

public class Braid
{
    public string name;
    public Vector3[] vectors;
    public double[] matValues;
    public double[] radValues; 

    public Braid(string newName, Vector3[] newVectors, double[] newMatValues = null, double[] newRadValues = null)
    {
        this.name = newName;
        this.vectors = newVectors;
        this.matValues = newMatValues;
        this.radValues = newRadValues;  
    }
}

