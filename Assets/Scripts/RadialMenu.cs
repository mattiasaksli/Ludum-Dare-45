using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public Sprite[] allySprites;
    public string[] allyText;
    public Image[] buttons;
    public Ally[] allyPrefabs = new Ally[3];
    public Ally newAlly1;
    public Ally newAlly2;
    public int chosenAlly;
    public (int index, int type) selectedAlly;
    public bool newAllyPlaced = false;
    public Dictionary<int, int> allyCircle = new Dictionary<int, int>();

    public int indexOfSelectedAlly = -1;

    public Button ChooseNewUnitButton1;
    public Button ChooseNewUnitButton2;
    public Image NewUnitImage;
    void Start()
    {
        if (PlayerPrefs.GetInt("DoneEncounters") == 0)
        {
            Debug.Log("##############RESET##############");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("DoneEncounters", 0);
            for (int i = 0; i < 6; i++)
            {
                PlayerPrefs.SetInt("PostEncounterIndex" + i, i);
                PlayerPrefs.SetInt("PostEncounterType" + i, 7);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Loading town start sector " + PlayerPrefs.GetInt("PostEncounterIndex" + i) + " With value " + PlayerPrefs.GetInt("PostEncounterType" + i));
        }
        LoadCircleState();
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Got town start sector " + i + " With value " + allyCircle[i]);
        }


        int r1 = Random.Range(0, 3);
        int r2 = r1;
        while (r2 == r1)
        {
            r2 = Random.Range(0, 3);
        }

        newAlly1 = allyPrefabs[r1];
        newAlly2 = allyPrefabs[r2];

        ChooseNewUnitButton1.GetComponentInChildren<Image>().sprite = allySprites[(int)newAlly1.unitType];
        ChooseNewUnitButton1.GetComponentInChildren<TextMeshProUGUI>().SetText(allyText[(int)newAlly1.unitType]);
        ChooseNewUnitButton2.GetComponentInChildren<Image>().sprite = allySprites[(int)newAlly2.unitType];
        ChooseNewUnitButton2.GetComponentInChildren<TextMeshProUGUI>().SetText(allyText[(int)newAlly2.unitType]);

        indexOfSelectedAlly = -1;
        newAllyPlaced = false;
    }
    public void SetButtonImages()
    {
        for (int i = 0; i < 6; i++)
        {
            if (allyCircle[i] == 7)
            {
                buttons[i].sprite = allySprites[3];
            }
            else
            {
                //Debug.Log(i);
                buttons[i].sprite = allySprites[allyCircle[i]];
            }
        }
    }
    public void LoadCircleState()
    {

        for (int i = 0; i < 6; i++)
        {
            int index = PlayerPrefs.GetInt("PostEncounterIndex" + i);
            int type = PlayerPrefs.GetInt("PostEncounterType" + i);
            allyCircle[index] = type;
        }
    }
    public void Ally1ButtonClicked()
    {
        chosenAlly = (int)newAlly1.unitType;
    }

    public void Ally2ButtonClicked()
    {
        chosenAlly = (int)newAlly2.unitType;
    }
    public void SectorButtonClicked(int n)
    {
        if (allyCircle[n] == 7)  // If there is no ally in the clicked sector.
        {
            if (indexOfSelectedAlly == -2)  // If a reset selection clicks on an empty sector.
            {
                return; // Don't do anything.
            }

            if (indexOfSelectedAlly == -1)  // If the new unit was selected previously (default).
            {
                allyCircle[n] = chosenAlly; // The new unit will be in the empty sector.
                newAllyPlaced = true;
                indexOfSelectedAlly = -2;   // Reset selection.
                NewUnitImage.enabled = false;
            }
            else    // If another unit was selected previously.
            {
                int prevSelected = allyCircle[indexOfSelectedAlly];
                int target = allyCircle[n];
                allyCircle[n] = prevSelected;
                allyCircle[indexOfSelectedAlly] = target;// The previously selected unit will be in the empty sector;
                indexOfSelectedAlly = -2;   // Reset selection.
                SetButtonImages();
                return;
            }
        }
        else    // If an ally is in the clicked sector.
        {
            if (newAllyPlaced)  // Only if the new ally has been placed in an empty sector.
            {
                if (indexOfSelectedAlly == -2)  // If the selction was reset.
                {
                    indexOfSelectedAlly = n;    // The unit in the current sector becomes selected.
                }
                else    // If a previous ally was selected.
                {
                    int temp = allyCircle[n];  // Switch the previously selected unit and the one in the current sector.
                    allyCircle[n] = allyCircle[indexOfSelectedAlly];
                    allyCircle[indexOfSelectedAlly] = temp;
                    indexOfSelectedAlly = -2;   // Reset selection.
                }
            }
        }
        SetButtonImages();
    }
    public void ResetEncounter()
    {
        Debug.Log("##############RESET##############");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("DoneEncounters", 0);
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt("PostEncounterIndex" + i, i);
            PlayerPrefs.SetInt("PostEncounterType" + i, 7);
        }

        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Loading town start sector " + PlayerPrefs.GetInt("PostEncounterIndex" + i) + " With value " + PlayerPrefs.GetInt("PostEncounterType" + i));
        }
        LoadCircleState();
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Got town start sector " + i + " With value " + allyCircle[i]);
        }
        SceneManager.LoadScene("Town");
    }
    public void Quit()
    {
        Save();
        Application.Quit();
    }
    public void Save()
    {
        for (int i = 0; i < 6; i++)
        {
            if (allyCircle[i] != 7)
            {
                PlayerPrefs.SetInt("PreEncounterIndex" + i, i);

                PlayerPrefs.SetInt("PreEncounterType" + i, allyCircle[i]);
                Debug.Log("Saved at sector " + i + " With value " + allyCircle[i]);
            }
            else
            {
                PlayerPrefs.SetInt("PreEncounterIndex" + i, i);

                PlayerPrefs.SetInt("PreEncounterType" + i, 7);
                Debug.Log("Saved at sector " + i + " With value " + 7);
            }
        }
        for (int i = 0; i < 6; i++)
        {
            Debug.Log("Finally save in sector " + PlayerPrefs.GetInt("PreEncounterIndex" + i) + " With value " + PlayerPrefs.GetInt("PreEncounterType" + i));
        }
        int allyNumberCommit = 0;
        for (int i = 0; i < 6; i++)
        {
            if (allyCircle[i] != 7)
            {
                allyNumberCommit += 1;
            }
        }
        PlayerPrefs.SetInt("AllyNumber", allyNumberCommit);
        PlayerPrefs.Save();
    }
}
