using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderController : MonoBehaviour
{
    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    BuildingController buildingPrefab;
    int woodPlaced = 0;
    int rockPlaced = 0;
    Vector3 buildingPosition;

    private void Start()
    {
        gm = GameManager.instance;

        hc = GetComponent<HealthController>();
        hc.builder = this;
    }

    public void ChooseBuildingPosition()
    {
        print("READY TO BUILD HOUSE");

        // choose place for building
        buildingPosition = Vector3.zero; // NEED TO CHANGE TO RANDOMIZED
        // TILE SYSTEM?

        BringMaterials();
    }

    void BringMaterials()
    {
        if (buildingPrefab.woodNeed > woodPlaced)
        {
            // find closest wood


        }
    }
}