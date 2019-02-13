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
        data = FindObjectOfType<Data>();
    }

    // We marked this as "Fixed"Update because we
    // are using it to mess with physics.
    void FixedUpdate()
    {
        Matrix[] userData = new Matrix[2];
        Matrix measureValues = new Matrix(1, FindObjectOfType<NeuralNetwork>().inputNeurons);
        Matrix userDecisions = new Matrix(1, 2);

        // Position Left Right in [-8.5,8.5] -> map to [0,1] // maybe good with 1/(1+e^-0.5x)
        // Debug.Log(player.position.x);
        // Debug.Log(MapTo01(player.position.x, -8.5f, 8.5f));
        measureValues[0, 0] = MapTo01(player.position.x, -8f, 8f); // changed! -7.5f, 7.5f?!
        // measureValues[0, 0] = MapWithSigmoid(0.5f, player.position.x);

        // Velocity Left Right in [-25,25] -> map to [0,1] // maybe good with 1/(1+e^-0.18x)
        // Debug.Log(rb.velocity.x);
        // Debug.Log(MapTo01(rb.velocity.x, -25f, 25f));
        // measureValues[0, 1] = MapTo01(rb.velocity.x, -10f, 10f); // changed!
        measureValues[0, 1] = MapWithSigmoid(0.18f, rb.velocity.x);

        // Distance Front in [0,50] -> map to [0,1]
        // Debug.Log(50 - (player.position.z % 50));
        // Debug.Log(MapTo01(50 - (player.position.z % 50), 0, 50));
        measureValues[0, 2] = MapTo01(50 - (player.position.z % 50), 0, 50);

        // Obstacles next
        // Debug.Log(Mathf.FloorToInt(player.position.z / 50));
        for(int i = 3; i <= (FindObjectOfType<NeuralNetwork>().inputNeurons - 1); i++)
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
        data.newData.Add(userData);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 33.8f);
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

    private float MapWithSigmoid(float a, float val)
    {
        return 1.0f / (1.0f + Mathf.Exp(-a * val));
    }
}
