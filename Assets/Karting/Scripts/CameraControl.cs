using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{

    CinemachineVirtualCamera activeCamera;
    public Camera mainCamera;
    public CinemachineVirtualCamera cinemachine3POV;
    public CinemachineVirtualCamera cinemachine1POV;

    int cameraModifier = 10;

    // Start is called before the first frame update
    void Start()
    {
        ChangeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){ ChangeCamera();}
    }

    private void ChangeCamera(){
        if(activeCamera == cinemachine3POV){
            ChangePriorities(cinemachine3POV, cinemachine1POV);
        }
        else if (activeCamera == cinemachine1POV){
            ChangePriorities(cinemachine1POV, cinemachine3POV);
        }
        else{
            cinemachine3POV.Priority += cameraModifier;
            activeCamera = cinemachine3POV;
        }
    }

    private void ChangePriorities(CinemachineVirtualCamera oldCam, CinemachineVirtualCamera newCam){
        oldCam.Priority -= cameraModifier;
        newCam.Priority += cameraModifier;
        activeCamera = newCam;
    }
}
