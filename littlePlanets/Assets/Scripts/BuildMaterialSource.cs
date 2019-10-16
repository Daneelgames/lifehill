using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaterialSource : MonoBehaviour
{
    [HideInInspector]
    public int materialsCurrent = 0;

    [HideInInspector]
    public HealthController characterWhoTargeted;

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