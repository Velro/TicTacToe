using UnityEngine;
using System.Collections;

public class Space {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		public Rect rect = new Rect(0,0,Screen.width/4,Screen.width/4);
		//bool on = false;
		bool circleOn = false;
		bool xOn = false;
		
		Texture2D circTex;
		Texture2D xTex;
		
		public void Init (Texture2D circleTex, Texture2D xTexture, float x, float y){
			rect.x = x;
			rect.y = y;
			circTex = circleTex;
			xTex = xTexture;
		}
		
		public void Draw (){
			
			if (circleOn){
				GUI.DrawTexture(rect, circTex, ScaleMode.StretchToFill);
			}
			if (xOn){
				GUI.DrawTexture(rect, xTex, ScaleMode.StretchToFill);
			}
		}
		
		public void SetCircleOn (){
			circleOn = true;
		}
		
		public bool GetCircleOn (){
			return circleOn;
		}
		
		public void SetXOn (){
			xOn = true;
		}
		
		public bool GetXOn (){
			return xOn;
		}
		
		public void Reset (){
			circleOn = false;
			xOn = false;
		}
}
