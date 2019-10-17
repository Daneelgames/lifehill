using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int health = 100;

    [HideInInspector]
    public GameManager gm;

    // character
    [HideInInspector]
    public TaskController task;
    [HideInInspector]
    public SatietyController satiety;
    [HideInInspector]
    public MovementController movement;
    [HideInInspector]
    public Interactor interactor;
    [HideInInspector]
    public BuilderController builder;
    [HideInInspector]
    public OwnershipController ownership;

    public HealthController owner;
    [HideInInspector]
    public HealthController characterWhoTargeted;

    //objects
    [HideInInspector]
    public TreeController tree;
    [HideInInspector]
    public FoodController food;
    [HideInInspector]
    public MountainController mountain;
    [HideInInspector]
    public BuildMaterial buildMaterial;
    [HideInInspector]
    public FoodSource fs;
    [HideInInspector]
    public BuildMaterialSource bms;
    [HideInInspector]
    public BuildingController building;

    [HideInInspector] public Rigidbody rb;

    private void Awake()
    {
        gm = GameManager.instance;
        gm.objectsInWorld.Add(this);

        rb = GetComponent<Rigidbody>();
        satiety = GetComponent<SatietyController>();
    }

    public void NewOwner(HealthController newOwner)
    {
        owner = newOwner;

        if (food)
            owner.ownership.ownFood.Add(this);
        if (buildMaterial)
            owner.ownership.ownMaterials.Add(this);
    }

    public void DestroyObject()
    {
        if (owner)
        {
            if (food)
                owner.ownership.ownFood.Remove(this);
            if (buildMaterial)
                owner.ownership.ownMaterials.Remove(this);
            if (building)
                owner.ownership.ownBuildings.Remove(this);
        }

        gm.objectsInWorld.Remove(this);

        Destroy(gameObject);
    }
}