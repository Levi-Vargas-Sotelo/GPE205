using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public Text textToSave;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save () 
    {
        PlayerPrefs.SetString ("TextData", textToSave.text);
        PlayerPrefs.Save ();
    }

    public void Load () 
    {
        textToSave.text = PlayerPrefs.GetString ("TextData");
    }
}
