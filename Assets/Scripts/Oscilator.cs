using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour {

    [SerializeField] Vector3 MovementVector;
    [SerializeField] float period = 2f;

    
    float movementFactor; //0 not move 1 full move


    Vector3 startingPos;
	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave / 2f + 0.5f;

        Vector3 offset = MovementVector * movementFactor;
        transform.position = startingPos + offset;
	}
}
