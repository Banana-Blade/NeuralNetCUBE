using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainOrTestMenuManager : MonoBehaviour
{
    public void StartTraining()
    {
        SceneManager.LoadScene(2);
    }

    public void StartTesting()
    {
        SceneManager.LoadScene(3);
    }
}
