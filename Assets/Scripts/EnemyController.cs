using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Enemy[] enemies;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Combat()
    {
        foreach (Enemy e in enemies)
        {
            e.Combat();
        }
    }
    public void RoundStart()
    {
        foreach (Enemy e in enemies)
        {
            e.RoundStart();
        }
    }
}
