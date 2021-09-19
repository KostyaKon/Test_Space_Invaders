using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bulletEnemy;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private byte health = 1;
    [SerializeField] private float score = 10;
    [SerializeField] private bool isSuperEnemy;
    [SerializeField] private Color color;
    [SerializeField] private GameObject[] enemyParts;

    public bool isFront;
    private bool isStartFire = false;

    private int timeCharge;

    void Start()
    {
        if (isSuperEnemy)
        {
            ChangeColor(color);
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            GameController.playerScore += score;
            Destroy(gameObject);
        }
        if (isFront && !isStartFire)
        {
            StartCoroutine(FireEnemy());
            isStartFire = true;
        }
    }

    IEnumerator FireEnemy()
    {
        while (true)
        {
            timeCharge = Random.Range(3, 10);
            yield return new WaitForSeconds(timeCharge);
            Instantiate(bulletEnemy, bulletSpawn.position, Quaternion.identity);
        }
    }

    void ChangeColor(Color color)
    {
        Color newColor = color;
        foreach (var item in enemyParts)
        {
            item.GetComponent<MeshRenderer>().material.color = newColor;
        }
    }

    public void DecreaseHealth(byte forceAttack)
    {
        health -= forceAttack;
        if (health < 0) health = 0;
    }

    public void LightEnemy()
    {
        health = 1;
        score = 10;
        isSuperEnemy = false;
        ChangeColor(Color.green);
    }
}
