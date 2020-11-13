using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public bool matched = false;

    public int targetX;
    public int targetY;

    public int previousRow;
    public int previousColumn;

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    private MatchFinder matchFinder;
    private Board board;
    private Dot otherDot;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;
    private bool anim = false;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        matchFinder = FindObjectOfType<MatchFinder>();
        //targetX = (int) transform.position.x;
        //targetY = (int) transform.position.y;
        //column = targetX;
        //row = targetY;
        //previousRow = row;
        //previousColumn = column;
    }

    private void Update()
    {
        if (matched)
        {
            if (!anim)
            {
                GameObject particle = Instantiate(board.destroyEffect, transform.position, Quaternion.identity);
                anim = true;
            }
            else
            {
                transform.localScale = Vector2.Lerp(transform.localScale, Vector3.zero, 0.1f);
            }
        }

        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.2f);
            if (board.allDots[column, row] != this)
            {
                board.allDots[column, row] = this;
            }

            matchFinder.FindAllMatches();
        }
        else
        {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
        }

        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.2f);
            if (board.allDots[column, row] != this)
            {
                board.allDots[column, row] = this;
            }

            matchFinder.FindAllMatches();
        }
        else
        {
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
            board.allDots[column, row] = this;
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.Move)
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void OnMouseUp()
    {
        if (board.currentState == GameState.Move)
        {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    private void CalculateAngle()
    {
        if (Math.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist ||
            Math.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 /
                         Mathf.PI;
            MovePieces();
            board.currentState = GameState.Wait;
        }
        else
        {
            board.currentState = GameState.Move;
        }
    }

    private void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //right swipe
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //up swipe
            otherDot = board.allDots[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //left swipe
            otherDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down swipe
            otherDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }

    private void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            Dot leftDot = board.allDots[column - 1, row];
            Dot rightDot = board.allDots[column + 1, row];
            if (leftDot != null && rightDot != null)
            {
                if (Compare(leftDot) && Compare(rightDot))
                {
                    leftDot.matched = true;
                    rightDot.matched = true;
                    matched = true;
                }
            }
        }

        if (row > 0 && row < board.height - 1)
        {
            Dot upDot = board.allDots[column, row + 1];
            Dot downDot = board.allDots[column, row - 1];
            if (upDot != null && downDot != null)
            {
                if (Compare(upDot) && Compare(downDot))
                {
                    upDot.matched = true;
                    downDot.matched = true;
                    matched = true;
                }
            }
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(0.5f);
        if (otherDot != null)
        {
            if (!matched && !otherDot.matched)
            {
                otherDot.row = row;
                otherDot.column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(0.5f);
                board.currentState = GameState.Move;
            }
            else
            {
                board.DestroyMatches();
            }

            otherDot = null;
        }
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public bool Compare(Dot dot)
    {
        return GetComponent<SpriteRenderer>().color == dot.GetComponent<SpriteRenderer>().color;
    }

    public bool Compare(Color color)
    {
        return GetComponent<SpriteRenderer>().color == color;
    }
}