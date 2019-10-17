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
    public BuildingController buildingInstance;

    int woodPlaced = 0;
    int rockPlaced = 0;
    Vector3 buildingPosition;

    [HideInInspector]
    public BuildingController buildingInConstruction;

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
        buildingPrefab = gm.buildingTree.buildingsPrefabs[0];
        buildingPosition = Vector3.zero; // NEED TO CHANGE TO RANDOMIZED
        // TILE SYSTEM?
        buildingInstance = Instantiate(buildingPrefab, buildingPosition, Quaternion.identity);

        BringMaterials();
    }

    void BringMaterials()
    {
        if (buildingInstance.woodNeed > woodPlaced)
        {
            // find closest wood
            hc.task.PickUpMaterial(BuildMaterial.Type.Wood);
        }
        else if (buildingInstance.rockNeed > rockPlaced)
        {
            hc.task.PickUpMaterial(BuildMaterial.Type.Rock);
        }
    }

    public void PlaceMaterial(HealthController mat)
    {
        switch(mat.buildMaterial.materialType)
        {
            case BuildMaterial.Type.Rock:
                rockPlaced++;
                break;

            case BuildMaterial.Type.Wood:
                woodPlaced++;
                break;
        }
        if (woodPlaced >= buildingInstance.woodNeed && rockPlaced >= buildingInstance.rockNeed)
        {
            StartBuilding();
        }
        else
            BringMaterials();
    }

    public void StartBuilding()
    {
        print("START BUILDING");
        buildingInConstruction = buildingInstance;
        buildingInstance.StartSession(this);
    }

    public void CompleteBuilding()
    {
        rockPlaced = 0;
        woodPlaced = 0;
    }
}