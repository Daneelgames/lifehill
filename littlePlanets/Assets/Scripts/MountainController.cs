using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainController : MonoBehaviour
{
    [HideInInspector] public HealthController hc;
    [HideInInspector] public BuildMaterialSource bms;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        gm.mountains.Add(this);
        hc = GetComponent<HealthController>();
        hc.mountain = this;
    }
}
