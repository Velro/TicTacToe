using UnityEngine;
using System.Collections;

public class ReturnToChoose : MonoBehaviour {
    bool windowOpen = false;
    Rect windowRect = new Rect(0, 0, 380, 180);
    public GUISkin mySkin;

	// Use this for initialization
	void Start () 
    {
        windowRect.center = new Vector2(Screen.width / 2, Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Back"))
        {
            windowOpen = true;
        }
	}

    void OnGUI()
    {
        GUI.skin = mySkin;
        if (windowOpen)
        {
            GUI.Window(0, windowRect, ConfirmWindow, "Confirm");
        }
    }

    Rect yesButton = new Rect(30,40,160,100);
    Rect noButton = new Rect(210,40,160,100);
    void ConfirmWindow(int windowID)
    {
        if (GUI.Button(yesButton, "Return"))
        {
            Application.LoadLevel("StartScene");
        }
        if (GUI.Button(noButton, "Cancel"))
        {
            windowOpen = false;
        }
    }
}