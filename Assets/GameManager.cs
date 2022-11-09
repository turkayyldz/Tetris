using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public MyBlockController Current { get; set; }


    private const int GridSizeX = 10;
    private const int GridSizeY = 20;

    public bool[,] Grid = new bool[GridSizeX, GridSizeY];

    
    public float GameSpeed => gameSpeed;

    [SerializeField, Range(.1f, 1f)] private float gameSpeed = 1f;

    
    [SerializeField] private List<MyBlockController> ListPrefabs;
    private List<MyBlockController> _listHistory = new List<MyBlockController>();
    

private void Awake()
    {
        Instance = this;
       

    }
    private void Start()
    {
       
        Spawn();
       
    }
    public bool IsInside(List<Vector2> listCoordinate)
    {
        foreach (var coordinate in listCoordinate)
        {
            int x = Mathf.RoundToInt(coordinate.x);
            int y = Mathf.RoundToInt(coordinate.y);

            if (x<0||x>=GridSizeX)
            {
                return false;
            }
            if (y < 0 || y >= GridSizeY)
            {
                return false;
            }
            if (Grid[x,y])
            {
                return false;
            }
        }
        return true;
    }
    public void Spawn()
    {
        var index = Random.Range(0, ListPrefabs.Count);
        var blockController = ListPrefabs[index];
        var newBlock=Instantiate(blockController);
        Current = newBlock;
        _listHistory.Add(newBlock);
       
    }

    private bool IsFullRow(int index)
    {
        for (int i = 0; i < GridSizeX; i++)
        {
            if (!Grid[i, index])
                return false;
        }
        return true;
    }
    public void UpdateRemoveObjectController()
    {
        for (int i = 0; i < GridSizeY; i++)
        {
            var isFull = IsFullRow(i);
            if (isFull)
            {
                //remove
                foreach (var myBlock in _listHistory)
                {
                    var willDestroy = new List<Transform>();
                    foreach (var piece in myBlock.ListPiece)
                    {
                        int y = Mathf.RoundToInt(piece.position.y);
                        if (y==i)
                        {
                            // Add Remove
                            willDestroy.Add(piece);
                            
                        }
                        else if (y>i)
                        {
                            //move down
                            var position = piece.position;
                            position.y--;
                            piece.position = position;
                        }
                    }
                    foreach (var item in willDestroy)
                    {
                        myBlock.ListPiece.Remove(item);
                        Destroy(item.gameObject);
                    }

                }
                //ChangeData
                for (int j = 0; j < GridSizeX; j++)
                {
                    Grid[j, i] = false;
                }
                for (int j = i+1; j < GridSizeY; j++)
                {
                    for (int k = 0; k < GridSizeX; k++)
                    {
                        Grid[k, j - 1] = Grid[k, j];
                    }
                }


                //call Again
                UpdateRemoveObjectController();
                return;

            }
        }
    }
}
