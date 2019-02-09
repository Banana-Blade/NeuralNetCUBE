using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // This is a reference to the Rigidbody component called "rb"
    public Rigidbody rb;
    public Transform ground;
    public Transform player;

    public float forwardForce = 40f;  // Variable that determines the forward force
    public float sidewaysForce = 50f;  // Variable that determines the sideways force

    public Data data;
    public GameManager manager;

    void Start()
    {
        rb.velocity = new Vector3(0, 0, 33.8f);
    }

    // We marked this as "Fixed"Update because we
    // are using it to mess with physics.
    void FixedUpdate()
    {
        Matrix[] userData = new Matrix[2];
        Matrix measureValues = new Matrix(1, 11);
        Matrix userDecisions = new Matrix(1, 2);

        // Position Left Right in [-8.5,8.5] -> map to [0,1]
        // Debug.Log(player.position.x);
        // Debug.Log(MapTo01(player.position.x, -8.5f, 8.5f));
        measureValues[0, 0] = MapTo01(player.position.x, -8.5f, 8.5f);

        // Velocity Left Right in [-25,25] -> map to [0,1]
        // Debug.Log(rb.velocity.x);
        // Debug.Log(MapTo01(rb.velocity.x, -25f, 25f));
        measureValues[0, 1] = MapTo01(rb.velocity.x, -25f, 25f);

        // Distance Front in [0,50] -> map to [0,1]
        // Debug.Log(50 - (player.position.z % 50));
        // Debug.Log(MapTo01(50 - (player.position.z % 50), 0, 50));
        measureValues[0, 2] = MapTo01(50 - (player.position.z % 50), 0, 50);

        // Obstacles next
        // Debug.Log(Mathf.FloorToInt(player.position.z / 50));
        for(int i = 3; i<=10; i++)
        {
            measureValues[0, i] = manager.boolObstacles[Mathf.FloorToInt(player.position.z / 50), i-3] ? 1f: 0f;
        }
        
        // Debug.Log(rb.velocity);
        // Add a forward force
        rb.AddForce(0, 0, forwardForce * Time.deltaTime, ForceMode.VelocityChange);

        if (Input.GetKey("d") || Input.GetKey("right"))  // If the player is pressing the "d" or rightarrow key
        {
            // Add a force to the right
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            userDecisions[0,1] = 1;
        } else
        {
            userDecisions[0, 1] = 0;
        }

        if (Input.GetKey("a") || Input.GetKey("left"))  // If the player is pressing the "a" or leftarrow key
        {
            // Add a force to the left
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            userDecisions[0, 0] = 1;
        }
        else
        {
            userDecisions[0, 0] = 0;
        }

        if ( Mathf.Abs(player.position.x - ground.position.x)>=  0.5f * (player.lossyScale.x + ground.lossyScale.x) )
        {
            FindObjectOfType<GameManager>().EndGame();
        }

        userData[0] = measureValues;
        userData[1] = userDecisions;
        // userData[0].Print(); // works fine!!!
        // userData[1].Print(); // works fine!!!
        data.list.Add(userData);
    }

    // mapping val linearly from [minIs,maxIs] to [minShould,maxShould]
    private float Map (float val, float minIs, float maxIs, float minShould, float maxShould)
    {
        return ((val - minIs) / (maxIs - minIs)) * (maxShould - minShould) + minShould;
    }

    // mapping val linearly from [minIs,maxIs] to [0,1] and clamp it to be sure!
    private float MapTo01(float val, float minIs, float maxIs)
    {
        return Mathf.Clamp01(((val - minIs) / (maxIs - minIs)));
    }
}
