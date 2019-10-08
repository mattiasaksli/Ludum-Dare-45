using UnityEngine;

public class HideIfFirst : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int num = PlayerPrefs.GetInt("AllyNumber");
        if (num != 1)
        {
            this.gameObject.active = false;
        }
    }
}
