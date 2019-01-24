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

public class Tile : MonoBehaviour {
	private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	private static Tile previousSelected = null;
    public int X;
    public int Y;
    public int type;
    public int numadjc;
	public SpriteRenderer render;
	private bool isSelected = false;
	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public bool matchFound = false;

    void Awake() {
		render = GetComponent<SpriteRenderer>();
    }
	private void Select() {
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile>();	
	}
	private void Deselect() {
		isSelected = false;
		render.color = Color.white;
		previousSelected = null;
	}
    void OnMouseDown()
    { 
        if (render.sprite == null || BoardManager.instance.IsShifting)
        {
            return;
        }
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            if (previousSelected == null)
            {
                Select();
            }
            else
            {
                SwapSprite(previousSelected.render);
                previousSelected.Deselect(); 
                BoardManager.instance.checkadjacent();
            }
        }
    }
    public void SwapSprite(SpriteRenderer render2)
    { 
        if (render.sprite == render2.sprite)
        { 
            return;
        }
        int temptype = render2.gameObject.GetComponent<Tile>().type;
        Sprite tempSprite = render2.sprite; 
        render2.sprite = render.sprite; 
        render2.gameObject.GetComponent<Tile>().type = this.type;
        render.sprite = tempSprite; 
        this.type = temptype;
    }
}