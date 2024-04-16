using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void StageSelect()
    {
        SceneManager.LoadScene("StageScene");
    }
    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Home()
    {
        SceneManager.LoadScene("StartScene");
    }
}
