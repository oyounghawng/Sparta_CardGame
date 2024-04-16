using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Define.GameState state = Define.GameState.Ready;

    public void Ready()
    {
        // AudioManager.instance.Stop();
        timeTxt.gameObject.SetActive(false);
    }

    public event Action onPlay;

    public void Play()
    {
        if (onPlay != null)
        {
            state = Define.GameState.Play;
            timeTxt.gameObject.SetActive(true);
            onPlay.Invoke();

        }
    }

    public event Action onClear;

    public void Clear()
    {
        if (onClear != null)
            onClear.Invoke();
    }

    public event Action onGameOver;

    public void GameOver()
    {
        if (onGameOver != null)
            onGameOver.Invoke();
    }

    public Text timeTxt;
    public Text limitTimeTxt; // 제한 시간 텍스트
    public Text resultTxt; // 결과 텍스트
    public GameObject minusTimeTxt; // 시간 감소 알려주는 텍스트
    public GameObject endTxt;
    public Card firstCard;
    public Card secondCard;

    float time = 15.0f;
    float countdownTime = 5f;
    public int cardCount = 0;
    public float speed = 0;

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
        state = Define.GameState.Ready;
        Ready();
        audioSource = GetComponent<AudioSource>();
        resultTxt.text = "";
    }
    private void Update()
    {
        switch (state)
        {
            case Define.GameState.Ready:

                break;
            case Define.GameState.Play:
                GamePlay();
                break;
            case Define.GameState.Clear:

                break;
            case Define.GameState.GameOver:

                break;
        }
        OpenCountDown();
    }
    private void OpenCountDown()
    {
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

    private void GamePlay()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");


        if (time <= 0f)
        {
            Time.timeScale = 0f;
            timeTxt.text = "0.00";
            timeTxt.rectTransform.localScale = new Vector3(1, 1, 1);

            endTxt.SetActive(true);

        }
        else if (time < 10f)
        {
            // TODO 타이머의 색깔 변경
            timeTxt.color = Color.red;


            float alertSize = 0;

            if (timeTxt.rectTransform.localScale.x <= 1.0f)
            {
                alertSize = Mathf.Lerp(timeTxt.rectTransform.localScale.x, speed, 1f * Time.deltaTime);

                if (alertSize >= 1.0f)
                {
                    AudioManager.instance.PlayOneShot(AudioManager.instance.alert, 0.35f);
                }
            }

            timeTxt.rectTransform.localScale = new Vector3(alertSize, alertSize, 1);
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
            string name = "";
            switch (firstCard.idx)
            {
                case 0:
                    name = "황오영";
                    break;

            }
            resultTxt.text = name;
            resultTxt.color = Color.white;
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();

            audioSource.clip = AudioManager.instance.failClip;
            audioSource.Play();
            resultTxt.text = "실패";
            resultTxt.color = Color.red;


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
