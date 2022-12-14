using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    public MusicToggle musicToggle;                    // Scripting object that controls music note visuals

    // Used to move the player upon fail
    public CharacterController2D controller;
    public PlayerMovement playerMovement;
    public PowerUps powerUps;
    public Rigidbody2D player;
    public GameObject bottomBoundary;
    public GameObject topBoundary;
    public float rangeMin = -400f;
    public float rangeMax = 400f;

    // Variables used to add spin force to character upon fail
    [SerializeField] public Transform playerTransform;
    Vector3 rotVector;
    float rotAmount;

    private void Start()
    {   
        // Initializes rotation vector
        playerTransform.eulerAngles = rotVector;
        // Selects random value for rotation in range of 60 degrees
        rotAmount = Random.Range(-60f, 60f);
        powerUps = FindObjectOfType<PowerUps>();

    }

    private void Update()
    {
        if (playerMovement.isGravityInverted)
        {
            topBoundary.SetActive(true);
            bottomBoundary.SetActive(false);
        }
        else
        {
            topBoundary.SetActive(false);
            bottomBoundary.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        // If collision is detected between player dangers or boundaries end the game
        if(collision.collider.tag == "Dangers")
        {
            PlayerLaunch();
            FindObjectOfType<GameManager>().EndGame();
        }

        // If collision is detected with boundaries and Wraparound is disabled, end game.
        if (collision.collider.tag == "Boundaries" && (!Input.GetButton("Wraparound") || !powerUps.hasWraparoundPower))
        {
            // Disables colliders of boundary boxes to let player pass through when hit
            // DO NOT SET TO DESTROY WILL CREATE ERRORS
            bottomBoundary.GetComponent<BoxCollider2D>().enabled = false;
            topBoundary.GetComponent<BoxCollider2D>().enabled = false;
            FindObjectOfType<GameManager>().EndGame();
        }

        //If collision is detected with boundaries and Wraparound is enabled, wrap to other side.
        if (collision.collider.tag == "Boundaries" && Input.GetButton("Wraparound") && powerUps.hasWraparoundPower)
        {
            //Wrap to bottom if gravity inverted
            if (playerMovement.isGravityInverted)
                player.transform.position = new Vector3(player.transform.position.x, bottomBoundary.transform.position.y, player.transform.position.z);
            //Wrap to top if gravity normal.
            else
                player.transform.position = new Vector3(player.transform.position.x, topBoundary.transform.position.y, player.transform.position.z);
        }

  

        if (collision.collider.tag == "Enemy")
        {   
            // Get the musicToggle script from the object collided with
            musicToggle = collision.gameObject.GetComponent<MusicToggle>();

            // If the script has inverted enemy tag set to false
            if (!musicToggle.invertedEnemy)
            {
                PlayerLaunch();
                FindObjectOfType<GameManager>().EndGame();
            }
            // If the script has inverted enemy tag set to true
            else if (musicToggle.invertedEnemy)
            {
                //Get collided enemy game object and destroy it.
                GameObject enemy = collision.collider.gameObject;
                Destroy(enemy);
            }

        }

        if (collision.collider.tag == "EtherealPlatform")
        {
            powerUps.hasEtherealPower = true;
            GameObject power = collision.collider.gameObject;
            Destroy(power);
        }

        if (collision.collider.tag == "InvertKill")
        {
            powerUps.hasInvertKillPower = true;
            GameObject power = collision.collider.gameObject;
            Destroy(power);
        }

        if (collision.collider.tag == "Wraparound")
        {
            powerUps.hasWraparoundPower = true;
            GameObject power = collision.collider.gameObject;
            Destroy(power);
        }

        if (collision.collider.tag == "InvertGravity")
        {
            powerUps.hasGravityPower = true;
            GameObject power = collision.collider.gameObject;
            Destroy(power);
        }

    }

    // Launches player
    public void PlayerLaunch()
    {
        // Disable controller scripts so player can't control flight
        controller.enabled = false;
        playerMovement.enabled = false;

        // Selects random value for rotation in range of 60 degrees
        rotAmount = Random.Range(30f, 90f);

        // Function to randomly select if the rotation is negative or not
        // This is to have the rotation picked be between -90 to -30 and 30 to 90 degrees
        int invertRot = Random.Range(0, 1);
        if (invertRot == 1)
        {
            rotAmount = -rotAmount;
        }

        //Removes z axis rotation restrictions
        player.constraints = RigidbodyConstraints2D.None;
        Vector2 launchVector = new Vector2(Random.Range(rangeMin, rangeMax), Random.Range(0, rangeMax));  
        player.AddForce(launchVector);
        playerTransform.Rotate(0, 0, rotAmount);          // Rotates character
    }
}
