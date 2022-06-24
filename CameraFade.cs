using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CameraFade : MonoBehaviour
{
    private Car _car;

    [SerializeField] private Color _fadeColor;
    [SerializeField] private Color _beginColor;

    [SerializeField] private float _fadeTime; 
    private float _currentTime;

    public Image image;
    
    public void TriggerFade()
    {
        StartCoroutine(FadeCoroutine(_fadeColor, _beginColor));
        StopCoroutine(FadeCoroutine(_fadeColor, _beginColor));
    }

    private IEnumerator FadeCoroutine(Color targetColor, Color beginColor)
    {
        while (_currentTime <_fadeTime)
        {
            image.color = Color.Lerp(Color.clear, targetColor, _currentTime / _fadeTime);
            _currentTime += Time.deltaTime;

            yield return null;
        }
    }
}
