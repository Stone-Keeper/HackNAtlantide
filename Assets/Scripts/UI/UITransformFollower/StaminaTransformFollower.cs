using System.Collections;
using System.Collections.Generic;
using UI.UITransformFollower;
using UnityEngine;

public class StaminaTransformFollower : MonoBehaviour
{
    [SerializeField] private PlayerDetectionScriptableObject _playerPosData;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private float smoothTimeUpdtater = 0.1f;
    private Camera _camera;
    Vector3 currentVelocityRef = Vector3.zero;
    private void Awake()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        UpdatePosition();
    }
    void UpdatePosition()
    {
         transform.position = Vector3.SmoothDamp(transform.position, _playerPosData.PlayerPosition + _positionOffset, ref currentVelocityRef, smoothTimeUpdtater);
         transform.forward = _camera.transform.forward;
    }

}
