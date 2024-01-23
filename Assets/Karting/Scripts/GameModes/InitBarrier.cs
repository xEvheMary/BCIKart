using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBarrier : MonoBehaviour
{
    [SerializeField] private GameObject firstOn;
    [SerializeField] private GameObject firstOff;

    // Start is called before the first frame update
    void Start()
    {
        firstOn.SetActive(true);
        firstOff.SetActive(false);
    }

}
