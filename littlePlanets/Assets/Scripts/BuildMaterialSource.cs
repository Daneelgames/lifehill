using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaterialSource : MonoBehaviour
{
    public BuildMaterial.Type materialType = BuildMaterial.Type.Wood;
    public int materialsCurrent = 0;


    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    [HideInInspector] public TreeController tree;
    [HideInInspector] public MountainController mountain;

    private void Awake()
    {
        gm = GameManager.instance;
        hc = GetComponent<HealthController>();

        tree = GetComponent<TreeController>();
        if (tree)
            tree.bms = this;

        mountain = GetComponent<MountainController>();

        gm.buildMaterialSources.Add(this);
    }
}