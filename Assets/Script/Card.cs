using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public GameObject front;
    public GameObject back;

    public Animator anim;
    private SpriteRenderer frontImage;
    private SpriteRenderer backImage;

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
    private void Awake()
    {
        frontImage = transform.GetComponentsInChildren<SpriteRenderer>()[0];
        backImage = transform.GetComponentsInChildren<SpriteRenderer>()[1];
    }

    private void Start()
    {
        canTouch = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isMove)
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

        if (Vector3.Distance(dest, transform.position) <= 0.1f)
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
        if (canTouch)
        {
            if (GameManager.instance.secondCard != null) return;

            audioSource.PlayOneShot(clip);
            anim.SetBool("isOpen", true);
            // 클릭된 카드 뒷면 색깔 회색으로 고정
            backImage.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 255f);

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
    }
}
