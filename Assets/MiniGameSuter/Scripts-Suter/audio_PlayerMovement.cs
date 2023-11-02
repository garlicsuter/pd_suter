using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_PlayerMovement : MonoBehaviour
{
    public AudioSource audioSource;
    public float minMoveDistance = 0.1f;
    private Vector3 previousPosition;
    private bool isPlayed = false;

    //For changing pitch of audio based on player speed
    private Rigidbody rb;
    public float minPitch = 0.5f;
    public float maxPitch = 2.0f;
    public float speedToPitchRatio = 0.1f;

    void Start()
    {
        //get Rigidbody to scale the audio pitch based on the rb's speed magnitude
        rb = GetComponent<Rigidbody>();
        // Store the initial position of the object
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Change pitch
        if (rb != null)
        {
            float speed = rb.velocity.magnitude;
            float pitch = Mathf.Lerp(minPitch, maxPitch, speed * speedToPitchRatio);
            audioSource.pitch = pitch;
        }

        // Calculate the distance the object has moved since the last frame
        float moveDistance = Vector3.Distance(transform.position, previousPosition);

        // If the object has moved more than the minimum distance
        if (moveDistance >= minMoveDistance)
        {
            StartCoroutine(waitForSound());

            if (!isPlayed)
            {
                audioSource.Play();
                isPlayed = true;
            }
            
            // Store the current position of the object for the next frame
            previousPosition = transform.position;
        }
    }

    IEnumerator waitForSound()
    {
        //Wait Until Sound has finished playing
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        isPlayed = false;
    }
}
