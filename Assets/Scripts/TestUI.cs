using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{

    public Text myUIText;
    public Image myUIImage;
    public RectTransform myUITextTransform;

    public Sprite alertSprite;
    public Sprite okSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKey (KeyCode.Space)) 
        {
            myUIText.text = "Emergency meeting! Emergency meeting! The impostor is acting sussy!!";

            myUIImage.sprite = alertSprite;

            myUITextTransform.anchoredPosition3D = new Vector3 (500, -500, 0);
        }
        else 
        {
            myUIText.text = "Nevermind lol";

            myUIImage.sprite = okSprite;

            myUITextTransform.anchoredPosition3D = new Vector3 (1000, -500, 0);
        }
    }
}
