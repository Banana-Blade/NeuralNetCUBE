using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataCountUpdate : MonoBehaviour
{
    public Text dataCount;

    void OnEnable()
    {
        Data data = FindObjectOfType<Data>();
        dataCount.text = data.newData.Count + " Datensätzen";
        Debug.Log("New Data: " + data.newData.Count + "; Old Data: " + data.list.Count);
    }
}
