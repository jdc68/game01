using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    private static GameMaster _instance;

    public static GameMaster Instance
    {
        get
        {
            //create logic to create the instance
            if (_instance == null)
            {
                GameObject go = new GameObject("GameManager");
                go.AddComponent<GameMaster>();
            }

            return _instance;
        }
    }

    public float maxHealth;
    public float minHealth;
    public float currentHealth;
    public bool isDead;
    public List<int> grades = new List<int>();
    public int coins;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if (grades.Count == 0)
        {
            grades.Add(10);
            currentHealth = maxHealth;
        }
    }
}

