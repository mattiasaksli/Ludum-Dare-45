using UnityEngine;
using UnityEngine.SceneManagement;

public class exit : MonoBehaviour
{
    public void Exit()
    {
        PlayerPrefs.SetInt("AllyNumber", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
