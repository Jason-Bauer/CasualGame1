/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	public static BoardManager instance;
	public List<Sprite> characters = new List<Sprite>();
    public GameObject A, B, C;
	public GameObject tile;
	public int xSize, ySize;
    
	public GameObject[,] tiles;

	public bool IsShifting { get; set; } 

	void Start () {
		instance = GetComponent<BoardManager>();
        
		Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard(offset.x, offset.y);
    }

	private void CreateBoard (float xOffset, float yOffset) {
		tiles = new GameObject[xSize, ySize];

        float startX = transform.position.x;
		float startY = transform.position.y;

        Sprite[] previousLeft = new Sprite[ySize];
        Sprite previousBelow = null;


        for (int X = 0; X < xSize; X++) {
			for (int Y = 0; Y < ySize; Y++) {


				GameObject newTile = Instantiate(tile, new Vector3(startX + ((xOffset+.3f) * X), startY + ((yOffset+.3f) * Y),0), tile.transform.rotation);
				tiles[X,Y] = newTile;
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
               if(newTile.GetComponent<SpriteRenderer>().sprite.name.ToString() == "a")
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
                else 
                {
                    newTile.GetComponent<Tile>().type = 5;
                }


                previousLeft[Y] = newSprite;
                previousBelow = newSprite;


            }
        }
    }

    public void FindNullTiles()
    {
        for (int x = xSize - 1; x > 0; x--)
        {
            for (int y = ySize - 1; y > 0; y--)
            {
                if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    Debug.Log("Shift Attempt");
                    ShiftDown(x, x, y);
                }
            }
        }
    }

    public void ShiftDown(int columnX, int x,int y)
    {
        
    }

    public void SwapSprite(SpriteRenderer render2, SpriteRenderer render)
    { // 1
        if (render.sprite == render2.sprite)
        { // 2
            return;
        }
        int temptype = render2.gameObject.GetComponent<Tile>().type;
        Sprite tempSprite = render2.sprite; // 3
        render2.sprite = render.sprite; // 4
        render2.gameObject.GetComponent<Tile>().type = render.gameObject.GetComponent<Tile>().type;
        //render.sprite = tempSprite; // 5
        //render.gameObject.GetComponent<Tile>().type = temptype;
    }

    private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .03f)
    {
        IsShifting = true;
        List<SpriteRenderer> renders = new List<SpriteRenderer>();
        int nullCount = 0;

        for (int y = yStart; y < ySize; y++)
        {  // 1
            SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
            if (render.sprite == null)
            { // 2
                nullCount++;
            }
            renders.Add(render);
        }

        for (int i = 0; i < nullCount; i++)
        { // 3
            yield return new WaitForSeconds(shiftDelay);// 4
            for (int k = 0; k < renders.Count - 1; k++)
            { // 5
                renders[k].sprite = renders[k + 1].sprite;
                renders[k + 1].sprite = GetNewSprite(x, ySize-1 );
            }
        }
        IsShifting = false;
    }

    private Sprite GetNewSprite(int x, int y)
    {
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        if (x > 0)
        {
            possibleCharacters.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x < xSize - 1)
        {
            possibleCharacters.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCharacters.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
        }

        return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
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
        for(int i = 0; i < xSize; i++)
        {
            for(int j=0; j < ySize; j++)
            {

                if (i == 0)
                {
                    if (j == 0)
                    {
                        if (tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j+1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                    else if (j == 6)
                    {
                        if (tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                    else
                    {
                        if (tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j + 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                }
                else if (i == 6)
                {
                    if (j == 0)
                    {
                        if (tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j + 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                    else if (j == 6)
                    {
                        if (tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                    else
                    {
                        if (tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j + 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                }
                else
                {
                    if (j == 0)
                    {
                        if (tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j + 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                    else if (j == 6)
                    {
                        if (tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                    else
                    {
                        if (tiles[i - 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i + 1, j].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j - 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                        if (tiles[i, j + 1].GetComponent<Tile>().type == tiles[i, j].GetComponent<Tile>().type)
                        {
                            tiles[i, j].GetComponent<Tile>().numadjc++;
                        }
                    }
                }
            }
        }//end for loops
        foreach(GameObject t in tiles)
        {
            if (t.GetComponent<Tile>().numadjc >= 2)
            {
                t.GetComponent<Tile>().matchFound = true;
                if (t.GetComponent<Tile>().X == 0)
                {
                    if (t.GetComponent<Tile>().Y == 0)
                    {
                        if (tiles[t.GetComponent<Tile>().X+1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X , t.GetComponent<Tile>().Y+1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X , t.GetComponent<Tile>().Y+1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                    else if (t.GetComponent<Tile>().Y == 6)
                    {
                        if (tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X , t.GetComponent<Tile>().Y-1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X , t.GetComponent<Tile>().Y-1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                    else
                    {
                        if (tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                }
                else if (t.GetComponent<Tile>().X == 6)
                {
                    if (t.GetComponent<Tile>().Y == 0)
                    {
                        if (tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                    else if (t.GetComponent<Tile>().Y == 6)
                    {
                        if (tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                    else
                    {
                        if (tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                }
                else
                {
                    if (t.GetComponent<Tile>().Y == 0)
                    {
                        if (tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                    else if (t.GetComponent<Tile>().Y == 6)
                    {
                        if (tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                    else
                    {
                        if (tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X - 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X + 1, t.GetComponent<Tile>().Y].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y - 1].GetComponent<Tile>().matchFound = true;
                        }
                        if (tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().type == tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y].GetComponent<Tile>().type)
                        {
                            tiles[t.GetComponent<Tile>().X, t.GetComponent<Tile>().Y + 1].GetComponent<Tile>().matchFound = true;
                        }
                    }
                }
            }
        }//end of foreach
        bool callcheck = false;
        foreach (GameObject t in tiles)
        {
            if (t.GetComponent<Tile>().matchFound)
            {
                Destroytile(t);
                callcheck = true;
            }
        }
        if (callcheck)
        {
            FindNullTiles();
        }
        }//end checkadj

    public void Destroytile(GameObject t)
    {
        t.GetComponent<SpriteRenderer>().sprite = null;

    }
     public List<GameObject> adjacenttiles(int x, int y)
    {
        List<GameObject> returntiles=new List<GameObject>();
        if (x == 0)
        {
            if (y == 0)
            {
                if (tiles[x + 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x + 1, y]);
                }
                if (tiles[x, y + 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y+1]);
                }
            }
            else if (y == 6)
            {
                if (tiles[x + 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x + 1, y]);
                }
                if (tiles[x, y - 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x, y-1]);
                }
            }
            else
            {
                if (tiles[x + 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x + 1, y]);
                }
                if (tiles[x, y - 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x, y-1]);
                }
                if (tiles[x, y + 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y+1]);
                }

            }
        }
        else if (x == 6)
        {
            if (y == 0)
            {
                if (tiles[x - 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x - 1, y]);
                }
                if (tiles[x, y + 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y+1]);
                }
            }
            else if (y == 6)
            {
                if (tiles[x - 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x - 1, y]);
                }
                if (tiles[x, y - 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x, y-1]);
                }
            }
            else
            {
                if (tiles[x - 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x - 1, y]);
                }
                if (tiles[x, y - 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y-1]);
                }
                if (tiles[x, y + 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y+1]);
                }

            }
        }
        else
        {
            if (y == 0)
            {
                if (tiles[x - 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x - 1, y]);
                }
                if (tiles[x + 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x + 1, y]);
                }
                if (tiles[x, y + 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y+1]);
                }
            }
            else if (y == 6)
            {
                if (tiles[x + 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x + 1, y]);
                }
                if (tiles[x - 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x - 1, y]);
                }
                if (tiles[x, y- 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y-1]);
                }
            }
            else
            {
                if (tiles[x - 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x - 1, y]);
                }
                if (tiles[x + 1, y].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x + 1, y]);
                }
                if (tiles[x, y - 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y-1]); 
                }
                if (tiles[x, y + 1].GetComponent<Tile>().type == tiles[x, y].GetComponent<Tile>().type)
                {
                    returntiles.Add(tiles[x , y+1]);
                }

            }


        }

   
        return returntiles;
    }


}
