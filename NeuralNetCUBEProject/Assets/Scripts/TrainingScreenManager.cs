using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrainingScreenManager : MonoBehaviour
{
    public Text dataCount;
    public Text EpochCount;
    public Text BatchCount;
    public Text LearningRateCount;

    public Slider batchSlider;
    public GameObject startTrainingButton;
    public GameObject loadingBar;
    public GameObject meanErrorText1;
    public GameObject meanErrorText2;
    public GameObject meanErrorTextDisplay;
    public GameObject correctClassifiedText1;
    public GameObject correctClassifiedTextDisplay;
    public Slider loadingBarSlider;
    public Button equilibrationButton;
    public Slider epochSlider;
    public Slider learningRateSlider;
    public Button doneButton;

    private void OnEnable()
    {
        Data data = FindObjectOfType<Data>();
        dataCount.text = data.list.Count.ToString();
        batchSlider.maxValue = data.list.Count;
        batchSlider.value = batchSlider.maxValue;
        BatchCount.text = batchSlider.value.ToString();
    }

    public void EpochSliderChange(float val)
    {
        FindObjectOfType<NeuralNetwork>().epochs = (int) val;
        EpochCount.text = val.ToString();
    }

    public void BatchSliderChange(float val)
    {
        FindObjectOfType<NeuralNetwork>().batchsize = (int)val;
        BatchCount.text = val.ToString();
    }

    public void LearningRateSliderChange(float val)
    {
        FindObjectOfType<NeuralNetwork>().learningRate = val;
        LearningRateCount.text = val.ToString("0.0##", System.Globalization.CultureInfo.InvariantCulture);
    }

    public void StopTraining()
    {
        SceneManager.LoadScene(1);
    }

    public void StartTraining()
    {
        loadingBarSlider.value = 0;
        loadingBarSlider.maxValue = FindObjectOfType<NeuralNetwork>().epochs * FindObjectOfType<Data>().list.Count;
        DisableInteractions();
        meanErrorText1.SetActive(true);
        meanErrorText2.SetActive(true);
        meanErrorTextDisplay.SetActive(true);
        correctClassifiedText1.SetActive(true);
        correctClassifiedTextDisplay.SetActive(true);

        // TODO Start training and dont forget to EnableInteractions() after training!
        // change loadingBar value and text on the bar while training
    }

    public void DisableInteractions()
    {
        startTrainingButton.SetActive(false);
        loadingBar.SetActive(true);
        equilibrationButton.interactable = false;
        epochSlider.interactable = false;
        batchSlider.interactable = false;
        learningRateSlider.interactable = false;
        doneButton.interactable = false;
    }

    public void EnableInteractions()
    {
        startTrainingButton.SetActive(true);
        loadingBar.SetActive(false);
        equilibrationButton.interactable = true;
        epochSlider.interactable = true;
        batchSlider.interactable = true;
        learningRateSlider.interactable = true;
        doneButton.interactable = true;
    }
}
