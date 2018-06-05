using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public static class TweenUtils
    {
        public static void MoveAdd(this GameObject target, float amount, float time, iTween.EaseType easyType = iTween.EaseType.linear)
        {
            var hash = iTween.Hash(
                "z", amount,
                "time", time,
                "easetype", easyType,
                "oncomplete", "onMoveEnd",
                "oncompletetarget", target
                );

            iTween.MoveAdd(target, hash);
        }

        public static void MoveTo(this GameObject target, Vector3 newPosition, float time, iTween.EaseType easyType = iTween.EaseType.linear, string onComplete = "onMoveEnd")
        {
            var hash = iTween.Hash(
                "position", newPosition,
                "time", time,
                "easetype", easyType,
                "oncomplete", onComplete,
                "oncompletetarget", target
                );

            iTween.MoveTo(target, hash);
        }
    }
}
