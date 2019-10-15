using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int health = 100;

    [HideInInspector]
    public GameManager gm;
    [HideInInspector]
    public TaskController task;
    [HideInInspector]
    public SatietyController satiety;
    [HideInInspector]
    public MovementController movement;
    [HideInInspector]
    public Interactor interactor;

    [HideInInspector]
    public TreeController tree;
    [HideInInspector]
    public FoodController food;

    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        gm = GameManager.instance;
        gm.objectsInWorld.Add(this);

        rb = GetComponent<Rigidbody>();
        satiety = GetComponent<SatietyController>();
    }

    public void DestroyObject()
    {
        gm.objectsInWorld.Remove(this);

        Destroy(gameObject);
    }
}