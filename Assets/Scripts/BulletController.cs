using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Transform bullet;
    private float bulletDestroyY = 8f;

    [SerializeField] private float speed;
    [SerializeField] private byte forceAttack = 1;
    void Start()
    {
        bullet = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        bullet.position += Vector3.up * speed;
        if (bullet.position.y >= bulletDestroyY)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyController ec = other.gameObject.GetComponent<EnemyController>();
            ec.DecreaseHealth(forceAttack);
            Destroy(gameObject);
        }
        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.tag == "Wall")
            Destroy(gameObject);
    }
}
