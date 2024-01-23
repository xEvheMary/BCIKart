using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordText;
    private const string PLAYER_PREFS_TIME_RECORD = "recordTime";
    // Start is called before the first frame update
    void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual(){
        if (PlayerPrefs.HasKey(PLAYER_PREFS_TIME_RECORD)){recordText.text = PlayerPrefs.GetInt(PLAYER_PREFS_TIME_RECORD, 2).ToString();}
    }
}
