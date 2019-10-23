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

    public bool isPoisoned;
    public bool isDebuffed;
    public AudioSource audio;

    public ParticleSystem[] particles;

    public enum enemyClass
    {
        Basic = 0,
        Thicc = 1,
        Assassin = 2
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
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
        if (isPoisoned)
        {
            Damage(10f, 4);
        }
    }
    public void Damage(float hp, int animType)    //TODO: Pass in damage type for particle effects.
    {
        if (health > 0)
        {
            StartCoroutine(DoDamage(hp, animType));
        }
    }
    IEnumerator DoDamage(float hp, int animType)
    {
        this.health -= hp;
        yield return new WaitForSeconds(1f);
        if (health <= 0)
        {
            particles[1].Play();
            Death();
        }
        else
        {
            if (isDebuffed)
            {
                this.health -= hp;
                particles[5].Stop();
                particles[6].Play();
                isDebuffed = false;
            }
            audio.clip = CM.audioClips[17];
            audio.Play();
            anim.Play("Damage");
            particles[animType].Play();
        }
        healthbar.SetValue(health);
    }

    public void Poisoned(float hp)
    {
        Damage(hp, 2);
        Damage(0, 3);
        isPoisoned = true;
    }

    public void Debuffed()
    {
        particles[5].Play();
        isDebuffed = true;
    }
    public void Combat()
    {
        if (this.health <= 0)
        {
            Death();
            particles[1].Play();
            return;
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
                        audio.clip = CM.audioClips[14];
                        audio.Play();
                        a.Damage(15f);
                        hasOpponent = true;
                        break;
                    }
                }

                if (!hasOpponent)
                {
                    audio.clip = CM.audioClips[14];
                    audio.Play();
                    AC.DamageMaster(15f);
                }
                break;

            case enemyClass.Thicc:
                anim.Play("Attack");
                foreach (Ally a in AC.allies)
                {
                    if (a.sectorIndex == sectorIndex)
                    {
                        audio.clip = CM.audioClips[15];
                        audio.Play();
                        a.Damage(30f);
                        hasOpponent = true;
                        break;
                    }
                }
                if (!hasOpponent)
                {
                    audio.clip = CM.audioClips[15];
                    audio.Play();
                    AC.DamageMaster(30f);
                }
                break;
            case enemyClass.Assassin:
                anim.Play("Attack");
                audio.clip = CM.audioClips[16];
                audio.Play();
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
        isDebuffed = false;
        isPoisoned = false;
        particles[5].Stop();
        particles[3].Stop();
        anim.Play("Death");
        audio.clip = CM.audioClips[18];
        audio.Play();
        EC.enemiesToRemove.Push(this);
        this.enabled = false;
    }
}
