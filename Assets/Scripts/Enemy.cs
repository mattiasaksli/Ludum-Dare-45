using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public int sectorIndex;
    public enemyClass unitType = 0;
    public EnemyController controller;
    public AllyController allies;

    public enum enemyClass
    {
        Basic,
        Thicc,
        Assassin
    }
    void Start()
    {
        controller = this.GetComponentInParent<EnemyController>();
    }
    public void RoundStart()
    {

    }
    public void Damage(float hp)
    {
        this.health -= hp;
    }
    public void Combat()
    {
        switch (unitType)
        {
            case enemyClass.Basic:
                foreach (Ally a in allies.allies)
                {
                    if (a.sectorIndex == sectorIndex)
                    {
                        a.Damage(15f);
                    }
                }
                break;

        }
    }
}
