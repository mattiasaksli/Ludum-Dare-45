using UnityEngine;

public class CombatMaster : MonoBehaviour
{
    public EnemyController EC;
    public AllyController AC;
    public int roundCount;
    public float roundTime = 20f;

    private float timer;
    private float roundEndTime;
    private float turnEndTime;
    private bool turningLeft = false;
    private bool turningRight = false;
    private float yEulerAngle;

    void Start()
    {
        roundCount = 1;
        timer = Time.time;
        roundEndTime = timer + roundTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.time;

        if (timer < roundEndTime)
        {

            if (turningLeft || turningRight)
            {
                if (timer >= turnEndTime)
                {
                    turningRight = false;
                    turningLeft = false;
                }
            }

            if (!turningLeft && !turningRight)
            {

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    AC.Combat();
                    EC.Combat();
                    AC.RoundStart();
                    EC.RoundStart();

                    roundCount += 1;
                    roundEndTime = Time.time + roundTime;
                }

                if (Input.GetKeyDown(KeyCode.A))
                {
                    turningLeft = true;
                    foreach (Ally ally in AC.allies)
                    {
                        ally.sectorIndex = (ally.sectorIndex - 1) % 6;
                        if (ally.sectorIndex == -1)
                        {
                            ally.sectorIndex = 5;
                        }
                    }
                    turnEndTime = Time.time + 0.5f;
                    yEulerAngle = AC.transform.eulerAngles.y;
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    turningRight = true;
                    foreach (Ally ally in AC.allies)
                    {
                        ally.sectorIndex = (ally.sectorIndex + 1) % 6;
                    }
                    turnEndTime = Time.time + 0.5f;
                    yEulerAngle = AC.transform.eulerAngles.y;
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
        else
        {
            AC.Combat();
            EC.Combat();
            AC.RoundStart();
            EC.RoundStart();

            roundCount += 1;
            roundEndTime = Time.time + roundTime;
        }
    }
}
