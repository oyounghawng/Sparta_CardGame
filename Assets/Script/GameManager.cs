using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeTxt;
    public Text limitTimeTxt; // ���� �ð� �ؽ�Ʈ
    public GameObject minusTimeTxt; // �ð� ���� �˷��ִ� �ؽ�Ʈ
    public GameObject endTxt;
    public Card firstCard;
    public Card secondCard;

    float time = 30;
    bool isCount = false; // ī��Ʈ �ٿ� �����ϴ��� ���ϴ���
    public int cardCount = 0;

    AudioSource audioSource;
    public AudioClip clip;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        if(time < 0f)
        {
            endTxt.SetActive(true);
            Time.timeScale = 0f;
        }

        // firstCard�� null�� �ƴϰ� SecondCard�� null�� �� 5�� ���� ī�� �ݱ�
        if (firstCard != null && secondCard == null)
        {
            
            if (!isCount)
            {
                StartCoroutine(CountDown()); // �ڷ�ƾ ����
            }
        }
    }

    IEnumerator CountDown()
    {
        isCount = true;
        limitTimeTxt.gameObject.SetActive(true);

        float countdownTime = 5f;

        if (secondCard != null)
        {
            Debug.Log("�ι�°ī�弱��");
            StopCoroutine(CountDown());
            limitTimeTxt.gameObject.SetActive(false);
        }

        while (countdownTime > 0f)
        {
            limitTimeTxt.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime -= 1f;
        }

        // ī��Ʈ�ٿ� ���� �� firstCard ������
        if(firstCard != null)
        {
            firstCard.CloseCard();
            firstCard = null;
        }

        limitTimeTxt.gameObject.SetActive(false);
        isCount = false;
    }

    public void isMatched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0)
            {
                Time.timeScale = 0;
                endTxt.SetActive(true);
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();

            minusTimeTxt.SetActive(true);
            time -= 1; // �����߸� �ð� -1��
            Invoke("TimeMinus", 0.5f); // 0.5�ʵ��� �ؽ�Ʈ ����
        }

        firstCard = null;
        secondCard = null;
    }
    public void TimeMinus()
    {
        minusTimeTxt.SetActive(false);
    }
}
