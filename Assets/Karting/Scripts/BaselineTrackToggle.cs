using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Services;
using KartGame.KartSystems;
using UnityEngine;

public class BaselineTrackToggle : MonoBehaviour
{
    public static event EventHandler<Controller.OnCueArgs> OnTrackChange;
    [SerializeField] private List<GameObject> tracks;
    private ArcadeKart playerKart;
    ArcadeKart[] karts;
    //int trackIndex = 1;
    // Start is called before the first frame update
    void Start()
    {   
        // Prepare the karts
        karts = FindObjectsOfType<ArcadeKart>();
        if (karts.Length > 0)
        {
            if (!playerKart) playerKart = karts[0];
        }
        Controller.OnCueTrigger += Controller_OnCueTrigger;
    }

    private void Controller_OnCueTrigger(object sender, Controller.OnCueArgs e)
    {
        playerKart.SetCanMove(false);
        if (e.cueClass < 3){
            playerKart.PositionReset(tracks[e.cueClass].transform.position);
            OnTrackChange?.Invoke(this, e);
        }
        playerKart.SetCanMove(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
