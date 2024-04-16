using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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

    private float speed = 30f;

    public bool isMove = false;
    public bool canTouch = false;

    private Vector3 _dest = Vector3.zero;

    public Vector3 Dest
    {
        get => _dest;
        set
        {
            _dest = value;
            isMove = true;
        }
    }
    private void Start()
    {
        canTouch = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(isMove)
        {
            MoveCurve(Dest);
        }
    }

    private void OnEnable()
    {
        GameManager.instance.onPlay += CanTouch;
    }

    private void OnDisable()
    {
        GameManager.instance.onPlay -= CanTouch;
    }
    public void CanTouch() => canTouch = true;
    public void MoveCurve(Vector3 dest)
    {
        transform.position = Vector3.Slerp(transform.position, dest, speed * Time.deltaTime);

        if(Vector3.Distance(dest, transform.position) <= 0.1f)
        {
            transform.position = dest;
            AudioManager.instance.PlayOneShot(AudioManager.instance.cardSet, 0.5f);
            isMove = false;
        }
    }
    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"Images/TeamPic/{idx}");
    }

    public void OnepnCard()
    {
        if(canTouch)
        {
            if (GameManager.instance.secondCard != null) return;


            audioSource.PlayOneShot(clip);
            anim.SetBool("isOpen", true);
            front.SetActive(true);
            back.SetActive(false);

            if (GameManager.instance.firstCard == null)
            {
                GameManager.instance.firstCard = this;
            }
            else
            {
                GameManager.instance.secondCard = this;
                GameManager.instance.isMatched();
            }
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
