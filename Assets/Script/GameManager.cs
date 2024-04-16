using System;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region  GameState
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
    #endregion

    [Header("TimeText")]
    public int level;

    public Text timeTxt;
    public Text limitTimeTxt;
    public Text resultTxt;
    public GameObject minusTimeTxt;

    [Header("ScoreText")]
    public Text BestScoreTxt;
    public Text CurrentScoreTxt;
    public Text TryTimesTxt;

    [Header("Card")]
    public Card firstCard;
    public Card secondCard;

    public GameObject endTxt;

    public Text ResultTimeTxt;
    public Text ResultTryTimesTxt;
    public Text ResultCurrentScoreTxt;

    [SerializeField] int TimeVar = 100;
    [SerializeField] int TryVar = 1;

    /////////////////////////// inspector�� �ְ���� Ȯ�ο� ////////////////////////////////
    [SerializeField] int[] BestRecords = new int[3];

    public GameObject endOverlay;
    [SerializeField] int BestScore = 0;

    float time = 15.0f;
    float countdownTime = 5f;
    public Card firstCard;
    public Card secondCard;

    float time = 0;
    int CurrentScore;
    int TryTimes;

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
        level = PlayerPrefs.GetInt("Level");
        Time.timeScale = 1f;
        state = Define.GameState.Ready;
        Ready();
        CurrentScore = 0;
        TryTimes = 0;
        audioSource = GetComponent<AudioSource>();
        resultTxt.text = "";
    }
    private void Update()
    {
        switch (state)
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        BestScoreTxt.text = LoadBestRecord(level).ToString() ;
        CurrentScoreTxt.text = CurrentScore.ToString();
        TryTimesTxt.text = TryTimes.ToString();
        if (time > 30f)
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
            time += Time.deltaTime;
            timeTxt.text = time.ToString("N2");
            BestScoreTxt.text = BestScore.ToString();
            CurrentScoreTxt.text = CurrentScore.ToString();
            TryTimesTxt.text = TryTimes.ToString();
            if (time > 30f)
            {
                EndGame();
            }
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
            TryTimes++;
        UpdateScore();
        if (firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if (cardCount == 0)
            {
                EndGame();
            }
            string name = "";
            switch (firstCard.idx)
            {
                case 0:
                case 1:
                    name = "이수현";
                    break;
                case 2:
                case 3:
                    name = "황오영";
                    break;
                case 4:
                case 5:
                    name = "김한진";
                    break;
                case 6:
                case 7:
                    name = "문향기";
                    break;
                case 8:
                case 9:
                    name = "김홍진";
                    break;
                default:
                    name = "";
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
            time -= 1; //못맞추면 시간 -1초
            Invoke("TimeMinus", 0.5f); //0.5초동안 텍스트 실행
        }

        firstCard = null;
        secondCard = null;
    }
    public void TimeMinus()
    {
        minusTimeTxt.SetActive(false);
    }

    public void UpdateScore()
    {
        CurrentScore = (int)(time * TimeVar - TryTimes * TryVar);
        int BestScore = LoadBestRecord(level);
        if (CurrentScore > BestScore)
        {
            SaveBestRecord(CurrentScore, level);
        }
        /////////////////////////// inspector�� �ְ���� Ȯ�ο� ////////////////////////////////
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
