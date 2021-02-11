using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : AutoCleanupSingleton<ScoreManager>
{
    [SerializeField] public int coins;
    [SerializeField] public float maxHealth = 10f;
    [SerializeField] public float minHealth = 5f;
    [SerializeField] public float currentHealth;
    [SerializeField] public bool isDead;
    [SerializeField] public List<int> grades = new List<int>();

}
