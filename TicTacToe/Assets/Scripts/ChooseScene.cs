using UnityEngine;
using System.Collections;

public class ChooseScene : MonoBehaviour {
    Rect hostRect = new Rect(0,0,300,175);
    Rect findRect = new Rect(0,0,300,175);
    public GUISkin mySkin;

	// Use this for initialization
	void Start () 
    {
        
    #if ANDROID
        Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
		Screen.orientation = ScreenOrientation.AutoRotation;
    #endif
        hostRect.center = new Vector2(Screen.width / 2, Screen.height / 3);
        findRect.center = new Vector2(Screen.width / 2, (Screen.height / 3) * 2);

    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    
    
	void OnGUI ()
    {
        GUI.skin = mySkin;
		if (GUI.Button(hostRect, "Host A Match"))
        {
			Application.LoadLevel ("ServerScene");
		}
        if (GUI.Button(findRect, "Find A Match"))
        {
			Application.LoadLevel ("ClientScene");
		}
	}
}
