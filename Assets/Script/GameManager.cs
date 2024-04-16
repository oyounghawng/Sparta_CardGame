using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeTxt;
    public Text limitTimeTxt; // 제한 시간 텍스트
    public GameObject minusTimeTxt; // 시간 감소 알려주는 텍스트
    public GameObject endTxt;
    public Card firstCard;
    public Card secondCard;

    float time = 30f;
    float countdownTime = 5f;
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

        // firstCard가 null이 아니고 SecondCard가 null일 때 5초 세고 카드 닫기
        // 첫번째 카드를 눌렀을 때
        if (firstCard != null && secondCard == null)
        {
            countdownTime -= Time.deltaTime;
            limitTimeTxt.gameObject.SetActive(true);
            limitTimeTxt.text = countdownTime.ToString("N0");

            if (countdownTime < 1f)
            {
                limitTimeTxt.gameObject.SetActive(false);
                firstCard.CloseCard();
                firstCard = null;
            }
        }
        else
        {
            limitTimeTxt.gameObject.SetActive(false);
            countdownTime = 5f;
        }
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
            time -= 1; // 못맞추면 시간 -1초
            Invoke("TimeMinus", 0.5f); // 0.5초동안 텍스트 실행
        }

        firstCard = null;
        secondCard = null;
    }
    public void TimeMinus()
    {
        minusTimeTxt.SetActive(false);
    }
}
