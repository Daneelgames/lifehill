﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int dayLenght = 300; //seconds

    public List<HealthController> objectsInWorld = new List<HealthController>();

    public List<BuildMaterialSource> buildMaterialSources = new List<BuildMaterialSource>();
    public List<FoodSource> foodSources = new List<FoodSource>();

    public List<TreeController> trees = new List<TreeController>();

    public List<BuildMaterial> wood = new List<BuildMaterial>();
    public List<BuildMaterial> rock = new List<BuildMaterial>();
    public List<FoodController> food = new List<FoodController>();

    public List<MountainController> mountains = new List<MountainController>();
    public List<BuildingController> buildings = new List<BuildingController>();

    [HideInInspector] public BuildingTree buildingTree;


    private void Awake()
    {
        instance = this;
        buildingTree = GetComponent<BuildingTree>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
