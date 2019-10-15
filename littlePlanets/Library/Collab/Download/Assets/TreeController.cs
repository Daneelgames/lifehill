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
            GrowFruits();
            fruitsGrowCooldownCurrent = Random.Range(fruitsGrowCooldownMin, fruitsGrowCooldownMax);
        }
    }

    void GrowFruits()
    {
        fruitsCurrent = Random.Range(fruitsMin, fruitsMax + 1);
        fruitsObject.SetActive(true);
    }

    public void Shake(HealthController character)
    {
        anim.SetTrigger("Shake");
        DropFruit(character);
    }

    void DropFruit(HealthController character)
    {
        // randomize drop
        if (Random.value > 0.5f)
        {
            HealthController fruit = Instantiate(fruitPrefab, fruitHolder.position, Quaternion.identity);
            fruitsCurrent--;
            
            if (Random.value > 0.05f)
            {
                fruit.rb.AddExplosionForce(Random.Range(100, 500), fruitHolder.position + new Vector3(Random.Range(-1, 1), -1, Random.Range(-1, 1)), 5);
            }
            else  // fruit falls on character's head
            {
                fruit.rb.MovePosition(character.transform.position + Vector3.up * fruitHolder.transform.position.y);
            }
        }

        if (fruitsCurrent > 0) Invoke("Shake", harvestTime);
    }
}