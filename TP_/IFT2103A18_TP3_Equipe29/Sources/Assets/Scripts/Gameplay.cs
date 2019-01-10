using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{

    public Player player1;
    public Player player2;
    public GameObject pauseObject;
    public int maxScore = 1000;
    public GameObject bonus;
    public GameObject malus;
    public int bonusMax = 3;
    public int malusMax = 8;
    public Text pointsPlayerOne;
    public Text pointsPlayerTwo;
    public Text powerPlayerOne;
    public Text powerPlayerTwo;
    public GameObject winObject;
    public float winTimeLeft = 6;
    public GameObject lightEffectPlayerOne;
    public GameObject lightEffectPlayerTwo;
    public AudioSource winSound;
    public AudioSource backGroundSound;
    public AudioSource backGroundSound2;

    void Start()
    {
        backGroundSound = GameObject.Find("BackGroundMusic").GetComponent<AudioSource>();
        backGroundSound2 = GameObject.Find("BackGroundMusic2").GetComponent<AudioSource>();
        backGroundSound2.volume = 0;
        backGroundSound.Play();
        backGroundSound2.Play();
        winSound = GameObject.Find("Win").GetComponent<AudioSource>();

        Environnement.gamesPlayed += 1;

        pauseObject.SetActive(false);
        Environnement.pause = false;
        // DEBUG, A ENLEVER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /*Environnement.P1 = new Controls();
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
        Environnement.P2.Shoot = KeyCode.KeypadEnter;*/
        ////////////////////////////////////////////////


        player1 = new Player();
        player1.controls = Environnement.P1;

        player2 = new Player();
        player2.controls = Environnement.P2;

        if (Environnement.difficulty == "Easy")
        {
            bonusMax = 3;
            malusMax = 8;
            Environnement.powerPlayerOne = 3;
            Environnement.powerPlayerTwo = 3;
        }
        else if (Environnement.difficulty == "Medium")
        {
            bonusMax = 2;
            malusMax = 11;
            Environnement.powerPlayerOne = 2;
            Environnement.powerPlayerTwo = 2;
        }
        else if (Environnement.difficulty == "Hard")
        {
            bonusMax = 1;
            malusMax = 14;
            Environnement.powerPlayerOne = 1;
            Environnement.powerPlayerTwo = 1;
        }
        Environnement.pointsPlayerOne = 0;
        Environnement.pointsPlayerTwo = 0;
        Environnement.bonus = 0;
        Environnement.malus = 0;
        Environnement.playerOneGainedPoints = false;
        Environnement.playerTwoGainedPoints = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
            pauseGame();
        if (win() == true)
        {
            Environnement.pause = true;
            winObject.SetActive(true);
            if (winTimeLeft == 6)
            {
                backGroundSound.Stop();
                backGroundSound2.Stop();
                winSound.Play();
            }
            winTimeLeft -= Time.deltaTime;
            GameObject.Find("PlayerWon").GetComponent<Text>().text = "Player " +
                Environnement.playerWin +
                " wins !\nBack at main menu in " +
                winTimeLeft.ToString("F2");
            if (winTimeLeft <= 0)
                SceneManager.LoadScene("Menu");

        }
        if (Environnement.bonus < bonusMax)
        {
            spawnPoint("bonus");
            Environnement.bonus++;
        }
        if (Environnement.malus < malusMax)
        {
            spawnPoint("malus");
            Environnement.malus++;
        }
        adjustScore();
        fadeInOutSound();
    }

    public void adjustScore()
    {
        if (Environnement.playerOneGainedPoints == true)
        {
            Environnement.playerOneGainedPoints = false;
            lightEffectPlayerOne.GetComponent<ParticleSystem>().Play();
        }
        if (Environnement.playerTwoGainedPoints == true)
        {
            Environnement.playerTwoGainedPoints = false;
            lightEffectPlayerTwo.GetComponent<ParticleSystem>().Play();
        }

        pointsPlayerOne.text = "Player 1\n" + Environnement.pointsPlayerOne + " Points";
        pointsPlayerTwo.text = "Player 2\n" + Environnement.pointsPlayerTwo + " Points";
        powerPlayerOne.text = "Power left:\n" + Environnement.powerPlayerOne;
        powerPlayerTwo.text = "Power left:\n" + Environnement.powerPlayerTwo;
    }

    public void fadeInOutSound()
    {
        if (Environnement.pointsPlayerOne >= 600 ||
            Environnement.pointsPlayerTwo >= 600)
        {
            backGroundSound.volume -= 0.003f;
            backGroundSound2.volume += 0.003f;
        }
        else
        {
            backGroundSound.volume += 0.003f;
            backGroundSound2.volume -= 0.003f;
        }
    }

    public void spawnPoint(string name)
    {
        // Random Position
        Vector3 pos = new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-4, 6), -0.52f);
        if (name == "bonus")
            Instantiate(bonus, pos, Quaternion.identity);
        else
            Instantiate(malus, pos, Quaternion.identity);
    }

    bool win()
    {
        if (Environnement.pointsPlayerOne >= maxScore || Environnement.pointsPlayerTwo >= maxScore)
        {
            if (Environnement.pointsPlayerOne >= maxScore)
                Environnement.playerWin = "one";
            else
                Environnement.playerWin = "two";
            return true;
        }
        return false;
    }

    public void pauseGame()
    {
        Environnement.pause = !Environnement.pause;
        if (Environnement.pause)
            pauseObject.SetActive(true);
        else
            pauseObject.SetActive(false);
    }

    public void pauseGameMainMenu()
    {
        pauseGame();
        SceneManager.LoadScene("Menu");
    }

    public void pauseGameBack()
    {
        pauseGame();
    }
}
