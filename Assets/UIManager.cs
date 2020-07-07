using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject lifeIconPrefab;
    public GameObject lifePanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    public GameObject mainMenuPanel;
    public GameObject diedPanel;
    public GameObject wonPanel;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreManager == null)
            scoreManager = FindObjectOfType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.livesChangedEvent.AddListener(LivesChanged);
            scoreManager.scoreChangedEvent.AddListener(ScoreChanged);

            LivesChanged(scoreManager.lives);
            ScoreChanged(scoreManager.score);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && mainMenuPanel != null)
            mainMenuPanel.SetActive(!mainMenuPanel.activeSelf);

        Time.timeScale = mainMenuPanel == null || !mainMenuPanel.activeInHierarchy ? 1f : 0f;
    }

    void LivesChanged(int newLives)
    {
        if (lifePanel != null && lifeIconPrefab != null && newLives >= 0)
        {
            if (newLives > lifePanel.transform.childCount)
            {
                var count = newLives - lifePanel.transform.childCount;
                for (var x = 0; x < count; x++)
                {
                    var newIcon = Instantiate(lifeIconPrefab);
                    newIcon.transform.SetParent(lifePanel.transform);
                }
            }
            else if (newLives < lifePanel.transform.childCount)
            {
                var count = lifePanel.transform.childCount - newLives;
                for (var x = 0; x < count; x++)
                {
                    Destroy(lifePanel.transform.GetChild(x).gameObject);
                }
            }
        }

        if (scoreManager.lives <= 0)
            GameOver();
    }
    void ScoreChanged(long newScore)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {newScore}";

        if (scoreManager.aliensAbducted >= scoreManager.aliensInLevel)
            CompleteLevel(true);
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NewGame()
    {
        scoreManager.LoadLevel(0);
        if (levelText != null)
            levelText.text = $"LVL:{scoreManager.level}";
    }

    public void NextLevel()
    {
        scoreManager.LoadLevel(scoreManager.level + 1);
        if (levelText != null)
            levelText.text = $"LVL:{scoreManager.level}";
    }


    internal void CompleteLevel(bool won)
    {
        if (won && wonPanel != null)
        {
            wonPanel.SetActive(true);
        }
    }
    internal void GameOver()
    {
        if (diedPanel != null)
            diedPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
