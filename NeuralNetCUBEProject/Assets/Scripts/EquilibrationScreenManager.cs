using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EquilibrationScreenManager : MonoBehaviour
{
    public List<Matrix[]> dataList;
    public List<Matrix[]> nothingList;
    public List<Matrix[]> leftList;
    public List<Matrix[]> rightList;
    public List<Matrix[]> bothList;
    public Text dataNumber;

    public Text nothingNumber;
    public Text leftNumber;
    public Text rightNumber;
    public Text bothNumber;

    public Slider nothingSlider;
    public Slider leftSlider;
    public Slider rightSlider;
    public Slider bothSlider;

    public int nothingVal = 0;
    public int leftVal = 0;
    public int rightVal = 0;
    public int bothVal = 0;

    public GameObject trainingScreen;
    public Button doneButton;

    public void Equilibrate()
    {
        if (nothingVal == 0 && leftVal == 0 && rightVal == 0 && bothVal == 0)
        {
            FindObjectOfType<Data>().list.Clear();
            SceneManager.LoadScene(1);
        }
        else
        {
            List<Matrix[]> equiData = new List<Matrix[]>();
            for (int i = 0; i < nothingVal; i++)
            {
                equiData.Add(nothingList[ i % nothingList.Count ]);
            }
            for (int i = 0; i < leftVal; i++)
            {
                equiData.Add(leftList[i % leftList.Count]);
            }
            for (int i = 0; i < rightVal; i++)
            {
                equiData.Add(rightList[i % rightList.Count]);
            }
            for (int i = 0; i < bothVal; i++)
            {
                equiData.Add(bothList[i % bothList.Count]);
            }
            FindObjectOfType<Data>().list = equiData;
            trainingScreen.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        FindObjectOfType<Data>().Shuffle();

        dataList = FindObjectOfType<Data>().list;
        dataNumber.text = dataList.Count.ToString();
        nothingSlider.maxValue = dataList.Count;
        leftSlider.maxValue = dataList.Count;
        rightSlider.maxValue = dataList.Count;
        bothSlider.maxValue = dataList.Count;

        nothingList = new List<Matrix[]>();
        leftList = new List<Matrix[]>();
        rightList = new List<Matrix[]>();
        bothList = new List<Matrix[]>();

        foreach(Matrix[] element in dataList)
        {
            if (element[1][0,0] == 0 && element[1][0, 1] == 0)
            {
                nothingList.Add(element);
            }
            else if (element[1][0, 0] == 1 && element[1][0, 1] == 0)
            {
                leftList.Add(element);
            }
            else if(element[1][0, 0] == 0 && element[1][0, 1] == 1)
            {
                rightList.Add(element);
            }
            else if (element[1][0, 0] == 1 && element[1][0, 1] == 1)
            {
                bothList.Add(element);
            }
        }

        nothingVal = nothingList.Count;
        leftVal = leftList.Count;
        rightVal = rightList.Count;
        bothVal = bothList.Count;

        nothingSlider.value = nothingVal;
        leftSlider.value = leftVal;
        rightSlider.value = rightVal;
        bothSlider.value = bothVal;

        nothingNumber.text = nothingVal.ToString();
        leftNumber.text = leftVal.ToString();
        rightNumber.text = rightVal.ToString();
        bothNumber.text = bothVal.ToString();

        if (nothingVal == 0)
        {
            nothingSlider.interactable = false;
        }
        if (leftVal == 0)
        {
            leftSlider.interactable = false;
        }
        if (rightVal == 0)
        {
            rightSlider.interactable = false;
        }
        if (bothVal == 0)
        {
            bothSlider.interactable = false;
        }
    }

    public void NothingSliderChanged(float val)
    {
        nothingVal = (int)val;
        nothingNumber.text = nothingVal.ToString();
    }

    public void LeftSliderChanged(float val)
    {
        leftVal = (int)val;
        leftNumber.text = leftVal.ToString();
    }

    public void RightSliderChanged(float val)
    {
        rightVal = (int)val;
        rightNumber.text = rightVal.ToString();
    }

    public void BothSliderChanged(float val)
    {
        bothVal = (int)val;
        bothNumber.text = bothVal.ToString();
    }
}
