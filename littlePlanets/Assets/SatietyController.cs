using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatietyController : MonoBehaviour
{
    public int satietyLevel = 100;
    [HideInInspector]
    public int satietyLevelMax = 100;

    HealthController hc;
    GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
        hc = GetComponent<HealthController>();

        satietyLevelMax = satietyLevel;

        StartCoroutine("Hunger");
    }

    IEnumerator Hunger()
    {
        while(true)
        {
            yield return new WaitForSeconds(2f);

            if (satietyLevel > 0)
                satietyLevel -= 1;

            if (hc.health <= 0) StopCoroutine("Hunger");
        }
    }

    public void FillSatiety(int amount)
    {
        satietyLevel += amount;
        if (satietyLevel > satietyLevelMax) satietyLevel = satietyLevelMax;
    }
}