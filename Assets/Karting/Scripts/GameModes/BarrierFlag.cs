using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierFlag : MonoBehaviour
{
    [Tooltip("Layers to trigger with")]
    public LayerMask layerMask;
    [SerializeField] private GameObject toOn;
    [SerializeField] private GameObject toOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!((layerMask.value & 1 << other.gameObject.layer) > 0 && other.CompareTag("Player")))
            // Anything other than the kart
            return;
        toOn.SetActive(true);
        toOff.SetActive(false);
    }
}
