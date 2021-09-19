using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private List<EnemyController> enemies;
    private byte count = 0, maxCount = 0;

    private void Awake()
    {
        enemies = new List<EnemyController>();
    }

    void Update()
    {
        if (enemies[count] != null)
            enemies[count].isFront = true;
        else if(enemies[count] == null)
        {
            count++;
            if(count >= maxCount)
            {
                Destroy(gameObject);
            }
        }
    }

    public void InstantiateEnemy(GameObject enemyPrefab, byte numberOfEnemies, byte lightEnemy)
    {
        for(byte i = 0; i < numberOfEnemies; i++)
        {
            Vector3 positionEnemy = transform.position + new Vector3(0, i * 0.85f, 0);
            EnemyController enemy = (Instantiate(enemyPrefab, positionEnemy, Quaternion.identity) as GameObject).GetComponent<EnemyController>();
            enemy.gameObject.transform.parent = this.gameObject.transform;
            if(i < lightEnemy)
            {
                enemy.LightEnemy();
            }
            enemies.Add(enemy);
        }
        maxCount = numberOfEnemies;
    }
}
