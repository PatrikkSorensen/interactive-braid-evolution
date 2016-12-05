using UnityEngine;
using System.Collections;

public class Braid
{
    public string name;
    public Vector3[] vectors;
    public double[] mat_values;
    public double[] rad_values; 

    public Braid(string newName, Vector3[] newVectors, double[] newMatValues = null, double[] newRadValues = null)
    {
        this.name = newName;
        this.vectors = newVectors;
        this.mat_values = newMatValues;
        this.rad_values = newRadValues;  
    }
}

