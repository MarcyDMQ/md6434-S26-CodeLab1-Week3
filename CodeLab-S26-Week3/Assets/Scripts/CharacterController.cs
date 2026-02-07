using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{
    Rigidbody rb; //Rigidbody for the GameObject
    public static CharacterController instance;

    public float moveForce = 10f; //the force we add to a GameObject
    public float turnSpeed = 10f; //how fast the character turns

    public Key keyForward = Key.W;
    public Key keyBackward = Key.S;
    public Key keyLeft = Key.A;
    public Key keyRight = Key.D;

    public GameObject explodePrefab; //Prefab to spawn when the character dies

    Keyboard keyboard = Keyboard.current; //get the keyboard input

    void Start()
    {
        rb = GetComponent<Rigidbody>(); //get Rigidbody
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = Vector3.zero; //movement direction

        if (keyboard[keyForward].isPressed)
            dir += Vector3.forward;

        if (keyboard[keyBackward].isPressed)
            dir += Vector3.back;

        if (keyboard[keyLeft].isPressed)
            dir += Vector3.left;

        if (keyboard[keyRight].isPressed)
            dir += Vector3.right;

        if (dir != Vector3.zero)
        {
            dir.Normalize();

            //add force to move the character
            rb.AddForce(dir * moveForce);

        }
        
    }

    //trigger detection for death
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) //check if collided with a bullet
        {
            Die();
        }
    }

    void Die()
    {
        //spawn explosion prefab at the character's position
        if (explodePrefab != null)
            Instantiate(explodePrefab, transform.position, Quaternion.identity);

        //destroy the character GameObject
        Destroy(gameObject);
        GameManager gm = GameManager.FindFirstObjectByType<GameManager>();
        if (gm != null)
            gm.GameOver();
    }
}
