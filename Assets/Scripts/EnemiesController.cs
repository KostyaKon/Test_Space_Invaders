using System;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private float enemySpeed, enimiesSpeed = 0.3f;
    [SerializeField] private float minBound = -7f;
    [SerializeField] private float maxBound = 7f;

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject LineEnemy;
    [SerializeField] [Range(0, 7)]private byte lightEnemy = 2;

    private byte numberLine;
    private byte numberEnemyOfLine;
    private Transform enemies;
    private bool lastEnemi;
    private float minPositionBottom = -2f;

    private float levevEnemySpeed, levelEnimiesSpeed;
    private float coefficientLevel = 0.02f, coefficientLastEnemy = 0.15f;
    private Vector3 startPosition;

    public event Action PlayerLose;
    public event Action PlayerWin;

    private void Awake()
    {
        enemies = GetComponent<Transform>();
        startPosition = enemies.position;
    }

    public void StartEnemiesController(byte level)
    {
        SetSettingsLevel(level);
        InstantiateEnemies();
        InvokeRepeating("MoveEnemies", 0.1f, levelEnimiesSpeed);
    }

    void MoveEnemies()
    {
        enemies.position += Vector3.right * levevEnemySpeed;
        foreach (Transform enemyL in enemies)
        {
            foreach (Transform enemy in enemyL)
            {
                if (enemy.position.x < minBound || enemy.position.x > maxBound)
                {
                    levevEnemySpeed = -levevEnemySpeed;
                    enemies.position += Vector3.down * 0.5f;
                    return;
                }
                if (enemy.position.y <= minPositionBottom)
                {
                    Time.timeScale = 0;
                    PlayerLose?.Invoke();
                }
            }
        }
        if (enemies.childCount == 1 && !lastEnemi)
        {
            if (enemies.GetChild(0).childCount == 1)
            {
                lastEnemi = true;
                levelEnimiesSpeed -= coefficientLastEnemy;
                levevEnemySpeed *= 2f;
                CancelInvoke();
                InvokeRepeating("MoveEnemies", 0.1f, levelEnimiesSpeed);
            }
        }
        if (enemies.childCount == 0)
        {
            CancelInvoke();
            PlayerWin?.Invoke();
        }
    }

    void InstantiateEnemies()
    {
        for(byte i = 0; i < numberLine; i++)
        {
            Vector3 positionLine = transform.position + new Vector3(2.5f - i * 1.1f, -1.28f, 0.0284109f);
            GameObject line = Instantiate(LineEnemy, positionLine, Quaternion.identity) as GameObject;
            line.transform.parent = this.gameObject.transform;
            line.GetComponent<LineController>().InstantiateEnemy(EnemyPrefab, numberEnemyOfLine, lightEnemy);
        }
    }

    void SetSettingsLevel(byte level)
    {
        levelEnimiesSpeed = enimiesSpeed - level * coefficientLevel;
        if(levelEnimiesSpeed < (coefficientLastEnemy + 0.01f))
        {
            levelEnimiesSpeed = coefficientLastEnemy + 0.01f;
            levevEnemySpeed = enemySpeed + level * coefficientLevel;
        }
        else
        {
            levevEnemySpeed = enemySpeed;
        }
        lastEnemi = false;
        enemies.position = startPosition;
    }

    public void SetNumberLine(byte numberLine)
    {
        this.numberLine = numberLine;
    }

    public void SetNumberEnemyOfLine(byte numberEnemyOfLine)
    {
        this.numberEnemyOfLine = numberEnemyOfLine;
    }

}
