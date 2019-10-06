using Doozy.Engine.UI;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Enemy[] enemyPrefabs;
    public List<Enemy> enemies = new List<Enemy>();
    public AllyController AC;
    public float enemyRadius;
    public Stack<Enemy> enemiesToRemove = new Stack<Enemy>();
    private float angle = Mathf.PI / 3;
    void Start()
    {
        AC = GameObject.FindGameObjectWithTag("AllyController").GetComponent<AllyController>();
        enemyRadius = AC.allyRadius + 2.5f;
        PositionEnemies();
    }

    void Update()
    {

    }
    public void Combat()
    {
        foreach (Enemy e in enemies)
        {
            e.Combat();
        }
    }
    public void RoundStart()
    {
        foreach (Enemy e in enemies)
        {
            e.RoundStart();
        }
    }

    public void PositionEnemies()
    {
        Dictionary<int, Enemy.enemyClass> enemyAndIndex = loadEnemyPosition();
        foreach (KeyValuePair<int, Enemy.enemyClass> pair in enemyAndIndex)
        {
            int enemyIndex = pair.Key;
            int enemyClass = (int)pair.Value;

            float enemyAngle = angle * enemyIndex;
            float x = Mathf.Sin(enemyAngle) * enemyRadius;
            float z = Mathf.Cos(enemyAngle) * enemyRadius;

            Enemy enemy = Instantiate(enemyPrefabs[enemyClass], transform, this);
            UICanvas canvas = enemy.GetComponentInChildren<UICanvas>();
            canvas.CanvasName = "Enemy--" + enemyIndex * 1000;
            canvas.name = canvas.CanvasName;

            enemy.transform.localPosition = new Vector3(x, 0, z);
            enemy.transform.localRotation = Quaternion.Euler(0, enemyAngle * (180f / Mathf.PI) + 180f, 0);
            enemy.sectorIndex = enemyIndex;
            enemy.unitType = (Enemy.enemyClass)enemyClass;
            enemies.Add(enemy);
        }
    }

    public Dictionary<int, Enemy.enemyClass> loadEnemyPosition()
    {
        Dictionary<int, Enemy.enemyClass> enemyAndIndex = new Dictionary<int, Enemy.enemyClass>
        {
            { 0, Enemy.enemyClass.Basic },
            { 1, Enemy.enemyClass.Basic },
            { 2, Enemy.enemyClass.Basic },
            { 3, Enemy.enemyClass.Basic },
            { 4, Enemy.enemyClass.Basic },
            { 5, Enemy.enemyClass.Basic },
        };

        return enemyAndIndex;
    }
}
