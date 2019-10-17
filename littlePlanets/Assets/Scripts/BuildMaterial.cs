using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMaterial : MonoBehaviour
{
    public enum Type {Wood, Rock};
    public Type materialType = Type.Wood;
    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    public bool usedInBuilding = false;

    private void Awake()
    {
        gm = GameManager.instance;
        switch (materialType)
        {
            case Type.Rock:
                gm.rock.Add(this);
                break;

            case Type.Wood:
                gm.wood.Add(this);
                break;
        }

        hc = GetComponent<HealthController>();
        hc.buildMaterial = this;
    }

    public void DestroyObject()
    {
        switch (materialType)
        {
            case Type.Rock:
                gm.rock.Remove(this);
                break;

            case Type.Wood:
                gm.wood.Remove(this);
                break;
        }

        hc.DestroyObject();
    }
}