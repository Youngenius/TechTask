using System;
using UnityEngine;

public class Door : ActiveObject
{
    private Material _outlineMaterial;
    private string _currColor;
    private string _startColor = "Color1";
    private string _finishColor = "FinishColor";
    private string _colorChanger = "ChangeColor";

    public static EventHandler OnUItextCreated;

    [SerializeField] private GameObject _UI_message;
    private void Start()
    {
        StartColorSetup(out _outlineMaterial, _startColor, _finishColor, _colorChanger);

        SubscribeToEvents();
    }

    public override void OnObjectClicked(object sender, EventArgs e)
    {
        SwitchColors(_outlineMaterial, out var currColor, 
            _startColor, _finishColor, _colorChanger);
        _currColor = currColor;
    }

    public override void ApplyEffect()
    {
        Debug.Log("is OK");
        _UI_message.SetActive(true);
        OnUItextCreated.Invoke(this, EventArgs.Empty);
        _playerInputManager.OnActiveObjectMouseButtonDown -= OnObjectClicked;

        Debug.Log(_currColor);
        StartCoroutine(SwitchOffColourCoroutine(_outlineMaterial, _currColor, _startColor, _finishColor, 0.01f * Time.deltaTime));
        StopCoroutine(SwitchOffColourCoroutine(_outlineMaterial, _currColor, _startColor, _finishColor, 0.01f * Time.deltaTime));

        //changing tag, so the function could be called only once
        this.gameObject.tag = "Untagged";
    }
}
