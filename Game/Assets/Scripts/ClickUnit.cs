using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickUnit : MonoBehaviour 
{
    public bool selected;
    public GameObject player;
    public Map map;
    public int health; 
	public int maxHealth;
    public int damage;
    public int morale;
	public double maxMoveDistance;
	public double movesLeft;
    public double attackRange;
	public ClickTile currentTile;
	public Dictionary<string, int> terrainTolorances;
    public PotentialMove potMove;
	public static string grass;
	public static string mountain;
	public static string dirt;
	public static string water;
    public string name;
	public GameObject[] abilities;

	Vector2 position;

	void Start()
	{
		movesLeft = maxMoveDistance;

		grass = "Grass (Instance)";
		mountain = "Brown Stony (Instance)";
		dirt = "Brown Stony Light (Instance)";
		water = "Water Deep Blue (Instance)";
		terrainTolorances = new Dictionary<string, int> () 
		{
				{ grass, 1 },
				{ mountain, 2 },
				{ dirt, 1 },
				{ water, 99 }
		};
	}

    void OnMouseUp() 
	{
		position = map.GetPositionFromTransform (this.transform);
        map.ChangeUnit(player);

		//print (map.getClickTiles().Length);

		for (int i = 0; i < map.getSizeX(); i++) 
		{
			for (int j = 0; j < map.getSizeY(); j++) 
			{
				ClickTile t = map.getTileOnMap (i, j);
				//within move distance
				/*if (getPossibleMoves().Contains(t)) 
				{
					
				}*/
			}
		}
		HealthDisplay bar = GameObject.FindGameObjectWithTag("health bar").GetComponents<HealthDisplay>()[0];
        bar.change (this);
		Debug.Log (bar);
    }

	void OnMouseOver()
	{
		if (Input.GetMouseButtonUp (1)) 
		{
			map.displayActions (this);
		}
	}

	public List<PotentialMove> getPossibleMoves()
	{

		List<PotentialMove> possible = new List<PotentialMove>();
		ArrayList toCheckSurroundings = new ArrayList ();
		ArrayList numMoves = new ArrayList();

		toCheckSurroundings.Add (position);
		numMoves.Add (0);

		for(int n = 0; n < toCheckSurroundings.Count; n++)
		{
			Vector2 space = (Vector2)toCheckSurroundings[n];

			for (int i = 0, j = -1; i <= 1; j = (j == -1)? 1:0, i = (i == 1)? 100:i, i = (i == -1)? 1:i, i = (j == 0 && i == 0)? -1:i) 
			{
				int checkX = (int)space.x + i;
				int checkY = (int)space.y + j;
				if (checkX == 1 && checkY == 2) {

				}
				Vector2 check = new Vector2 (checkX, checkY);
				if (map.isOnBoard (check)) 
				{
					ClickTile cT = map.getTileFromVector (check);
					if (this.terrainTolorances [cT.terrain.name] != 0) 
					{
						int addedMoves = this.terrainTolorances[cT.terrain.name];
						if((int)numMoves[n] + addedMoves <= this.movesLeft)
						{
							int moveDistance = (int)numMoves [n] + addedMoves;
							if (toCheckSurroundings.Contains (check))
							{
								for (int b = toCheckSurroundings.Count - 1; b >= 0; b--) 
								{
									if (toCheckSurroundings [b].Equals (check)) 
									{
									if (moveDistance < (int)numMoves [b] && cT.IsOccupied() == false) 
										{
                                            /*for (int q = 0; q < possible.Count; q++)
                                            {
                                    
                                                if (possible[q].getCt().getX() == cT.getX() && possible[q].getCt().getY() == cT.getY())
                                                {
                                                    
                                                }
                                                else
                                                {
                                                    
                                                }
                                            }*/
											possible.Add (new PotentialMove (cT, moveDistance));
											        toCheckSurroundings.Add (check);
											        numMoves.Add (moveDistance);
										}
										b = -11;
									}
								}
							} 
							else 
							{
								if (cT.IsOccupied() == false) 
								{
									possible.Add (new PotentialMove (cT, moveDistance));
									toCheckSurroundings.Add (check);
									numMoves.Add (moveDistance);
								}
							}
						}
					}
				}	
			}
		}

		return possible;
	}

	public struct PotentialMove
	{
		public ClickTile t;
		public int moves;

		public PotentialMove(ClickTile t, int moves)
		{
			this.t = t;
			this.moves = moves;
		}
        public int getMoves()
        {
            return moves;
        }
        public ClickTile getCt()
        {
            return t;
        }
	}

}