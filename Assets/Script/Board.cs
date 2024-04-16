using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;


public class Board : MonoBehaviour
{
    public GameObject card;
    public int CardCount;
    public int ImageCount;
    public int[] arr;
    private List<int> randomList = new List<int>();

    public bool[] CardMap = new bool[16];

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
        int[] arr = new int[Resources.LoadAll<Sprite>("Images/TeamPic").Length * 2];
        ImageCount = arr.Length/2;
        for (int q = 0; q < arr.Length; q++)
        {
            arr[q] = q / 2;
        }

        // 배열 수 만큼 랜덤하게 나눔
        //arr = arr.OrderBy(x => Random.Range(0f, arr.Length)).ToArray();
        foreach (bool b in CardMap)
        {
            if (b) CardCount++;
        }
        /*
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
        //CreateUnDuplicateRandomArray();
        int i = 0;
        while (i < CardCount)
        {
            if (CardMap[i])
            {

                GameObject go = Instantiate(card, this.transform);
                float x = (i % 4) * 1.4f - 2.1f;
                float y = (i / 4) * 1.4f - 3.0f;

                Vector3 dest = new Vector3(x, y, 0f);
                go.transform.position = dest + new Vector3(x + 2f, y - 2f, 0);
                Card cd = go.GetComponent<Card>();
                cd.Dest = dest;
                cd.Setting(arr[i]);

                yield return new WaitUntil(() => Vector3.Distance(dest, cd.transform.position) <= 0f);
            }
            i++;
        }
        GameManager.instance.cardCount = arr.Length;
        AudioManager.instance.Play(AudioManager.instance.startSound);
        StopAllCoroutines();
    }
    void CreateUnDuplicateRandomArray()
    {
        int currentNumber = Random.Range(0, ImageCount);
        for (int i = 0; i < CardCount;)
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
                i += 2;
            }
        }
        arr = arr.OrderBy(x => Random.Range(0, ImageCount - 1)).ToArray();
    }

}
