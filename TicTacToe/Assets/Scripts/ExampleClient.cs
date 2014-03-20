using UnityEngine;
using System.Collections;

public class ExampleClient : MonoBehaviour {

	public Texture2D circleTexture;
	public Texture2D xTexture;
	public Texture2D linesTexture;

	public Space[] board = new Space[9];

	public bool myTurn = false;
	public int playerOneChoice = -1;

	public bool iWin;
	public bool iLose;
	public bool draw;

	public int myScore;
	public int theirScore;

    public GUISkin mySkin;

	void Awake ()
    {
		MasterServer.RequestHostList("LOL");
	}

	void Start ()
    {
		int a = 0;
		for (int k = 0; k < 3; k++)
        {
			for (int i = 0; i < 3; i++)
            {
				board[a] = new Space();
				board[a].Init(circleTexture, xTexture, i*((Screen.width/3)+20), k*((Screen.width/3)+20));
				a++;
			}
		}
	}

	void OnGUI ()
    {
        GUI.skin = mySkin;
        GUI.Label(new Rect(10, Screen.height - 160, 600, 50), str);
		HostData[] data = MasterServer.PollHostList();
		//if (GUILayout.Button("Connect to Server List")){
			//if (data.Length > 0){
				foreach (HostData element in data)
                {
					GUILayout.BeginHorizontal();
					string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
					GUILayout.Label(name);
					GUILayout.Space(5);
					string hostInfo = "[";
					foreach(string host in element.ip)
                    {
						hostInfo = hostInfo + host+":"+element.port+"";
					}
					hostInfo = hostInfo +"]";
                    GUILayout.Label(hostInfo);
					GUILayout.Space(5);
                    GUILayout.Label(element.comment);
					GUILayout.Space(5);
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Connect"))
                    {
						Network.Connect(element);
					}
					GUILayout.EndHorizontal();
				}
		if (MasterServer.PollHostList().Length == 0)
        {
			GUILayout.Label("No hosting servers found");
		}
			
		if (Network.peerType == NetworkPeerType.Connecting)
			GUILayout.Label ("Connecting...");
		if (Network.peerType != NetworkPeerType.Client)
			return;
		/*
		 * 
		 * BEGIN GAME CODE
		 * 
		 */
		Board.Lines (linesTexture);
		foreach (Space sp in board){
			sp.Draw();
			//sp.CircleOn();
		}
	}

    string str;
	void Update (){
		var mousePos = Input.mousePosition;
		mousePos.y = Screen.height - Input.mousePosition.y;
        str = Network.TestConnectionNAT().ToString();
		for (int i = 0; i < board.Length; i++)
        {
			if (!myTurn)
				return;
			if (board[i].rect.Contains (mousePos) && Input.GetButtonDown ("Fire1"))
            {
				if (board[i].GetCircleOn() || board[i].GetXOn ()) //if already taken
					return;

				networkView.RPC ("PlayerTwoPosition", RPCMode.Server, i);
				board[i].SetXOn ();
				myTurn = false;
			}
		}

		if (playerOneChoice != -1)
        {
			board[playerOneChoice].SetCircleOn ();
		}

		if (iWin || iLose){
			foreach(Space sp in board)
            {
				sp.Reset ();
			}
		}
        
	}

	[RPC]
	void MyTurn (bool b)
    {
		myTurn = b;
	}
	
	[RPC]
	void PlayerOneChoice (int i)
    {
		playerOneChoice = i;
	}

	[RPC]
	void IWinBool (bool b)
    {
		iWin = b;
		Invoke ("IWin", 2);
	}

	[RPC]
	void ILoseBool (bool b
        ){
		iLose = b;
		Invoke ("ILose", 2);
	}

	[RPC]
	void DrawCalled (bool b)
    {

	}

	void IWin ()
    {
		iWin = false;
		myScore++;
		//networkView.RPC score
		foreach (Space sp in board){
			sp.Reset ();
		}
		myTurn = false;
	}

	void ILose ()
    {
		iLose = false;
		theirScore++;
		//networkView.RPC score
		foreach (Space sp in board){
			sp.Reset ();
		}
		myTurn = true;
	}

	void Draw ()
    {
		draw = false;
		foreach (Space sp in board)
        {
			sp.Reset ();
		}
	}
	void OnFailedToConnect (NetworkConnectionError error)
	{
		Rect failedRect = new Rect(0,0,200,100);
			failedRect.center = new Vector2(Screen.width/2,Screen.height/2);
		GUI.Label(failedRect, "Could not connect to "+error);
	}
}
