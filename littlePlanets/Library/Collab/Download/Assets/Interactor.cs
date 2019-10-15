using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    HealthController hc;
    GameManager gm;

    void Start()
    {
        hc = GetComponent<HealthController>();
        hc.interactor = this;

        gm = hc.gm;
    }

    public void InteractWithTarget(HealthController target)
    {
        if (target.tree)
        {
            if (hc.task.currentTask == TaskController.Task.FindFood)
            {
                target.tree.Shake(hc);
            }
        }
    }
}