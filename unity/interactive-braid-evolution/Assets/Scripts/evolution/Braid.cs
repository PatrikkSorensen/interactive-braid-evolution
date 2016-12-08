using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Braid
{
    public string name;
    public Vector3[] vectors;
    public List<Vector3[]> vectors2; 
    public double[] mat_values;
    public double[] rad_values; 

    public Braid(string newName, Vector3[] newVectors, double[] newMatValues = null, double[] newRadValues = null)
    {
        this.name = newName;
        this.vectors = newVectors;
        this.mat_values = newMatValues;
        this.rad_values = newRadValues;  
    }

    public Braid(string newName, List<Vector3[]> newVectors, double[] newMatValues = null, double[] newRadValues = null)
    {
        this.name = newName;
        this.vectors2 = newVectors;
        this.mat_values = newMatValues;
        this.rad_values = newRadValues;
    }
}

