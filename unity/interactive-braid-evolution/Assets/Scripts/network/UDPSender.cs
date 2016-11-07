using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSender : MonoBehaviour
{

	public int port;
    public string IP;
    private IPEndPoint remoteEndPoint;
	private UdpClient client;

	void Start ()
	{
        remoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), port);
        client = new UdpClient();
	}

	public void SendString (string message)
	{
		try {
			// encode string to UTF8-coded bytes
			byte[] data = Encoding.UTF8.GetBytes (message);
			
			// send the data
			client.Send (data, data.Length, remoteEndPoint);

        } catch (Exception err) {
			print (err.ToString ());
		}
	}
}
