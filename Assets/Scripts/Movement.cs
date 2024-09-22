using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float rocketSpeed = 100f;
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;
    [SerializeField] ParticleSystem mainBooster;
    AudioSource audioSource;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
       if (Input.GetAxis("Up") > 0)
        {
            StartThrusting();
        }
        else
        {
            StopTrusting();
        }
    }

    private void StopTrusting()
    {
        audioSource.Stop();
        mainBooster.Stop();
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * rocketSpeed * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            mainBooster.Play();
        }
    }

    void ProcessRotation()
    {
        
       if (Input.GetAxis("Horizontal") < -0.01)
        {
            RotateLeft();
        }
        else if (Input.GetAxis("Horizontal") > 0.01)
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }

    }

 private void RotateRight()
    {
        ApplyRotation(-rotateSpeed);
        if (!rightBooster.isPlaying)
        {
            rightBooster.Play();
        }
    }

    private void StopRotating()
    {
        rightBooster.Stop();
        leftBooster.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotateSpeed);
        if (!leftBooster.isPlaying)
        {
            leftBooster.Play();
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
