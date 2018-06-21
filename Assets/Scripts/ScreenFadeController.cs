using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFadeController : MonoBehaviour
{
    private const string FADEIN_KEY = "FadeIn";
    private const string FADEOUT_KEY = "FadeOut";
    
    [SerializeField]
    private Animator _gameCanvas;
    [SerializeField]
    private Animator _loadingCanvas;
    
    public void FadeIn()
    {
        _gameCanvas.SetTrigger(FADEIN_KEY);
        _loadingCanvas.SetTrigger(FADEOUT_KEY);
    }

    public void FadeOut()
    {
        _gameCanvas.SetTrigger(FADEOUT_KEY);
        _loadingCanvas.SetTrigger(FADEIN_KEY);
    }
    
    public void FadeOutCanvas()
    {
        _gameCanvas.SetTrigger(FADEOUT_KEY);
    }
}
