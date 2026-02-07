using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 moveDir;

    public void Init(Vector3 direction)
    {
        direction.y = 0;
        moveDir = direction.normalized;
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
         if (other.CompareTag("wall"))//if the bullet hit the wall
        {
            Destroy(gameObject); //destroy the bullet
        }
    }
}
