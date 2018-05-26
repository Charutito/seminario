using Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Util
{
    static public class FrameUtil
    {
        static public void AtEndOfFrame(Action action)
        {
            CoroutineManager.Instance.RunCoroutine(RunAtEndOfFrame(action));
        }

        static public void OnNextFrame(Action action)
        {
            CoroutineManager.Instance.RunCoroutine(RunOnNextFrame(action));
        }

        static public void AfterFrames(int frames, Action action)
        {
            CoroutineManager.Instance.RunCoroutine(RunAfterFrames(frames, action));
        }

        static public void AfterDelay(float delayInSeconds, Action action)
        {
            CoroutineManager.Instance.RunCoroutine(RunAfterDelay(delayInSeconds, action));
        }

        static private IEnumerator RunAtEndOfFrame(Action action)
        {
            yield return new WaitForEndOfFrame();

            action();
        }

        static private IEnumerator RunOnNextFrame(Action action)
        {
            yield return null;

            action();
        }

        static private IEnumerator RunAfterFrames(int frames, Action action)
        {
            for (var x = 0; x < frames; x++)
            {
                yield return null;
            }

            action();
        }

        static private IEnumerator RunAfterDelay(float delayInSeconds, Action action)
        {
            yield return new WaitForSeconds(delayInSeconds);

            action();
        }
    }
}

