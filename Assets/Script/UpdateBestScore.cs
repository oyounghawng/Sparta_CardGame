using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBestScore : MonoBehaviour
{
    public Text BestScore_0;
    public Text BestScore_1;
    public Text BestScore_2;

    // Start is called before the first frame update
    void Start()
    {
        BestScore_0.text = PlayerPrefs.GetInt("BestRecord0").ToString();
        BestScore_1.text = PlayerPrefs.GetInt("BestRecord1").ToString();
        BestScore_2.text = PlayerPrefs.GetInt("BestRecord2").ToString();
    }
}
