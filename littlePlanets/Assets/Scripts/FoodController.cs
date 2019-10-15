using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public int satiety = 10;
    public float eatTime = 5;
    public float spoilTime = 600;

    GameManager gm;
    [HideInInspector]
    public HealthController hc;

    HealthController character;

    private void Start()
    {
        hc = GetComponent<HealthController>();
        hc.food = this;
        gm = hc.gm;
        gm.food.Add(this);
    }

    private void Update()
    {
        if (spoilTime > 0) spoilTime -= Time.deltaTime;
        else
        {
            gm.food.Remove(this);
            hc.DestroyObject();
        }
    }

    public IEnumerator Eat(HealthController c)
    {
        character = c;
        yield return new WaitForSeconds(eatTime);
        character.satiety.FillSatiety(satiety);
        character.task.TaskComplete();
    }
}