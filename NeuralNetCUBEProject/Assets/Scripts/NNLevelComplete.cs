using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NNLevelComplete : MonoBehaviour
{
    public GameObject finalScore;
    public GameObject complete;
    public GameObject goodJob;
    public GameObject trainingStage;
    public Text endScreenScore;

    public void LoadTrainingStage()
    {
        finalScore.SetActive(false);
        complete.SetActive(false);
        goodJob.SetActive(false);
        trainingStage.SetActive(true);
        endScreenScore.text = finalScore.GetComponent<Text>().text;
    }

    public void Finished()
    {
        SceneManager.LoadScene(1);
    }
}
