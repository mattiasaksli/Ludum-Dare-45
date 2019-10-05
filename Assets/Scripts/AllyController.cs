using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float maxMasterHealth;
    public float masterHealth;
    public GameObject[] allyPrefabs;
    public Ally[] allies;
    public EnemyController enemies;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Combat()
    {
        foreach (Ally a in allies)
        {
            a.Combat();
        }
        enemies.Combat();
    }
    public void RoundStart()
    {
        foreach (Ally a in allies)
        {
            a.RoundStart();
        }
        enemies.RoundStart();
    }
}
