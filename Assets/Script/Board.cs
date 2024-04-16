using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;
using Random = UnityEngine.Random;
using System.Security.Cryptography.X509Certificates;


public class Board : MonoBehaviour
{
    public GameObject card;

    private bool isArrived = false;

    void Start()
    {
        StartCoroutine(CreateCard());
    }


    /// <summary>
    /// ���� ���� �� ī�带 ��ġ�ϴ� �޼ҵ�
    /// ī�� ��ġ -> Start ���� -> ��带 ���� ���� ����
    /// </summary>
    public IEnumerator CreateCard()
    {
        AudioManager.instance.Stop();

        // ī���� �迭�� ���������� ���� ����
        // int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        int[] arr = new int[Resources.LoadAll<Sprite>("Images/CardImages").Length * 2] ;

        for (int q = 0; q < arr.Length; q++)
        {
            arr[q] = q / 2;
        }

        // �迭 �� ��ŭ �����ϰ� ����
        arr = arr.OrderBy(x => Random.Range(0f, arr.Length)).ToArray();

        int i = 0; 
        while(i < arr.Length)
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

            i++;
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
            Debug.Log("ī�� �� �� Ȧ�� �̰ų� 1�� �����Դϴ�.");
            Application.Quit();
        }
        if (CardCount / 2 < ImageCount)
        {
            Debug.Log("�̹����� ������ �����մϴ�.");
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
        AudioManager.instance.Play(AudioManager.instance.startSound);
        StopAllCoroutines();
    }
    /*
     *         
     *         int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
        arr = arr.OrderBy(x => Random.Range(0f, 7f)).ToArray();

        
        for (int i = 0; i < 16; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * 1.4f - 2.1f;
            float y = (i / 4) * 1.4f - 3.0f;

            Vector3 dest = new Vector3(x, y, 0f);
            StartCoroutine(go.GetComponent<Card>().Move(dest));
            go.GetComponent<Card>().Setting(arr[i]);
        }

        StopAllCoroutines();

        GameManager.instance.cardCount = arr.Length;
     */
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
