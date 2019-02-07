using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Range(0,1)]
    public float difficulty = 0.5f;
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;
    public PlayerMovement movement;
    public bool [,] boolObstacles;
    public GameObject[] realObstacles;

    void Start() // initialize all Obstacles randomly with repect to the difficulty
    {
        boolObstacles = new bool[20,8];
        for(int i = 0; i < boolObstacles.GetLength(0); i++)
        {
            for(int j = 0; j < boolObstacles.GetLength(1); j++)
            {
                boolObstacles[i,j] = (Random.value <= difficulty);
            }
            // at least one place has to be free!
            boolObstacles[i, Random.Range(0, 8)] = false;
        }
        // last obstacles missing (END)
        for(int k = 0; k < boolObstacles.GetLength(1); k++)
        {
            boolObstacles[19, k] = false;
        }
        
        for (int i = 0; i < realObstacles.Length; i++)
        {
            realObstacles[i].SetActive(boolObstacles[Mathf.FloorToInt(i / 8), i % 8]);
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        movement.enabled = false;
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
