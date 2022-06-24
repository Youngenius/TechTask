using System;
using UnityEngine.Events;
using UnityEngine;

public class SideScene : MonoBehaviour
{
    private bool _wasCalled;

    public EventHandler OnSceneEnabled;

    private void OnEnable()
    {
        if (!_wasCalled)
        {
            OnSceneEnabled?.Invoke(this, EventArgs.Empty);
        }
    }
}
