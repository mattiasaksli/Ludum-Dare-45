using Cinemachine;
using Doozy.Engine.Progress;
using System.Collections;
using UnityEngine;

public class CombatMaster : MonoBehaviour
{
    public EnemyController EC;
    public AllyController AC;
    public int roundCount;
    public int roundTime = 20;
    public int currentTime;
    public bool turningLeft = false;
    public bool turningRight = false;
    private float yEulerAngle;
    public bool skip;
    public bool inputDisable;
    public Progressor timeBar;
    public CinemachineFreeLook freeLook;
    public bool inCombat = false;
    public bool endCombat = false;
    void Start()
    {
        inputDisable = false;
        turningLeft = false;
        turningRight = false;
        inCombat = false;
        roundCount = 1;
        timeBar = GetComponent<Progressor>();
        timeBar.SetMax(roundTime);
        timeBar.SetValue(0);
        EC = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        AC = GameObject.FindGameObjectWithTag("AllyController").GetComponent<AllyController>();
        freeLook = GameObject.FindGameObjectWithTag("FreeLook").GetComponent<CinemachineFreeLook>();
        StartCoroutine(Timer());
    }
    public void RoundStart()
    {
        timeBar.SetValue(0);
        timeBar.AnimationDuration = 0.5f;
        skip = false;
        inputDisable = false;
        inCombat = false;
        roundCount += 1;

        AC.RoundStart();
        EC.RoundStart();

        while (AC.alliesToRemove.Count > 0)
        {
            AC.allies.Remove(AC.alliesToRemove.Pop());
        }
        while (EC.enemiesToRemove.Count > 0)
        {
            EC.enemies.Remove(EC.enemiesToRemove.Pop());
        }

        StartCoroutine(Timer());
    }

    public void EndEncounter()
    {

    }
    IEnumerator Timer()
    {
        for (int i = 0; i < roundTime; i++)
        {
            currentTime = i;
            timeBar.SetValue(i);
            if (skip)
            {
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        inCombat = true;
        currentTime = 20;
        timeBar.AnimationDuration = 2f;
        timeBar.SetValue(0);
        inputDisable = true;
        skip = false;
        AC.Combat();
    }
    IEnumerator Rotating()
    {
        yield return new WaitForSeconds(.5f);
        turningLeft = false;
        turningRight = false;

        if (!inCombat)
        {
            inputDisable = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            skip = true;
            inCombat = true;
        }

        if (!turningLeft && !turningRight)
        {
            if (!inputDisable)
            {
                if (Input.GetMouseButton(1))
                {
                    freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
                    freeLook.m_XAxis.m_InputAxisName = "Mouse X";
                }
                else
                {
                    freeLook.m_YAxis.m_InputAxisName = "";
                    freeLook.m_XAxis.m_InputAxisName = "";
                    freeLook.m_YAxis.m_InputAxisValue = 0;
                    freeLook.m_XAxis.m_InputAxisValue = 0;
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    turningLeft = true;
                    inputDisable = true;
                    foreach (Ally ally in AC.allies)
                    {
                        ally.sectorIndex = (ally.sectorIndex - 1) % 6;
                        if (ally.sectorIndex == -1)
                        {
                            ally.sectorIndex = 5;
                        }
                    }
                    yEulerAngle = AC.transform.eulerAngles.y;
                    StartCoroutine(Rotating());
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    turningRight = true;
                    inputDisable = true;
                    foreach (Ally ally in AC.allies)
                    {
                        ally.sectorIndex = (ally.sectorIndex + 1) % 6;
                    }
                    yEulerAngle = AC.transform.eulerAngles.y;
                    StartCoroutine(Rotating());
                }
            }
        }
        else if (turningLeft)
        {
            AC.transform.rotation = Quaternion.Slerp
                (AC.transform.rotation, Quaternion.Euler(0, yEulerAngle - 60f, 0), 0.25f);
        }
        else if (turningRight)
        {
            AC.transform.rotation = Quaternion.Slerp
                (AC.transform.rotation, Quaternion.Euler(0, yEulerAngle + 60f, 0), 0.25f);
        }
    }
}
