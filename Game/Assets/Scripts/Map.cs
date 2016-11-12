using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Map : MonoBehaviour {
    public bool firstPlayerTurn = true;
    public GameObject selectedUnit;
    public GameObject TileMove;
    public GameObject TileAttack;
    public TileType[] tileType;
    public ClickUnit selectedPlayer;
    public ClickUnit attackedPlayer;
    public Texture2D background;
    public GameObject[] moveTiles;
    public GameObject[] attackTiles; 
  	private ClickTile[,] clickTiles;
    public GameObject[] player1Units;
    public GameObject[] player2Units;
    int[,] tiles;
    public Text WinText;
    public Text player1MoraleText;
    public Text player2MoraleText;
   // int[,] moveTiles;
    int sizeX = 18; //Manually change for map
    int sizeY = 18;
    int tileSizeX = 200;
    int tileSizeY = 200;
    public int player1Morale = 100, player2Morale = 100;
	Vector2 origin;
    public Text playerInfo;
    public Image unitImage;

    void Start() {
        //Create map tiles
        tiles = new int[sizeX, sizeY];
		clickTiles = new ClickTile[sizeX, sizeY];

        //Moves tile to tile checking
        for (int x = 100; x < sizeX * tileSizeX; x += tileSizeX)
        {

            for (int y = 100; y < sizeY *tileSizeY; y += tileSizeY)
            {
               // Debug.Log(background.GetPixel(x, y));
                //checks if it is green and assigns the grass tile
                if (background.GetPixel(x, y) == new Color(0,1,0,1))
                {
                    tiles[(x-100)/tileSizeX , (y-100)/tileSizeY] = 0;
                }
                //checks if it is blue and assigns the water tile
                else if (background.GetPixel(x, y) == new Color(0,0,1,1))
                {
                    tiles[(x-100)/tileSizeX, (y-100)/tileSizeY] = 1;
                }
                //checks if it is red and assigns the dirt tile
                else if (background.GetPixel(x, y) == new Color(1,0,0,1))
                {
                    tiles[(x-100)/tileSizeX, (y-100)/tileSizeY] = 2;
                }
                
            }
        }
        generateMap();
        generatePlayers();
		origin.Set (clickTiles[0,0].transform.position.x,clickTiles[0,0].transform.position.y);
    }
    void generateMap() {
		clickTiles = new ClickTile[sizeX,sizeY];
        //goes through the grid
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                //finds the tile type and creates a clone of it in the grid section it is assigned to and gives it an x and y coord.
                TileType tt = tileType[tiles[x, y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisual, new Vector3(x, y, 0), Quaternion.identity);
                ClickTile ct = go.GetComponent<ClickTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
				clickTiles [x,y] = ct;
            }
        }
    }
    public void generatePlayers() {
        player1Units = GameObject.FindGameObjectsWithTag("Player 1");
        player2Units = GameObject.FindGameObjectsWithTag("Player 2");
    }
    public void MoveUnitTo(int x, int y)
    {
        //checks  to see if it is selected
        if ((selectedPlayer.selected && firstPlayerTurn && selectedPlayer.tag.Equals("Player 1")) || (selectedPlayer.selected && !firstPlayerTurn && selectedPlayer.tag.Equals("Player 2")))
        {
                //moves the selected unit. Note the -.75 is for the unit to appear on the grid. It does not move in the z direction
                selectedUnit.transform.position = new Vector3(x, y, (float)-0.75);
                Vector2 v = new Vector2(x, y);
                ClickTile cT = this.getTileFromVector(v);
                if (selectedPlayer.currentTile != null)
                {
                selectedPlayer.currentTile.containedUnit = null;
                }
            selectedPlayer.currentTile = cT;
            cT.containedUnit = selectedPlayer;
            DestroyTiles();
            selectedPlayer.selected = false;
        }
        
    }
    public void ChangeUnit(GameObject Cu) {
        selectedPlayer = Cu.GetComponent<ClickUnit>();
        selectedPlayer.map = this;
        selectedUnit = Cu;
        playerInfo.text = "             " + selectedPlayer.name + "\n\nPlayer Selected: \n    " + selectedPlayer.tag + "\nAttack Damage: \n    " + selectedPlayer.damage + "\nHealth: \n    " + selectedPlayer.health + "\nUnit Morale: \n    " + selectedPlayer.morale + "\nMove: \n     " + selectedPlayer.maxMoveDistance;

        unitImage.enabled = true;
        unitImage.material = ((MeshRenderer)selectedUnit.GetComponent<MeshRenderer>()).material;
        
        if (selectedPlayer.selected)
        {
            selectedPlayer.selected = false;
            DestroyTiles();
        }
        else
        {
            selectedPlayer.selected = true;
            DestroyTiles();
            CreateTile();
        }
    }
    public double Distance(int x1, int y1, int x2, int y2) {
        double dis = 0;
        dis = Mathf.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2)* (y1 - y2)));
        return dis;
    }
    public void DestroyTiles() {
        moveTiles = GameObject.FindGameObjectsWithTag("Move");
        attackTiles = GameObject.FindGameObjectsWithTag("Attack");
        for (int i = 0; i < moveTiles.Length; i++)
        {
            Destroy(moveTiles[i]);
        }

        for (int i = 0; i < attackTiles.Length; i++)
        {
            Destroy(attackTiles[i]);
        }
    }
    public void CreateTile() {//change so one doesnt spawn on other objects.
        if ((selectedPlayer.selected && firstPlayerTurn && selectedPlayer.tag.Equals("Player 1")) || (selectedPlayer.selected && !firstPlayerTurn && selectedPlayer.tag.Equals("Player 2")))
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
					foreach (ClickUnit.PotentialMove p in selectedPlayer.getPossibleMoves()) 
					{
						if (p.t.Equals(this.getTileFromVector(new Vector2(x,y))) && tileOpen(this.getTileFromVector(new Vector2(x,y))))
						{
							//Debug.Log("X =" + x + "Y=" + y);
							GameObject go = (GameObject)Instantiate (TileMove, new Vector3 (x, y, (float)-.5), Quaternion.identity);
							ClickTile ct = go.GetComponent<ClickTile> ();
							ct.tileX = x;
							ct.tileY = y;
							ct.map = this;
							ct.movesTo = p.moves;
						}
					}
                    //Creating Attack Tiles
						if (selectedPlayer.movesLeft != -1 && Distance ((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.y, x, y) <= selectedPlayer.attackRange && Distance ((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.y, x, y) > 0 && checkEnemy (clickTiles [x, y])) {
							//Debug.Log("X =" + x + "Y=" + y);
							GameObject go = (GameObject)Instantiate (TileAttack, new Vector3 (x, y, (float)-1.5), Quaternion.identity);
							ClickTile ct = go.GetComponent<ClickTile> ();
							ct.tileX = x;
							ct.tileY = y;
							ct.map = this;

						}
                }
            }
        }
    /* for (int x = 0; x < moveTiles.Length; x++) {
            for (int y = 0; y < moveTiles.Length; y++)
            {
                if (moveTiles[x, y] == 1)
                    Instantiate(TileMove, new Vector3(x, y, (float)-.5), Quaternion.identity); 
            }
        }
     */
    }
    public void moraleChange(ClickUnit cu) {
        if (cu.tag.Equals("Player 1")) {
            player1Morale -= cu.morale;
            if (player1Morale <= 0)
                player1Morale = 0;
            checkWinConditions(2, player1Morale);
        }
        if (cu.tag.Equals("Player 2"))
        {
            player2Morale -= cu.morale;
            if (player2Morale <= 0)
                player2Morale = 0;
            checkWinConditions(1, player2Morale);
        }
        player1MoraleText.text = "Player 1 Morale: " + player1Morale;
        player2MoraleText.text = "Plyaer 2 Morale: " + player2Morale;
    }
    public bool tileOpen(ClickTile ct) {
        bool open = true;
        // search through player units then if x and y are same then not clickable.'
        for (int i = 0; i < player1Units.Length; i++)
        {
            if (player1Units[i] != null)
            {
                if (player1Units[i].transform.position.x == ct.transform.position.x && player1Units[i].transform.position.y == ct.transform.position.y)
                {
                    open = false;
                }
            }
        }
        for (int i = 0; i < player2Units.Length; i++) {

            if (player2Units[i] != null)
            {
                if (player2Units[i].transform.position.x == ct.transform.position.x && player2Units[i].transform.position.y == ct.transform.position.y)
                {
                    open = false;
                }
            }
        }
        
        return open;
    }
    public bool checkEnemy(ClickTile ct) {
        bool ret = false;
        bool turn = firstPlayerTurn;
        if (turn) {
            for (int i = 0; i < player2Units.Length; i++)
            {
                if (player2Units[i] != null)
                {
                    if (player2Units[i].transform.position.x == ct.transform.position.x && player2Units[i].transform.position.y == ct.transform.position.y)
                    {
                        ret = true;
                    }
                }
            }
        }

        else {
            for (int i = 0; i < player1Units.Length; i++)
            {
                if (player1Units[i] != null)
                {
                    if (player1Units[i].transform.position.x == ct.transform.position.x && player1Units[i].transform.position.y == ct.transform.position.y)
                    {
                        ret = true;
                    }
                }
            }
        }
        
        return ret;
    }

	public ClickTile getTileOnMap(int x, int y)
	{
		return clickTiles [x, y];
	}

	public ClickTile[,] getClickTiles()
	{
		return clickTiles;
	}

	public int getSizeX()
	{
		return sizeX;
	}

	public int getSizeY()
	{
		return sizeY;
	}

	public Vector2 GetPositionFromTransform(Transform t)
	{
		Vector2 tile = new Vector2();
		tile.x = (int)((t.position.x - origin.x) * POSITION_GRID_RATIO);
		tile.y = (int)((t.position.y - origin.y) * POSITION_GRID_RATIO);
		//print (t.position.x);
		return tile;
	}

	public ClickTile getTileFromVector(Vector2 v)
	{
		return clickTiles [(int)v.x, (int)v.y].GetComponent<ClickTile>();
	}
		

	public bool isOnBoard(Vector2 v)
	{
		if (v.x >= 0 && v.x < sizeX && v.y >= 0 && v.y < sizeY) 
		{
			return true;
		}
		return false;
	}

	const float POSITION_GRID_RATIO = 1.0f;
    public void attackObject(int x, int y)
    {
        if (!firstPlayerTurn)
        {
            for (int i = 0; i < player1Units.Length; i++)
            {
                if (player1Units[i] != null && player1Units[i].transform.position.x == x && player1Units[i].transform.position.y == y && selectedPlayer.movesLeft != -1)
                {
                    attackedPlayer = player1Units[i].GetComponent<ClickUnit>();
                    attackedPlayer.health -= selectedPlayer.damage;
                    selectedPlayer.movesLeft = -1;
                }
            }
        }
        if (firstPlayerTurn)
        {
            for (int i = 0; i < player2Units.Length; i++)
            {

                if (player2Units[i] != null && player2Units[i].transform.position.x == x && player2Units[i].transform.position.y == y && selectedPlayer.movesLeft != -1)
                {
                    attackedPlayer = player2Units[i].GetComponent<ClickUnit>();
                    attackedPlayer.health -= selectedPlayer.damage;
                    selectedPlayer.movesLeft = -1;
                }
             }
        }
        DestroyTiles();
        if (attackedPlayer != null && attackedPlayer.health <= 0)
        {
            //attackedPlayer.currentTile.containedUnit = null;
            moraleChange(attackedPlayer);
             DestroyObject(attackedPlayer.player);
     
        }
    }

    protected void checkWinConditions(int player, int playerMorale)
    {
        if(playerMorale <= 0)
        {
            WinText.color = new Color(WinText.color.r, WinText.color.g, WinText.color.b, 255);
            WinText.text = "Player " + player + " WINS!!!";
        }
    }
}
