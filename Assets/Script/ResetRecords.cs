using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRecords : MonoBehaviour
{
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
