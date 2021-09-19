using UnityEngine;

public class WallLive : MonoBehaviour
{
    [SerializeField] private byte health = 3;
 
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }

    public void DecreaseHealth(byte forceAttack)
    {
        health -= forceAttack;
        if (health < 0) health = 0;
    }
}
