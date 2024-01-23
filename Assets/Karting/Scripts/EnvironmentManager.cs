using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectToggle = new List<GameObject>();

    private const string PLAYER_PREFS_ENVIRONMENT_TOGGLE = "envFlag";
    
    void Awake()
    {
        if (PlayerPrefs.HasKey(PLAYER_PREFS_ENVIRONMENT_TOGGLE)){
            if(PlayerPrefs.GetInt(PLAYER_PREFS_ENVIRONMENT_TOGGLE, 1) > 0){
                for (int i = 0; i < objectToggle.Count; i++){
                    objectToggle[i].SetActive(false);
                }
            }
            else {
                for (int i = 0; i < objectToggle.Count; i++){
                    objectToggle[i].SetActive(true);
                }
            }
        }
    }

    void Start()
    {
        
    }

}
