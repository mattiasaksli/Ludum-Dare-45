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
        anim = this.GetComponent<Animation>();
        controller = this.GetComponentInParent<AllyController>();
    }
    public void RoundStart()
    {

    }
    public void Combat()
    {
        Attack();
    }
    public void Damage(float hp)
    {

    }
    public void Select(int spell)
    {
        if ((this.attackType==allyAttack.Spell1 && spell == 1)||(this.attackType == allyAttack.Spell2 && spell == 2))
        {
            this.attackType = allyAttack.Basic;
            return;
        }
        if (spell == 1){
            this.attackType = allyAttack.Spell1;
        }
        else if(spell == 2)
        {
            this.attackType = allyAttack.Spell2;
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
                        break;
                    case allyAttack.Spell1:
                        break;
                    case allyAttack.Spell2:
                        break;
                }
                break;
            case allyClass.Mage:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        break;
                    case allyAttack.Spell1:
                        break;
                    case allyAttack.Spell2:
                        break;
                }
                break;
            case allyClass.Priest:
                switch (attackType)
                {
                    case allyAttack.Basic:
                        break;
                    case allyAttack.Spell1:
                        break;
                    case allyAttack.Spell2:
                        break;
                }
                break;
        }
    }
}
