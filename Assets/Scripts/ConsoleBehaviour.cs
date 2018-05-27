using System;
using System.Linq;
using Metadata;
using SaveSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConsoleBehaviour : MonoBehaviour
{
    public int[] Combination;
    public Text[] ConsoleText;
    public RectTransform SelectedGameObject;

    public UnityEvent OnValidationCorrect;
    public UnityEvent OnValidationFail;
    public UnityEvent OnSaveRecover;

    private readonly int[] _numbers = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
    private readonly int[] _currentValues = {0, 0, 0};

    private int _currentSlider = 0;
    private int _currentSliderValue = 0;

    private int CurrentSlider { get { return Math.Abs(_currentSlider % _currentValues.Length); } }
    private int CurrentSliderValue { get { return Math.Abs(_currentSliderValue % _numbers.Length); } }

    private bool _active;
    private bool _resolved;

    private SaveGUID _guid;

    private void Validate()
    {
        var errors = _currentValues.Where((t, i) => t != Combination[i]).Count();

        if (errors > 0)
        {
            OnValidationFail.Invoke();
        }
        else
        {
            PlayerPrefs.SetString(string.Format(SaveKeys.ConsoleSave, _guid.GameObjectId), "true");
            _resolved = true;
            SelectedGameObject.gameObject.SetActive(false);
            OnValidationCorrect.Invoke();
        }
    }

    private void UpdateSliderValue()
    {
        _currentValues[CurrentSlider] = CurrentSliderValue;

        for (var i = 0; i < _currentValues.Length; i++)
        {
            ConsoleText[i].text = _currentValues[i].ToString();
        }
    }

    private void UpdateSlider()
    {
        _currentSliderValue = _currentValues[CurrentSlider];
        SelectedGameObject.anchoredPosition = new Vector3(ConsoleText[CurrentSlider].rectTransform.anchoredPosition.x, SelectedGameObject.anchoredPosition.y);
    }

    private void RecoverSave()
    {
        OnSaveRecover.Invoke();
        SelectedGameObject.gameObject.SetActive(false);
        _resolved = true;

        for (var i = 0; i < Combination.Length; i++)
        {
            ConsoleText[i].text = Combination[i].ToString();
        }
    }

    private void Awake()
    {
        _guid = GetComponent<SaveGUID>();
        
        if (PlayerPrefs.HasKey(string.Format(SaveKeys.ConsoleSave, _guid.GameObjectId)))
        {
            RecoverSave();
        }
    }

    private bool _resetHorizontal = true;
    private bool _resetVertical = true;

    private void Update()
    {
        if (!_active || _resolved) return;

        if (Math.Abs(Input.GetAxis("DpadHorizontal")) < 0.1f)
        {
            _resetHorizontal = true;
        }
        
        if (Math.Abs(Input.GetAxis("DpadVertical")) < 0.1f)
        {
            _resetVertical = true;
        }

        if (_resetVertical && Input.GetAxis("DpadVertical") > 0)
        {
            _resetVertical = false;
            _currentSliderValue += 1;
            UpdateSliderValue();
        }
        else if (_resetVertical && Input.GetAxis("DpadVertical") < 0)
        {
            _resetVertical = false;
            _currentSliderValue -= 1;
            UpdateSliderValue();
        }
        
        if (_resetHorizontal && Input.GetAxis("DpadHorizontal") > 0)
        {
            _resetHorizontal = false;
            _currentSlider += 1;
            UpdateSlider();
        }
        else if (_resetHorizontal && Input.GetAxis("DpadHorizontal") < 0)
        {
            _resetHorizontal = false;
            _currentSlider -= 1;
            UpdateSlider();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Validate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _active = false;
    }
}
