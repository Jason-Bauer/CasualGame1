

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> characters = new List<Sprite>();
    public List<GameObject> emptyTiles = new List<GameObject>();
    public GameObject A, B, C;
    public int lane1, lane2, lane3, lane4, lane5, lane6, lane7;
    public GameObject tile;
    public int xSize, ySize;
    public GameObject score;

    public GameObject[,] tiles;

    public bool canSelect = true;

    public bool IsShifting { get; set; }

    public GameObject RTSManager;
    private int amtToSpawn = 0;
    private int typeToSpawn = 1;

    void Start()
    {
        instance = GetComponent<BoardManager>();

        Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

    private void CreateBoard(float xOffset, float yOffset)
    {
        tiles = new GameObject[xSize, ySize];

        float startX = 10;
        float startY = 10;

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;


        for (int X = 0; X < xSize; X++)
        {
            for (int Y = 0; Y < ySize; Y++)
            {


                GameObject newTile = Instantiate(tile, new Vector3(startX + ((xOffset + .3f) * X), startY + ((yOffset + .3f) * Y), 0), tile.transform.rotation);
                tiles[X, Y] = newTile;
                newTile.transform.parent = transform;
                newTile.transform.localPosition = new Vector3(startX + ((xOffset + 13f) * X), startY + ((yOffset + 13f) * Y), 0);
                List<Sprite> possibleCharacters = new List<Sprite>();
                possibleCharacters.AddRange(characters);

                possibleCharacters.Remove(previousLeft[Y]);
                possibleCharacters.Remove(previousBelow);

                int rand = Random.Range(0, possibleCharacters.Count);
                Sprite newSprite = possibleCharacters[rand];
                newTile.GetComponent<Tile>().X = X;
                newTile.GetComponent<Tile>().Y = Y;
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;
                // Debug.Log(newTile.GetComponent<SpriteRenderer>().sprite.name.ToString());
                if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "a")
                {
                    newTile.GetComponent<Tile>().type = 1;
                }
                else if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "b")
                {
                    newTile.GetComponent<Tile>().type = 2;
                }
                else if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "c")
                {
                    newTile.GetComponent<Tile>().type = 0;
                }
                else if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "d")
                {
                    newTile.GetComponent<Tile>().type = 3;
                }
                else if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "e")
                {
                    newTile.GetComponent<Tile>().type = 4;
                }
                else if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "f")
                {
                    newTile.GetComponent<Tile>().type = 5;
                }
                else if (newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "g")
                {
                    newTile.GetComponent<Tile>().type = 6;
                }
                else
                {
                    newTile.GetComponent<Tile>().type = 7;
                }


                previousLeft[Y] = newSprite;
                previousBelow = newSprite;


            }
        }
    }

    public void FindNullTiles()
    {
        canSelect = false;
        for (int y = ySize - 1; y >= 0; y--)
        {
            for (int x = xSize - 1; x >= 0; x--)
                {
            
                tiles[x, y].GetComponent<BoxCollider2D>().enabled = false;

                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    emptyTiles.Add(tiles[x, y]);

                }
            }
        }
        StartCoroutine(ShiftDown(emptyTiles));
        
    }

    private IEnumerator ShiftDown(List<GameObject> Movtiles)
    {
        //Make tiles fall
        //Tile tileCur = tiles [x, y].GetComponent<Tile> ();
        for (int j = 0; j < emptyTiles.Count; j++)
        {
            Tile tileCur = emptyTiles[j].GetComponent<Tile>();
            
                if (tileCur.Y < ySize-1 && tiles[tileCur.X, tileCur.Y + 1].GetComponent<SpriteRenderer>().sprite != null) {
                    for (int i = tileCur.Y; i < ySize - 1; i++)
                    {
                        
                        tileCur.GetComponent<SpriteRenderer>().sprite = tiles[tileCur.X, i + 1].GetComponent<SpriteRenderer>().sprite;
                        tileCur.GetComponent<Tile>().type = tiles[tileCur.X, i + 1].GetComponent<Tile>().type;

                        tiles[tileCur.X, i + 1].GetComponent<SpriteRenderer>().sprite = null;
                        tiles[tileCur.X, i + 1].GetComponent<Tile>().type = -1;

                        tileCur = tiles[tileCur.X, i + 1].GetComponent<Tile>();
                        emptyTiles[j] = tileCur.gameObject;
                        //SwapSprite (tiles [x, y].GetComponent<SpriteRenderer> (), tiles [x, y + 1].GetComponent<SpriteRenderer> ());


                    }
                    yield return new WaitForSeconds(0.07f);
                }
            
        }
        yield return new WaitForSeconds(0.07f);
        
        emptyTiles.Clear();
        for (int x = xSize - 1; x >= 0; x--)
        {
            for (int y = ySize - 1; y >= 0; y--)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    emptyTiles.Add(tiles[x, y]);

                }
            }
        }

        for (int i =0; i< emptyTiles.Count; i++)
        {
            Vector3 temp = emptyTiles[i].transform.position;
            temp.y += 20;
            emptyTiles[i].transform.position = temp;

        }


        for (int j = 0; j < emptyTiles.Count; j++)
        {
            Tile tileCur = emptyTiles[j].GetComponent<Tile>();
            //Adds Tiles where there were none
            if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite == null)
            {
                List<Sprite> possibleCharacters = new List<Sprite>();
                possibleCharacters.AddRange(characters);

                int rand = Random.Range(0, possibleCharacters.Count);
                Sprite newSprite = possibleCharacters[rand];
                tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite = newSprite;

                if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "a")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 1;
                }
                else if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "b")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 2;
                }
                else if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "c")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 0;
                }
                else if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "d")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 3;
                }
                else if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "e")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 4;
                }
                else if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "f")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 5;
                }
                else if (tiles[tileCur.X, tileCur.Y].GetComponent<SpriteRenderer>().sprite.name.ToString() == "g")
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 6;
                }
                else
                {
                    tiles[tileCur.X, tileCur.Y].GetComponent<Tile>().type = 7;
                }
            }
		}

        for (int i = 0; i < emptyTiles.Count; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                Vector3 temp = emptyTiles[i].transform.position;
                temp.y -= 1;
                emptyTiles[i].transform.position = temp;

            }

           yield return new WaitForSeconds(.07f);

        }
        

        canSelect = true;
        for (int x = xSize - 1; x >= 0; x--)
        {
            for (int y = ySize - 1; y >= 0; y--)
            {
                tiles[x, y].GetComponent<BoxCollider2D>().enabled = true;


            }
        }
        
    }




    //don't try and look at the code past this point
    //_______________________________________________
    // ____________________it's real ugly____________



    public void checkadjacent()
    {
        foreach (GameObject t in tiles)
        {
            t.GetComponent<Tile>().numadjc = 0;
            t.GetComponent<Tile>().matchFound = false;

        }
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < ySize; j++)
            {

                if (i > 0 && tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                {
                    tiles[i, j].GetComponent<Tile>().numadjc++;
                }
                if (i < xSize - 1 && tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                {
                    tiles[i, j].GetComponent<Tile>().numadjc++;
                }
                if (j > 0 && tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                {
                    tiles[i, j].GetComponent<Tile>().numadjc++;
                }
                if (j < ySize - 1 && tiles[i, j + 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                {
                    tiles[i, j].GetComponent<Tile>().numadjc++;
                }
            }

        }//end for loops
        foreach (GameObject t in tiles)
        {
            if (t.GetComponent<Tile>().numadjc >= 2)
            {
                t.GetComponent<Tile>().matchFound = true;

                if (t.GetComponent<Tile>().X > 0 && tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                {
                    tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                }
                if (t.GetComponent<Tile>().X < xSize - 1 && tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                {
                    tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                }
                if (t.GetComponent<Tile>().Y > 0 && tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                {
                    tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().matchFound = true;
                }
                if (t.GetComponent<Tile>().Y < ySize - 1 && tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                {
                    tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().matchFound = true;
                }


            }
        }//end of foreach
        bool callcheck = false;
        lane1 = 0;
        lane2 = 0;
        lane3 = 0;
        lane4 = 0;
        lane5 = 0;
        lane6 = 0;
        lane5 = 0;
        foreach (GameObject t in tiles)
        {
            if (t.GetComponent<Tile>().matchFound)
            {
                //amtToSpawn++;
                //typeToSpawn = t.GetComponent<Tile>().type;
                Destroytile(t);
                callcheck = true;

                if (t.GetComponent<Tile>().type == 0)
                {
                    lane1++;
                }
                else if (t.GetComponent<Tile>().type == 1)
                {
                    lane2++;
                }
                else if (t.GetComponent<Tile>().type == 2)
                {
                    lane3++;
                }
                else if (t.GetComponent<Tile>().type == 3)
                {
                    lane4++;
                }
                else if (t.GetComponent<Tile>().type == 4)
                {
                    lane5++;
                }
                else if (t.GetComponent<Tile>().type == 5)
                {
                    lane6++;
                }
                else if (t.GetComponent<Tile>().type == 6)
                {
                    lane7++;
                }
            }
        }
        if (callcheck)
        {
            FindNullTiles();
        }
        for (int i = 0; i < 7; i++)
        {
            if (i == 0)
            {
                if (lane1 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane1 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane1;
                }
            }
            else if (i == 1)
            {
                if (lane2 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane2 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane2;
                }
            }
            else if(i == 2)
            {
                if (lane3 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane3 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane3;
                }
            }
            else if(i == 3)
            {
                if (lane4 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane4 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane4;
                }
            }
            else if(i == 4)
            {
                if (lane5 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane5 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane5;
                }
            }
            else  if (i == 5)
            {
                if (lane6 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane6 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane6;
                }
            }
            else if (i == 6)
            {
                if (lane7 > 0)
                {
                    RTSManager.GetComponent<RTSManager>().typeToSpawn = i;
                    RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (lane7 / 3.0f));
                    RTSManager.GetComponent<RTSManager>().Spawn();
                    score.GetComponent<ScoreKeep>().score += lane7;
                }
            }

            //amtToSpawn = 0;
        }

    }//end checkadj

    public void Destroytile(GameObject t)
    {
        //t.GetComponent<Tile>().type = -1;
        t.GetComponent<ParticleSystem>().Play();
        t.GetComponent<SpriteRenderer>().sprite = null;
    }
}
