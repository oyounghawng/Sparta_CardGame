using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateBestScore : MonoBehaviour
{
    public Text BestScore_0;
    public Text BestScore_1;
    public Text BestScore_2;

    public GameObject NormalStage;
    public GameObject NormalStageInactive;
    public GameObject HardStage;
    public GameObject HardStageInactive;

    [SerializeField] private int UnlockNormalStage = 2000;
    [SerializeField] private int UnlockHardStage = 2000;

    // Start is called before the first frame update
    void Start()
    {
        ActiveStages();
        BestScore_0.text = PlayerPrefs.GetInt("BestRecord0").ToString();
        BestScore_1.text = PlayerPrefs.GetInt("BestRecord1").ToString();
        BestScore_2.text = PlayerPrefs.GetInt("BestRecord2").ToString();
    }
    void ActiveStages()
    {
        int BestScore_0 = PlayerPrefs.GetInt("BestRecord0");
        int BestScore_1 = PlayerPrefs.GetInt("BestRecord1");

        if(BestScore_0 >= UnlockNormalStage)
        {
            NormalStage.SetActive(true);
            NormalStageInactive.SetActive(false);
        }
        else
        {
            NormalStage.SetActive(false);
            NormalStageInactive.SetActive(true);
        }

        if (BestScore_1 >= UnlockHardStage)
        {
           HardStage.SetActive(true);
           HardStageInactive.SetActive(false);
        }
        else
        {
            HardStage.SetActive(false);
            HardStageInactive.SetActive(true);
        }
    }
}
