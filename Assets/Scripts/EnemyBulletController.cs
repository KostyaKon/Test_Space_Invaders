using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    private Transform bullet;
    private float bulletDestroyY = -5f;

    [SerializeField] private float speed;
    [SerializeField] private byte forceAttack = 1;

    void Start()
    {
        bullet = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        bullet.position += Vector3.down * speed;
        if (bullet.position.y <= bulletDestroyY)
            Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameController.isPlayerDied = true;
            Destroy(gameObject);
        }
        else if (other.tag == "Wall")
        {
            WallLive wl = other.gameObject.GetComponent<WallLive>();
            wl.DecreaseHealth(forceAttack);
            Destroy(gameObject);
        }
    }
}
