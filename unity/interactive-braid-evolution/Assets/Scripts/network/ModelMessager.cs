using UnityEngine;
using System.Collections;

public class ModelMessager : MonoBehaviour {

    private UDPSender sender;
    private UDPMessage msg; 

    [Serializable]
    public class UDPMessage
    {
        public string name;
        public bool sucess; 
        public int generation;
    }

    void Start () {
        sender = Camera.main.GetComponent<UDPSender>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.X))
        {
            UDPMessage msg = new UDPMessage();
            msg.name = "SomeName";
            msg.sucess = true;
            msg.generation = 1;
            string s = JsonUtility.ToJson(msg);
            Debug.Log("ModelMsg: " + s);
            sender.SendString(s);
        }
    }
}
