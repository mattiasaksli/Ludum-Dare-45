using Doozy.Engine.Progress;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public float maxHealth = 50f;
    public float health = 50f;
    public int sectorIndex;
    public allyClass unitType = 0;
    public allyAttack attackType = 0;
    public AllyController controller;
    public EnemyController enemyController;
    public Progressor healthbar;
    public GameObject spell1Button;
    public GameObject spell2Button;
    public CombatMaster combatMaster;
    public int spell1Cd;
    public int spell2Cd;

    public bool isShielded;
    public enum allyClass
    {
        Knight = 0,
        Mage = 1,
        Priest = 2
    }
    public enum allyAttack
    {
        Basic = 0,
        Spell1 = 1,
        Spell2 = 2
    }
    void Start()
    {
        controller = this.GetComponentInParent<AllyController>();
        enemyController = GameObject.FindGameObjectWithTag("EnemyController").GetComponentInChildren<EnemyController>();
        combatMaster = GameObject.FindGameObjectWithTag("CombatMaster").GetComponent<CombatMaster>();
        controller = this.GetComponentInParent<AllyController>();
        healthbar = GetComponentInChildren<Progressor>();
        healthbar.SetMax(maxHealth);
        health = maxHealth;
        healthbar.SetValue(health);
    }
    public void RoundStart()
    {
        isShielded = false;
        attackType = allyAttack.Basic;
        if (health <= 0)
        {
            spell1Button.SetActive(false);
            spell2Button.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    public void Combat()
    {
        Attack();
    }
    public void Damage(float hp)
    {
        if (isShielded)
        {
            return;
        }
        else
        {
            this.health -= hp;
            healthbar.SetValue(health);
        }
    }
    public void Select(int spell)
    {
        if ((this.attackType == allyAttack.Spell1 && spell == 1) || (this.attackType == allyAttack.Spell2 && spell == 2))
        {
            this.attackType = allyAttack.Basic;
            return;
        }

        if (spell == 1)
        {
            if (spell1Cd < combatMaster.roundCount)
            {
                this.attackType = allyAttack.Spell1;
            }

        }
        else if (spell == 2)
        {
            if (spell2Cd < combatMaster.roundCount)
            {
                this.attackType = allyAttack.Spell2;
            }
        }
    }
    void BasicAttack(float dmg)
    {
        foreach (Enemy e in enemyController.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                e.Damage(dmg);
            }
        }
    }

    void Poison(float dmg)
    {
        foreach (Enemy e in enemyController.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                e.Poisoned(dmg);
            }
        }
    }

    void Debuff()
    {
        foreach (Enemy e in enemyController.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                e.Debuffed();
            }
        }
    }

    void Attack()
    {
        switch (unitType)
        {
            case allyClass.Knight:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        BasicAttack(20f);
                        break;
                    case allyAttack.Spell1:
                        isShielded = true;
                        spell1Cd = combatMaster.roundCount + 1;
                        break;
                    case allyAttack.Spell2:
                        BasicAttack(30f);
                        spell2Cd = combatMaster.roundCount + 2;
                        break;
                }
                break;
            case allyClass.Mage:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        BasicAttack(10f);
                        break;
                    case allyAttack.Spell1:
                        break;
                    case allyAttack.Spell2:
                        Poison(10f);
                        break;
                }
                break;
            case allyClass.Priest:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        BasicAttack(15f);
                        break;
                    case allyAttack.Spell1:
                        break;
                    case allyAttack.Spell2:
                        Debuff();
                        break;
                }
                break;
        }
    }
}
