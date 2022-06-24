using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraZoom : MonoBehaviour
{
    private Camera camera;
    private CinemachineBrain brain;
    private CinemachineVirtualCamera _vcam;

    [SerializeField] NPConroller[] _controllers;
    private int _controllerCounter = 0;

    [SerializeField] float _smooth;

    public GameObject frontView;
    public GameObject sideView;

    private Transform _target;

    [SerializeField] private List<Transform> targets;
    [SerializeField] private List<float> zooms;

    public bool BlockCamInput { get; set; }

    public EventHandler OnZoomed;

    private void Start()
    {
        camera = Camera.main;
        brain = (camera == null) ? null : camera.GetComponent<CinemachineBrain>();
        _vcam = (brain == null) ? null : brain.ActiveVirtualCamera as CinemachineVirtualCamera;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_controllerCounter > zooms.Count + 1)
                SwitchViews.SwitchView(sideView, frontView);

            if (!BlockCamInput)
            {

                BlockCamInput = true;
                _target = targets[0];

                Vector3 targPos = _target.position;

                StartCoroutine(LerpOrtographicSize(zooms[0], _smooth));
                StopCoroutine(LerpOrtographicSize(zooms[0], _smooth));

                StartCoroutine(LerpPosition(new Vector2(targPos.x, targPos.y), _smooth));
                StopCoroutine(LerpPosition(new Vector2(targPos.x, targPos.y), _smooth));

                targets.RemoveAt(0);
                zooms.RemoveAt(0);

            }
        }
    }

    private IEnumerator LerpOrtographicSize(float zoom, float duration)
    {
        float time = 0;
        float camOrtSize = _vcam.m_Lens.OrthographicSize;

        while (time < duration)
        {
            _vcam.m_Lens.OrthographicSize = 
                Mathf.Lerp(camOrtSize, zoom, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LerpPosition(Vector2 targetPosition, float duration)
    {
        float time = 0;
        Vector2 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = 
                Vector2.Lerp(startPosition, targetPosition, time / duration);

            time += Time.deltaTime;
            yield return null;
        }
        //When the camera is at needed position message will appear
        if (_controllerCounter < _controllers.Length)
        {
            _controllers[_controllerCounter].StartDialogue();
            _controllerCounter++;
        }

        transform.position = targetPosition;
        //BlockCamInput = false;
    }
}
