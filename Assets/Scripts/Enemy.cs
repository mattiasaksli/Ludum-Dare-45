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
    public CombatMaster CM;
    public Progressor healthbar;
    public int debuffActive;

    private bool isPoisoned;
    private bool isDebuffed;

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
        CM = GameObject.FindGameObjectWithTag("CombatMaster").GetComponentInChildren<CombatMaster>();
        healthbar = GetComponentInChildren<Progressor>();
        healthbar.SetMax(maxHealth);
        health = maxHealth;
        healthbar.SetValue(health);
        isPoisoned = false;
        isDebuffed = false;
    }
    public void RoundStart()
    {

    }
    public void Damage(float hp)    //TODO: Pass in damage type for particle effects.
    {
        this.health -= hp;
        if (isDebuffed)
        {
            this.health -= hp;
        }
        healthbar.SetValue(health);
    }

    public void Poisoned(float hp)
    {
        Damage(hp);
        isPoisoned = true;
    }

    public void Debuffed()
    {
        isDebuffed = true;
        debuffActive = CM.roundCount + 1;
    }
    public void Combat()
    {
        if (isPoisoned)
        {
            Damage(10f);
        }

        if (isDebuffed && debuffActive == CM.roundCount)
        {
            isDebuffed = false;
        }

        if (this.health <= 0)   //TODO: Add death animation.
        {
            gameObject.SetActive(false);
        }

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
