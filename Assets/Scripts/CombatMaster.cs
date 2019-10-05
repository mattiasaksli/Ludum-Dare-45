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
    void Start()
    {
        inputDisable = false;
        turningLeft = false;
        turningRight = false;
        roundCount = 1;
        timeBar = GetComponent<Progressor>();
        timeBar.SetMax(roundTime);
        timeBar.SetValue(0);
        EC = GameObject.FindGameObjectWithTag("EnemyController").GetComponent<EnemyController>();
        AC = GameObject.FindGameObjectWithTag("AllyController").GetComponent<AllyController>();
        StartCoroutine(Timer());
    }
    public void RoundStart()
    {
        timeBar.SetValue(0);
        skip = false;
        inputDisable = false;
        roundCount += 1;
        AC.RoundStart();
        EC.RoundStart();
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        for (int i = 0; i < roundTime; i++)
        {
            currentTime = i;
            timeBar.SetValue(i);
            if (skip)
            {
                currentTime = 20;
                timeBar.SetValue(0);
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        inputDisable = true;
        AC.Combat();
    }
    IEnumerator Rotating()
    {
        yield return new WaitForSeconds(.4f);
        inputDisable = false;
        turningLeft = false;
        turningRight = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!turningLeft && !turningRight)
        {
            if (!inputDisable)
            {

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    skip = true;
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
            AC.transform.rotation = Quaternion.Lerp
                (AC.transform.rotation, Quaternion.Euler(0, yEulerAngle - 60f, 0), 0.25f);
        }
        else if (turningRight)
        {
            AC.transform.rotation = Quaternion.Lerp
                (AC.transform.rotation, Quaternion.Euler(0, yEulerAngle + 60f, 0), 0.25f);
        }


    }
}
