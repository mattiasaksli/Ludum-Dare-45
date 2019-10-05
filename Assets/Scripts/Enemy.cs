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
    Animation anim;

    public enum enemyClass
    {
        Basic,
        Thicc,
        Assassin
    }
    void Start()
    {
        anim = this.GetComponent<Animation>();
        controller = this.GetComponentInParent<EnemyController>();
    }

    public void Damage(float hp)
    {
        this.health -= hp;
        anim.Play("Damage");
    }
}
