using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class Database : MonoBehaviour {

    //variables
    // player details 
    string playerName = "";
    string password = "";
    int score;
    int level;

    [SerializeField]
    InputField usernameInput;

    [SerializeField]
    InputField passwordInput;

    [SerializeField]
    GameObject[] toggleActive;

    [SerializeField]
    Text text;

    #region Properties
    public string Playername {
        get => playerName;
        set => playerName = value;
    }

    public string Password {
        get => password;
        set => password = value;
    }

    public int Score {
        get => score;
        set => score = value;
    }

    public int Level {
        get => level;
        set => level = value;
    }

    public Text ScoreText {
        get => text;
        set => text = value;
    }
    #endregion

    public void Login() {
        // starts the coroutine to login the player

        // basic check if the user entered a name and password
        if (usernameInput.text.Length == 0 || passwordInput.text.Length == 0) {
            if (text != null) {
                text.text = "Please enter a username and password";
            }
            return;
        }
        playerName = usernameInput.text;
        StartCoroutine(LoginPage());
    }

    public void CreateAccount() {
        // starts the coroutine to login the player

        // basic check if the user entered a name and password
        if (usernameInput.text.Length == 0 || passwordInput.text.Length == 0) {
            if (text != null) {
                text.text = "Please enter a username and password";
            }
            return;
        }
        playerName = usernameInput.text;
        StartCoroutine(CreateAccPage());
    }

    public void ShowScoreboard() {
        // starts the coroutine get all the player scores
        StartCoroutine(ShowScorePage());
    }

    private void Awake() {
        // have this gameobject persist throughtout all the levels
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateScore() {
        // starts the coroutine to update the player score
        if (playerName.Length == 0 || password.Length == 0) return;
        StartCoroutine(UpdateScorePage());
    }

    IEnumerator UpdateScorePage() {
        // load the update page
        WWW page = new WWW(Library.UPDATE + "?" + Library.USERNAME_PARAMETER + "=" + playerName + "&" + Library.PASSWORD_PARAMETER + "=" + password + "&" + Library.LEVEL_PARAMETER + "=" + level + "&" + Library.SCORE_PARAMETER + "=" + score);
        yield return page;
    }

    public void LoadFirstLevel() {
        // used in the main menu to allow the player to restart from the first level regardless of an existing save at a later level
        Library.SaveGame(score, Library.LEVEL_2);
        SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
    }

    public void LoadCurrentLevel() {
        // used in the main menu to allow the player to restart from the current level they last played
        Library.SaveGame(score, level);
        SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
    }

    IEnumerator LoginPage() {
        // get the contents of the login page
        WWW page = new WWW(Library.LOGIN + "?" + Library.USERNAME_PARAMETER + "=" + playerName + "&" + Library.PASSWORD_PARAMETER + "=" + password);
        yield return page;

        // split the data (score, level) for the user with the username and password
        string[] data = page.text.Split(new char[] { ',' });

        // check if a user was found
        if (data.Length == 2) {
            // store the password and username for updating the score later on
            if (!PlayerPrefs.HasKey("playername")) {
                PlayerPrefs.SetString("playername", playerName);
                PlayerPrefs.SetString("password", password);
            }

            // store the score and level
            score = int.Parse(data[0]);
            level = int.Parse(data[1]);

            // toggle the first canvas to be invisible, and toggle the second canvas to be active
            // the second canvas is used to let the player choose between continuing or restarting the game
            foreach (var toggle in toggleActive) {
                toggle.SetActive(!toggle.activeSelf);
            }

        } else {
            // no user with the given password and user name can be found
            // check if text contains a reference to a UI text object
            // if yes, output a message to the user prompting them to create an account
            if (text != null) {
                text.text = "Username does not exist try creating an account instead";
            }
        }
    }

    IEnumerator CreateAccPage() {
        // call the create account page
        WWW page = new WWW(Library.CREATE + "?" + Library.USERNAME_PARAMETER + "=" + playerName + "&" + Library.PASSWORD_PARAMETER + "=" + password);
        yield return page;
        // split the data recieved
        string[] data = page.text.Split(new char[] { ',' });
        if (data.Length == 2) {
            // store the password and username for updating the score later on
            if (!PlayerPrefs.HasKey("playername")) {
                PlayerPrefs.SetString("playername", playerName);
                PlayerPrefs.SetString("password", password);
            }
            // save the score and the level to the player prefs only
            // and load the level
            Library.SaveGame(int.Parse(data[0]), Library.LEVEL_2);
            SceneManager.LoadScene(PlayerPrefs.GetInt("level"));
        }
    }

    IEnumerator ShowScorePage() {
        // get the contents of the score page
        WWW page = new WWW(Library.SCORES + "?" + Library.USERNAME_PARAMETER + "=" + playerName + "&" + Library.PASSWORD_PARAMETER + "=" + password);
        yield return page;
        // change the html breaks for the new line, this puts the user and score onto a new line
        text.text = page.text.Replace("<br>", "\n");
        Debug.Log(page.text);
    }

    public void PasswordInputEnd() {
        // https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp

        // hash the password text for basic security
        using (HashAlgorithm algorithm = SHA256.Create()) {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in algorithm.ComputeHash(Encoding.UTF8.GetBytes(passwordInput.text))) sb.Append(b.ToString());
            password = sb.ToString().Replace("&","^");
        }
    }

    void Start() {
        // removes existing playerprefs values on start
        PlayerPrefs.DeleteAll();
    }

}
