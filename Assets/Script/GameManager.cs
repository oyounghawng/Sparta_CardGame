using System;
using System.Collections;
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
        BestScoreTxt.text = LoadBestRecord(stageLevel).ToString();
    }

    public event Action onPlay;
    public void Play()
    {
        state = Define.GameState.Play;
        timeTxt.gameObject.SetActive(true);
        AudioManager.instance.Play();
        onPlay?.Invoke();
    }

    public event Action onClear;

    public void GameClear()
    {
        StopAllCoroutines();
        UpdateScore();
        ResultTimeTxt.text = timeTxt.text;
        ResultTryTimesTxt.text = TryTimesTxt.text;
        ResultCurrentScoreTxt.text = CurrentScoreTxt.text;
        endOverlay.SetActive(true);
        Time.timeScale = 0f;
        onClear?.Invoke();
    }

    public event Action onGameOver;

    public void GameOver()
    {
        StopAllCoroutines();
        UpdateScore();
        ResultTimeTxt.text = timeTxt.text;
        ResultTryTimesTxt.text = TryTimesTxt.text;
        ResultCurrentScoreTxt.text = CurrentScoreTxt.text;
        endOverlay.SetActive(true);
        onGameOver?.Invoke();
    }
    #endregion

    public int stageLevel;

    [Header("TimeText")]
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

    /////////////////////////// inspector로 최고기록 확인용 ////////////////////////////////
    [SerializeField] int[] BestRecords = new int[3];

    public GameObject endOverlay;
    [SerializeField] int BestScore = 0;

    float playTime = 15.0f;
    float countdownTime = 5f;
    int CurrentScore;
    int TryTimes;

    public int cardCount = 0;
    public float speed = 0;
    private bool isEmphasis = false;

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
        audioSource = GetComponent<AudioSource>();
        stageLevel = PlayerPrefs.GetInt("Level");
        Time.timeScale = 1f;
        state = Define.GameState.Ready;
        Ready();
        CurrentScore = 0;
        TryTimes = 0;
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
                OpenCountDown();
                break;
            case Define.GameState.Clear:
                break;
        }
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
                firstCard.CloseCardInvoke();
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
        playTime -= Time.deltaTime;
        timeTxt.text = playTime.ToString("N2");

        if (playTime <= 0f)
        {
            Time.timeScale = 0f;
            timeTxt.text = "0.00";
            timeTxt.rectTransform.localScale = new Vector3(1, 1, 1);
            GameOver();
        }
        else if (playTime < 10f)
        {
            if (!isEmphasis)
                StartCoroutine(Emphasis(timeTxt.gameObject));

            timeTxt.color = Color.red;

            float alertSize = 0;

            alertSize = Mathf.Lerp(timeTxt.rectTransform.localScale.x, speed, 1f * Time.deltaTime);

            if (alertSize >= 1.0f)
            {
                AudioManager.instance.PlayOneShot(AudioManager.instance.alert, 0.35f);
            }

            

            timeTxt.rectTransform.localScale = new Vector3(alertSize, alertSize, 1);
        }
    }
    private IEnumerator Emphasis(GameObject gameObject)
    {
        isEmphasis = true;
        float increase = 0.1f;
        while (true)
        {
            while (gameObject.GetComponent<Transform>().localScale.x > 0.5f)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x - increase
                                                                , gameObject.transform.localScale.y - increase
                                                                , gameObject.transform.localScale.z - increase);
                yield return new WaitForSeconds(0.05f); 
            }
            yield return new WaitForSeconds(0.05f); 
            while (gameObject.GetComponent<Transform>().localScale.x < 1f)
            {
                gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x + increase
                                                                , gameObject.transform.localScale.y + increase
                                                                , gameObject.transform.localScale.z + increase);
                yield return new WaitForSeconds(0.05f); 
            }
            yield return new WaitForSeconds(0.05f); 
        }
    }
    public void isMatched()
    {
        TryTimes++;
        UpdateScore();
        UpdateUIScore();
        if (firstCard.idx == secondCard.idx)
        {
            audioSource.PlayOneShot(clip);
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
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
            playTime -= 1; //못맞추면 시간 -1초
            Invoke("TimeMinus", 0.5f); //0.5초동안 텍스트 실행
        }
        firstCard = null;
        secondCard = null;
        if (cardCount == 0)
        {
            GameClear();
        }
    }
    public void TimeMinus()
    {
        minusTimeTxt.SetActive(false);
    }
    public void UpdateScore()
    {
        CurrentScore = (int)(playTime * TimeVar - TryTimes * TryVar);
        int BestScore = LoadBestRecord(stageLevel);
        if (CurrentScore > BestScore)
        {
            SaveBestRecord(CurrentScore, stageLevel);
        }
        /////////////////////////// inspector�� �ְ���� Ȯ�ο� ////////////////////////////////
        for (int i = 0; i < 3; i++)
        {
            BestRecords[i] = LoadBestRecord(i);
        }
    }
    private void UpdateUIScore()
    {
        CurrentScoreTxt.text = CurrentScore.ToString();
        TryTimesTxt.text = TryTimes.ToString();
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
