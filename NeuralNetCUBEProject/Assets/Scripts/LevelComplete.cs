﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public GameObject finalScore;
    public GameObject complete;
    public GameObject goodJob;
    public GameObject trainingStage;
    public GameObject trainingScreen;
    public Data data;
    public NeuralNetwork NN;
    public int epochs = 1;

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadTrainingStage()
    {
        finalScore.SetActive(false);
        complete.SetActive(false);
        goodJob.SetActive(false);
        trainingStage.SetActive(true);
        NN = FindObjectOfType<NeuralNetwork>();
    }

    public void Finished()
    {
        SceneManager.LoadScene(1);
    }

    public void StartTraining()
    {
        trainingStage.SetActive(false);
        trainingScreen.SetActive(true);
        StartCoroutine("Train");
    }

    IEnumerator Train()
    {
        yield return null;

        for (int i = 0; i < epochs; i++)
        {
            // Make sure, training data is always shuffled!
            data.Shuffle();
            float totalError = 0;
            foreach (Matrix[] piece in data.list)
            {
                // Debug.Log("Error for one piece: " + NN.Backpropagation(piece[0], piece[1]));
                totalError += NN.Backpropagation(piece[0], piece[1]);
            }
            Debug.Log("Anzahl Daten: " + data.list.Count);
            Debug.Log("Mean Error: " + totalError / data.list.Count);
        }

        int correct = 0;
        foreach (Matrix[] piece in data.list)
        {
            Matrix output = NN.Feedforward(piece[0]);
            if (output[0, 0] == piece[1][0, 0] && output[0, 1] == piece[1][0, 1])
            {
                correct++;
            }
        }
        Debug.Log("Correct classified: " + (correct * 100f) / data.list.Count + "%");
        
        trainingStage.SetActive(true);
        trainingScreen.SetActive(false);
    }
}
