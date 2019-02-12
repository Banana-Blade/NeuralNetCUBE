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
    }

    public void Finished()
    {
        SceneManager.LoadScene(1);
    }
}
