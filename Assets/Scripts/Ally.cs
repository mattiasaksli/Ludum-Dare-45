using Doozy.Engine.Progress;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ally : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public int sectorIndex;
    public allyClass unitType;
    public allyAttack attackType;
    public AllyController AC;
    public EnemyController EC;
    public Progressor healthbar;
    public Image spell1;
    public Image spell2;
    public Image spell1Background;
    public Image spell2Background;
    public CombatMaster CM;
    public int spell1Cd;
    public int spell2Cd;
    public bool isShielded;
    public Animator anim;
    public AudioSource audio;
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
        AC = this.GetComponentInParent<AllyController>();
        anim = GetComponentInChildren<Animator>();
        spell1.color = Color.black;
        spell2.color = Color.black;
        EC = GameObject.FindGameObjectWithTag("EnemyController").GetComponentInChildren<EnemyController>();
        CM = GameObject.FindGameObjectWithTag("CombatMaster").GetComponent<CombatMaster>();
        AC = this.GetComponentInParent<AllyController>();
        healthbar = GetComponentInChildren<Progressor>();
        healthbar.SetMax(maxHealth);
        health = maxHealth;
        healthbar.SetValue(health);
        audio = GetComponent<AudioSource>();
        audio.clip = CM.audioClips[0];
        audio.Play();
    }
    public void RoundStart()
    {
        spell1.color = Color.black;
        spell1Background.color = Color.black;
        spell2.color = Color.black;
        spell2Background.color = Color.black;
        spell1Background.fillAmount = 1f;
        spell2Background.fillAmount = 1f;

        if (spell1Cd > CM.roundCount)
        {
            spell1Background.color = Color.gray;
            spell1Background.fillAmount = 0.5f;
        }
        if (spell2Cd > CM.roundCount)
        {
            spell2Background.color = Color.gray;
            spell2Background.fillAmount = (spell2Cd - CM.roundCount) / 3f;
        }
        if (isShielded)
        {
            anim.SetBool("isShielded", false);
            isShielded = false;
        }
        attackType = allyAttack.Basic;
    }
    public void Combat()
    {
        Attack();
    }
    public void Damage(float hp)
    {
        if (isShielded)
        {
            anim.Play("ShieldIdle");
            audio.clip = CM.audioClips[11];
            audio.Play();
            return;
        }
        else
        {
            this.health -= hp;
            StartCoroutine(DoDamage());
        }
    }
    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(0.7f);
        if (health <= 0)
        {
            Death();
        }
        else
        {
            anim.Play("Damage");
        }
        yield return new WaitForSeconds(0.5f);
        healthbar.SetValue(health);
    }
    public void Select(int spell)
    {
        if (!CM.inputDisable)
        {
            if ((this.attackType == allyAttack.Spell1 && spell == 1) || (this.attackType == allyAttack.Spell2 && spell == 2))
            {
                this.attackType = allyAttack.Basic;
                spell1.color = Color.black;
                spell2.color = Color.black;
                return;
            }

            if (spell == 1)
            {
                if (spell1Cd < CM.roundCount)
                {
                    this.attackType = allyAttack.Spell1;
                    spell1.color = Color.white;
                    spell2.color = Color.black;
                }
            }
            else if (spell == 2)
            {
                if (spell2Cd < CM.roundCount)
                {
                    this.attackType = allyAttack.Spell2;
                    spell2.color = Color.white;
                    spell1.color = Color.black;
                }
            }
        }
    }
    void BasicAttack(float dmg, AudioClip clip)
    {
        anim.Play("Attack");
        audio.clip = clip;
        audio.Play();

        foreach (Enemy e in EC.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                e.Damage(dmg);
            }
        }
    }

    void Poison(float dmg)
    {
        anim.Play("Cast");

        audio.clip = CM.audioClips[1];
        audio.Play();

        foreach (Enemy e in EC.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                e.Poisoned(dmg);
            }
        }
    }

    void Debuff()
    {
        anim.Play("Cast");

        audio.clip = CM.audioClips[3];
        audio.Play();

        foreach (Enemy e in EC.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                e.Debuffed();
            }
        }
    }

    void FireBall()
    {
        anim.Play("Cast");

        audio.clip = CM.audioClips[4];
        audio.Play();

        foreach (Enemy e in EC.enemies)
        {
            for (int i = -1; i < 2; i++)
            {
                int index = (sectorIndex + i) % 6;
                if (index == -1)
                {
                    index = 5;
                }
                if (index == e.sectorIndex)
                {
                    e.Damage(15f);
                }
            }
        }
    }

    void Heal()
    {
        anim.Play("Cast");

        audio.clip = CM.audioClips[2];
        audio.Play();

        foreach (Ally a in AC.allies)
        {
            a.health += a.maxHealth * 0.25f;
            if (a.health > a.maxHealth)
            {
                a.health = a.maxHealth;
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
                        BasicAttack(20f, CM.audioClips[10]);
                        break;
                    case allyAttack.Spell1:
                        anim.Play("Shield");

                        audio.clip = CM.audioClips[7];
                        audio.Play();

                        isShielded = true;
                        anim.SetBool("isShielded", true);
                        spell1Cd = CM.roundCount + 2;
                        break;
                    case allyAttack.Spell2:
                        BasicAttack(30f, CM.audioClips[6]);
                        spell2Cd = CM.roundCount + 3;
                        break;
                }
                break;
            case allyClass.Mage:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        BasicAttack(10f, CM.audioClips[9]);
                        break;
                    case allyAttack.Spell1:
                        FireBall();
                        spell1Cd = CM.roundCount + 2;
                        break;
                    case allyAttack.Spell2:
                        spell2Cd = CM.roundCount + 3;
                        Poison(10f);
                        break;
                }
                break;
            case allyClass.Priest:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        BasicAttack(15f, CM.audioClips[11]);
                        break;
                    case allyAttack.Spell1:
                        Heal();
                        spell1Cd = CM.roundCount + 2;
                        break;
                    case allyAttack.Spell2:
                        Debuff();
                        spell2Cd = CM.roundCount + 3;
                        break;
                }
                break;
        }
    }

    void Death()
    {
        AC.alliesToRemove.Push(this);
        anim.Play("Death");
        spell1.enabled = false;
        spell1Background.enabled = false;
        spell2.enabled = false;
        spell2Background.enabled = false;
        this.enabled = false;
    }
}
