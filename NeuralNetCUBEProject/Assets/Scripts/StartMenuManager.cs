using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public NeuralNetwork NN;
    public Text DiffNumber;
    public Text HiddenNumber;
    public Text RandomNumber;
    public Text LearningNumber;

    public void LoadTrainOrTest()
    {
        SceneManager.LoadScene(1);
    }

    public void DifficultySliderChanged(float dif)
    {
        NN.difficulty = dif;
        DiffNumber.text = (dif * 100f).ToString("0", System.Globalization.CultureInfo.InvariantCulture) +"%";
    }

    public void HiddenNeuronsSliderChanged(float num)
    {
        NN.hiddenNeurons = (int)num;
        HiddenNumber.text = num.ToString();
    }

    public void RandomizeSliderChanged(float ran)
    {
        NN.randomizeMin = -ran;
        NN.randomizeMax = ran;
        RandomNumber.text = "[-" + ran.ToString("0.0#", System.Globalization.CultureInfo.InvariantCulture) + "," + ran.ToString("0.0#", System.Globalization.CultureInfo.InvariantCulture) + "]";
    }

    public void LearningRateSliderChanged(float rate)
    {
        NN.learningRate = rate;
        LearningNumber.text = rate.ToString("0.0##", System.Globalization.CultureInfo.InvariantCulture);
    }
}
