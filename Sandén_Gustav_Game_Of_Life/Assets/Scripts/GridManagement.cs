using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManagement : MonoBehaviour
{

    public GameObject go; //sprite som printas
    public int nearbyLivingCells; //hur många levande celler som finns nära
    int maxX = 10; //x kolumn
    int maxY = 10; //y kolumn

    Cell[,] Grid; //gör en jagged array som tar in typen Cell

    void Start()
    {
        Grid = new Cell[maxX, maxY];
        Application.targetFrameRate = 10;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) //ritar ut gridden när backspace trycks ner
        {
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    Grid[i, j] = new Cell(i, j, Instantiate(go));
                }
            }
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    CheckAliveCells(i, j);
                    Grid[i, j].howManyNeighbour = nearbyLivingCells;
                    Debug.Log(Grid[i,j].howManyNeighbour);
                    nearbyLivingCells = 0;

                }
            }

        }
    }
    void CheckAliveCells(int x, int y) //funktion som kollar hur många levande celler som finns i närheten
    {
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (i == 0 && j == 0) 
                {
                continue;

                }

                if (x + i >= maxX || x + i < 0)
                {
                     continue;
                }

                if (y + j >= maxY || y + j < 0) 
                {
                     continue;
                }
               

                if (Grid[x + i, y + j].isAlive == true)
                {
                    nearbyLivingCells++; //plussa på för varje levande cell
                }
            }
        }
        void cellLive(bool willDie)
        {

        }
    }



    public class Cell //cell klass
    {
        public bool isAlive = true; //kollar ifall cellen lever eller är död
        public int howManyNeighbour = 0;
        public Cell(float x, float y, GameObject sprite) //konstruktor som tar in x och y koordinat samt ett gameobjekt
        {
            isAlive = true; //sätt att cellen är död som standard
            sprite.transform.position = new Vector3(x, y, 0); //sätt spritens position

        }
    }
}
