using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class GridManagement : MonoBehaviour
{

    public GameObject go; //sprite som printas
    [Range(0, 2)]
    public float tickRate;
    [Range(0, 5000)]
    public float cameraSize;
    private float tickValue = 0;
    public TextMeshProUGUI pauseText;
    [Range(0, 1)]
    public float cellAliveProbability;
    [SerializeField] public int maxX = 10; //x kolumn
    [SerializeField] public int maxY = 10; //y kolumn
    private Camera mainCamera; //kamera
    private bool pause = true; //bool för att pausa
    private Vector3 mousePos; //mus position
    private Vector3 worldPos; //musen position i världen
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
                    Grid[i, j].sprite.enabled = false;
                }
            }


        }
    }
    void Update()
    {
        mainCamera.orthographicSize = cameraSize;
        mousePos = Input.mousePosition;
        worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        tickValue += Time.deltaTime;

        if (tickRate <= tickValue && pause == false)
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
        if (Input.GetMouseButton(0) && pause == true)
        {
            Vector2Int gridPos = WorldPosToGridPos(worldPos);
            if (gridPos.x < 0 || gridPos.y < 0 || gridPos.x >= maxX || gridPos.y >= maxY)
            {
                Debug.Log("Out of bounds");
            }
            else
                Grid[gridPos.x, gridPos.y].enableCell();
        }

    }
    public Vector2Int WorldPosToGridPos(Vector3 mousePos)
    {
        mousePos.x -= -maxX / 2;
        mousePos.y -= -maxY / 2;
        return new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
    }
    public void PauseGame()
    {
        if (pause == false)
        {
            pause = true;
            pauseText.text = "Unpause";
        }
        else
        {
            pause = false;
            pauseText.text = "Pause";
        }
    }
    public void ClearAllCells()
    {
        for (int i = 0; i < maxX; i++)
        {
            for (int j = 0; j < maxY; j++)
            {
                Grid[i, j].disableCell();
            }
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
        public bool isAlive = false; //kollar ifall cellen lever eller är död

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
        public Cell(int x, int y, SpriteRenderer sprite, int maxX, int maxY) //konstruktor som tar in x och y koordinat samt ett gameobjekt
        {
            this.sprite = sprite;
            isAlive = true; //sätt att cellen är död som standard
            sprite.transform.position = new Vector3(-maxX / 2 + x, -maxY / 2 + y, 0); //sätt spritens position
        }
    }
}
