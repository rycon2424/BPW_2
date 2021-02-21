using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Text subtitles;

    public void UpdateSubtitle(string newText)
    {
        subtitles.text = newText;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

}
