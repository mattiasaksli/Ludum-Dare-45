using Cinemachine;
using Doozy.Engine.Progress;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    public float maxMasterHealth;
    public float masterHealth;
    public float allyRadius = 6f;
    public Ally[] allyPrefabs;
    public List<Ally> allies = new List<Ally>();
    public CinemachineStateDrivenCamera camRig;
    public EnemyController EC;
    public CombatMaster CM;
    public Stack<Ally> alliesToRemove = new Stack<Ally>();
    public Animator camAnim;
    private float angle = Mathf.PI / 3;
    public Progressor masterHealthBar;
    public Animator elder;

    void Start()
    {
        masterHealthBar = GetComponent<Progressor>();
        masterHealthBar.SetMax(maxMasterHealth);
        masterHealthBar.SetValue(masterHealth);
        CM = GameObject.FindGameObjectWithTag("CombatMaster").GetComponent<CombatMaster>();
        camAnim = GetComponent<Animator>();
        camAnim.SetInteger("sweep", -1);
        camAnim.SetBool("roundEnd", false);
        EC = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        camRig = GameObject.FindGameObjectWithTag("CamRig").GetComponent<CinemachineStateDrivenCamera>();
        PositionAllies();
    }

    public void PositionAllies()
    {
        Dictionary<int, int> allyAndIndex = loadAllyPosition();
        foreach (KeyValuePair<int, int> pair in allyAndIndex)
        {
            int allyIndex = pair.Key;
            int allyClass = pair.Value;
            if (allyClass < 7)
            {
                Debug.Log("Creating ally at sector " + allyIndex + " With type " + allyClass);
                float allyAngle = angle * allyIndex;
                float x = Mathf.Sin(allyAngle) * allyRadius;
                float z = Mathf.Cos(allyAngle) * allyRadius;

                Ally ally = Instantiate(allyPrefabs[allyClass], transform, this);
                UICanvas canvas = ally.GetComponentInChildren<UICanvas>();
                canvas.CanvasName = "Ally--" + allyIndex * 1000;
                canvas.name = canvas.CanvasName;

                ally.transform.localPosition = new Vector3(x, 0, z);
                ally.transform.localRotation = Quaternion.Euler(0, allyAngle * (180f / Mathf.PI), 0);
                ally.sectorIndex = allyIndex;
                ally.unitType = (Ally.allyClass)allyClass;
                allies.Add(ally);
            }
        }
    }

    public Dictionary<int, int> loadAllyPosition()
    {
        Dictionary<int, int> allyAndIndex = new Dictionary<int, int>();

        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Loaded sector " + PlayerPrefs.GetInt("PreEncounterIndex" + i) + " With type " + PlayerPrefs.GetInt("PreEncounterType" + i));
            allyAndIndex[PlayerPrefs.GetInt("PreEncounterIndex" + i)] = PlayerPrefs.GetInt("PreEncounterType" + i);
        }
        return allyAndIndex;
    }
    public void Combat()
    {
        StartCoroutine(StartCombat());
    }
    IEnumerator StartCombat()
    {
        CM.skip = false;
        camAnim.SetBool("roundEnd", true);
        List<int> indexes = new List<int>();
        foreach (Ally a in allies)
        {
            indexes.Add(a.sectorIndex);
        }
        yield return new WaitForSeconds(1.5f);
        camAnim.SetInteger("sweep", 0);

        bool firstWait = true;
        for (int i = 0; i < 6; i++)
        {
            if (indexes.Contains(i))
            {
                if (CM.skip)
                {
                    if (firstWait)
                    {
                        camAnim.SetInteger("sweep", -1);
                        yield return new WaitForSeconds(1f);
                        firstWait = false;
                    }
                    foreach (Ally a in allies)
                    {
                        if (a.sectorIndex == i)
                        {
                            a.Combat();
                        }
                    }
                }
                else
                {
                    camAnim.SetInteger("sweep", i);
                    foreach (Ally a in allies)
                    {
                        if (a.sectorIndex == i)
                        {
                            yield return new WaitForSeconds(0.5f);
                            a.Combat();
                        }
                    }
                    yield return new WaitForSeconds(2f);
                }
            }
        }
        if (CM.skip)
        {
            yield return new WaitForSeconds(1.5f);
        }
        camAnim.SetInteger("sweep", -1);
        yield return new WaitForSeconds(1f);
        bool areEnemies = false;
        foreach (Enemy e in EC.enemies)
        {
            if (e.health > 0)
            {
                areEnemies = true;
            }
        }
        if (!areEnemies)
        {
            yield return new WaitForSeconds(1f);
            CM.EndEncounter();
        }
        else
        {
            EC.Combat();
            yield return new WaitForSeconds(2f);
            if (masterHealth <= 0)
            {
                elder.Play("Death");
                yield return new WaitForSeconds(1f);
                CM.EndEncounter();
            }
            else
            {
                yield return new WaitForSeconds(1f);
                camAnim.SetBool("roundEnd", false);
                yield return new WaitForSeconds(1f);
                CM.RoundStart();
            }
        }
    }
    public void DamageMaster(float dmg)
    {
        StartCoroutine(DamageMasterCo(dmg));
    }
    IEnumerator DamageMasterCo(float dmg)
    {
        elder.Play("Damage");
        yield return new WaitForSeconds(1f);
        masterHealth -= dmg;
        masterHealthBar.SetValue(masterHealth);
    }
    public void RoundStart()
    {
        foreach (Ally a in allies)
        {
            a.RoundStart();
        }
        EC.RoundStart();
    }
}
