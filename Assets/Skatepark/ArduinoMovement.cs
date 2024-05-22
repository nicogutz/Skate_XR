using System;
using System.Collections;
using System.Collections.Concurrent; // For ConcurrentQueue
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class ArduinoMovement : MonoBehaviour
{
	#region private members
	CancellationTokenSource cts = new();
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	private TextMeshProUGUI textMesh;
	private ConcurrentQueue<string> rotationQueue = new ConcurrentQueue<string>(); // Thread-safe queue for storing rotations
	#endregion

	// Use this for initialization 	
	private void OnDestroy()
	{
		cts?.Cancel();
		cts?.Dispose();
	}

	void Start()
	{
		if (textMesh == null)
			textMesh = GetComponent<TextMeshProUGUI>();
		ConnectToTcpServer();
	}

	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(() => ListenForData(cts.Token));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	void Update()
	{
		// Check if there is a new rotation in the queue
		if (rotationQueue.TryDequeue(out string message))
		{
			textMesh.SetText(message);
		}
	}

	// ... rest of your code ...

	private void ListenForData(CancellationToken token)
	{
		try
		{
			socketConnection = new TcpClient("192.168.4.1", 80);
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						string serverMessage = Encoding.ASCII.GetString(incommingData);
						rotationQueue.Enqueue(serverMessage);

						if (token.IsCancellationRequested)
						{
							stream.Close();
							socketConnection.Close();
							return;
						}
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			textMesh.SetText("Socket exception: " + socketException);
		}
	}
}
