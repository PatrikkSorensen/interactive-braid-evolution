using UnityEngine;
using System.Collections;

public class Braid
{

    public string m_name;
    public Vector3[] m_vectors; 

    public Braid(string name, Vector3[] vectors)
    {
        this.m_name = name;
        this.m_vectors = vectors; 
    }
}

