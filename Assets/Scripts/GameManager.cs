using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string userNameInput;
    public string displayHighScore;
    public string DisplayHighScoreUserName;
    public Text BestText;
    //public TextMeshProUGUI DisplayUserNameText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
    }

    [System.Serializable]
     class SaveData
     {
         public string userNameInput;
         public string displayHighScore;
         public string DisplayHighScoreUserName;
     }

    public void SaveUserName()
    {
        SaveData data = new SaveData();
        data.userNameInput = userNameInput;
    }


    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            displayHighScore = data.displayHighScore;
            DisplayHighScoreUserName = data.DisplayHighScoreUserName;
            Debug.Log("HighScore Loaded");
        }
        else
        {
            Debug.Log("HighScore Defaulted");
            DisplayHighScoreUserName = "None";
            displayHighScore = "0";
        }
        BestText.text = "High Score: " + DisplayHighScoreUserName + " : " + displayHighScore;
    }

    /*public void DisplayUsername(string userNameInput)
    {
        DisplayUserNameText.text = "Score: " + score;
    }*/


}
