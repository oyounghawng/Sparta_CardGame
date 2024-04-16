using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using System.Security.Cryptography.X509Certificates;

[System.Serializable]
public struct MapArray
{
    public bool[] map;
}


public class Board : MonoBehaviour
{
    public GameObject card;
    public int CardCount;
    public int ImageCount = 5;
    public int[] arr;

    private int level;

    public bool[] CardMap = new bool[20];

    public MapArray[] stageArray;

    void Start()
    {
        level = GameManager.instance.level;
        foreach (bool b in stageArray[level].map)
        {
            if (b) CardCount++;
        }
        if (CardCount % 2 == 1 || CardCount < 2)
        {
            Debug.Log("카드 수 가 홀수 이거나 1개 이하입니다.");
            Application.Quit();
        }
        if (CardCount / 2 < ImageCount)
        {
            Debug.Log("이미지의 종류가 부족합니다.");
            Application.Quit();
        }

        arr = new int[CardCount];
        CreateDuplicateRandomArray();

        int temp = 0;
        for (int i = 0; i < 20; i++)
        {
            if (stageArray[level].map[i])
            {
                GameObject go = Instantiate(card, this.transform);

                float x = (i % 4) * 1.4f - 2.1f;
                float y = 1.6f - (i / 4) * 1.4f;

                go.transform.position = new Vector2(x, y);
                go.GetComponent<Card>().Setting(arr[temp]);
                temp++;
            }
        }

        GameManager.instance.cardCount = arr.Length;
    }
    void CreateDuplicateRandomArray()
    {
        for (int i = 0; i < CardCount ; )
        {
                int currentNumber = Random.Range(0, ImageCount);
                arr[i] = currentNumber;
                arr[i + 1] = currentNumber;
                i+=2;
        }
        arr = arr.OrderBy(x => Random.Range(0, ImageCount-1)).ToArray();
    }

}
