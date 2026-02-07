using UnityEngine;

public class BulletSpawn : MonoBehaviour
{
    public GameObject bulletPrefab;   //use bullet Prefab
    public float spawnInterval = 2f;  //every 2 seconds generate a new bullet

    private float timer = 0f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); //find player
    }

    void Update()
    {
        if (player == null) return; //if player died stop generate bullet

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()//make the bullet move toward where the player was when it was generated
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(direction);
    }
}
