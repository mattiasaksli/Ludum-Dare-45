using Doozy.Engine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    bool SettingsOpen = false;
    public UIPopup settingspopup; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SettingsOpen == true)
        {
            settingspopup.Show();
        }
    }
    public void OpenSettings()
    {
        SettingsOpen = true;
    }
    public void ExitSettings()
    {
        SettingsOpen = false;
    }
}
