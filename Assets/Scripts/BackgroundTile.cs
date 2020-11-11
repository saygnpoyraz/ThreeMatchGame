using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundTile : MonoBehaviour
{

    public GameObject[] dots;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int dotUse = Random.Range(0,dots.Length);
        GameObject dot = Instantiate(dots[dotUse], transform.position, Quaternion.identity,transform);
        dot.name = name;

    }
}
