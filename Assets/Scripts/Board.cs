﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles; 
    
    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width,height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(tilePrefab, new Vector2(i,j), Quaternion.identity);
            }
        }
    }
}
