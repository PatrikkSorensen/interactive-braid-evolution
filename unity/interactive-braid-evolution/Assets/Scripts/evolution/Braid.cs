using UnityEngine;
using System.Collections;

public class Braid
{
    public string name;
    public Vector3[] vectors;
    public double[] m_matValues;
    public double[] m_radValues; 

    public Braid(string newName, Vector3[] newVectors, double[] matValues = null, double[] radValues = null)
    {
        this.name = newName;
        this.vectors = newVectors;
        this.m_matValues = matValues;
        this.m_radValues = radValues;  
    }
}

