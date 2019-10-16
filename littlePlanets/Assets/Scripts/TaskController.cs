using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public enum Task {Null, FindFood, Eat, FindBuildMaterials, Build};
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
        //ChooseTask();
    }

    private void Update()
    {
        print(targetObject);
    }

    void ChooseTask()
    {
        Task newTask = Task.Null;
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

        // if character can BUILD
        if (hc.builder)
        {
            if (gm.buildMaterials.Count <= 0)
            {
                newTask = Task.FindBuildMaterials;
            }
            else
            {
                newTask = Task.Build;
            }
        }

        if (newTask != Task.Null) currentTask = newTask;
        else
        {
            Invoke("ChooseTask", 1);
            return;
        }

        switch (currentTask)
        {
            case Task.FindFood:
                FindFood();
                break;

            case Task.Eat:
                EatFood();
                break;

            case Task.FindBuildMaterials:
                FindBuildMaterials();
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
            hc.movement.Move(f.gameObject);
            StartCoroutine(GetDistanceToTarget());
        }
        else
        {
            Invoke("ChooseTask", 1);
        }
    }

    void FindBuildMaterials()
    {
        // find closest build material source
        if (gm.buildMaterialSources.Count > 0)
        {
            List<BuildMaterialSource> materialSourcesReady = new List<BuildMaterialSource>();
            foreach (BuildMaterialSource bms in gm.buildMaterialSources)
            {
                if (bms.materialsCurrent > 0 && !bms.characterWhoTargeted)
                {
                    materialSourcesReady.Add(bms);
                }
            }

            BuildMaterialSource closestSource = null;
            float distance = 100;

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

    void Build()
    {

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
            if (targetObject.bms && targetObject.bms.characterWhoTargeted != hc)
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