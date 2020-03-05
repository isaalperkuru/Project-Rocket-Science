using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioData;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winClip;
    [SerializeField] AudioClip deadClip;
    [SerializeField] ParticleSystem thrustParticle;
    [SerializeField] ParticleSystem winParticle;
    [SerializeField] ParticleSystem deadParticle;
    [SerializeField] float levelLoadDelay = 2f;

    enum State { Dying, Alive , Transcending};
    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioData = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive) { 
        Thrust();
        Rotate();
        }
    }
    private void Thrust()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioData.Stop();
            thrustParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioData.isPlaying)
        {
            audioData.PlayOneShot(mainEngine);
        }
        thrustParticle.Play();
    }

    private void Rotate()
    {
        
        float rotationSpeed = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSeq();
                break;
            default:
                StartFailSeq();
                break;
        }
    }

    private void StartFailSeq()
    {
        state = State.Dying;
        audioData.Stop();
        thrustParticle.Stop();
        audioData.PlayOneShot(deadClip);
        deadParticle.Play();
        Invoke("DeadScene", levelLoadDelay);
    }

    private void StartSuccessSeq()
    {
        state = State.Transcending;
        audioData.Stop();
        thrustParticle.Stop();
        audioData.PlayOneShot(winClip);
        winParticle.Play();
        Invoke("LoadingNextScene", levelLoadDelay);
    }

    private void DeadScene()
    {
        
        SceneManager.LoadScene(0);
    }

    private void LoadingNextScene()
    {
        
        if (SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            SceneManager.LoadScene(1);
        }
        else if (SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            SceneManager.LoadScene(1);
        }
    }
}
