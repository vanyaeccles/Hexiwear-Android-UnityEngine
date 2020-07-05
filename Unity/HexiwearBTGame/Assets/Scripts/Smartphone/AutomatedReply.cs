using UnityEngine;
using System.Collections;

public class AutomatedReply : MonoBehaviour
{

	public SmartphoneExternalTool tool;

	// Use this for initialization
	void Start()
	{
		// Useful to trigger an automatic response to some commands
		tool.registerOnCommand(SmartphoneCommand.Commands.SnapToSurfaces, genericReply);
		tool.registerOnCommand(SmartphoneCommand.Commands.SnapToAxis, genericReply);
		tool.registerOnCommand(SmartphoneCommand.Commands.AutoFuse, genericReply);
		tool.registerOnCommand(SmartphoneCommand.Commands.StartPrismBuild, genericReply);
		tool.registerOnCommand(SmartphoneCommand.Commands.EndPrismBuild, genericReply);
		tool.registerOnCommand(SmartphoneCommand.Commands.ExtrudeCustomMesh, genericReplyOnRelease);
		tool.registerOnCommand(SmartphoneCommand.Commands.Translate, onTransform);
		tool.registerOnCommand(SmartphoneCommand.Commands.Rotate, onTransform);
		tool.registerOnCommand(SmartphoneCommand.Commands.Scale, onTransform);
		tool.registerOnCommand(SmartphoneCommand.Commands.TranslateRotate, onTransform);
		tool.registerOnCommand(SmartphoneCommand.Commands.MoveScene, onTransform);
		tool.registerOnCommand(SmartphoneCommand.Commands.CreateCustomMesh, onDrawMesh);
		//tool.registerOnCommand(SmartphoneCommand.Commands.Fuse, onTransform);
		//tool.registerOnCommand(SmartphoneCommand.Commands.TouchSelect, replyWithSameCommand);
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}



	// public



	private void genericReply(SmartphoneCommand command)
	{
		tool.confirmReception(command);
	}

	private void genericReplyOnRelease(SmartphoneCommand command)
	{
		if (command.state == SmartphoneCommand.States.Released)
		{
			tool.confirmReception(command);
		}
	}

	private void replyWithSameCommand(SmartphoneCommand command)
	{
		tool.sendCommand(command);
	}

	private void onTransform(SmartphoneCommand command)
	{
		if (command.mode == SmartphoneCommand.Modes.EndTransform && command.state == SmartphoneCommand.States.Released)
		{
			tool.confirmReception(command);
		}
		else if (command.mode == SmartphoneCommand.Modes.InDirectTrans && command.state == SmartphoneCommand.States.Released)
		{
			tool.confirmReception(command);
		}
	}

	private void onDrawMesh(SmartphoneCommand command)
	{
		if (command.mode == SmartphoneCommand.Modes.DrawMesh && command.state == SmartphoneCommand.States.Released)
		{
			SmartphoneCommand c = new SmartphoneCommand();
			c.mode = SmartphoneCommand.Modes.ExtrudeMesh;
			tool.sendCommand(c);
		}
	}
}















