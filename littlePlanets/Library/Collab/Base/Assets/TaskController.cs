using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskController : MonoBehaviour
{
    public enum Task {Null, FindFood, Eat };
    public Task currentTask = Task.Null;
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
        // if creature can feel hunger
        if (satiety)
        {
            // if there is no food
            if (gm.food.Count <= 0)
            {
                newTask = Task.FindFood;
            }
        }

        if (newTask != Task.Null) currentTask = newTask;
        else
        {
            Invoke("ChooseTask", 1);
            return;
        }

        if (currentTask == Task.FindFood)
        {
            FindFood();
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

                print("here");
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
                    hc.movement.Move(closestTree.gameObject);
                }
            }
        }
    }
}