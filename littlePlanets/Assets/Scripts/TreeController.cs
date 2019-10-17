using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeController : MonoBehaviour
{
    public GameObject fruitsObject;
    public Transform fruitHolder;
    public HealthController fruitPrefab;
    //seconds
    float fruitsGrowCooldownCurrent = 0;
    public float fruitsGrowCooldownMin = 600; 
    public float fruitsGrowCooldownMax = 6000;

    public float chopTime = 1;
    public int chopAmount = 5;
    public int woodCurrent = 0;

    public float shakeTime = 1;

    public int fruitsCurrent = 0;
    public int fruitsMin = 3;
    public int fruitsMax = 10;

    [HideInInspector] public HealthController hc;
    [HideInInspector] public Animator anim;
    [HideInInspector] public FoodSource fs;
    [HideInInspector] public BuildMaterialSource bms;
    [HideInInspector] public NavMeshObstacle obstacle;

    GameManager gm;

    HealthController character;

    public List<HealthController> woods;
    public GameObject foliage;

    private void Start()
    {
        gm = GameManager.instance;
        gm.trees.Add(this);

        hc = GetComponent<HealthController>();
        hc.tree = this;

        anim = GetComponent<Animator>();
        anim.Play(0, -1, Random.value);

        obstacle = GetComponent<NavMeshObstacle>();

        fruitsObject.SetActive(false);
        if (Random.value > 0.5)
            fruitsGrowCooldownCurrent = Random.Range(0, fruitsGrowCooldownMax);

        fs.foodCurrent = fruitsCurrent;
        bms.materialsCurrent = woodCurrent;
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

        fs.foodCurrent = fruitsCurrent;
    }

    public IEnumerator Shake(HealthController c)
    {
        character = c;

        while(fruitsCurrent > 0)
        {
            anim.SetTrigger("Shake");
            DropFruit(c);
            yield return new WaitForSeconds(shakeTime);
        }
    }

    public IEnumerator Chop(HealthController c)
    {
        character = c;

        while (chopAmount > 0)
        {
            yield return new WaitForSeconds(chopTime);
            chopAmount--;
            anim.SetTrigger("Shake"); // replace to chop animation
        }

        TreeFall();
    }

    void DropFruit(HealthController c)
    {
        // randomize drop
        if (Random.value > 0.5f)
        {
            HealthController fruit = Instantiate(fruitPrefab, fruitHolder.position, Quaternion.identity);
            fruitsCurrent--;


            float x = 1;
            if (Random.value > 0.5f) x = -1;
            float z = 1;
            if (Random.value > 0.5f) z = -1;

            var explosionPosition = fruitHolder.position + new Vector3(x, -1, z);

            fruit.NewOwner(character);

            fruit.rb.AddExplosionForce(Random.Range(50, 500), explosionPosition, 5);

            /*
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
            */
        }

        fs.foodCurrent = fruitsCurrent;

        if (fruitsCurrent <= 0)
        {
            character.task.TaskComplete();
            fruitsObject.SetActive(false);
        }
    }

    void TreeFall()
    {
        if (fruitsCurrent > 0)
        {
            for (int i = 0; i < fruitsCurrent; i ++)
            {
                HealthController fruit = Instantiate(fruitPrefab, fruitHolder.position, Quaternion.identity);

                float x = 1;
                if (Random.value > 0.5f) x = -1;
                float z = 1;
                if (Random.value > 0.5f) z = -1;

                var explosionPosition = fruitHolder.position + new Vector3(x, -1, z);

                fruit.rb.AddExplosionForce(Random.Range(50, 200), explosionPosition, 5);

                fruit.NewOwner(character);
            }
            fruitsObject.SetActive(false);
        }
        float x2 = 1;
        if (Random.value > 0.5f) x2 = -1;
        float z2 = 1;
        if (Random.value > 0.5f) z2 = -1;

        var explosionPosition2 = fruitHolder.position + new Vector3(x2, -1, z2);

        //anim.SetTrigger("Fall");

        hc.rb.constraints = RigidbodyConstraints.None;
        hc.rb.AddExplosionForce(100, explosionPosition2, 5);

        gm.trees.Remove(this);
        gm.buildMaterialSources.Remove(bms);
        gm.foodSources.Remove(fs);

        character.task.TaskComplete();

        Invoke("DropLogs", 1);
    }

    void DropLogs()
    {
        foliage.transform.parent = null;
        foliage.SetActive(true);

        foreach(HealthController wood in woods)
        {
            wood.gameObject.SetActive(true);
            wood.transform.parent = null;

            wood.NewOwner(character);
        }

        hc.DestroyObject();
    }
}