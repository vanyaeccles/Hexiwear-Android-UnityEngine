using UnityEngine;
using System.Collections.Generic;

public class SmartphoneCommand
{

	public enum Commands
	{
		Yes = 0,
		No,
		Cancel,
		ConfirmReception,
		Reset,
		SwitchCAD,
		CreatePrimitive,
		CreateCube,
		CreateSphere,
		CreateCylinder,
		CreatePlane,
		CreateTextBoard,
		CreateCustomMesh,
		ExtrudeCustomMesh,
		StartPrismBuild,
		EndPrismBuild,
		CancelSelection,
		Transform,
		Translate,
		Rotate,
		Scale,
		TranslateRotate,

		MoveScene,
		TranslateScene,		// uses coord
		RotateScene,		// uses floatValue
		ScaleScene,			// uses floatValue

		Fuse,
		Unfuse,
		SpecialTrans,
		Draw,
		Write,
		Color,
		Drawing,
		Writing,
		Coloring,
		Delete,
		ValidateDelete,
		SelectWandMode,
		SelectNormalMode,
		AutoFuse,
		SnapToSurfaces,
		SnapToAxis,
		TouchSelect,
		Count,
		None
	};
	public enum States
	{
		Touched = 0,	// Happens once when a new selection is made
		Pressed,		// Happens continuously when a selection is held
		Canceled,		// Happens once when a selection changes
		Released,		// Happens once when a selection is released
		Count,
		None
	};
	public enum Modes
	{
		NoSelection = 0,
		CreatePrimitive,
		DefineIP,
		ModelSelected,
		Transform,
		SpecialTrans,
		DirectTransform,
		SceneTrans,
		Draw,
		Write,
		Color,
		InTransform,
		InDirectTrans,
		EndTransform,
		ValidateDelete,
		ChangePhoneMode,
		DrawMesh,
		ExtrudeMesh,
		BuildPrism,
		Options,
		TouchSelect,
		Count,
		NoChange
	};

	public struct Line2D
	{
		public Vector2 start;
		public Vector2 end;
		public int color;
		public int width;
	}

	// Array index is the Mode
	// List index is the Command Index
	// First element of tuple is the command string
	// Second element of tuple is the command enum value
	// Third element of tuple is the mode change
	// Fourth element of tuple is the state for which we execute
	[System.Xml.Serialization.XmlIgnore]
	public List<Tuple<string, Commands, Modes, States>>[] ListOfCommands;

	public static int staticCommandId = 0;
	public int commandId;

	public Commands commandType;
	public States state;
	public Modes mode;
	public string text;
	public Vector3 coord;
	public int color;
	public List<Line2D> lines;
	public float floatValue;
	public int returnValue;
	public bool boolValue;

	public SmartphoneCommand()
	{
		staticCommandId++;
		commandId = staticCommandId;
		commandType = Commands.None;
		state = States.None;
		mode = Modes.NoSelection;
		text = "";
		coord = Vector3.zero;
		color = 1;
		lines = new List<Line2D>();
		floatValue = 0;
		returnValue = 0;
		boolValue = false;

		populateListOfCommands();
	}

	public SmartphoneCommand(SmartphoneCommand c)
	{
		commandId = c.commandId;
		commandType = c.commandType;
		state = c.state;
		mode = c.mode;
		text = c.text;
		coord = c.coord;
		color = c.color;
		lines = new List<Line2D>(c.lines);
		floatValue = c.floatValue;
		returnValue = c.returnValue;
		
		populateListOfCommands();
	}

