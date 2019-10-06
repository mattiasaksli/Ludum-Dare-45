using Doozy.Engine.UI;
using UnityEngine;

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

    void Start()
    {
        canvas = GameObject.Find("View - MainTownView");
        w = canvas.GetComponent<RectTransform>().rect.width;
    }

    void Update()
    {
        animator.SetBool("InCenter", isCenter);

        if (gameObject.transform.localPosition.x < 0)
        {
            gameObject.transform.localPosition += new Vector3(speed, 0f, 0f);
        }

        if (HeroChosen == false)
        {
            if (gameObject.transform.localPosition.x >= 0)
            {
                isCenter = true;
                popup.Show();
            }
        }

        if (FormationChosen == true)
        {
            formationpopup.Hide();
        }

        if (HeroChosen == true && FormationChosen == false)
        {
            formationpopup.Show();
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
            townview.Hide();

            if (gameObject.transform.localPosition.x < w + 50)
            {
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

    public void ExitFormation()
    {
        FormationChosen = true;
    }
}
