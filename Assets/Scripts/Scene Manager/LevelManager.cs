using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Button[] btn = new Button[5];
    public Sprite[] imgCleared = new Sprite[5];
    public Sprite[] imgLocked = new Sprite[5];

    private void Start()
    {
        CheckLevel();
    }

    public void CheckLevel()
    {
        int LevelStatus2 = PlayerPrefs.GetInt("Level 2");
        int LevelStatus3 = PlayerPrefs.GetInt("Level 3");
        int LevelStatus4 = PlayerPrefs.GetInt("Level 4");
        int LevelStatus5 = PlayerPrefs.GetInt("Level 5");

        if (LevelStatus2 == 1)
        {
            btn[1].interactable = true;
            btn[1].GetComponent<Image>().sprite = imgCleared[1];
            
        }
        else
        {
            btn[1].interactable = false;
            btn[1].GetComponent<Image>().sprite = imgLocked[1];
        }

        if (LevelStatus3 == 1)
        {
            btn[1].interactable = true;
            btn[1].GetComponent<Image>().sprite = imgCleared[1];

            btn[2].interactable = true;
            btn[2].GetComponent<Image>().sprite = imgCleared[2];
        }
        else
        {
            btn[2].interactable = false;
            btn[2].GetComponent<Image>().sprite = imgLocked[2];
        }

        if (LevelStatus4 == 1)
        {
            btn[1].interactable = true;
            btn[1].GetComponent<Image>().sprite = imgCleared[1];

            btn[2].interactable = true;
            btn[2].GetComponent<Image>().sprite = imgCleared[2];

            btn[3].interactable = true;
            btn[3].GetComponent<Image>().sprite = imgCleared[3];
        }
        else
        {
            btn[3].interactable = false;
            btn[3].GetComponent<Image>().sprite = imgLocked[3];
        }

        if (LevelStatus5 == 1)
        {
            btn[1].interactable = true;
            btn[1].GetComponent<Image>().sprite = imgCleared[1];

            btn[2].interactable = true;
            btn[2].GetComponent<Image>().sprite = imgCleared[2];

            btn[3].interactable = true;
            btn[3].GetComponent<Image>().sprite = imgCleared[3];

            btn[4].interactable = true;
            btn[4].GetComponent<Image>().sprite = imgCleared[4];
        }
        else
        {
            btn[4].interactable = false;
            btn[4].GetComponent<Image>().sprite = imgLocked[4];
        }
    }
}
