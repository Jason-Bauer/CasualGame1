﻿

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public List<Sprite> characters = new List<Sprite>();
    public List<GameObject> emptyTiles = new List<GameObject>();
    public GameObject A, B, C;
    public GameObject tile;
    public int xSize, ySize;

    public GameObject[,] tiles;

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

        float startX = transform.position.x;
        float startY = transform.position.y;

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;


        for (int X = 0; X < xSize; X++)
        {
            for (int Y = 0; Y < ySize; Y++)
            {


                GameObject newTile = Instantiate(tile, new Vector3(startX + ((xOffset + .3f) * X), startY + ((yOffset + .3f) * Y), 0), tile.transform.rotation);
                tiles[X, Y] = newTile;
                newTile.transform.parent = transform;

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
        StartCoroutine(ShiftDown(emptyTiles));

    }

    private IEnumerator ShiftDown(List<GameObject> Movtiles)
    {
        //Make tiles fall
        //Tile tileCur = tiles [x, y].GetComponent<Tile> ();
        for (int j = 0; j < Movtiles.Count; j++)
        {
            Tile tileCur = Movtiles[j].GetComponent<Tile>();
            for (int i = tileCur.Y; i < ySize - 1; i++)
            {
                tileCur.GetComponent<SpriteRenderer>().sprite = tiles[tileCur.X, i + 1].GetComponent<SpriteRenderer>().sprite;
                tileCur.GetComponent<Tile>().type = tiles[tileCur.X, i + 1].GetComponent<Tile>().type;

                tiles[tileCur.X, i + 1].GetComponent<SpriteRenderer>().sprite = null;
                tiles[tileCur.X, i + 1].GetComponent<Tile>().type = -1;

                tileCur = tiles[tileCur.X, i + 1].GetComponent<Tile>();
                emptyTiles[j] = tileCur.gameObject;
                //SwapSprite (tiles [x, y].GetComponent<SpriteRenderer> (), tiles [x, y + 1].GetComponent<SpriteRenderer> ());
                yield return new WaitForSeconds(.07f);

            }
        }


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
        foreach (GameObject t in tiles)
        {
            if (t.GetComponent<Tile>().matchFound)
            {
                amtToSpawn++;
                typeToSpawn = t.GetComponent<Tile>().type;
                Destroytile(t);
                callcheck = true;
            }
        }
        if (callcheck)
        {
            FindNullTiles();
        }
        RTSManager.GetComponent<RTSManager>().typeToSpawn = typeToSpawn;
        RTSManager.GetComponent<RTSManager>().friendly.GetComponent<UnitBehavior>().health = Mathf.Ceil(10.0f + (amtToSpawn / 3.0f));
        RTSManager.GetComponent<RTSManager>().Spawn();
        amtToSpawn = 0;

    }//end checkadj

    public void Destroytile(GameObject t)
    {

        t.GetComponent<ParticleSystem>().Play();
        t.GetComponent<SpriteRenderer>().sprite = null;
    }
}