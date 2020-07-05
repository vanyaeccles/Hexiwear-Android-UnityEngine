using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//simple plugin for android, adapted from https://www.youtube.com/watch?v=0ahGeTNUPLM

public class PluginWrapper : MonoBehaviour {

    public int data = 7;

	// Use this for initialization
	void Start () {
        TextMesh textMesh = GetComponent<TextMesh>();
        var plugin = new AndroidJavaClass("com.fitreo.unityplugin.PluginClass");
        textMesh.text = plugin.CallStatic<string>("GetTextFromPlugin", data);
	}
	

}
