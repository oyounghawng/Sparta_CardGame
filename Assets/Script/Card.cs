using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public GameObject front;
    public GameObject back;

    public Animator anim;
    public SpriteRenderer frontImage;

    AudioSource audioSource;
    public AudioClip clip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"Images/rtan{idx}");
    }

    public void OnepnCard()
    {
        if (GameManager.instance.secondCard != null) return;


        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);

        if(GameManager.instance.firstCard == null)
        {
            GameManager.instance.firstCard = this;
        }
        else
        {
            GameManager.instance.secondCard = this;
            GameManager.instance.isMatched();
        }
    }

    public void DestroyCard()
    {
        Invoke("DestroyCardInvoke", 1f);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1f);
    }

    void DestroyCardInvoke()
    {
        Destroy(this.gameObject);
    }
    public void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }
}
