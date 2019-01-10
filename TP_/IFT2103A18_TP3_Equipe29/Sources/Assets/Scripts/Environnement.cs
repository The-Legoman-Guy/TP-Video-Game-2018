using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Environnement{

    public static Controls P1;
    public static Controls P2;
    public static bool ia = false;
    public static bool pause = false;
    public static int bonus = 0;
    public static int malus = 0;
    public static int pointsPlayerOne = 0;
    public static int pointsPlayerTwo = 0;
    public static string playerWin = "";
    public static string difficulty = "Medium";
    public static string hasTheController = "none";
    public static bool controllerInverted = false;
    public static int powerPlayerOne = 0;
    public static int powerPlayerTwo = 0;
    public static bool playerOneGainedPoints = false;
    public static bool playerTwoGainedPoints = false;
    public static int gamesPlayed = 0;
}
