using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject fruitsObject;
    public Transform fruitHolder;
    public HealthController fruitPrefab;
    //seconds
    float fruitsGrowCooldownCurrent = 0;
    public float fruitsGrowCooldownMin = 600; 
    public float fruitsGrowCooldownMax = 6000;

    public float harvestTime = 1;

    public int fruitsCurrent = 0;
    public int fruitsMin = 3;
    public int fruitsMax = 10;

    [HideInInspector] public HealthController hc;
    [HideInInspector] public Animator anim;
    GameManager gm;

    HealthController character;

    private void Start()
    {
        gm = GameManager.instance;
        gm.trees.Add(this);

        hc = GetComponent<HealthController>();
        hc.tree = this;

        anim = GetComponent<Animator>();
        anim.Play(0, -1, Random.value);

        fruitsObject.SetActive(false);
        if (Random.value > 0.5)
            fruitsGrowCooldownCurrent = Random.Range(0, fruitsGrowCooldownMax);
    }

    private void Update()
    {
        if (fruitsGrowCooldownCurrent > 0)
            fruitsGrowCooldownCurrent -= Time.deltaTime;
        else
        {
            fruitsGrowCooldownCurrent = Random.Range(fruitsGrowCooldownMin, fruitsGrowCooldownMax);
            GrowFruits();
        }
    }

    void GrowFruits()
    {
        fruitsCurrent = Random.Range(fruitsMin, fruitsMax + 1);
        fruitsObject.SetActive(true);
    }

    public IEnumerator Shake(HealthController c)
    {
        character = c;
        while(fruitsCurrent > 0)
        {
            anim.SetTrigger("Shake");
            DropFruit(c);
            yield return new WaitForSeconds(harvestTime);
        }
    }

    void DropFruit(HealthController c)
    {
        // randomize drop
        if (Random.value > 0.5f)
        {
            HealthController fruit = Instantiate(fruitPrefab, fruitHolder.position, Quaternion.identity);
            fruitsCurrent--;
            
            if (Random.value > 0.1f)
            {
                float x = 1;
                if (Random.value > 0.5f) x = -1;
                float z = 1;
                if (Random.value > 0.5f) z = -1;

                var explosionPosition = fruitHolder.position + new Vector3(x, -1, z);

                fruit.rb.AddExplosionForce(Random.Range(50, 500), explosionPosition, 5);
            }
            else  // fruit falls on character's head
            {
                fruit.rb.MovePosition(c.transform.position + Vector3.up * fruitHolder.transform.position.y);
            }
        }

        if (fruitsCurrent <= 0)
        {
            character.task.TaskComplete();
            fruitsObject.SetActive(false);
        }
    }
}