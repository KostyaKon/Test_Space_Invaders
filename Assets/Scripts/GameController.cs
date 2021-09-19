using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static bool isPlayerDied;
    public static float playerScore;

    [SerializeField] private byte livePlayer = 3;
    [SerializeField] private GameObject Player;
    [SerializeField] private Transform playerSpawn;

    [SerializeField] [Range(1, 9)] private byte numberLine;
    [SerializeField] [Range(1, 7)] private byte numberEnemyOfLine;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text bestScoreText;
    [SerializeField] private Text liveText;
    [SerializeField] private Text levelText;

    [SerializeField] private GameObject explosion;
    [SerializeField] private bool lastLevel = false;

    private byte levelPlayer = 1;

    private GameObject player;
    private EnemiesController EC;

    private bool isPlayerLose;

    private bool startPlayer;
    private bool isExplosion;

    public event Action OnPlayerLose;
    public event Action OnPlayerWin;

    private void Awake()
    {
        EC = FindObjectOfType<EnemiesController>();
        EC.PlayerLose += PlayerLose;
        EC.PlayerWin += PlayerWin;
        EC.SetNumberLine(numberLine);
        EC.SetNumberEnemyOfLine(numberEnemyOfLine);
        //PlayerPrefs.SetFloat("BestScore", 0); // Reset 
    }
    void Start()
    {
        levelPlayer = 1;
        playerScore = 0;
        if (livePlayer > 0)
        {
            player = Instantiate(Player, playerSpawn.position, Quaternion.Euler(new Vector3(0, -180, 90)));
            player.GetComponent<PlayerController>().ChangeColor(PlayerPrefs.GetInt("Color"));
            startPlayer = true;
        }
        Time.timeScale = 1;
        StartGame();
    }

    void Update()
    {
        if (isPlayerDied && !isExplosion)
        {
            PlayerDead();
        }
        if(player == null && !isPlayerLose && startPlayer)
        {
            startPlayer = false;
            CollisionWithPlayer();
        }

        scoreText.text = "Score: " + playerScore;
        liveText.text = "Live: " + livePlayer;
    }

    void StartGame()
    {
        SetSettingsLevel();
        EC.StartEnemiesController(levelPlayer);
        levelText.text = "Level: " + levelPlayer;
        bestScoreText.text = "Best Score: " + PlayerPrefs.GetFloat("BestScore", 0f).ToString();
    }

    void SetSettingsLevel()
    {
        isPlayerLose = false;
        isPlayerDied = false;
        isExplosion = false;
    }

    private void CollisionWithPlayer()
    {
        livePlayer--;
        if (livePlayer <= 0)
        {
            RecordingBestScore();
            Invoke("StopTime", 1.1f);
        }
        else if(livePlayer > 0)
        {
            isPlayerDied = false;
            isExplosion = false;
            StartCoroutine(InstPlayer());
        }
    }

    IEnumerator InstPlayer()
    {
        yield return new WaitForSeconds(1f);
        player = Instantiate(Player, playerSpawn.position, Quaternion.Euler(new Vector3(0, -180, 90)));
        player.GetComponent<PlayerController>().ChangeColor(PlayerPrefs.GetInt("Color"));
        startPlayer = true;
    }

    private void PlayerDead()
    {
        GameObject expl = Instantiate(explosion, player.transform.position, Quaternion.identity) as GameObject;
        isExplosion = true;
        Destroy(player);
        Destroy(expl, 1f);
    }

    private void StopTime()
    {
        OnPlayerLose?.Invoke();
        Time.timeScale = 0;
    }

    private void PlayerLose()
    {
        RecordingBestScore();
        isPlayerLose = true;
        OnPlayerLose?.Invoke();
    }

    private void RecordingBestScore()
    {
        if (PlayerPrefs.GetFloat("BestScore", 0f) < playerScore)
        {
            PlayerPrefs.SetFloat("BestScore", playerScore);
        }
    }

    private void PlayerWin()
    {
        if (!lastLevel)
        {
            levelPlayer = (levelPlayer == 127) ? (byte)126 : (++levelPlayer);
            livePlayer++;
            StartGame();
        }
        else
        {
            OnPlayerWin?.Invoke();
        }
    }

    public void OnEventClean()
    {
        EnemiesController EC = FindObjectOfType<EnemiesController>();
        EC.PlayerLose -= PlayerLose;
        EC.PlayerWin -= PlayerWin;
    }
}
