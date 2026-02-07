using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;


public class GameManager : MonoBehaviour
{
    const string DIR_DATA = "/Data/";
    const string FILE_HIGHSCORE = DIR_DATA + "highScore.txt";
    const string KeyScore = "Score";

    public static GameManager instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI winText;


    public float survivalTime = 0f;   // the time player exists
    public float targetTime = 10f;    // the time player needs to survive and pass the level
    public int currentLevel = 0;
    private bool gameOver = false;
    
    private float score;
    public float Score 
    {
        set 
        {
            Debug.Log("Set Score: " + value);
            
            score = value; 
            
            PlayerPrefs.SetFloat(KeyScore, score); 
            if (score > HighScore) 
            {
                HighScore = score;
            }
        }
        get
        {
            score = PlayerPrefs.GetFloat(KeyScore, 0); 
            
            Debug.Log("Got Score: " + score);
            return score;  
        }
    }
    
    private float highScore;
    public float HighScore
    {
        get
        {
            string fullPath = Application.dataPath + FILE_HIGHSCORE;

            if (!File.Exists(fullPath))
            {
                highScore = 0;
            }
            else
            {
                string contents = File.ReadAllText(fullPath);
                highScore = float.Parse(contents);
            }

            return highScore;
        }

        set
        {
            highScore = value;

            string fullPath = Application.dataPath + FILE_HIGHSCORE;

            if (!Directory.Exists(Application.dataPath + DIR_DATA))
            {
                Directory.CreateDirectory(Application.dataPath + DIR_DATA);
            }

            File.WriteAllText(fullPath, highScore.ToString("F0"));
        }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Score = 0;
        }
        else
        {
            Destroy(gameObject);
        }
        // hide Game Over UI at  first
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);
        if (winText != null)
            winText.gameObject.SetActive(false);

    }

    void Update()
    {
        if (gameOver) return;
        survivalTime += Time.deltaTime;

        // turn survivalTime into Score
        Score += Time.deltaTime;
        
        
        // update score and highscore
        if (scoreText != null)
        {
            scoreText.text = "Your Score: " + Mathf.FloorToInt(Score+0.5f) + "  HighScore: " + Mathf.FloorToInt(HighScore);
        }

        // pass to next level
        if (survivalTime >= targetTime)
        {
            GoToNextLevel();
        }

    }


    public void GameOver()
    {
        gameOver = true;
        // display Game Over UI
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);
        // hide UI
        //if (scoreText != null)
         //   scoreText.gameObject.SetActive(false);
    }

    void GoToNextLevel()
    {
        currentLevel++;

        int totalScenes = SceneManager.sceneCountInBuildSettings; // BuildSettings scenes

        //if (currentLevel >= totalScenes)
        //{
        //    if (winText != null)
        //        winText.gameObject.SetActive(true);
        //    if (scoreText != null)
        //        scoreText.gameObject.SetActive(false);
        //    gameOver = true; // stop counting time
        //    return;
        //}

        // next level
        targetTime *= 1.0f;
        survivalTime = 0f;
        SceneManager.LoadScene(currentLevel);
    }
}