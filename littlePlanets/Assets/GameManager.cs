using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int dayLenght = 300; //seconds

    public List<HealthController> objectsInWorld = new List<HealthController>();

    public List<TreeController> trees = new List<TreeController>();
    public List<RockController> rocks = new List<RockController>();
    public List<FoodController> food = new List<FoodController>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
