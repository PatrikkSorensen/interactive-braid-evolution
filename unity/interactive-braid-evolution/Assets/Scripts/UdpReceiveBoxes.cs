using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UdpReceiveBoxes : MonoBehaviour
{


	public int port = 8050;

    private Thread readThread;
    private UdpClient client;
	
	void Start ()
	{
		readThread = new Thread (new ThreadStart (ReceiveData));
		readThread.IsBackground = true;
		readThread.Start ();
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
                Debug.Log (">> " + text);
			} catch (Exception err) {
				print (err.ToString ());
			}
		}
	}

	//parse the string message from Grasshopper into coordiantes and dimensions for the 4 boxes
	private void parseData (string msg)
	{
        //TODO: Implement 
    }
}
