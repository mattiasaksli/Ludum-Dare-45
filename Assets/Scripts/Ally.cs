using UnityEngine;

public class Ally : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public int sectorIndex;
    public allyClass unitType = 0;
    public allyAttack attackType = 0;
    public AllyController allyController;
    public EnemyController enemyController;
    public GameObject spell1Button;
    public GameObject spell2Button;
    Animation anim;

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
        anim = this.GetComponent<Animation>();
        allyController = this.GetComponentInParent<AllyController>();
        enemyController = this.GetComponentInParent<EnemyController>();
    }
    public void RoundStart()
    {
        isShielded = false;
        if (health <= 0)
        {
            anim.Play("Death");
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
        this.health -= hp;
        anim.Play("Damage");
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
            this.attackType = allyAttack.Spell1;
        }
        else if (spell == 2)
        {
            this.attackType = allyAttack.Spell2;
        }
    }
    void BasicAttack(float dmg)
    {
        foreach (Enemy e in enemyController.enemies)
        {
            if (e.sectorIndex == sectorIndex)
            {
                anim.Play("Attack");
                e.Damage(dmg);
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
                        anim.Play("Cast");
                        isShielded = true;
                        break;
                    case allyAttack.Spell2:
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
                        break;
                }
                break;
        }
    }
}
