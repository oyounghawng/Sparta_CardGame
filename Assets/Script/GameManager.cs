using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeTxt;
    public Text BestScoreTxt;
    public Text CurrentScoreTxt;
    public Text TryTimesTxt;

    public Text ResultTimeTxt;
    public Text ResultTryTimesTxt;
    public Text ResultCurrentScoreTxt;

    [SerializeField] int TimeVar = 100;
    [SerializeField] int TryVar = 1;

    public GameObject endOverlay;
    public Card firstCard;
    public Card secondCard;

    float time = 0;
    [SerializeField] int BestScore = 0;
    int CurrentScore;
    int TryTimes;

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
        CurrentScore = 0;
        TryTimes = 0;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        BestScoreTxt.text = BestScore.ToString() ;
        CurrentScoreTxt.text = CurrentScore.ToString();
        TryTimesTxt.text = TryTimes.ToString();
        if (time > 30f)
        {
            EndGame();
        }
    }
    public void isMatched()
    {
        TryTimes++;
        UpdateScore();
        if(firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0)
            {
                EndGame();
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
    public void UpdateScore()
    {
        CurrentScore = (int)(time * TimeVar - TryTimes * TryVar);
    }
    public void EndGame()
    {
        ResultTimeTxt.text = timeTxt.text;
        ResultTryTimesTxt.text = TryTimesTxt.text;
        ResultCurrentScoreTxt.text = CurrentScoreTxt.text;
        endOverlay.SetActive(true);
        Time.timeScale = 0f;
    }
}
