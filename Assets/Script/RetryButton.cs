using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("MainScene_2");
    }
    public void Home()
    {
        SceneManager.LoadScene("StartScene");
    }
}
