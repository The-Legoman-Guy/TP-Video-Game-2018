using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject[] titles;
    public GameObject[] buttonScaling;
    public float minButtonScaling = 1.5450f;
    public float maxButtonScaling = 1.89f;
    public float valButtonScaling = 1.5450f;
    public bool boolButtonScaling = true;
    public int speedSpinning = 1;
    public float welcomeTime = 0;
    public int durationWelcome = 4;
    public GameObject welcome;
    public GameObject menu;
    public float colorTransp = 0;
    public GameObject[] menuBasicButtons;
    public GameObject[] menuQuitConfirmation;
    public GameObject[] menuOptions;
    public GameObject[] menuPlay;
    public GameObject[] menuLoading;
    public string changeButton = "";
    public float loadingTime = 0;
    public bool lauchLoading = false;
    public AudioSource clickSound;

    void Start()
    {
        Environnement.P1 = new Controls();
        Environnement.P1.Up = KeyCode.W;
        Environnement.P1.Down = KeyCode.S;
        Environnement.P1.Left = KeyCode.A;
        Environnement.P1.Right = KeyCode.D;
        Environnement.P1.Shoot = KeyCode.Space;
        Environnement.P2 = new Controls();
        Environnement.P2.Up = KeyCode.UpArrow;
        Environnement.P2.Down = KeyCode.DownArrow;
        Environnement.P2.Left = KeyCode.LeftArrow;
        Environnement.P2.Right = KeyCode.RightArrow;
        Environnement.P2.Shoot = KeyCode.KeypadEnter;
        welcomeTime = 0;
        welcome.SetActive(true);
        unActiveList(menuQuitConfirmation);
        clickSound = GameObject.Find("SoundClick").GetComponent<AudioSource>();
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.isKey && changeButton != "")
            if (e.keyCode.ToString() != "None" &&
                e.keyCode.ToString() != "escape")
                changeTouch(e.keyCode);
    }

    void changeTouch(KeyCode e)
    {
        //Debug.Log("Detected key code: " + e);
        switch (changeButton)
        {
            case "P1Up":
                Environnement.P1.Up = e;
                GameObject.Find("P1Up").GetComponentInChildren<Text>().text = Environnement.P1.Up.ToString();
                break;
            case "P1Down":
                Environnement.P1.Down = e;
                GameObject.Find("P1Down").GetComponentInChildren<Text>().text = Environnement.P1.Down.ToString();
                break;
            case "P1Left":
                Environnement.P1.Left = e;
                GameObject.Find("P1Left").GetComponentInChildren<Text>().text = Environnement.P1.Left.ToString();
                break;
            case "P1Right":
                Environnement.P1.Right = e;
                GameObject.Find("P1Right").GetComponentInChildren<Text>().text = Environnement.P1.Right.ToString();
                break;
            case "P1Shoot":
                Environnement.P1.Shoot = e;
                GameObject.Find("P1Shoot").GetComponentInChildren<Text>().text = Environnement.P1.Shoot.ToString();
                break;
            case "P2Up":
                Environnement.P2.Up = e;
                GameObject.Find("P2Up").GetComponentInChildren<Text>().text = Environnement.P2.Up.ToString();
                break;
            case "P2Down":
                Environnement.P2.Down = e;
                GameObject.Find("P2Down").GetComponentInChildren<Text>().text = Environnement.P2.Down.ToString();
                break;
            case "P2Left":
                Environnement.P2.Left = e;
                GameObject.Find("P2Left").GetComponentInChildren<Text>().text = Environnement.P2.Left.ToString();
                break;
            case "P2Right":
                Environnement.P2.Right = e;
                GameObject.Find("P2Right").GetComponentInChildren<Text>().text = Environnement.P2.Right.ToString();
                break;
            case "P2Shoot":
                Environnement.P2.Shoot = e;
                GameObject.Find("P2Shoot").GetComponentInChildren<Text>().text = Environnement.P2.Shoot.ToString();
                break;
        }
        changeButton = "";
    }

    void Update()
    {
        welcomeTime += Time.deltaTime;
        if (welcomeTime < durationWelcome && Environnement.gamesPlayed == 0)
        {
            colorTransp += 0.005f;
            ShowIntro();
        }
        else
        {
            welcome.SetActive(false);
            menu.SetActive(true);
            spinThatItem();
            scaleThatItem();
        }

        if (lauchLoading == true)
        {
            activeList(menuLoading);
            loadingTime += Time.deltaTime;
            GameObject.Find("LoadingSlider").GetComponent<Slider>().value += 0.01f;
            if (loadingTime > 3)
            {
                lauchLoading = false;
                unActiveList(menuLoading);
                SceneManager.LoadScene("Game");
            }
        }
        else if (Input.GetKeyDown("escape"))
            quit();
    }

    void spinThatItem()
    {
        for (int i = 0; i < 4; i++)
            titles[i].transform.Rotate(Vector3.forward, speedSpinning * Time.deltaTime);
    }

    void scaleThatItem()
    {
        if (boolButtonScaling == true)
            valButtonScaling += Time.deltaTime;
        else if (boolButtonScaling == false)
            valButtonScaling -= Time.deltaTime;

        if (valButtonScaling > maxButtonScaling && boolButtonScaling == true)
            boolButtonScaling = false;
        else if (valButtonScaling < minButtonScaling && boolButtonScaling == false)
            boolButtonScaling = true;

        for (int i = 0; i < 3; i++)
            buttonScaling[i].transform.localScale = new Vector3(valButtonScaling, valButtonScaling, valButtonScaling);
    }

    public void fasterSpin()
    {
        clickSound.Play();
        speedSpinning += 3;
    }

    public void quit()
    {
        clickSound.Play();
        unActiveList(menuBasicButtons);
        unActiveList(menuOptions);
        unActiveList(menuPlay);
        activeList(menuQuitConfirmation);
    }

    public void quitYes()
    {
        clickSound.Play();
        Application.Quit();
    }

    public void quitNo()
    {
        clickSound.Play();
        unActiveList(menuQuitConfirmation);
        activeList(menuBasicButtons);
    }

    public void ShowIntro()
    {
        GameObject.Find("WelcomeText").GetComponent<Text>().color = new Color(1, 1, 1, colorTransp);
        welcome.SetActive(true);
        menu.SetActive(false);
    }

    public void unActiveList(GameObject[] tmp)
    {
        for (int i = 0; i < tmp.Length; i++)
            tmp[i].SetActive(false);
    }

    public void activeList(GameObject[] tmp)
    {
        for (int i = 0; i < tmp.Length; i++)
            tmp[i].SetActive(true);
    }

    public void openOptions()
    {
        clickSound.Play();
        unActiveList(menuBasicButtons);
        activeList(menuOptions);
        GameObject.Find("P1Up").GetComponentInChildren<Text>().text = Environnement.P1.Up.ToString();
        GameObject.Find("P1Down").GetComponentInChildren<Text>().text = Environnement.P1.Down.ToString();
        GameObject.Find("P1Left").GetComponentInChildren<Text>().text = Environnement.P1.Left.ToString();
        GameObject.Find("P1Right").GetComponentInChildren<Text>().text = Environnement.P1.Right.ToString();
        GameObject.Find("P1Shoot").GetComponentInChildren<Text>().text = Environnement.P1.Shoot.ToString();

        GameObject.Find("P2Up").GetComponentInChildren<Text>().text = Environnement.P2.Up.ToString();
        GameObject.Find("P2Down").GetComponentInChildren<Text>().text = Environnement.P2.Down.ToString();
        GameObject.Find("P2Left").GetComponentInChildren<Text>().text = Environnement.P2.Left.ToString();
        GameObject.Find("P2Right").GetComponentInChildren<Text>().text = Environnement.P2.Right.ToString();
        GameObject.Find("P2Shoot").GetComponentInChildren<Text>().text = Environnement.P2.Shoot.ToString();

        if (Environnement.difficulty == "Easy")
            GameObject.Find("Difficulty").GetComponent<Dropdown>().value = 0;
        else if (Environnement.difficulty == "Medium")
            GameObject.Find("Difficulty").GetComponent<Dropdown>().value = 1;
        else if (Environnement.difficulty == "Hard")
            GameObject.Find("Difficulty").GetComponent<Dropdown>().value = 2;

        if (Environnement.hasTheController == "none")
            GameObject.Find("Controller").GetComponent<Dropdown>().value = 0;
        else if (Environnement.hasTheController == "one")
            GameObject.Find("Controller").GetComponent<Dropdown>().value = 1;
        else if (Environnement.hasTheController == "two")
            GameObject.Find("Controller").GetComponent<Dropdown>().value = 2;

        if (Environnement.controllerInverted == false)
            GameObject.Find("ControllerInvert").GetComponent<Dropdown>().value = 0;
        else if (Environnement.controllerInverted == true)
            GameObject.Find("ControllerInvert").GetComponent<Dropdown>().value = 1;
    }

    public void closeOptions()
    {
        clickSound.Play();
        unActiveList(menuOptions);
        activeList(menuBasicButtons);
    }

    public void changeTouch(Button button)
    {
        clickSound.Play();
        changeButton = button.name;
    }

    public void openPlay()
    {
        clickSound.Play();
        unActiveList(menuBasicButtons);
        activeList(menuPlay);
    }

    public void closePlay()
    {
        clickSound.Play();
        unActiveList(menuPlay);
        activeList(menuBasicButtons);
    }

    public void lauchGame(string tmp)
    {
        clickSound.Play();
        if (tmp == "one")
            Environnement.ia = true;
        else
            Environnement.ia = false;
        lauchLoading = true;
        loadingTime = 0;
    }


    public void changeControllerUser()
    {
        clickSound.Play();
        int value = GameObject.Find("Controller").GetComponent<Dropdown>().value;
        if (value == 0)
            Environnement.hasTheController = "none";
        else if (value == 1)
            Environnement.hasTheController = "one";
        else if (value == 2)
            Environnement.hasTheController = "two";
    }

    public void changeControllerInversion()
    {
        clickSound.Play();
        int value = GameObject.Find("ControllerInvert").GetComponent<Dropdown>().value;
        if (value == 0)
            Environnement.controllerInverted = false;
        else if (value == 1)
            Environnement.controllerInverted = true;
    }

    public void changeDifficulty()
    {
        clickSound.Play();
        int value = GameObject.Find("Difficulty").GetComponent<Dropdown>().value;
        if (value == 0)
            Environnement.difficulty = "Easy";
        else if (value == 1)
            Environnement.difficulty = "Medium";
        else if (value == 2)
            Environnement.difficulty = "Hard";
    }
}