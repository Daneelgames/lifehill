using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    public int woodNeed = 1;
    public int rockNeed = 1;

    private void Awake()
    {
        hc = GetComponent<HealthController>();
        hc.building = this;

        gm = GameManager.instance;
        gm.buildings.Add(this);
    }
}
