using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUI : MonoBehaviour
{
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;
    // Start is called before the first frame update
    void Start()
    {
        ChangeAlpha(leftImage, .05f);
        ChangeAlpha(rightImage, .05f);
        if (FindAnyObjectByType<CalibObject>() != null){
			CalibObject.OnCheckpointTrigger += CalibObject_OnCheckpoint;
		}
    }

    private void CalibObject_OnCheckpoint(object sender, CalibObject.OnCheckpointArgs e)
    {
        switch(e.miclass){
			case CalibObject.MIClass.Left:
				ChangeAlpha(leftImage, 1f);
                ChangeAlpha(rightImage, .05f);
				break;
			case CalibObject.MIClass.Rest:
				ChangeAlpha(leftImage, .05f);
                ChangeAlpha(rightImage, .05f);
				break;
			case CalibObject.MIClass.Right:
                ChangeAlpha(leftImage, .05f);
				ChangeAlpha(rightImage, 1f);
				break;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAlpha(Image image, float alpha)
    {
        // Get the current color of the image
        Color currentColor = image.color;

        // Set the alpha value
        currentColor.a = alpha;

        // Assign the modified color back to the image
        image.color = currentColor;
    }

    private void OnDestroy(){
		CalibObject.OnCheckpointTrigger -= CalibObject_OnCheckpoint;
    }
}
