using UnityEngine;
using UnityEngine.Events;
using Util;

public class ConsoleDoorActivation : MonoBehaviour
{
    public float CameraDeactivateDelay = 3f;
    public float TriggerActionsDelay = 1.5f;

    public UnityEvent OnCameraDeactivate;
    public UnityEvent OnTriggerAction;
    public UnityEvent OnSaveRecover;
    
    public void Activate()
    {
        FrameUtil.AfterDelay(CameraDeactivateDelay, () => OnCameraDeactivate.Invoke());
        FrameUtil.AfterDelay(TriggerActionsDelay, () => OnTriggerAction.Invoke());
    }

    public void RecoverSave()
    {
        OnSaveRecover.Invoke();
    }
}
