using Cinemachine;
using Doozy.Engine.Progress;
using Doozy.Engine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public UIView winView;
    public UIView loseView;
    public Animator transition;
    public Canvas mainCanvas;
    private bool first = true;
    public AudioSource audio;

    public AudioClip[] audioClips;
    public ParticleSystem plasma;
    public ParticleSystem explosion;
    void Start()
    {
        StartCoroutine(CanvasDisable());
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
        audio = GetComponent<AudioSource>();
        audio.clip = audioClips[0];
        audio.Play();
        StartCoroutine(Timer());
    }
    IEnumerator CanvasDisable()
    {
        transition.Play("FadeIn");
        yield return new WaitForSeconds(1f);
        mainCanvas.sortingOrder = -3;
        mainCanvas.gameObject.SetActive(false);
    }
    public void SaveAllies()
    {

        int num = AC.allies.Count;
        for (int i = 0; i < num; i++)
        {
            Debug.Log("Saving combat end sector " + AC.allies[i].sectorIndex + " With value " + (int)AC.allies[i].unitType);
        }
        for (int i = 0; i < 6; i++)
        {
            foreach (Ally a in AC.allies)
            {
                if (a.sectorIndex == i && a.health > 0)
                {
                    PlayerPrefs.SetInt("PostEncounterIndex" + i, i);
                    PlayerPrefs.SetInt("PostEncounterType" + i, (int)a.unitType);
                    break;
                }
                else
                {
                    PlayerPrefs.SetInt("PostEncounterIndex" + i, i);
                    PlayerPrefs.SetInt("PostEncounterType" + i, 7);
                }
            }
        }
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Checking combat end sector " + PlayerPrefs.GetInt("PostEncounterIndex" + i) + " With value " + PlayerPrefs.GetInt("PostEncounterType" + i));
        }
        PlayerPrefs.Save();
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

        audio.clip = audioClips[0];
        audio.Play();

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

    public void EndEncounter(bool win)
    {
        mainCanvas.sortingOrder = 101;
        mainCanvas.gameObject.SetActive(true);
        if (!win)
        {
            loseView.Show();
        }
        else
        {
            winView.Show();
        }
    }

    public void winButtonClicked()
    {
        int done = PlayerPrefs.GetInt("DoneEncounters");
        PlayerPrefs.SetInt("DoneEncounters", done + 1);
        winView.Hide();
        SaveAllies();
        StartCoroutine(ChangeLevel());
    }

    public void loseButtonClicked()
    {
        SaveAllies();
        loseView.Hide();
        StartCoroutine(ChangeLevel());
    }
    IEnumerator ChangeLevel()
    {
        transition.Play("FadeOut");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Town");
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
        audio.clip = audioClips[0];
        audio.Play();
        AC.Combat();
    }
    IEnumerator Rotating()
    {
        yield return new WaitForSeconds(.7f);
        turningLeft = false;
        turningRight = false;

        if (!inCombat)
        {
            inputDisable = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            foreach (Enemy e in EC.enemies)
            {
                e.Damage(e.health, 0);
            }
            plasma.Play();
            explosion.Play();
            audio.clip = audioClips[20];
            audio.Play();
            //StartCoroutine(Rotating());
        }
        if (Input.GetKeyDown(KeyCode.Space))
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
                (AC.transform.rotation, Quaternion.Euler(0, yEulerAngle - 60f, 0), 0.2f);
        }
        else if (turningRight)
        {
            AC.transform.rotation = Quaternion.Slerp
                (AC.transform.rotation, Quaternion.Euler(0, yEulerAngle + 60f, 0), 0.2f);
        }
    }
}
