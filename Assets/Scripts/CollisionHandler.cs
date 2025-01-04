using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

//Hello World 2!

public class CollisionHandler : MonoBehaviour
{
    bool collisionEnabled = true;
    AudioSource audioSource;
    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] AudioClip Success;
    [SerializeField] AudioClip Crash;
    [SerializeField] ParticleSystem SuccessParticles;
    [SerializeField] ParticleSystem CrashParticles;

    bool isTransitioning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
       #if UNITY_EDITOR 
            if (Input.GetKey(KeyCode.L))
            {
            LoadNextLevel();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                collisionEnabled = !collisionEnabled; //toggle collision
            }
        #endif
    }

   void OnCollisionEnter(Collision other)
   {
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
            if (collisionEnabled == true)
            {
                StartCrashSequence();
            }
                break;

         }    
   }

    void StartSuccessSequence()
    {   
        if(isTransitioning == false)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(Success);
            SuccessParticles.Play();
        }
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        Invoke ("LoadNextLevel", LevelLoadDelay);
    }

    void StartCrashSequence()
   {
            if(isTransitioning == false)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(Crash);
                CrashParticles.Play();
            }
            isTransitioning = true;
            GetComponent<Movement>().enabled = false;
            Invoke ("ReloadLevel", LevelLoadDelay);
            
   }


    void ReloadLevel()
    {   
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex +1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

}
