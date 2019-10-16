using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float t = 3;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, t);
    }
}