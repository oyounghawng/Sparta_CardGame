using UnityEngine;
using UnityEngine.UI;
using System;

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
        if(onPlay != null)
        {
            state = Define.GameState.Play;
            timeTxt.gameObject.SetActive(true);
            onPlay.Invoke();

        }
    }

    public event Action onClear;

    public void Clear()
    {
        if(onClear != null)
            onClear.Invoke();
    }

    public event Action onGameOver;

    public void GameOver()
    {
        if(onGameOver != null)
            onGameOver.Invoke();
    }


    public Text timeTxt;
    public GameObject endTxt;
    public Card firstCard;
    public Card secondCard;


    float time = 15.0f;
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
    }
    private void Update()
    {
        switch(state)
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
        if(firstCard.idx == secondCard.idx)
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
        }

        firstCard = null;
        secondCard = null;
    }

}
