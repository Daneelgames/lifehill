using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderController : MonoBehaviour
{
    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    private void Start()
    {
        gm = GameManager.instance;

        hc = GetComponent<HealthController>();
        hc.builder = this;
    }


}