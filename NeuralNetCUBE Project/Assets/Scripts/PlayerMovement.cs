using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // This is a reference to the Rigidbody component called "rb"
    public Rigidbody rb;
    public Transform ground;
    public Transform player;

    public float forwardForce = 2000f;  // Variable that determines the forward force
    public float sidewaysForce = 500f;  // Variable that determines the sideways force

    // We marked this as "Fixed"Update because we
    // are using it to mess with physics.
    void FixedUpdate()
    {
        // Add a forward force
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        if (Input.GetKey("d") || Input.GetKey("right"))  // If the player is pressing the "d" or rightarrow key
        {
            // Add a force to the right
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a") || Input.GetKey("left"))  // If the player is pressing the "a" or leftarrow key
        {
            // Add a force to the left
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if ( Mathf.Abs(player.position.x - ground.position.x)>=  0.5f * (player.lossyScale.x + ground.lossyScale.x) )
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