	private void populateListOfCommands()
	{
		// ListOfCommands
		// ****************************************************************
		ListOfCommands = new List<Tuple<string, Commands, Modes, States>>[(int)Modes.Count];
		// NoSelection
		ListOfCommands[(int)Modes.NoSelection] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("DesktopCAD", 		Commands.SwitchCAD, 		Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Options", 			Commands.None, 				Modes.Options, 			States.Released),
			new Tuple<string, Commands, Modes, States>("Draw Mesh", 		Commands.None, 				Modes.DrawMesh, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Create Primitive >",Commands.CreatePrimitive, 	Modes.CreatePrimitive, 	States.Touched),
			new Tuple<string, Commands, Modes, States>("Move Scene", 		Commands.MoveScene, 		Modes.SceneTrans, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Direct Transform >",Commands.Transform, 		Modes.DirectTransform, 	States.Touched)
		};
		// TouchSelect
		ListOfCommands[(int)Modes.TouchSelect] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Touch Selection", 	Commands.TouchSelect, 		Modes.NoChange, 		States.Pressed)
		};
		// CreatePrimitive
		ListOfCommands[(int)Modes.CreatePrimitive] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cube", 				Commands.CreateCube, 		Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Sphere", 			Commands.CreateSphere, 		Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cylinder", 			Commands.CreateCylinder, 	Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Plane", 			Commands.CreatePlane, 		Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Text Board", 		Commands.CreateTextBoard, 	Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Build Prism", 		Commands.None, 				Modes.BuildPrism, 		States.Released)
		};
		// ModelSelected
		ListOfCommands[(int)Modes.ModelSelected] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("DesktopCAD", 		Commands.SwitchCAD, 		Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cancel Selection", 	Commands.CancelSelection, 	Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Transform >", 		Commands.Transform, 		Modes.Transform, 		States.Touched),
			new Tuple<string, Commands, Modes, States>("Special >", 		Commands.SpecialTrans, 		Modes.SpecialTrans, 	States.Touched),
			new Tuple<string, Commands, Modes, States>("Move Scene", 		Commands.MoveScene, 		Modes.SceneTrans, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Delete", 			Commands.Delete, 			Modes.ValidateDelete, 	States.Released)
		};
		// Transform
		ListOfCommands[(int)Modes.Transform] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Translate", 		Commands.Translate, 		Modes.InTransform, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Rotate",	 		Commands.Rotate, 			Modes.InTransform, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Scale", 			Commands.Scale, 			Modes.InTransform, 		States.Released),
			new Tuple<string, Commands, Modes, States>("6 DoF", 			Commands.TranslateRotate, 	Modes.InTransform, 		States.Released),
			new Tuple<string, Commands, Modes, States>("", 					Commands.None, 				Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// SpecialTrans
		ListOfCommands[(int)Modes.SpecialTrans] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released),
			new Tuple<string, Commands, Modes, States>("Draw", 				Commands.Draw, 				Modes.Draw, 			States.Released),
			new Tuple<string, Commands, Modes, States>("Write", 			Commands.Write, 			Modes.Write, 			States.Released),
			new Tuple<string, Commands, Modes, States>("Color", 			Commands.Color, 			Modes.Color, 			States.Released),
			new Tuple<string, Commands, Modes, States>("Separate", 			Commands.Unfuse, 			Modes.ModelSelected, 	States.Released),
			new Tuple<string, Commands, Modes, States>("", 					Commands.None, 				Modes.NoChange, 		States.Released)
		};
		// DirectTransform
		ListOfCommands[(int)Modes.DirectTransform] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("6 DoF", 			Commands.TranslateRotate, 	Modes.InDirectTrans, 	States.Released),
			new Tuple<string, Commands, Modes, States>("", 					Commands.None, 				Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Translate", 		Commands.Translate, 		Modes.InDirectTrans, 	States.Released),
			new Tuple<string, Commands, Modes, States>("Rotate",	 		Commands.Rotate, 			Modes.InDirectTrans, 	States.Released),
			new Tuple<string, Commands, Modes, States>("Scale", 			Commands.Scale, 			Modes.InDirectTrans, 	States.Released)
		};
		// SceneTrans
		ListOfCommands[(int)Modes.SceneTrans] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// ValidateDelete
		ListOfCommands[(int)Modes.ValidateDelete] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Confirm Delete", 	Commands.ValidateDelete, 	Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// Draw
		ListOfCommands[(int)Modes.Draw] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// Write
		ListOfCommands[(int)Modes.Write] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// Color
		ListOfCommands[(int)Modes.Color] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// InTransform
		ListOfCommands[(int)Modes.InTransform] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// InDirectTrans
		ListOfCommands[(int)Modes.InDirectTrans] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// EndTransform
		ListOfCommands[(int)Modes.EndTransform] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// DefineIP
		ListOfCommands[(int)Modes.DefineIP] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// ChangePhoneMode
		ListOfCommands[(int)Modes.ChangePhoneMode] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Normal Mode", 		Commands.SelectNormalMode, 	Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("", 					Commands.None, 				Modes.NoChange, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Wand Mode", 		Commands.SelectWandMode, 	Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.NoSelection, 		States.Released)
		};
		// DrawMesh
		ListOfCommands[(int)Modes.DrawMesh] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// ExtrudeMesh
		ListOfCommands[(int)Modes.ExtrudeMesh] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// BuildPrism
		ListOfCommands[(int)Modes.BuildPrism] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Cancel", 			Commands.Cancel, 			Modes.ModelSelected, 	States.Released)
		};
		// Options
		ListOfCommands[(int)Modes.Options] = new List<Tuple<string, Commands, Modes, States>>
		{
			new Tuple<string, Commands, Modes, States>("Change Mode", 		Commands.None, 				Modes.ChangePhoneMode, 	States.Released),
			new Tuple<string, Commands, Modes, States>("Choose server", 	Commands.None, 				Modes.DefineIP, 		States.Released),
			new Tuple<string, Commands, Modes, States>("Back", 				Commands.Cancel, 			Modes.NoSelection, 		States.Released),
			new Tuple<string, Commands, Modes, States>("", 					Commands.None, 				Modes.NoChange, 		States.Released),
		};
		// ****************************************************************
	}

	public override string ToString()
	{
		return "Command '" + commandType.ToString() + "' in mode '" + mode.ToString() + "' (State=" + state + ")";
	}

	public string logString
	{
		get
		{
			return "command='" + commandType.ToString() + "';mode='" + mode.ToString() + "';state='" + state + "'";
		}
	}

	public void nextCommand()
	{
		staticCommandId++;
		commandId = staticCommandId;
	}

	public List<Tuple<string, Commands, Modes, States>> commandsForMode(Modes mode)
	{
		return ListOfCommands[(int)mode];
	}

	public List<Tuple<string, Commands, Modes, States>> commandsForCurrentMode()
	{
		return ListOfCommands[(int)mode];
	}

	public Commands setCommandFromIndex(int index)
	{
		commandType = ListOfCommands[(int)mode][index].Second;
		return commandType;
	}

	public Modes getModeChange(int index)
	{
		//Debug.Log("Current mode = '" + mode.ToString() + "' | Nb of commands = " + 
		//          ListOfCommands[(int)mode].Count.ToString() + " | index = " + index.ToString());
		return ListOfCommands[(int)mode][index].Third;
	}

	public States getStateToChange(int index)
	{
		return ListOfCommands[(int)mode][index].Fourth;
	}

	public string getCommandString()
	{
		string ret = "";
		var commands = ListOfCommands[(int)mode];
		foreach (var t in commands)
		{
			if (t.Second == commandType)
			{
				ret = t.First;
				break;
			}
		}
		return ret;
	}

}














