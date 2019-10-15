using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    HealthController hc;
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        gm.rocks.Add(this);
        hc = GetComponent<HealthController>();
    }
}