using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaterial : MonoBehaviour
{
    GameManager gm;
    HealthController hc;

    private void Start()
    {
        gm = GameManager.instance;
        gm.buildMaterials.Add(this);

        hc = GetComponent<HealthController>();
        hc.buildMaterial = this;
    }
}
