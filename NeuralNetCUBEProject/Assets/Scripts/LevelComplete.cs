using System.Collections;
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
        float totalError = 0;
        foreach(Matrix[] piece in data.list)
        {
            // Debug.Log(NN.Backpropagation(piece[0], piece[1]));
            totalError += NN.Backpropagation(piece[0], piece[1]);
        }
        Debug.Log(totalError / data.list.Count);

        trainingStage.SetActive(true);
        trainingScreen.SetActive(false);
    }
}
