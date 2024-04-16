using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level;

    public Text timeTxt;
    public Text BestScoreTxt;
    public Text CurrentScoreTxt;
    public Text TryTimesTxt;

    public Text ResultTimeTxt;
    public Text ResultTryTimesTxt;
    public Text ResultCurrentScoreTxt;

    [SerializeField] int TimeVar = 100;
    [SerializeField] int TryVar = 1;

    /////////////////////////// inspector로 최고기록 확인용 ////////////////////////////////
    [SerializeField] int[] BestRecords = new int[3];

    public GameObject endOverlay;
    public Card firstCard;
    public Card secondCard;

    float time = 0;
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
        level = PlayerPrefs.GetInt("Level");
        Time.timeScale = 1f;
        CurrentScore = 0;
        TryTimes = 0;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        BestScoreTxt.text = LoadBestRecord(level).ToString() ;
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
        int BestScore = LoadBestRecord(level);
        if (CurrentScore > BestScore)
        {
            SaveBestRecord(CurrentScore, level);
        }
        /////////////////////////// inspector로 최고기록 확인용 ////////////////////////////////
        for (int i = 0; i < 3; i++)
        {
            BestRecords[i] = LoadBestRecord(i);
        }
    }
    public void EndGame()
    {
        UpdateScore();
        ResultTimeTxt.text = timeTxt.text;
        ResultTryTimesTxt.text = TryTimesTxt.text;
        ResultCurrentScoreTxt.text = CurrentScoreTxt.text;
        endOverlay.SetActive(true);
        Time.timeScale = 0f;
    }
    public void SaveBestRecord(int score, int level)
    {
        PlayerPrefs.SetInt("BestRecord" + level.ToString(), score);
    }
    public int LoadBestRecord(int level)
    {
        return PlayerPrefs.GetInt("BestRecord" + level.ToString());
    }
}
