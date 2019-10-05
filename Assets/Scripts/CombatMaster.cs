using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMaster : MonoBehaviour
{
    public EnemyController enemies;
    public AllyController allies;
    public int roundCount;
    void Start()
    {
        roundCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left shift"))
        {
            allies.Combat();
            allies.RoundStart();
            enemies.RoundStart();
            roundCount += 1;
        }
    }
}
