using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Text timeTxt;
    public Text resultTxt;

    public GameObject endTxt;
    public Card firstCard;
    public Card secondCard;

    float time = 0;
    public int cardCount = 0;

    AudioSource audioSource;
    public AudioClip clip;
    public AudioClip failclip;
    public AudioClip startClip;
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

        //지연시작 방식 진행 방식 고민
        audioSource.clip = startClip;
        audioSource.Play();
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
            string name = "";
            switch(firstCard.idx)
            {
                case 0:
                    name = "황오영";
                    break;
  
            }
            if (cardCount == 0)
            {
                Time.timeScale = 0;
                endTxt.SetActive(true);
            }
            resultTxt.text = name;
            resultTxt.color = Color.white;
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
            audioSource.clip = failclip;
            audioSource.Play();
            resultTxt.text = "실패";
            resultTxt.color = Color.red;
        }
        firstCard = null;
        secondCard = null;
    }
   
}
