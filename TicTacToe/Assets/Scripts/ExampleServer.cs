using UnityEngine;
using System.Collections;

public class ExampleServer : MonoBehaviour {
	public Texture2D linesTexture;
	public Texture2D circleTexture;
	public Texture2D xTexture;
	public bool playerOneTurn = true;
	public bool playerTwoTurn = false;

	public int playerOneWins = 0;
	public int playerTwoWins = 0;

	public Space[] board = new Space[9];
	public bool winOne = false;
	public bool winTwo = false;
	private bool draw = false;

	public string serverName;
	public string serverComment;

	private int playerTwoChoice = -1;

    public GUISkin mySkin;

	void Start (){
		int a = 0;
		for (int k = 0; k < 3; k++){
			for (int i = 0; i < 3; i++){
				board[a] = new Space();
				board[a].Init(circleTexture, xTexture, i*((Screen.width/3)+20), k*((Screen.width/3)+20));
				a++;
			}
		}
	}

	void OnGUI(){
        GUI.skin = mySkin;
        GUI.Label(new Rect(10, Screen.height - 160, 600, 50), str);
		if (Network.peerType == NetworkPeerType.Disconnected)
        {
            serverName = GUI.TextField(new Rect(40, 10, 300, 60), "Name", 10);
            serverComment = GUI.TextField(new Rect(40, 80, 300, 60), "Comment", 10);
            if (GUI.Button(new Rect(40, 200, 300, 180), "StartServer"))
            {
				Network.InitializeServer (1, 25002, true);
				MasterServer.RegisterHost("LOL",serverName,serverComment);
			}
		}
		if (Network.peerType == NetworkPeerType.Connecting)
        {
            GUILayout.Label("Connecting...");
		}
		if (Network.peerType == NetworkPeerType.Server)
        {
			if (Network.connections.Length < 1)
            {
                GUILayout.Label("Waiting for challenger...");
				return;
			}
			Board.Lines (linesTexture);
			foreach (Space sp in board)
            {
				sp.Draw();
				//sp.CircleOn();
			}
			if (playerOneTurn)
                GUI.Label(new Rect((Screen.width / 2) - 70, ((Screen.height / 6) * 5) - 80, 140, 100), "Player One's Turn");
			if (playerTwoTurn)
                GUI.Label(new Rect((Screen.width / 2) - 70, ((Screen.height / 5) * 5) - 80, 140, 100), "Player Two's Turn");
			if (winOne)
                GUI.Label(new Rect((Screen.width / 2) - 50, ((Screen.height / 6) * 5) - 50, 100, 100), "Player One Wins!");
			if (winTwo)
                GUI.Label(new Rect((Screen.width / 2) - 50, ((Screen.height / 6) * 5) - 50, 100, 100), "Player Two Wins!");
			if (draw)
                GUI.Label(new Rect((Screen.width / 2) - 50, ((Screen.height / 6) * 5) - 50, 100, 100), "Draw");
		}
        
	}

	public string str;
	void Update (){
        str = Network.TestConnectionNAT().ToString();
		if (Network.connections.Length < 1)
			return;
		
		var mousePos = Input.mousePosition;
		mousePos.y = Screen.height - Input.mousePosition.y;
		for(int i = 0; i < board.Length; i++){
			if (winOne || winTwo)
				return;
			if (playerOneTurn)
            {
				if (board[i].rect.Contains (mousePos) && Input.GetButtonDown ("Fire1"))
                {
					if (board[i].GetCircleOn() || board[i].GetXOn ()) //if already taken
						return;
					board[i].SetCircleOn ();
					networkView.RPC ("PlayerOneChoice", RPCMode.Others, i);
					networkView.RPC ("myTurn", RPCMode.Others, true);
					Turn ();
				}
			}
		}
		if (playerTwoTurn)
        {
			if (playerTwoChoice != -1)
            {
				board[playerTwoChoice].SetXOn ();
				playerTwoChoice = -1;
				Turn ();
			}
		}
		StartCoroutine("CheckForWin");
	}

