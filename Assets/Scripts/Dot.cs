using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{

    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    public float swipeAngle = 0;


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
    }
}
