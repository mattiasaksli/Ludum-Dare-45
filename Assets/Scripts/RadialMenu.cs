using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public Sprite[] allySprites;
    public Image[] buttons;
    public Ally[] allyPrefabs = new Ally[3];
    public Ally[] allyCircle = new Ally[6];
    public Ally newAlly1;
    public Ally newAlly2;
    public Ally chosenAlly;
    public Ally selectedAlly;
    public bool newAllyPlaced = false;

    public int indexOfSelectedAlly = -1;

    public Button ChooseNewUnitButton1;
    public Button ChooseNewUnitButton2;
    public Image NewUnitImage;
    void Start()
    {
        for (int i = 0; i < allyCircle.Length; i++)
        {
            allyCircle[i] = null;
        }

        LoadCircleState();

        int r1 = Random.Range(0, 3);
        int r2 = r1;
        while (r2 == r1)
        {
            r2 = Random.Range(0, 3);
        }

        newAlly1 = allyPrefabs[r1];
        newAlly2 = allyPrefabs[r2];

        ChooseNewUnitButton1.GetComponentInChildren<Image>().sprite = allySprites[(int)newAlly1.unitType];
        ChooseNewUnitButton2.GetComponentInChildren<Image>().sprite = allySprites[(int)newAlly2.unitType];

        indexOfSelectedAlly = -1;
        newAllyPlaced = false;
    }
    public void SetButtonImages()
    {
        for (int i = 0; i < 6; i++)
        {
            if (allyCircle[i] == null)
            {
                buttons[i].sprite = allySprites[3];
            }
            else
            {
                buttons[i].sprite = allySprites[(int)allyCircle[i].unitType];
            }
        }
    }
    public void LoadCircleState()
    {

        int num = PlayerPrefs.GetInt("PostEncounterPositionsNr");
        for (int i = 0; i < num; i++)
        {
            int index = PlayerPrefs.GetInt("PostEncounterIndex" + i);
            int type = PlayerPrefs.GetInt("PostEncounterType" + i);
            Ally a = new Ally { unitType = (Ally.allyClass)type };
            allyCircle[index] = a;
        }
    }

    public void Ally1ButtonClicked()
    {
        chosenAlly = newAlly1;
    }

    public void Ally2ButtonClicked()
    {
        chosenAlly = newAlly2;
    }
    public void SectorButtonClicked(int n)
    {
        if (allyCircle[n] == null)  // If there is no ally in the clicked sector.
        {
            if (indexOfSelectedAlly == -2)  // If a reset selection clicks on an empty sector.
            {
                return; // Don't do anything.
            }

            if (indexOfSelectedAlly == -1)  // If the new unit was selected previously (default).
            {
                allyCircle[n] = chosenAlly; // The new unit will be in the empty sector.
                chosenAlly = null;
                newAllyPlaced = true;
                indexOfSelectedAlly = -2;   // Reset selection.
                NewUnitImage.enabled = false;
            }
            else    // If another unit was selected previously.
            {
                allyCircle[n] = allyCircle[indexOfSelectedAlly];    // The previously selected unit will be in the empty sector;
                allyCircle[indexOfSelectedAlly] = null;
                indexOfSelectedAlly = -2;   // Reset selection.
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
                    Ally temp = allyCircle[n];  // Switch the previously selected unit and the one in the current sector.
                    allyCircle[n] = allyCircle[indexOfSelectedAlly];
                    allyCircle[indexOfSelectedAlly] = temp;
                    indexOfSelectedAlly = -2;   // Reset selection.
                }
            }
        }
        SetButtonImages();
    }
}
