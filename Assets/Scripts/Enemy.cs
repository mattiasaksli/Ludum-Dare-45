using Doozy.Engine.Progress;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float maxHealth = 40f;
    public float health = 40f;
    public int sectorIndex;
    public enemyClass unitType = 0;
    public EnemyController controller;
    public AllyController allies;
    public Progressor healthbar;

    public enum enemyClass
    {
        Basic = 0,
        Thicc = 1,
        Assassin = 2
    }
    void Start()
    {
        allies = GameObject.FindGameObjectWithTag("AllyController").GetComponentInChildren<AllyController>();
        controller = this.GetComponentInParent<EnemyController>();
        healthbar = GetComponentInChildren<Progressor>();
        healthbar.SetMax(maxHealth);
        health = maxHealth;
        healthbar.SetValue(health);
    }
    public void RoundStart()
    {

    }
    public void Damage(float hp)
    {
        this.health -= hp;
        healthbar.SetValue(health);
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
