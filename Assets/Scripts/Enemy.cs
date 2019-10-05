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
        Basic = 0,
        Thicc = 1,
        Assassin = 2
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
