using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class GridManagement : MonoBehaviour
{

    public GameObject go; //sprite som printas
    //[HideInInspector] public int nearbyLivingCells; //hur många levande celler som finns nära


    [Range(0, 2)]
    public float tickRate;
    [Range(0, 5000)]
    public float cameraSize;
    private float tickValue = 0;
    [Range(0, 1)]
    public float cellAliveProbability;
    [SerializeField] public int maxX = 10; //x kolumn
    [SerializeField] public int maxY = 10; //y kolumn
    private Camera mainCamera;

    Cell[,] Grid; //gör en jagged array som tar in typen Cell

    void Start()
    {
        mainCamera = Camera.main;
        Grid = new Cell[maxX, maxY];
        Application.targetFrameRate = 60;
        {
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    Grid[i, j] = new Cell(i, j, Instantiate(go).GetComponent<SpriteRenderer>(), maxX, maxY);
                    if (Random.value >= cellAliveProbability)
                    {
                        Grid[i, j].disableCell();
                    }

                }
            }
        }
    }
    void Update()
    {
        mainCamera.orthographicSize = cameraSize;

        tickValue += Time.deltaTime;

        if (tickRate <= tickValue)
        {
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    Grid[i, j].howManyNeighbour = CheckAliveCells(i, j);
                }
            }
            for (int i = 0; i < maxX; i++)
            {
                for (int j = 0; j < maxY; j++)
                {
                    Grid[i, j].applyRules();
                }
            }
            tickValue = 0;
        }

    }
    int CheckAliveCells(int x, int y) //funktion som kollar hur många levande celler som finns i närheten
    {
        int nearbyLivingCells = 0;
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
        return nearbyLivingCells;

    }



    public class Cell //cell klass
    {
        public bool isAlive = true; //kollar ifall cellen lever eller är död
        public int howManyNeighbour = 0;
        public SpriteRenderer sprite;
        public void applyRules()
        {
            if (howManyNeighbour < 2 && isAlive)
            {
                disableCell();
            }
            if (howManyNeighbour > 3 && isAlive)
            {
                disableCell();
            }
            if (howManyNeighbour == 3 && !isAlive)
            {
                enableCell();
            }
        }
        public void disableCell()
        {
            sprite.enabled = false;
            isAlive = false;
        }
        public void enableCell()
        {
            sprite.enabled = true;
            isAlive = true;
        }
        public Cell(float x, float y, SpriteRenderer sprite, int maxX, int maxY) //konstruktor som tar in x och y koordinat samt ett gameobjekt
        {
            this.sprite = sprite;
            isAlive = true; //sätt att cellen är död som standard
            sprite.transform.position = new Vector3(-maxX / 2 + x, -maxY / 2 + y, 0); //sätt spritens position
        }
    }
}
