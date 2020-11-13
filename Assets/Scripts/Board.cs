using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameState currentState = GameState.Move;
    public int width;
    public int height;
    public int offSet;

    public List<Color> colors;
    public Dot dotPrefab;
    
    public GameObject tilePrefab;
    public GameObject destroyEffect;

    public Dot[,] allDots;

    private BackgroundTile[,] allTiles;
    private MatchFinder matchFinder;
    
    // Start is called before the first frame update
    void Start()
    {
        matchFinder = FindObjectOfType<MatchFinder>();
        allTiles = new BackgroundTile[width,height];
        allDots = new Dot[width,height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 cellPos = new Vector2(i, j + offSet); 

                GameObject backgroundTiles = Instantiate(tilePrefab, cellPos, Quaternion.identity,transform);
                backgroundTiles.name = "( " + i + ", " + j + " )";
                
                int dotColor = Random.Range(0,colors.Count);
                int maxIterations = 0;

                while(MatchesAt(i, j, colors[dotColor]) && maxIterations < 100){
                    dotColor = Random.Range(0, colors.Count);
                    maxIterations++;
                }
                maxIterations = 0;
                
                Dot dot = Instantiate(dotPrefab, cellPos, Quaternion.identity,transform);
                dot.SetColor(colors[dotColor]);
                dot.row = j;
                dot.column = i;
                dot.name = "( " + i + ", " + j + " )";;
                allDots[i, j] = dot;
            }
        }
    }


    private bool MatchesAt(int column, int row, Color color){
        if(column > 1 && row > 1){
            if((allDots[column -1, row].Compare(color)) && (allDots[column -2, row].Compare(color))){
                return true;
            }
            if (allDots[column, row-1].Compare(color) && allDots[column, row-2].Compare(color))
            {
                return true;
            }

        }else if(column <= 1 || row <= 1){
            if(row > 1){
                if(allDots[column, row - 1].Compare(color) && allDots[column, row -2].Compare(color)){
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column-1, row].Compare(color) && allDots[column-2, row].Compare(color))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column,int row)
    {
        if (allDots[column,row].matched)
        {
            matchFinder.currentMatches.Remove(allDots[column, row]);
           
            Destroy(allDots[column,row]);
            allDots[column, row] = null;
        }
    }
    
    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i,j] != null)
                {
                    DestroyMatchesAt(i,j);
                }
            }
        }

        StartCoroutine(DecreaseRowCo());
    }
    
    private IEnumerator DecreaseRowCo(){
        int nullCount = 0;
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                if(allDots[i, j] == null){
                    nullCount++;
                }else if(nullCount > 0){
                    allDots[i, j].row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                if(allDots[i, j] == null){
                    Vector2 tempPos = new Vector2(i,j + offSet);
                    int dotColor = Random.Range(0, colors.Count);
                    Dot dot = Instantiate(dotPrefab, tempPos, Quaternion.identity,transform);
                    dot.SetColor(colors[dotColor]);
                    dot.name = "( " + i + ", " + j + " )";;
                    allDots[i, j] = dot;
                    dot.row = j;
                    dot.column = i;
                }
            }
        } 
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i ++){
            for (int j = 0; j < height; j ++){
                if(allDots[i, j] != null){
                    if (allDots[i,j].matched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.1f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(0.2f);
        currentState = GameState.Move;
    }
}
