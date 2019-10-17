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
    Animator anim;

    bool lerpToPosition = false;
    Vector3 targetPosition;

    private void Awake()
    {
        hc = GetComponent<HealthController>();
        hc.food = this;
        gm = GameManager.instance;
        gm.food.Add(this);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (spoilTime > 0) spoilTime -= Time.deltaTime;
        else
        {
            gm.food.Remove(this);
            hc.DestroyObject();
        }

        if (lerpToPosition)
        {
            hc.rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, 0.1f));
        }
    }

    public IEnumerator Eat(HealthController c)
    {
        character = c;
        hc.rb.isKinematic = true;

        targetPosition = transform.position + Vector3.up * 1.5f;
        lerpToPosition = true;

        anim.SetBool("Eat", true);

        yield return new WaitForSeconds(eatTime);
        character.satiety.FillSatiety(satiety);
        character.task.TaskComplete();
        gm.food.Remove(this);
        hc.DestroyObject();
    }
}