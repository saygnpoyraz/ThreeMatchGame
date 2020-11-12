﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] dots;

    public GameObject[,] allDots;
    
    private BackgroundTile[,] allTiles; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width,height];
        allDots = new GameObject[width,height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 cellPos = new Vector2(i, j);
                GameObject backgroundTiles = Instantiate(tilePrefab, cellPos, Quaternion.identity,transform);
                backgroundTiles.name = "( " + i + ", " + j + " )";
                 
                int dotUse = Random.Range(0,dots.Length);
                GameObject dot = Instantiate(dots[dotUse], cellPos, Quaternion.identity,transform);
                dot.name = "( " + i + ", " + j + " )";;
                allDots[i, j] = dot;
            }
        }
    }
}
