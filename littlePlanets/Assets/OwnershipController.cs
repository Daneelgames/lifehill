using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnershipController : MonoBehaviour
{
    GameManager gm;
    HealthController hc;

    public List<HealthController> ownFood = new List<HealthController>();
    public List<HealthController> ownMaterials = new List<HealthController>();
    public List<HealthController> ownBuildings = new List<HealthController>();

    private void Start()
    {
        gm = GameManager.instance;
        hc = GetComponent<HealthController>();
        hc.ownership = this;
    }
}