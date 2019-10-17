using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public HealthController carriedObject;

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
                StartCoroutine(target.tree.Shake(hc));
            }
            else if (hc.task.currentTask == TaskController.Task.GatherMaterials)
            {
                StartCoroutine(target.tree.Chop(hc));
            }
        }
        else if (target.food)
        {
            if (hc.task.currentTask == TaskController.Task.Eat)
            {
                StartCoroutine(target.food.Eat(hc));
            }
        }
        else if (target.mountain)
        {

        }
        else if (target.buildMaterial)
        {
            if (hc.task.currentTask == TaskController.Task.Build)
            {
                // pick up material
                carriedObject = target;
                target.rb.isKinematic = true;
                target.transform.position += Vector3.up * 1.5f;
                target.transform.parent = transform;

                hc.task.CarryMaterial();
            }
        }
        else if (target.building)
        {
            if (hc.task.currentTask == TaskController.Task.Build)
            {
                if (carriedObject != null)
                {
                    carriedObject.transform.parent = null;
                    carriedObject.rb.isKinematic = false;
                    carriedObject.buildMaterial.usedInBuilding = true;
                    hc.builder.buildingInstance.AddMaterial(carriedObject.buildMaterial);

                    hc.builder.PlaceMaterial(carriedObject);
                    carriedObject = null;
                }
                else
                {
                    hc.builder.StartBuilding();
                }
            }
        }
    }
}