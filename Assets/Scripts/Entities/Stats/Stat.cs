using System;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class Stat
    {
        #region Delegates
        public delegate void ActualChangeDelegate(float old, float current);
        #endregion

        #region Events
        public event ActualChangeDelegate OnActualChange = delegate {};
        public event Action<float> OnMinChange = delegate {};
        public event Action<float> OnMaxChange = delegate {};
        #endregion

        #region Properties

        public float Current
        {
            get { return _Actual; }
            set
            {
                if (value != _Actual)
                {
                    var normalized = Mathf.Clamp(value, _Min, _Max);

                    if(OnActualChange != null) OnActualChange(_Actual, normalized);

                    _Actual = normalized;
                }
            }
        }

        public float Min
        {
            get { return _Min; }
            set
            {
                if (value != _Min)
                {
                    _Min = value;
                    if (OnMinChange != null) OnMinChange(_Min);
                }
            }
        }

        public float Max
        {
            get { return _Max; }
            set
            {
                if (value != _Max)
                {
                    _Max = value;
                    if (OnMaxChange != null) OnMaxChange(_Max);
                }
                
            }
        }
        #endregion

        #region Local Vars
         
        [SerializeField] private float _Actual;
        [SerializeField] private float _Min;
        [SerializeField] private float _Max;
        #endregion

        public Stat(int actual, int min, int max)
        {
            _Actual = actual;
            _Min = min;
            _Max = max;
        }
    }
}
