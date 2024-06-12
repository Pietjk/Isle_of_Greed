using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public int chests = 0;
    public int money = 0;
    public Vector3 shipPosition;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        shipPosition = transform.position;
    }
}

