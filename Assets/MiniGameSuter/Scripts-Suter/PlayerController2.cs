using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController2 : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI remainingText;
    private int count;
    public GameObject winTextObject;
    public GameObject restartObject;
    public int pickupsCount = 0;
    public int startingPickupsCount = 0;
    public AudioSource audioDroplet;
    public ParticleSystem particleBoom;
    public ParticleSystem pickupFX;
    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false;   

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        startingPickupsCount = GameObject.FindGameObjectsWithTag("PickUp").Length;
        pickupsCount = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();
        winTextObject.SetActive(false);
        Debug.Log("pickupsCount: " + pickupsCount);
        
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        remainingText.text = "Remaining: " + pickupsCount.ToString();
        if (count >= startingPickupsCount)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
        
    }

    public void Update()
    {
        if (Input.GetMouseButton(0)) // Check if left mouse button is held down
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);

            RaycastHit hit; // Define variable to hold raycast hit information


            // Check if raycast hits an object
            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    targetPos = hit.point; // Set target position
                    isMoving = true; // Start player movement
                }
                
               // targetPos = hit.point; // Set target position
               // isMoving = true; // Start player movement
            }
        }

        else
        {
            isMoving = false; // Stop player movement
        }
    }
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (isMoving)
        {
            // Move the player towards the target position
            Vector3 direction = targetPos - rb.position;
            direction.Normalize();
            rb.AddForce(direction * speed);
        }

        if (Vector3.Distance(rb.position, targetPos) < 0.5f)
        {
            isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            var currentPickupFX = Instantiate(pickupFX, other.transform.position, Quaternion.identity);

            other.gameObject.SetActive(false);
            count++;
            pickupsCount--;
            SetCountText();
            audioDroplet.Play();
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //play the boom particle
            particleBoom.Play();

            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            restartObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You LOOOOSE!";
        }
    }
}
