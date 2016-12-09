using UnityEngine;
using System.Collections;

public class BraidNodeData {


    Vector3 _vector;
    public Vector3 vector
    {
        get { return _vector; }
        set { _vector = value; }
    }

    float _radius;
    public float radius 
    {
        get { return _radius; }
        set { _radius = value; }
    }

    string _name;
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }

    int _id;
    public int id
    {
        get { return _id; }
        set { _id = value; }
    }

    public BraidNodeData (string name, Vector3 v, float radius = 0.0f, int id = 0)
    {
        this.id = id; 
        this.name = name + id.ToString();
        this.vector = v;
        this.radius = radius; 
    }

    public override string ToString()
    {
        return "[NAME: " + name + "]";
    }
}
