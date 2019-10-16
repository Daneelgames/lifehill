using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public enum Task {Null, FindFood, Eat };
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
        ChooseTask();
    }

    void ChooseTask()
    {
        Task newTask = Task.Null;

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
        }
    }

    void FindFood()
    {
        // find fruit trees
        if (gm.trees.Count > 0)
        {
            List<TreeController> treesWithFruits = new List<TreeController>();
            foreach(TreeController tree in gm.trees)
            {
                if (tree.fruitsCurrent > 0)
                {
                    treesWithFruits.Add(tree);
                }
            }

            TreeController closestTree = null;
            float distance = 100;

            if (treesWithFruits.Count > 0)
            {
                // find closest tree with fruits
                foreach (TreeController t in treesWithFruits)
                {
                    float newDistance = Vector3.Distance(t.transform.position, transform.position);
                    if (newDistance <= distance)
                    {
                        distance = newDistance;
                        closestTree = t;
                    }
                }

                if (closestTree != null)
                {
                    targetObject = closestTree.hc;
                    hc.movement.Move(closestTree.gameObject);
                    StartCoroutine(GetDistanceToTarget());
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
    }

    IEnumerator GetDistanceToTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (targetObject == null) break;

            if (hc.movement.agent.velocity == Vector3.zero && Vector3.Distance(transform.position, targetObject.transform.position) <= 2)
            {
                if (hc.interactor)
                    hc.interactor.InteractWithTarget(targetObject);
                break;
            }
        }
    }

    public void TaskComplete()
    {

        ChooseTask();
    }
}