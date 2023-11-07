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
    public int pickupsCount = 0;
    public int startingPickupsCount = 0;
    public AudioSource audioDroplet;
    public ParticleSystem particleBoom;
    public ParticleSystem pickupFX;
   

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

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
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
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You LOOOOSE!";
        }
    }
}
