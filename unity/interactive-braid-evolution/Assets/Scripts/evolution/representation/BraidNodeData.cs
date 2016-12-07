using UnityEngine;
using System.Collections;

public class BraidNodeData {

    string _name;
    Vector3 vector;
    float radius; 

    public string name
    {
        get { return _name; }
        set { _name = value; }
    }

    public BraidNodeData (string name, Vector3 v, float radius = 0.0f)
    {
        this.name = name; 
    }

    public override string ToString()
    {
        return "[NAME: " + name + " RADIUS: " + radius + " V: " + vector + "]";
    }
}
