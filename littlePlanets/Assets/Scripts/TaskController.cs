using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public enum Task {Null, FindFood, Eat, GatherMaterials, Build};
    public Task currentTask = Task.Null;
    public HealthController targetObject;
    HealthController hc;
    GameManager gm;

    SatietyController satiety;

    void Start()
    {
        hc = GetComponent<HealthController>();
        hc.task = this;

        gm = hc.gm;

        if (hc.satiety)
            satiety = hc.satiety;

        Invoke("ChooseTask", 1);
    }

    void ChooseTask()
    {
        Task newTask = Task.Null;
        currentTask = Task.Null;
        targetObject = null;

        // if creature is HUNGRY
        if (satiety && satiety.satietyLevel < satiety.satietyLevelMax / 2)
        {
            // if there is no food
            if (gm.food.Count <= 0)
            {
                newTask = Task.FindFood;
            }
            else
            {
                newTask = Task.Eat;
            }
        }
        else if (hc.builder) // if character can BUILD
        {
            newTask = Task.Build;
        }

        if (newTask != Task.Null) currentTask = newTask;
        else
        {
            Invoke("ChooseTask", 1);
            return;
        }

        print(currentTask);

        switch (currentTask)
        {
            case Task.FindFood:
                FindFood();
                break;

            case Task.Eat:
                EatFood();
                break;

            case Task.Build:
                Build();
                break;
        }
    }

    void FindFood()
    {
        // find closest food source
        if (gm.foodSources.Count > 0)
        {
            List<FoodSource> foodSourcesReady = new List<FoodSource>();
            foreach(FoodSource fs in gm.foodSources)
            {
                if (fs.foodCurrent > 0 && !fs.characterWhoTargeted)
                {
                    foodSourcesReady.Add(fs);
                }
            }

            FoodSource closestSource = null;
            float distance = 100;

            if (foodSourcesReady.Count > 0)
            {
                // find closest tree with fruits
                foreach (FoodSource fs in foodSourcesReady)
                {
                    float newDistance = Vector3.Distance(fs.transform.position, transform.position);
                    if (newDistance <= distance)
                    {
                        distance = newDistance;
                        closestSource = fs;
                    }
                }

                if (closestSource != null)
                {
                    closestSource.characterWhoTargeted = hc;
                    targetObject = closestSource.hc;
                    hc.movement.Move(closestSource.gameObject);
                    StartCoroutine(GetDistanceToTarget());
                }
                else
                {
                    Invoke("ChooseTask", 1);
                }
            }
        }
    }

    void EatFood()
    {
        FoodController f = null;
        float distance = 100;

        foreach (FoodController food in gm.food)
        {
            float newDistance = Vector3.Distance(food.transform.position, transform.position);
            if (newDistance <= distance)
            {
                distance = newDistance;
                f = food;
            }
        }

        if (f != null)
        {
            targetObject = f.hc;

            if (!f.hc.owner)
            {
                f.hc.NewOwner(hc);
            }

            hc.movement.Move(f.gameObject);
            StartCoroutine(GetDistanceToTarget());
        }
        else
        {
            Invoke("ChooseTask", 1);
        }
    }

    void Build()
    {
        if (hc.builder.buildingInConstruction == null)
        {
            if (hc.ownership.ownBuildings.Count == 0)
            {
                if (gm.buildingTree.buildingsPrefabs[0].woodNeed > gm.wood.Count)
                {
                    FindBuildMaterials(BuildMaterial.Type.Wood);
                }
                else if (gm.buildingTree.buildingsPrefabs[0].rockNeed > gm.rock.Count)
                {
                    FindBuildMaterials(BuildMaterial.Type.Rock);
                }
                else
                {
                    //there are enough materials

                    hc.builder.ChooseBuildingPosition();
                }
            }
        }
        else // continue construction
        {
            targetObject = hc.builder.buildingInConstruction.hc;
            hc.movement.Move(targetObject.gameObject);
            StartCoroutine(GetDistanceToTarget());
        }
    }

    void FindBuildMaterials(BuildMaterial.Type type)
    {
        // find closest build material source
        if (gm.buildMaterialSources.Count > 0)
        {
            currentTask = Task.GatherMaterials;

            List<BuildMaterialSource> materialSourcesReady = new List<BuildMaterialSource>();
            foreach (BuildMaterialSource bms in gm.buildMaterialSources)
            {
                if (bms.materialsCurrent > 0 && !bms.hc.characterWhoTargeted && bms.materialType == type)
                {
                    materialSourcesReady.Add(bms);
                }
            }

            BuildMaterialSource closestSource = null;
            float distance = 1000;

            if (materialSourcesReady.Count > 0)
            {
                // find closest tree with fruits
                foreach (BuildMaterialSource bms in materialSourcesReady)
                {
                    float newDistance = Vector3.Distance(bms.transform.position, transform.position);
                    if (newDistance <= distance)
                    {
                        distance = newDistance;
                        closestSource = bms;
                    }
                }

                if (closestSource != null)
                {
                    closestSource.hc.characterWhoTargeted = hc;
                    targetObject = closestSource.hc;
                    hc.movement.Move(closestSource.gameObject);
                    StartCoroutine(GetDistanceToTarget());
                }
                else
                {
                    Invoke("ChooseTask", 1);
                }
            }
        }
    }

    public void PickUpMaterial(BuildMaterial.Type type)
    {
        BuildMaterial closestMaterial = null;
        float distance = 1000;

        List<BuildMaterial> materialsOnLevel = new List<BuildMaterial>();

        if (type == BuildMaterial.Type.Rock)
        {
            materialsOnLevel = new List<BuildMaterial>(gm.rock);
        }
        else if (type == BuildMaterial.Type.Wood)
        {
            materialsOnLevel = new List<BuildMaterial>(gm.wood);
        }

        foreach(BuildMaterial bm in materialsOnLevel)
        {
            if (!bm.usedInBuilding)
            {
                float newDistance = Vector3.Distance(transform.position, bm.transform.position);
                if (newDistance <= distance)
                {
                    distance = newDistance;
                    closestMaterial = bm;
                }
            }
        }

        if (closestMaterial != null)
        {
            closestMaterial.hc.characterWhoTargeted = hc;
            targetObject = closestMaterial.hc;
            hc.movement.Move(closestMaterial.gameObject);
            StartCoroutine(GetDistanceToTarget());
        }
        else
        {
            Invoke("ChooseTask", 1);
        }
    }

    public void CarryMaterial()
    {
        targetObject = hc.builder.buildingInstance.hc;
        hc.movement.Move(targetObject.gameObject);
        StartCoroutine(GetDistanceToTarget());
    }

    IEnumerator GetDistanceToTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (targetObject == null)
            {
                ChooseTask();
                break;
            }

            if (targetObject.fs && targetObject.fs.characterWhoTargeted != hc)
            {
                ChooseTask();
                break;
            }
            if (targetObject.bms && targetObject.bms.hc.characterWhoTargeted != hc)
            {
                ChooseTask();
                break;
            }

            if (hc.movement.agent.velocity.magnitude < 1f && Vector3.Distance(transform.position, targetObject.transform.position) <= 2)
            {
                hc.movement.agent.isStopped = true;
                if (hc.interactor)
                    hc.interactor.InteractWithTarget(targetObject);
                break;
            }
        }
    }

    public void TaskComplete()
    {
        Invoke("ChooseTask", 2);
    }
}