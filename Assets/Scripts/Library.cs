using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Library : MonoBehaviour {

    #region varaibles
    // levels
    public const int LEVEL_0 = 0; // splash screen
    public const int LEVEL_1 = 1; // menu/ sign in screen
    public const int LEVEL_2 = 2; // binary maze
    public const int LEVEL_3 = 3; // perlin noise
    public const int LEVEL_4 = 4; // planets
    public const int LEVEL_5 = 5; // tanks
    public const int LEVEL_6 = 6; // view all high scores
    public const int LEVEL_7 = 7; // end of game

    // php pages
    public const string UPDATE = "https://adv3dgamedev.000webhostapp.com/update.php"; // updating the user score and level
    public const string LOGIN = "https://adv3dgamedev.000webhostapp.com/login.php"; // login into existing account
    public const string CREATE = "https://adv3dgamedev.000webhostapp.com/create.php"; // create a new account
    public const string SCORES = "https://adv3dgamedev.000webhostapp.com/scores.php"; // get all user names, and scores

    // parameters in url string
    public const string USERNAME_PARAMETER = "name";
    public const string PASSWORD_PARAMETER = "password";
    public const string SCORE_PARAMETER = "score";
    public const string LEVEL_PARAMETER = "level";
    #endregion

    #region functions
    // saves the game data to the database and playerprefs file
    public static void SaveGame(Database database, int score, int level) {
        SaveGame(score, level);
        database.Level = level;
        database.Score = score;
        database.Playername = PlayerPrefs.GetString("playername");
        database.Password = PlayerPrefs.GetString("password");
        database.UpdateScore();
    }

    // saves the game data to the playerprefs file
    public static void SaveGame(int score, int level) {
        PlayerPrefs.SetInt("score", PlayerPrefs.HasKey("score") ? score + PlayerPrefs.GetInt("score") : score);
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.Save();
    }

    #endregion
}
