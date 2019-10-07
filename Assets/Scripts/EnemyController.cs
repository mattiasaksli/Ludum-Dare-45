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
        enemyRadius += AC.allyRadius;
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
        Dictionary<int, Enemy.enemyClass> enemyAndIndex = new Dictionary<int, Enemy.enemyClass>();
        int num = PlayerPrefs.GetInt("AllyNumber") - 1;
        Debug.Log(num);
        if (num < 5)
        {
            switch (num)
            {
                case 0:
                    enemyAndIndex.Add(0, Enemy.enemyClass.Basic);
                    break;
                case 1:
                    enemyAndIndex.Add(0, Enemy.enemyClass.Thicc);
                    break;
                case 2:
                    enemyAndIndex.Add(1, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(3, Enemy.enemyClass.Assassin);
                    break;
                case 3:
                    enemyAndIndex.Add(0, Enemy.enemyClass.Thicc);
                    enemyAndIndex.Add(2, Enemy.enemyClass.Thicc);
                    enemyAndIndex.Add(4, Enemy.enemyClass.Thicc);
                    break;
                case 4:
                    enemyAndIndex.Add(0, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(1, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(2, Enemy.enemyClass.Assassin);
                    enemyAndIndex.Add(3, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(5, Enemy.enemyClass.Thicc);
                    break;
                case 5:
                    enemyAndIndex.Add(0, Enemy.enemyClass.Assassin);
                    enemyAndIndex.Add(1, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(2, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(3, Enemy.enemyClass.Assassin);
                    enemyAndIndex.Add(4, Enemy.enemyClass.Basic);
                    enemyAndIndex.Add(5, Enemy.enemyClass.Basic);
                    break;
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                enemyAndIndex.Add(i, (Enemy.enemyClass)Random.Range(0, 3));
            }
        }
        return enemyAndIndex;
    }
}
