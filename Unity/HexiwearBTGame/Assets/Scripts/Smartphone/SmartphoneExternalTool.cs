using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ServerClient;
using System.Threading;

public class SmartphoneExternalTool : MonoBehaviour
{

	public DataServer dataServer;

	public delegate void SmartphoneInput(SmartphoneCommand command);
	public delegate void SmartphoneCommandDelegate(SmartphoneCommand command);

	// onInput will be called whenever a Command is received
	public event SmartphoneInput onInput;

	public bool debug = false;

	private SmartphoneCommandDelegate[] onCommand;
	private bool firstClientSinceLaunch = true;
	private bool resetHasBeenConfirmed = false;

	void Awake()
	{
		onCommand = new SmartphoneCommandDelegate[(int)SmartphoneCommand.Commands.Count];
	}

	// Use this for initialization
	void Start()
	{
        //add the delegate to the data server
		dataServer.getDataDelegate += getData;
		dataServer.clientConnectedDelegate += OnClientConnected;
		registerOnCommand(SmartphoneCommand.Commands.Reset, OnResetConfirmed);
	}
	
	// Update is called once per frame
	void Update()
	{

	}

	void onDestroy()
	{
		// Just so the thread can exit
		resetHasBeenConfirmed = true;
	}



	// callback will be called only when a Command of type command is received
	public void registerOnCommand(SmartphoneCommand.Commands command, SmartphoneCommandDelegate callback)
	{
		onCommand[(int)command] += callback;
	}

	public void sendCommand(SmartphoneCommand command)
	{
        //dataServer.sendData(SerializerTool.SerializeObject<SmartphoneCommand>(command));
        //dataServer.sendData("Hello from Unity!");
	}

	public void confirmReception(SmartphoneCommand command, int returnValue = 0)
	{
		command.commandType = SmartphoneCommand.Commands.ConfirmReception;
		command.returnValue = returnValue;
		sendCommand(command);
	}

	// Useful for sending a command locally only
	public void sendCommandToCallback(SmartphoneCommand command)
	{
		int commandValue = (int)command.commandType;
		if (commandValue < (int)SmartphoneCommand.Commands.Count)
		{
			// Specific callback
			if (onCommand[commandValue] != null)
				onCommand[commandValue].Invoke(command);
		}
	}



	private void getData(byte[] data)
	{

        //Debug.Log("got data");

        //Debug.Log(data);
		//SmartphoneCommand command = SerializerTool.DeserializeObject<SmartphoneCommand>(data);

		//if (debug)
		//	Debug.Log(command.ToString());

		//// Generic callback
		//if (onInput != null)
		//	onInput.Invoke(command);

		//int commandValue = (int)command.commandType;
		//if (commandValue < (int)SmartphoneCommand.Commands.Count)
		//{
		//	// Specific callback
		//	if (onCommand[commandValue] != null)
		//		onCommand[commandValue].Invoke(command);
		//}
	}

	private void OnClientConnected()
	{
		if (firstClientSinceLaunch)
		{
			Thread aThread = new Thread(new ThreadStart(ResetClientLater));
			aThread.Name = "SmartphoneExternalTool: ResetClientLater";
			aThread.Start();
		}
	}

	private void OnResetConfirmed(SmartphoneCommand command)
	{
		resetHasBeenConfirmed = true;
	}

	private void ResetClientLater()
	{
		// Because of the BluetoothToTCP man-in-the-middle, we may be connected
		// to the middle-man, but not yet to the device.
		// Send data continuously until we get a confirm
		while (!resetHasBeenConfirmed)
		{
			Thread.Sleep(200);

			SmartphoneCommand command = new SmartphoneCommand();
			command.commandType = SmartphoneCommand.Commands.Reset;
			sendCommand(command);
			firstClientSinceLaunch = false;
		}
	}
}














