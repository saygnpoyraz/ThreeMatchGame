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

    
    private Board board;
    private GameObject otherDot;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 tempPos;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int) transform.position.x;
        targetY = (int) transform.position.y;
        column = targetX;
        row = targetY;
        previousRow = row;
        previousColumn = column;
    }

    private void Update()
    {
        FindMatches();
        if (matched)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(0,0,0,0.2f);
        }
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        { 
            tempPos = new Vector2(targetX,transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.2f);
        }
        else
        {
            tempPos = new Vector2(targetX,transform.position.y);
            transform.position = tempPos;
            board.allDots[column, row] = gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        { 
            tempPos = new Vector2(transform.position.x,targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.2f);
        }
        else
        {
            tempPos = new Vector2(transform.position.x,targetY);
            transform.position = tempPos;
            board.allDots[column, row] = gameObject;
        }
       
    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
    }
    
    
    private void OnMouseUp()
    {
        finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) ;
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y,finalTouchPos.x-firstTouchPos.x) * 180 / Mathf.PI;
        MovePieces();
    }

    private void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //right swipe
            otherDot = board.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        }else  if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //up swipe
            otherDot = board.allDots[column, row +1 ];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        }else  if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //left swipe
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }else  if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down swipe
            otherDot = board.allDots[column , row-1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    private void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot = board.allDots[column - 1, row];
            GameObject rightDot = board.allDots[column + 1, row];
            if (CompareTag(leftDot.tag) && CompareTag(rightDot.tag))
            {
                leftDot.GetComponent<Dot>().matched = true;
                rightDot.GetComponent<Dot>().matched = true;
                matched = true;
            }
        }
        
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot = board.allDots[column, row+1];
            GameObject downDot = board.allDots[column, row-1];
            if (CompareTag(upDot.tag) && CompareTag(downDot.tag))
            {
                upDot.GetComponent<Dot>().matched = true;
                downDot.GetComponent<Dot>().matched = true;
                matched = true;
            }
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds (0.5f);
        if (otherDot != null)
        {
            if (!matched && !otherDot.GetComponent<Dot>().matched )
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            otherDot = null;
        }
    }
}
