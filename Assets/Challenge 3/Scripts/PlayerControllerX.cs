using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private float upperBound = 15f;
    private Rigidbody playerRb; // Physics Variables
    
    //particle system variables 
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    // Sound variables
    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && transform.position.y <= upperBound)
        {
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime, ForceMode.Impulse);
        }
        if (transform.position.y > upperBound)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.down * floatForce * Time.deltaTime, ForceMode.Impulse);
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        //if the player collides with the ground,
        if (other.gameObject.CompareTag("Ground"))
        {
             playerRb.velocity = new Vector3(0,playerRb.velocity.y * (-1), 0);
             //Play the sound once the ballon touches the ground
             playerAudio.PlayOneShot(bounceSound, 1.0f);

        }
       

    }

}
