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
    public Text loadingBarText;
    public Button resetButton;
    public GameObject CorrectClassifiedPerClassTextDisplay;

    public Data data;
    public NeuralNetwork NN;
    public int updateUIDataCounter = 1000;

    private void OnEnable()
    {
        data = FindObjectOfType<Data>();
        NN = FindObjectOfType<NeuralNetwork>();
        dataCount.text = data.list.Count.ToString();
        batchSlider.maxValue = data.list.Count;
        batchSlider.value = Mathf.CeilToInt(batchSlider.maxValue * 0.05f);
        NN.batchsize = (int)batchSlider.value;
        BatchCount.text = batchSlider.value.ToString();
        NN.epochs = (int)epochSlider.value;
        NN.learningRate = learningRateSlider.value;
    }

    public void ResetButtonPress()
    {
        NN.ResetNetwork();
    }

    public void EpochSliderChange(float val)
    {
        NN.epochs = (int)val;
        EpochCount.text = val.ToString();
    }

    public void BatchSliderChange(float val)
    {
        NN.batchsize = (int)val;
        BatchCount.text = val.ToString();
    }

    public void LearningRateSliderChange(float val)
    {
        NN.learningRate = val;
        LearningRateCount.text = val.ToString("0.0##", System.Globalization.CultureInfo.InvariantCulture);
    }

    public void LoadingBarChanged()
    {
        float percentage = (100f * loadingBarSlider.value) / loadingBarSlider.maxValue;
        loadingBarText.text = percentage.ToString("0") + "%";
    }

    public void StopTraining()
    {
        SceneManager.LoadScene(1);
    }

    public void StartTraining()
    {
        loadingBarSlider.value = 0;
        loadingBarSlider.maxValue = NN.epochs * data.list.Count;
        LoadingBarChanged();
        DisableInteractions();
        meanErrorText1.SetActive(true);
        meanErrorText2.SetActive(true);
        meanErrorTextDisplay.SetActive(true);
        correctClassifiedText1.SetActive(true);
        correctClassifiedTextDisplay.SetActive(true);

        StartCoroutine("Train");
    }

    IEnumerator Train()
    {
        yield return null;

        for (int i = 0; i < NN.epochs; i++)
        {
            // Make sure, training data is always shuffled!
            data.Shuffle();

            int currentUIDataCounter = 0;
            float totalError = 0;

            foreach (Matrix[] piece in data.list)
            {
                float errorOnePiece = NN.Backpropagation(piece[0], piece[1]);
                // Debug.Log("Error for one piece: " + errorOnePiece);
                totalError += errorOnePiece;
                currentUIDataCounter++;

                if ((currentUIDataCounter % updateUIDataCounter) == 0)
                {
                    loadingBarSlider.value = i * data.list.Count + currentUIDataCounter;
                    LoadingBarChanged();
                    yield return null;
                }
            }

            // eventually update the last Batch which was smaller than batchsize!
            if (NN.batchIndex != 0)
            {
                NN.UpdateWeightsAndBiases();
                NN.batchIndex = 0;
            }

            float meanError = totalError / data.list.Count;
            // Debug.Log("Epoche: " + (i + 1) + "; Anzahl Daten: " + data.list.Count + "; Mean Error: " + meanError);
            meanErrorTextDisplay.GetComponent<Text>().text = meanError.ToString("0.0######", System.Globalization.CultureInfo.InvariantCulture);
            loadingBarSlider.value = data.list.Count * (i + 1);
            LoadingBarChanged();
            yield return null;
        }

        correctClassifiedTextDisplay.GetComponent<Text>().text = "wird berechnet...";
        CorrectClassifiedPerClassTextDisplay.SetActive(false);
        yield return null;

        int correct = 0;
        int[] correctPerClass = new int[4];
        foreach (Matrix[] piece in data.list)
        {
            Matrix output = NN.Feedforward(piece[0]);
            if (output[0, 0] == piece[1][0, 0] && output[0, 1] == piece[1][0, 1])
            {
                correct++;
                if(piece[1][0, 0] == 0 && piece[1][0, 1] == 0)
                {
                    correctPerClass[0]++;
                }
                if (piece[1][0, 0] == 1 && piece[1][0, 1] == 0)
                {
                    correctPerClass[1]++;
                }
                if (piece[1][0, 0] == 0 && piece[1][0, 1] == 1)
                {
                    correctPerClass[2]++;
                }
                if (piece[1][0, 0] == 1 && piece[1][0, 1] == 1)
                {
                    correctPerClass[3]++;
                }
            }
        }
        float correctPercentage = (correct * 100f) / data.list.Count;
        Debug.Log("N: " + ((correctPerClass[0] * 100f) / data.list.Count) + "--- L: " + ((correctPerClass[1] * 100f) / data.list.Count) + "--- R: " + ((correctPerClass[2] * 100f) / data.list.Count) + "--- B: " + ((correctPerClass[3] * 100f) / data.list.Count));
        // Debug.Log("Correct classified: " + correctPercentage + "%");
        NN.weightsHiddenOutput.Print();
        NN.weightsInputHidden.Print();
        NN.biasHidden.Print();
        NN.biasOutput.Print();
        correctClassifiedTextDisplay.GetComponent<Text>().text = correctPercentage.ToString("#0.0#####", System.Globalization.CultureInfo.InvariantCulture) + "%";
        CorrectClassifiedPerClassTextDisplay.SetActive(true);
        CorrectClassifiedPerClassTextDisplay.GetComponent<Text>().text = "( " + ((correctPerClass[0] * 100f) / data.list.Count).ToString("#0.0", System.Globalization.CultureInfo.InvariantCulture) + "% + " + ((correctPerClass[1] * 100f) / data.list.Count).ToString("#0.0", System.Globalization.CultureInfo.InvariantCulture) + "% + " + ((correctPerClass[2] * 100f) / data.list.Count).ToString("#0.0", System.Globalization.CultureInfo.InvariantCulture) + "% + " + ((correctPerClass[3] * 100f) / data.list.Count).ToString("#0.0", System.Globalization.CultureInfo.InvariantCulture) + "% )";
        yield return null;

        EnableInteractions();
        yield return null;
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
        resetButton.interactable = false;
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
        resetButton.interactable = true;
    }
}
