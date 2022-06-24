using System;
using UnityEngine;
using UnityEngine.Events;

public class Car : ActiveObject
{
    private SideScene _sideScene;

    private Material _outlineMaterial;
    private string _startColor = "StartColorCar";
    private string _finishColor = "FinishColorCar";
    private string _colorChanger = "ChangeColorCar";

    public UnityEvent OnCarClicked;
    public EventHandler<OnCarClickedArgs> OnCarClickedEffect;
    public class OnCarClickedArgs : EventArgs { public Transform finishPoint; }

    private void Start()
    {
        _outlineMaterial = GetComponent<SpriteRenderer>().sharedMaterial;
        _outlineMaterial.SetFloat(_colorChanger, 1);
        _outlineMaterial.SetColor(_startColor, Color.black);

        _sideScene = GameObject.FindObjectOfType<SideScene>();
        _sideScene.OnSceneEnabled += OnSceneEnabledMethod;
    }

    private void OnSceneEnabledMethod(object sender, EventArgs e)
    {
        StartColorSetup(out var outlineMaterial, _startColor,
            _finishColor, _colorChanger);
        EnableColliders2D(this.GetComponents<Collider2D>());

        _outlineMaterial = outlineMaterial;
    }

    private void EnableColliders2D(Collider2D[] colliders)
    {
        foreach (var col in colliders)
        {
            col.enabled = true;
        }
    }

    public override void ApplyEffect()
    {
        //Fade + disables input
        OnCarClicked?.Invoke();
    }

    public override void OnObjectClicked(object sender, EventArgs e)
    {
        Debug.Log("works");
        SwitchColors(_outlineMaterial, out var currColor, 
            _startColor, _finishColor, _colorChanger);
        
    }
}
