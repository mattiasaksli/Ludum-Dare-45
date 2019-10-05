using Doozy.Engine.UI;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float maxMasterHealth;
    public float masterHealth;
    public float allyRadius = 6f;
    public Ally[] allyPrefabs;
    List<Ally> allies = new List<Ally>();

    private float angle = Mathf.PI / 3;
    void Start()
    {
        Debug.Log("ALLYCONTROLLER START");
        PositionAllies();
        Debug.Log("ALLIES IN PLACE");
    }


    void Update()
    {

    }

    public void PositionAllies()
    {
        Dictionary<int, Ally.allyClass> allyAndIndex = loadAllyPosition();
        foreach (KeyValuePair<int, Ally.allyClass> pair in allyAndIndex)
        {
            Debug.Log(pair);
            int allyIndex = pair.Key;
            int allyClass = (int)pair.Value;

            float allyAngle = angle * allyIndex;
            float x = Mathf.Sin(allyAngle) * allyRadius;
            float z = Mathf.Cos(allyAngle) * allyRadius;

            Ally ally = Instantiate(allyPrefabs[allyClass], transform, this);
            UICanvas canvas = ally.GetComponentInChildren<UICanvas>();
            Debug.Log(canvas);
            canvas.CanvasName = "Ally" + allyIndex * 100;
            canvas.name = canvas.CanvasName;

            ally.transform.localPosition = new Vector3(x, 0, z);
            ally.transform.localRotation = Quaternion.Euler(0, allyAngle * (180f / Mathf.PI), 0);
            ally.sectorIndex = allyIndex;
            allies.Add(ally);
        }
    }

    public Dictionary<int, Ally.allyClass> loadAllyPosition()
    {
        Debug.Log("IN LOADPOSITIONS");
        Dictionary<int, Ally.allyClass> allyAndIndex = new Dictionary<int, Ally.allyClass>
        {
            { 1, Ally.allyClass.Knight },
            { 3, Ally.allyClass.Mage },
            { 5, Ally.allyClass.Priest },
            { 0, Ally.allyClass.Mage },
            { 2, Ally.allyClass.Priest},
            { 4, Ally.allyClass.Knight}
        };

        foreach (var a in allyAndIndex)
        {
            Debug.Log(a);
        }
        Debug.Log("------------------------------------------------------------------------");
        return allyAndIndex;
    }
}
