using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeTxt;
    public GameObject endTxt;
    public Card firstCard;
    public Card secondCard;

    float time = 0;
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
        time += Time.deltaTime;
        timeTxt.text = time.ToString("N2");
        if(time > 30f)
        {
            endTxt.SetActive(true);
            Time.timeScale = 0f;

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
