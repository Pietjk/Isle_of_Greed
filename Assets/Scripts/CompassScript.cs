using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassScript : MonoBehaviour
{
    [Header("Required object")]
    [SerializeField] private Camera compassCamera;
    [SerializeField] private GameObject ship;

    // Update is called once per frame
    void Update()
    {
        compassCamera.transform.rotation = ship.transform.rotation;
    }
}
