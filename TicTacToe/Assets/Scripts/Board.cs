using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {
	public Texture2D linesTexture;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public static void Lines (Texture2D line){
		Rect line1 = new Rect(0,0,10,350);
		line1.center = new Vector2(Screen.width/3, Screen.width/2);
		Rect line2 = new Rect(0,0,10,350);
		line2.center = new Vector2((Screen.width/3)*2, Screen.width/2);
		Rect line3 = new Rect(0,0,350,10);
		line3.center = new Vector2(Screen.width/2, Screen.width/3);
		Rect line4 = new Rect(0,0,350,10);
		line4.center = new Vector2(Screen.width/2, (Screen.width/3)*2);
		GUI.DrawTexture (line1, line, ScaleMode.StretchToFill);
		GUI.DrawTexture (line2, line, ScaleMode.StretchToFill);
		GUI.DrawTexture (line3, line, ScaleMode.StretchToFill);
		GUI.DrawTexture (line4, line, ScaleMode.StretchToFill);
	}
}
