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
        PositionAllies();
        masterHealthBar = GetComponent<Progressor>();
        masterHealthBar.SetMax(maxMasterHealth);
        masterHealthBar.SetValue(masterHealth);
        CM = GameObject.FindGameObjectWithTag("CombatMaster").GetComponent<CombatMaster>();
        camAnim = GetComponent<Animator>();
        camAnim.SetInteger("sweep", -1);
        camAnim.SetBool("roundEnd", false);
        EC = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        camRig = GameObject.FindGameObjectWithTag("CamRig").GetComponent<CinemachineStateDrivenCamera>();
    }


    void Update()
    {

    }

    public void PositionAllies()
    {
        allies = loadAllyPosition();
        foreach (Ally a in allies)
        {
            int allyIndex = a.sectorIndex;
            int allyClass = (int)a.unitType;

            float allyAngle = angle * allyIndex;
            float x = Mathf.Sin(allyAngle) * allyRadius;
            float z = Mathf.Cos(allyAngle) * allyRadius;

            Ally ally = Instantiate(allyPrefabs[allyClass], transform, this);
            UICanvas canvas = ally.GetComponentInChildren<UICanvas>();
            canvas.CanvasName = "Ally--" + allyIndex * 100;
            canvas.name = canvas.CanvasName;

            ally.transform.localPosition = new Vector3(x, 0, z);
            ally.transform.localRotation = Quaternion.Euler(0, allyAngle * (180f / Mathf.PI), 0);
            allies.Add(ally);
        }
    }

    public List<Ally> loadAllyPosition()
    {
        List<Ally> allyList = new List<Ally>(6);
        Ally a3 = allyPrefabs[2];
        Ally a1 = allyPrefabs[0];
        a3.sectorIndex = 3;
        a1.sectorIndex = 1;
        allyList.Add(a1);
        allyList.Add(a3);

        return allyList;
    }
    public void Combat()
    {
        StartCoroutine(StartCombat());
    }
    IEnumerator StartCombat()
    {

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
        EC.Combat();
        yield return new WaitForSeconds(2f);
        if (masterHealth <= 0 || EC.enemies.Count == 0)
        {
            CM.EndEncounter();
            StopCoroutine("StartCombat");
        }
        EC.Combat();
        yield return new WaitForSeconds(1f);
        camAnim.SetBool("roundEnd", false);
        yield return new WaitForSeconds(1f);
        CM.RoundStart();
    }
    public void DamageMaster(float dmg)
    {
        masterHealth -= dmg;
        masterHealthBar.SetValue(masterHealth);
        elder.Play("Damage");
    }
    public void RoundStart()
    {
        if (masterHealth <= 0)
        {
            elder.Play("Death");
            Application.Quit();
        }
        foreach (Ally a in allies)
        {
            a.RoundStart();
        }
        EC.RoundStart();
    }
}
