﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public float restartDelay = 1f;
    public GameObject completeLevelUI;
    public PlayerMovement movement;
    public bool [,] boolObstacles;
    public GameObject[] realObstacles;
    public Text scoreText;
    public Text finalScoreText;
    public GameObject goodJob;

    void Start() // initialize all Obstacles randomly with repect to the difficulty
    {
        float dif = 0.5f;
        if(FindObjectOfType<NeuralNetwork>() != null)
        {
            dif = FindObjectOfType<NeuralNetwork>().difficulty;
        }
        boolObstacles = new bool[20,8];
        for(int i = 0; i < boolObstacles.GetLength(0); i++)
        {
            for(int j = 0; j < boolObstacles.GetLength(1); j++)
            {
                boolObstacles[i,j] = (Random.value <= dif);
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
        // Debug.Log(FindObjectOfType<Data>().list.Count); // ~2950 Elements
        finalScoreText.text = scoreText.text;
        goodJob.SetActive(true);
    }

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            finalScoreText.text = scoreText.text;
            movement.enabled = false;   // Disable the players movement.
            gameHasEnded = true;
            Debug.Log("GAME OVER");
            completeLevelUI.SetActive(true);
            // Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
