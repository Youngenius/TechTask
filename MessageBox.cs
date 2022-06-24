using TMPro;
using System.Collections;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    [Range(0, 255)]
    [SerializeField] private float m_Red;
    [Range(0, 255)]  
    [SerializeField] private float m_Green;
    [Range(0, 255)]
    [SerializeField] private float m_Blue;
    [Range(0, 255)]
    [SerializeField] private float m_Alpha;

    private TMP_Text _text;
    [SerializeField] int _textChildCount;
    [SerializeField] private float _timeForTextToExist;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        _text = this.transform.GetChild(_textChildCount).GetComponent<TMP_Text>();

        StartCoroutine(ChangeColorAlphaCoroutine());
        StopCoroutine(ChangeColorAlphaCoroutine());
    }

    private IEnumerator ChangeColorAlphaCoroutine()
    {
        float alphaPointsPerTime = 0.005f;
        float currentAlpha = 0;
        while (currentAlpha < m_Alpha)
        {
            currentAlpha += alphaPointsPerTime;
            _text.color = new Color(m_Red, m_Green, m_Blue, currentAlpha);
            yield return null;
        }

        yield return new WaitForSeconds(_timeForTextToExist);
        this.gameObject.SetActive(false);
    }
}
