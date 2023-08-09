using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestText;
    public GameObject GameOverText;

    public string DisplayUserName;
    public string displayHighScore;
    public string DisplayHighScoreUserName;
    public int checkHighScore;
    public string checkPoints;

    private bool m_Started = false;
    private int m_Points = 0;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        DisplayUserName = GameManager.Instance.userNameInput;
        LoadHighScore();
        AddPoint(0);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string displayHighScore;
        public string DisplayUserName;
        public string DisplayHighScoreUserName;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = DisplayUserName + "'s Score : " + m_Points;
    }

    public void GameOver()
    {
        CheckIfNewHighScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void CheckIfNewHighScore()
    {
        string checkPoints = m_Points.ToString();
        int checkHighScore = Int32.Parse(displayHighScore);
        Debug.Log("HighScore Checked");

        if (m_Points > checkHighScore || checkHighScore == 0)
        {
            displayHighScore = checkPoints;
            DisplayHighScoreUserName = DisplayUserName;
            SaveHighScore();
            BestText.text = "High Score: " + DisplayHighScoreUserName + " : " + displayHighScore;
        }
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.displayHighScore = displayHighScore;
        data.DisplayHighScoreUserName = DisplayHighScoreUserName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("HighScore Saved");
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            DisplayHighScoreUserName = data.DisplayHighScoreUserName;
            displayHighScore = data.displayHighScore;
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

    /*public void SaveHighScoreUserName()
    {
        SaveData data = new SaveData();
        data.DisplayUserName = DisplayUserName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }*/
}
