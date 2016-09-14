using UnityEngine;
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

    private Thread readThread;
    private UdpClient client;
    private UINetworkWindow networkWindow;
    private UIMsgWindow msgWindow;

    private bool recievedMessage = false;
    private string newMessage = ""; 
    void Start ()
	{
		readThread = new Thread (new ThreadStart (ReceiveData));
		readThread.IsBackground = true;
		readThread.Start ();

        networkWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UINetworkWindow>();
        msgWindow = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIMsgWindow>();

        networkWindow.AddMessage("listening to any IP on this machine");
        msgWindow.AddMessage("Initialized network");
    }
	
    void Update()
    {
        if (recievedMessage)
        {
            recievedMessage = false;
            msgWindow.AddMessage("Recieved message: " + newMessage);

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
                Debug.Log (">> " + text);
                recievedMessage = true;
                newMessage = text; 
            } catch (Exception err) {
				print (err.ToString ());
			}
		}


    }
}
