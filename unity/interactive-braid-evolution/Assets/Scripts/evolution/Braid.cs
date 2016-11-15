using UnityEngine;
using System.Collections;

public class Braid
{
    public string name;
    public Vector3[] vectors; 

    public Braid(string newName, Vector3[] newVectors)
    {
        this.name = newName;
        this.vectors = newVectors; 
    }
}

