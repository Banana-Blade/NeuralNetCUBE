using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public List<Matrix[]> list;

    private void Start()
    {
        list = new List<Matrix[]>();
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
}
