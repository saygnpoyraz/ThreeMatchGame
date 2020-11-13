using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    public List<Dot> currentMatches; 
    
    private Board board;
    

    private void Start()
    {
        board = FindObjectOfType<Board>();
        currentMatches = new List<Dot>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < board.width; i ++){
            for (int j = 0; j < board.height; j ++)
            {
                Dot currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        Dot leftDot = board.allDots[i - 1, j];
                        Dot rightDot = board.allDots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            if (currentDot.Compare(leftDot) && currentDot.Compare(rightDot))
                            {
                                
                                if (!currentMatches.Contains(leftDot))
                                {
                                    currentMatches.Add(leftDot);
                                }
                                if (!currentMatches.Contains(rightDot))
                                {
                                    currentMatches.Add(rightDot);
                                }
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                leftDot.matched = true;
                                rightDot.matched = true;
                                currentDot.matched = true;
                                
                            }
                        }
                    }
                    
                    
                    if (j > 0 && j < board.height - 1)
                    {
                        Dot upDot = board.allDots[i, j+1];
                        Dot downDot = board.allDots[i, j-1];
                        if (upDot != null && downDot != null)
                        {
                            if (currentDot.Compare(upDot) && currentDot.Compare(downDot))
                            {
                                 
                                if (!currentMatches.Contains(upDot))
                                {
                                    currentMatches.Add(upDot);
                                }
                                if (!currentMatches.Contains(downDot))
                                {
                                    currentMatches.Add(downDot );
                                }
                                if (!currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                upDot.matched = true;
                                downDot.matched = true;
                                currentDot.matched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
