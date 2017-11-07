using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public class UIController : MonoBehaviour {

    public GameObject PausedMenu;
    public GameObject ControlMenu;
    public XboxController controller;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public Text player1Name;
    public Text player2Name;
    public Text player3Name;
    public Text player4Name;
    bool paused;

    // Use this for initialization
    //Sets menus to inactive
    void Start()
    {
        paused = false;
        PausedMenu.SetActive(false);
        ControlMenu.SetActive(false);

        string name = PlayerPrefs.GetString("Player1Name").ToUpper();
        if (name == "")
        {
            player1Name.text = "PLAYER 1";
        }
        else
        {
            player1Name.text = name;
        }

        name = PlayerPrefs.GetString("Player2Name").ToUpper();
        if (name == "")
        {
            player2Name.text = "PLAYER 2";
        }
        else
        {
            player2Name.text = name;
        }

        name = PlayerPrefs.GetString("Player3Name").ToUpper();
        if (name == "")
        {
            player3Name.text = "PLAYER 3";
        }
        else
        {
            player3Name.text = name;
        }

        name = PlayerPrefs.GetString("Player4Name").ToUpper();
        if (name == "")
        {
            player4Name.text = "PLAYER 4";
        }
        else
        {
            player4Name.text = name;
        }
        
    }

    // Update is called once per frame
    //Pause the game if start button is pressed on controller or if bool is true
    void Update()
    {
        if (XCI.GetButtonDown(XboxButton.Start, controller))
        {
            paused = !paused;
            if (paused)
            {
                PausedMenu.SetActive(true);
            }

        }
        if (paused)
        {

            Time.timeScale = 0;
        }
        else
        {
            PausedMenu.SetActive(false);
            ControlMenu.SetActive(false);
            Time.timeScale = 1;

            }
    }

    //Resume game and start time scale
    public void Resume()
    {
        paused = false;
    }

    //Show controls menu and get rid of current menu
    public void Controls()
    {
        PausedMenu.SetActive(false);
        ControlMenu.SetActive(true);
    }

    //Quit and go back to main menu
    public void Quit()
    {
		GameRoundManager.instance.QuitAndReset ();
    }

    //Unpause game and return.
    public void Return()
    {
        PausedMenu.SetActive(true);
        ControlMenu.SetActive(false);
    }

    //Set paused bool to true
    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            PausedMenu.SetActive(true);
        }
    }
}
