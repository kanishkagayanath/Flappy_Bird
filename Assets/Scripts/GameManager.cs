using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening; // Added DOTween

public class GameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject obstaclePrefab;
    public GameObject player;

    [Header("UI Panels")]
    public GameObject homePanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;

    [Header("UI Elements")]
    public Button playButton;
    public Button retryButton;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI bestScoreText;

    [Header("Game Settings")]
    public float obstacleSpawnInterval = 2f;
    public float obstacleSpeed = 2f;

    public enum GameState { Home, Playing, GameOver }
    public GameState currentState = GameState.Home;

    public float timer = 0;
    public bool isGameOver = false;
    public int currentScore = 0;
    public int bestScore = 0;
    public static GameManager instance;

    private PlayerController playerController;

    public void Awake()
    {
        instance = this;
        playerController = player.GetComponent<PlayerController>();
    }

    public void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        SetGameState(GameState.Home);
        Time.timeScale = 1f;

        if (playButton != null)
            playButton.onClick.AddListener(StartGame);
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryGame);
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            HandleObstacleSpawning();
        }

        if (currentState == GameState.Home && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartGame();
        }
    }

    private void HandleObstacleSpawning()
    {
        if (timer <= 0f)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(10f, Random.Range(-3f, 0f), 0f), Quaternion.identity);
            
            // Juice: Scale up obstacle smoothly on spawn
            obstacle.transform.localScale = Vector3.zero;
            obstacle.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);

            Destroy(obstacle, 30f);
            timer = obstacleSpawnInterval;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        homePanel.SetActive(false);
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        switch (newState)
        {
            case GameState.Home:
                homePanel.SetActive(true);
                AnimatePanelPopIn(homePanel);
                isGameOver = false;
                playerController.EnableInput(false);
                
                if (player != null)
                    player.SetActive(false);
                Time.timeScale = 1f;
                break;

            case GameState.Playing:
                gamePanel.SetActive(true);
                isGameOver = false;
                playerController.EnableInput(true);
                
                if (player != null)
                    player.SetActive(true);
                currentScore = 0;
                UpdateScoreUI();
                Time.timeScale = 1f;
                break;

            case GameState.GameOver:
                gameOverPanel.SetActive(true);
                AnimatePanelPopIn(gameOverPanel);
                isGameOver = true;
                playerController.EnableInput(false);
               
                if (player != null)
                    player.SetActive(true);
                
                UpdateGameOverUI();
                Time.timeScale = 0f; // Set scale after animations update
                break;
        }
    }

    private void AnimatePanelPopIn(GameObject panel)
    {
        panel.transform.DOKill();
        panel.transform.localScale = Vector3.one * 0.7f;
        panel.transform.DOScale(Vector3.one, 0.35f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    public void StartGame()
    {
        ClearObstacles();
        ResetPlayer();
        SetGameState(GameState.Playing);
    }

    public void GameOver()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        SetGameState(GameState.GameOver);
    }

    public void AddScore()
    {
        if (currentState == GameState.Playing)
        {
            currentScore++;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();

            // Juice: Score bump text animation
            scoreText.transform.DOKill(true);
            scoreText.transform.localScale = Vector3.one;
            scoreText.transform.DOPunchScale(Vector3.one * 0.4f, 0.2f, 8, 1f);
        }
    }

    private void UpdateGameOverUI()
    {
        if (finalScoreText != null)
            finalScoreText.text = "Score: " + currentScore.ToString();
        if (bestScoreText != null)
            bestScoreText.text = "Best: " + bestScore.ToString();
    }

    private void ClearObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }

    private void ResetPlayer()
    {
        if (player != null)
        {
            player.transform.DOKill();
            player.transform.rotation = Quaternion.identity;
            player.transform.position = new Vector3(-2f, 0f, 0f);
            
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
                playerRb.angularVelocity = 0f;
            }
        }
    }

    public void RetryGame()
    {
        StartGame();
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}