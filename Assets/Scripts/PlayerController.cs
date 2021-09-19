using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform playerTransform;
    private float moveH, fire;

    [SerializeField] private float speed, maxBound, minBound;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float timeCharge;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        moveH = Input.GetAxis("Horizontal");
        if (moveH < 0)
            MoveLeft();
        else if (moveH > 0)
            MoveRight();
    }

    private void Update()
    {
        if(Input.GetButton("Fire1") && Time.time > fire && Time.timeScale > 0)
        {
            fire = Time.time + timeCharge;
            Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
        }
    }

    public void MoveLeft()
    {
        if (playerTransform.position.x > minBound)
            playerTransform.position += Vector3.left * speed;
    }

    public void MoveRight()
    {
        if (playerTransform.position.x < maxBound)
            playerTransform.position += Vector3.right * speed;
    }

    public void ChangeColor(int numberColor)
    {
        Color newColor = new Color(0.4103269f, 0.3490196f, 0.6117647f);
        switch (numberColor)
        {
            case 1:
                newColor = Color.blue;
                break;
            case 2:
                newColor = Color.black;
                break;
            case 3:
                newColor = Color.red;
                break;
            case 4:
                newColor = Color.yellow;
                break;
            case 5:
                newColor = Color.green;
                break;
            case 0:
                break;
        }
        gameObject.GetComponent<MeshRenderer>().material.color = newColor;
        PlayerPrefs.SetInt("Color", numberColor);
    }
}
