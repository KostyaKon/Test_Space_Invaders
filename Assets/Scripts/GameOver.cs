using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Text gameOver;
    [SerializeField] private GameObject restart;
    [SerializeField] private GameObject menuPanel;

    private void Awake()
    {
        GameController GC = FindObjectOfType<GameController>();
        GC.OnPlayerLose += OnPlayerLose;
        GC.OnPlayerWin += OnPlayerWin;
    }
    void Start()
    {
        gameOver = GetComponent<Text>();
        gameOver.enabled = false;
        restart.SetActive(false);
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0;
            menuPanel.SetActive(true);
        }
    }

    void StopGame(string messeg)
    {
        Time.timeScale = 0;
        gameOver.text = messeg;
        gameOver.enabled = true;
        restart.SetActive(true);
    }

    void OnPlayerLose()
    {
        StopGame("Game Over");
    }
    void OnPlayerWin()
    {
        StopGame("You Win");
    }

    public void RestartGame()
    {
        OnEventClean();
        SceneManager.LoadScene("GameScene");
    }

    public void BackGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChangeColorPlayer(int numberColor)
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.ChangeColor(numberColor);
    }

    void OnEventClean()
    {
        GameController GC = FindObjectOfType<GameController>();
        GC.OnPlayerLose -= OnPlayerLose;
        GC.OnPlayerWin -= OnPlayerWin;
        GC.OnEventClean();
    }
}
