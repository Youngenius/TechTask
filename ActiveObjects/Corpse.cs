using System;
using UnityEngine;

public class Corpse : ActiveObject
{
    private Material _outlineMaterial;
    private string _startColor = "StartColorCorpse";
    private string _finishColor = "FinishColor1Corpse";
    private string _colorChanger = "ChangeColorCorpse";

    [SerializeField] GameObject frontView;
    [SerializeField] GameObject sideView;

    private void Start()
    {
        _outlineMaterial = GetComponent<SpriteRenderer>().sharedMaterial;
        StartColorSetup(out _outlineMaterial, _startColor, _finishColor, _colorChanger);

        SubscribeToEvents();
    }

    public override void OnObjectClicked(object sender, EventArgs e)
    {
        SwitchColors(_outlineMaterial, out var currColor, 
            _startColor, _finishColor, _colorChanger);
        _playerInputManager.OnActiveObjectMouseButtonDown -= OnObjectClicked;
    }

    public override void ApplyEffect()
    {
        SwitchViews.SwitchView(frontView, sideView);
    }
}
