using UnityEngine;
using System.Collections;
using System;

public abstract class ActiveObject : MonoBehaviour
{
    protected PlayerInputManager _playerInputManager;

    //Here will player run after clicking on active object
    public Vector2 _target;

    private int _colorChanger = -1;
    #region StartingColor
    [Header("StartingColor")]
    [Range(0f, 1f)]
    [SerializeField] protected float s_Hue;
    [Range(0f, 1f)]
    [SerializeField] protected float s_Saturation;
    [Range(0f, 100f)]
    [SerializeField] protected float s_Value;
    #endregion

    #region FinishColor
    [Header("FinishColor")]
    [Range(0f, 1f)]
    [SerializeField] protected float f_Hue;
    [Range(0f, 1f)]
    [SerializeField] protected float f_Saturation;
    [Range(0f, 100f)]
    [SerializeField] protected float f_Value;
    #endregion
    

    private void Awake()
    {
        _playerInputManager = GameObject.FindGameObjectWithTag("Player").
            GetComponent<PlayerInputManager>();
    }

    public abstract void OnObjectClicked(object sender, EventArgs e);

    public abstract void ApplyEffect();

    public void SwitchColors(Material material, out string currColor, 
        string startColor, string finishColor, string colorChanger)
    {
        material.SetColor
            (finishColor, Color.HSVToRGB(f_Hue, f_Saturation, f_Value));
        material.SetColor
            (startColor, Color.HSVToRGB(s_Hue, s_Saturation, s_Value));

        material.SetFloat(colorChanger, 0.5f + (0.5f * _colorChanger));
        currColor = _colorChanger > 0 ? startColor : finishColor;
        Debug.Log(_colorChanger);

        _colorChanger *= -1;
    }

    protected void SubscribeToEvents()
    {
        _playerInputManager.OnActiveObjectMouseButtonDown += OnObjectClicked;
    }

    protected void StartColorSetup(out Material material, 
        string startColor, string finishColor, string colorChanger)
    {
        material = GetComponent<SpriteRenderer>().sharedMaterial;
        material.SetColor(startColor, Color.HSVToRGB(s_Hue, s_Saturation, s_Value));
        material.SetColor(finishColor, Color.HSVToRGB(f_Hue, f_Saturation, f_Value));
        material.SetFloat(colorChanger, 1);
    }

    protected IEnumerator SwitchOffColourCoroutine
        (Material material ,string colourVarName, string startColor, string finishColor, float emissionDecreasedByTime = 0.1f)
    {
        if (colourVarName == startColor)
        {
            while (s_Value > 0)
            {
                s_Value -= emissionDecreasedByTime;
                material.SetColor(colourVarName, Color.HSVToRGB(s_Hue, s_Saturation, s_Value));

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        else if (colourVarName == finishColor)
        {
            while (f_Value > 0)
            {
                f_Value -= emissionDecreasedByTime;
                material.SetColor(colourVarName, Color.HSVToRGB(f_Hue, f_Saturation, f_Value));

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}
