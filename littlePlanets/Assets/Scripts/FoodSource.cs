using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public int foodCurrent = 0;

    [HideInInspector]
    public HealthController characterWhoTargeted;

    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    [HideInInspector] public TreeController tree;

    private void Awake()
    {
        gm = GameManager.instance;
        hc = GetComponent<HealthController>();

        tree = GetComponent<TreeController>();
        if (tree)
            tree.fs = this;

        gm.foodSources.Add(this);
    }
}