using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public List<Matrix[]> list;
    public List<Matrix[]> newData;

    // So that there is only one Data Object in the scene and will never be overwritten!
    public static Data instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        list = new List<Matrix[]>();
        newData = new List<Matrix[]>();
    }

    public void Shuffle()
    {
        // Fisher–Yates shuffle
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Matrix[] value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Save()
    {
        foreach (Matrix[] piece in newData)
        {
            list.Add(piece);
        }
        newData.Clear();
    }

    public void Forget()
    {
        newData.Clear();
    }
}