	[RPC]
	void PlayerTwoPosition (int pos)
    {
		playerTwoChoice = pos;
	}
	


	public void Turn (){
		if (playerOneTurn){
			playerOneTurn = false;
			playerTwoTurn = true;
			return;
		}
		if (playerTwoTurn){
			playerOneTurn = true;
			playerTwoTurn = false;
			return;
		}
	}

	void CheckForWin (){
		if (((board[0].GetCircleOn() == true && board[1].GetCircleOn() && board[2].GetCircleOn()) ||	//top hor
		    (board[3].GetCircleOn() == true && board[4].GetCircleOn() && board[5].GetCircleOn()) ||	//middle hor
		    (board[6].GetCircleOn() == true && board[7].GetCircleOn() && board[8].GetCircleOn()) ||	//bottom hor
		    (board[0].GetCircleOn() == true && board[3].GetCircleOn() && board[6].GetCircleOn()) || //left vert
		    (board[1].GetCircleOn() == true && board[4].GetCircleOn() && board[7].GetCircleOn()) || //middle vert
		    (board[2].GetCircleOn() == true && board[5].GetCircleOn() && board[8].GetCircleOn()) || //right vert
		    (board[0].GetCircleOn() == true && board[4].GetCircleOn() && board[8].GetCircleOn()) || //top-left to bottom-right diagonal
		    (board[2].GetCircleOn() == true && board[4].GetCircleOn() && board[6].GetCircleOn())) &&	//top-right to bottom-left diagonal
		    winOne == false
		   )
		{
			winOne = true;
			Invoke ("WinOne", 2);
			networkView.RPC ("iLose", RPCMode.Others, true);
		}
		if (((board[0].GetXOn() == true && board[1].GetXOn() && board[2].GetXOn()) || //top hor
		    (board[3].GetXOn() == true && board[4].GetXOn() && board[5].GetXOn()) || //middle hor
		    (board[6].GetXOn() == true && board[7].GetXOn() && board[8].GetXOn()) || //bottom hor
		    (board[0].GetXOn() == true && board[3].GetXOn() && board[6].GetXOn()) || //left vert
		    (board[1].GetXOn() == true && board[4].GetXOn() && board[7].GetXOn()) || //middle vert
		    (board[2].GetXOn() == true && board[5].GetXOn() && board[8].GetXOn()) || //right vert
		    (board[0].GetXOn() == true && board[4].GetXOn() && board[8].GetXOn()) || //top-left to bottom-right diagonal
		    (board[2].GetXOn() == true && board[4].GetXOn() && board[6].GetXOn())) && //top-right to bottom-left diagonal
		    winOne == false
		   )
		{
			winTwo = true;
			Invoke ("WinTwo", 2);
			networkView.RPC ("iWin", RPCMode.Others, true);
		}
		if (draw)
			return;
		int spotsFilled = 0;
		foreach (Space sp in board){
			if (sp.GetCircleOn () || sp.GetXOn ())
				spotsFilled++;
		}
		if (spotsFilled == 9 && !winOne && !winTwo){
			draw = true;
			Invoke ("Draw", 2);
		}
	}

	void WinOne (){
		winOne = false;
		playerOneWins++;
		//networkView.RPC score
		foreach (Space sp in board){
			sp.Reset ();
		}
		LoserFirst ();
	}

	void WinTwo (){
		winTwo = false;
		playerTwoWins++;
		//networkView.RPC score
		foreach (Space sp in board){
			sp.Reset ();
		}
		LoserFirst ();
	}

	void Draw (){
		foreach (Space sp in board){
			sp.Reset ();
		}
		draw = false;
	}
	void LoserFirst (){
		if (playerOneWins > playerTwoWins){
			playerOneTurn = false;
			playerTwoTurn = true;
		}
		if (playerTwoWins > playerOneWins){
			playerOneTurn = true;
			playerTwoTurn = false;
		}
	}
}
	