using Doozy.Engine.Progress;
using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float maxHealth = 40f;
    public float health = 40f;
    public int sectorIndex;
    public enemyClass unitType = 0;
    public EnemyController EC;
    public AllyController AC;
    public CombatMaster CM;
    public Progressor healthbar;
    public int debuffActive;
    public Animator anim;

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
        anim = GetComponentInChildren<Animator>();
        AC = GameObject.FindGameObjectWithTag("AllyController").GetComponentInChildren<AllyController>();
        EC = this.GetComponentInParent<EnemyController>();
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
        StartCoroutine(DoDamage());

        if (health <= 0)
        {
            Death();
        }
    }
    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.7f);
        anim.Play("Damage");
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

        if (this.health <= 0)
        {
            Death();
        }

        bool hasOpponent = false;

        switch (unitType)
        {
            case enemyClass.Basic:
                anim.Play("Attack");
                foreach (Ally a in AC.allies)
                {
                    if (a.sectorIndex == sectorIndex)
                    {
                        a.Damage(15f);
                        hasOpponent = true;
                        break;
                    }
                }

                if (!hasOpponent)
                {
                    AC.DamageMaster(15f);
                }
                break;

            case enemyClass.Thicc:
                anim.Play("Attack");
                foreach (Ally a in AC.allies)
                {
                    if (a.sectorIndex == sectorIndex)
                    {
                        a.Damage(30f);
                        hasOpponent = true;
                        break;
                    }
                }
                if (!hasOpponent)
                {
                    AC.DamageMaster(30f);
                }
                break;
            case enemyClass.Assassin:
                anim.Play("Attack");
                foreach (Ally a in AC.allies)
                {
                    if (a.sectorIndex == sectorIndex)
                    {
                        if (a.isShielded)
                        {
                            hasOpponent = true;
                        }
                        else
                        {
                            AC.DamageMaster(20f);
                            hasOpponent = true;
                        }
                        break;
                    }
                }
                if (!hasOpponent)
                {
                    AC.DamageMaster(15f);
                }
                break;
        }
    }

    void Death()
    {
        // TODO: Death animation here
        isDebuffed = false;
        isPoisoned = false;
        anim.Play("Death");
        EC.enemiesToRemove.Push(this);
        this.enabled = false;
        //Destroy(gameObject);
    }
}
