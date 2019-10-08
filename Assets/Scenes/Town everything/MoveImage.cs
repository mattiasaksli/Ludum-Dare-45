using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveImage : MonoBehaviour
{
    public Animator animator;
    public float speed;
    private float w;
    bool isCenter;
    bool HeroChosen = false;
    bool wantsToExit = false;
    bool GoToBattleButtonPressed = false;
    bool SettingsOpen = false;
    bool FormationChosen = false;
    GameObject canvas;
    public UIPopup popup;
    public UIPopup toBattlepopup;
    public UIPopup settingspopup;
    public UIPopup formationpopup;
    public UIView townview;
    public RadialMenu RM;
    int allyNumber = 0;
    void Start()
    {
        canvas = GameObject.Find("View - MainTownView");
        w = canvas.GetComponent<RectTransform>().rect.width;
        RM = GameObject.FindGameObjectWithTag("RadialMenu").GetComponent<RadialMenu>();
    }

    void Update()
    {

        if (gameObject.transform.localPosition.x > w * 0.4)
        {
            SceneManager.LoadScene("Combat");
        }

        if (gameObject.transform.localPosition.x < 0)
        {
            gameObject.transform.localPosition += new Vector3(speed, 0f, 0f);
        }

        if (HeroChosen == false)
        {
            bool radialFull = true;
            int allyNumber = 0;
            for (int i = 0; i < 6; i++)
            {
                if (RM.allyCircle[i] == 7)
                {
                    radialFull = false;
                    allyNumber += 1;
                }
            }
            if (radialFull)
            {
                Debug.Log("########################################################    " + radialFull);
                GoToBattleButtonPressed = true;
                HeroChosen = true;
                FormationChosen = true;
                animator.SetBool("InCenter", isCenter);
            }
            else
            {
                if (gameObject.transform.localPosition.x >= 0)
                {
                    isCenter = true;
                    popup.Show();
                }
            }
        }

        if (FormationChosen == true)
        {
            formationpopup.Hide();
        }

        if (HeroChosen == true && FormationChosen == false)
        {
            formationpopup.Show();
            RM.SetButtonImages();
        }

        if (HeroChosen == true)
        {
            popup.Hide();
        }

        if (HeroChosen == true && GoToBattleButtonPressed == false && FormationChosen == true)
        {
            toBattlepopup.Show();
        }

        if (GoToBattleButtonPressed == true)
        {
            toBattlepopup.Hide();
        }

        if (GoToBattleButtonPressed == true && HeroChosen == true)
        {
            isCenter = false;
            if (gameObject.transform.localPosition.x > 0)
            {
                if (gameObject.transform.localPosition.x > w * 0.1f)
                {
                    townview.Hide();
                }
                gameObject.transform.localPosition += new Vector3(speed, 0f, 0f);
            }
        }

        if (SettingsOpen == true)
        {
            settingspopup.Show();
        }
        else if (SettingsOpen == false)
        {
            settingspopup.Hide();
        }
    }

    public void ExitHeroSelect()
    {
        HeroChosen = true;
    }

    public void ToBattle()
    {
        GoToBattleButtonPressed = true;
    }

    public void OpenSettings()
    {
        SettingsOpen = true;
    }

    public void ExitSettings()
    {
        SettingsOpen = false;
    }
    public void Reset()
    {
        SceneManager.LoadScene("Town");
    }
    public void ExitFormation()
    {
        FormationChosen = true;
        RM.Save();
    }
}
