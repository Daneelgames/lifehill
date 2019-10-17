using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    bool build = false;

    public int amountOfSessions = 3; 
    public float sessionTime = 40; // seconds
    float sessionTimeMax = 40;
    public int woodNeed = 1;
    public int rockNeed = 1;

    BuilderController builder;

    Animator anim;

    List<BuildMaterial> materials = new List<BuildMaterial>();

    private void Awake()
    {
        hc = GetComponent<HealthController>();
        hc.building = this;

        gm = GameManager.instance;
        gm.buildings.Add(this);

        anim = GetComponent<Animator>();

        sessionTimeMax = sessionTime;
    }

    public void AddMaterial(BuildMaterial bm)
    {
        materials.Add(bm);
    }

    private void Update()
    {
        if (build)
        {
            if (sessionTime > 0)
                sessionTime -= Time.deltaTime;
            else
            {
                build = false;
                sessionTime = sessionTimeMax;
                amountOfSessions--;

                if (amountOfSessions <= 0)
                {
                    builder.buildingInConstruction = null;
                    builder.CompleteBuilding();
                    CompleteBuilding();
                }

                builder.hc.task.TaskComplete();
                builder = null;
            }
        }
    }

    public void StartSession(BuilderController b)
    {
        builder = b;
        build = true;
        anim.SetTrigger("StartSession");
    }

    void CompleteBuilding()
    {
        for (int i = materials.Count - 1; i >= 0; i --)
        {
            materials[i].DestroyObject();
        }
        builder.hc.ownership.ownBuildings.Add(hc);
    }
}