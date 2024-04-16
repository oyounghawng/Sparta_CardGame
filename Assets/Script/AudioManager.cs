
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgm;
    public AudioSource effect;

    public AudioClip clip;
    public AudioClip alert;
    public AudioClip startSound;
    public AudioClip cardSet;
    public AudioClip failClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Play();
    }

    private void Update()
    {
        if (bgm.clip == startSound)
        {
            if (bgm.time >= startSound.length - 0.1f)
            {
                Stop();
                
                GameManager.instance.Play();
            }
        }
    }
    public void Play()
    {
        if(bgm != null)
        {
            if(clip != null)
            {
                bgm.clip = this.clip;
                bgm.loop = true;
                bgm.Play();
            }
        }
    }

    public void Play(AudioClip clip)
    {
        if (bgm != null)
        {
            if (clip != null)
            {
                bgm.clip = clip;
                bgm.loop = true;
                bgm.Play();
            }
        }
    }

    public void Stop()
    {
        if(bgm != null)
        {
            if(bgm.isPlaying)
            {
                bgm.Stop();
            }
        }
    }
    public void PlayOneShot(AudioClip clip, float volume)
    {
        if(effect != null)
        {
            if(effect.isPlaying)
            {
                effect.Stop();
            }
            effect.PlayOneShot(clip, volume);

        }
    }
}
