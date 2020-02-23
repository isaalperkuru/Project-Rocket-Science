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
            
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioData.isPlaying)
            {
                audioData.Play();
            }
        }
        else
        {
            audioData.Stop();
        }
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
                print("hit finish");
                state = State.Transcending;
                Invoke("LoadingNextScene" , 1f);
                break;
            default:
                print("Dead");
                state = State.Dying;
                Invoke("DeadScene", 2f);
                break;
        }
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
