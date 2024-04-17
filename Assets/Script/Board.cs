using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

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

    private int level = 0;

    //public bool[] CardMap = new bool[20];

    public MapArray[] stageArray;

    private bool isArrived = false;

    void Start()
    {
        StartCoroutine(CreateCard());
    }

    /// <summary>
    /// 게임 시작 전 카드를 배치하는 메소드
    /// 카드 배치 -> Start 사운드 -> 모드를 게임 모드로 변경
    /// </summary>
    public IEnumerator CreateCard()
    {
        AudioManager.instance.Stop();

        // 카드의 배열을 수동적으로 생성 문제
        // int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        ImageCount = Resources.LoadAll<Sprite>("Images/TeamPic").Length;
        //for (int q = 0; q < arr.Length; q++)
        //{
        //    arr[q] = q / 2;
        //}

        // 배열 수 만큼 랜덤하게 나눔
        //arr = arr.OrderBy(x => Random.Range(0f, arr.Length)).ToArray();

        foreach (bool b in stageArray[level].map)
        {
            if (b) CardCount++;
        }
        /*
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
        */

        //arr = new int[CardCount];
        //int i = 0;
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
                Vector3 dest = new Vector3(x, y, 0f);
                go.transform.position = dest + new Vector3(x + 2f, y - 2f, 0);
                Card cd = go.GetComponent<Card>();
                cd.Dest = dest;
                cd.Setting(arr[temp]);
                yield return new WaitUntil(() => Vector3.Distance(dest, cd.transform.position) <= 0f);
                temp++;
            }
        }
        GameManager.instance.cardCount = arr.Length;
        AudioManager.instance.Play(AudioManager.instance.startSound);
        StopAllCoroutines();
    }
    void CreateDuplicateRandomArray()
    {
        for (int i = 0; i < CardCount;)
        {
            int currentNumber = Random.Range(0, ImageCount);
            arr[i] = currentNumber;
            arr[i + 1] = currentNumber;
            i += 2;
        }
        arr = arr.OrderBy(x => Random.Range(0, ImageCount - 1)).ToArray();
    }
}
