using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitUI : MonoBehaviour
{
    public static WaitUI Instance {get; private set;}
    // Start is called before the first frame update
    private void Awake(){
        Instance = this;
    }

    void Start()
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }
}
