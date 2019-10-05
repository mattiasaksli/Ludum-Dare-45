using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public int sectorIndex;
    public allyClass unitType=0;
    public allyAttack attackType=0;
    public AllyController controller;
    Animation anim;
    public enum allyClass
    {
        Knight,
        Mage,
        Priest
    }
    public enum allyAttack
    {
        Basic,
        Spell1,
        Spell2
    }
    void Start()
    {
        anim = this.GetComponentInChildren<Animation>();
        controller = this.GetComponentInParent<AllyController>();
    }
    public void RoundStart()
    {

    }
    public void Combat()
    {

    }
    public void Damage(float hp)
    {

    }
    void Attack(int unit,int attack)
    {
        switch (unitType)
        {
            case allyClass.Knight:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        break;
                }
                break;
        }
    }
}
