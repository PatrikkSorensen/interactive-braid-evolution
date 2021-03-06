﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReciever : MonoBehaviour
{
	public int port = 8050;
    public bool hasImportedAllModels; 

    private Thread readThread;
    private UdpClient client;
    private ObjImporter objImporter; 
    private int num_models_imported;

    [Serializable]
    public class UDPRecievedMessage
    {
        public int should_import;
        public int num_models;
        public int models_created;
        public int id; 
    }

    void Start ()
	{
		readThread = new Thread (new ThreadStart (ReceiveData));
		readThread.IsBackground = true;
		readThread.Start ();

        objImporter = GameObject.FindObjectOfType<ObjImporter>();
        num_models_imported = 0; 
    }

    public void ResetVariables()
    {
        num_models_imported = 0; 
    }

    private void DecodeJSON(string jsonString)
    {
        UDPRecievedMessage msg = new UDPRecievedMessage();
        msg = JsonUtility.FromJson<UDPRecievedMessage>(jsonString);

        if (msg.models_created > num_models_imported)
        {
            objImporter.StartImportSingleModel(num_models_imported);
            num_models_imported++;
        } 

        if (msg.models_created == Optimizer.PopulationSize)
            hasImportedAllModels = true; 
    }

    private void DecodeJSONSingleModel(string jsonString)
    {
        UDPRecievedMessage msg = new UDPRecievedMessage();
        msg = JsonUtility.FromJson<UDPRecievedMessage>(jsonString);

        if (msg.should_import == 1)
        {
            objImporter.StartImportSingleModel("braid_" + msg.id.ToString()); 
            //Debug.Log("Should import");
        } 
    }

    // Unity Application Quit Function
    void OnApplicationQuit ()
	{
		stopThread ();
	}
	
	// Stop reading UDP messages
	private void stopThread ()
	{
		if (readThread.IsAlive) {
			readThread.Abort ();
		}
		client.Close ();
	}
	
	// receive thread function
	private void ReceiveData ()
	{
        client = new UdpClient (port);
		while (true) {
			try {
				IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
				byte[] data = client.Receive (ref anyIP);
			
				// decode UTF8-coded bytes to text format
				string text = Encoding.UTF8.GetString (data);
                //Debug.Log (">> " + text);

                // hot model import object
                //DecodeJSON(text);
                DecodeJSONSingleModel(text);


            } catch (Exception err) {
				print (err.ToString ());
			}
		}
    }
}
