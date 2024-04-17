using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{
    public void MoveToStage_0()
    {
        PlayerPrefs.SetInt("Level", 0);
        SceneManager.LoadScene("MainScene");
    }
    public void MoveToStage_1()
    {
        PlayerPrefs.SetInt("Level", 1);
        SceneManager.LoadScene("MainScene");
    }
    public void MoveToStage_2()
    {
        PlayerPrefs.SetInt("Level", 2);
        SceneManager.LoadScene("MainScene");
    }
}
