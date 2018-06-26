using Entities;
using UnityEngine;
using Util;

namespace Hazards
{
    public class Pendulum : MonoBehaviour
    {
        public float LerpTime = 0.5f;
        public float StopDelay = 0.5f;
        public Transform[] positions;
    
        private int _currentIndex;
        private Coroutine _moveCoroutine;
        private float _currentTimeToMove;

        public void Start()
        {
            MoveNext();
        }

        private void MoveNext()
        {
            if (positions.Length <= 0) return;

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            var newPosition = positions[_currentIndex % positions.Length].position;
            _moveCoroutine = StartCoroutine(EntityMove.MoveToPosition(transform, newPosition, LerpTime, MoveDelay));
            _currentIndex++;
        }

        private void MoveDelay()
        {
            FrameUtil.AfterDelay(StopDelay, MoveNext);
        }
    }
}
