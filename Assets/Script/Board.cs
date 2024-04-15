using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using System.Security.Cryptography.X509Certificates;


public class Board : MonoBehaviour
{
    public GameObject card;
    public int CardCount;
    public int ImageCount = 8;
    public int[] arr;
    private List<int> randomList = new List<int>();

    public bool[] CardMap = new bool[16];

    void Start()
    {
        foreach(bool b in CardMap)
        {
            if (b) CardCount++;
        }
        if(CardCount % 2 == 1 || CardCount < 2)
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
        CreateUnDuplicateRandomArray();

        int temp = 0;
        for (int i = 0; i < 16; i++)
        {
            if (CardMap[i])
            {
                GameObject go = Instantiate(card, this.transform);

                float x = (i % 4) * 1.4f - 2.1f;
                float y = 1.2f - (i / 4) * 1.4f;

                go.transform.position = new Vector2(x, y);
                go.GetComponent<Card>().Setting(arr[temp]);
                temp++;
            }
        }

        GameManager.instance.cardCount = arr.Length;
    }
    void CreateUnDuplicateRandomArray()
    {
        int currentNumber = Random.Range(0, ImageCount);
        for (int i = 0; i < CardCount ; )
        {
            if (randomList.Contains(currentNumber))
            {
                currentNumber = Random.Range(0, ImageCount);
            }
            else
            {
                randomList.Add(currentNumber);
                arr[i] = currentNumber;
                arr[i + 1] = currentNumber;
                i+=2;
            }
        }
        arr = arr.OrderBy(x => Random.Range(0, ImageCount-1)).ToArray();
    }

}
