using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] float gameTime = 1f;

    void Update()
    {
        transform.Rotate(new Vector3(gameTime * Time.deltaTime, 0, 0));
    }
}
