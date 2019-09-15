using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeController : MonoBehaviour
{
    private const string FADEIN_KEY = "FadeIn";
    private const string FADEOUT_KEY = "FadeOut";
    
    [SerializeField]
    private Animator _gameCanvas;
    [SerializeField]
    private Animator _loadingCanvas;
    [SerializeField]
    private Text _tipText;

    private void Awake()
    {
        _tipText.text = LoadingScreenTips.GetRandom;
    }

    public void FadeIn()
    {
        _gameCanvas.SetTrigger(FADEIN_KEY);
        _loadingCanvas.SetTrigger(FADEOUT_KEY);
        _tipText.text = LoadingScreenTips.GetRandom;
    }

    public void FadeOut()
    {
        _gameCanvas.SetTrigger(FADEOUT_KEY);
        _loadingCanvas.SetTrigger(FADEIN_KEY);
        LoadingScreenTips.ClearTip();
    }
    
    public void FadeOutCanvas()
    {
        _gameCanvas.SetTrigger(FADEOUT_KEY);
    }
}
