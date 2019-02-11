using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataToTrainingTransition : MonoBehaviour
{
    public GameObject trainingScreen;

    public void Transition()
    {
        if(FindObjectOfType<Data>().list.Count == 0)
        {
            SceneManager.LoadScene(1);
            Debug.Log("There is no data to train with!");
        } else
        {
            trainingScreen.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        FindObjectOfType<Data>().Save();
    }

    public void ForgetData()
    {
        FindObjectOfType<Data>().Forget();
    }
}
