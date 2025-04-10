using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("반동 세기")]
    public Vector3 recoilKickBack = new Vector3(-0.1f, 0f, 0f); // 뒤로 물러남
    public Vector3 recoilRotation = new Vector3(5f, 2f, 2f);    // 회전 반동

    [Header("회복 속도")]
    public float returnSpeed = 5f;
    public float snappiness = 10f;

    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;


    private void Awake()
    {
        _startPosition = transform.localPosition;
    }
    void Update()
    { 
        _targetPosition = Vector3.Lerp(_targetPosition, _startPosition, returnSpeed * Time.deltaTime);
        _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, snappiness * Time.deltaTime);
        transform.localPosition = _currentPosition;


        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, snappiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void ApplyCamRecoil(float multiplier = 1f)
    {
         _targetRotation += new Vector3(
            recoilRotation.x * multiplier,
            Random.Range(-recoilRotation.y, recoilRotation.y) * multiplier,
            Random.Range(-recoilRotation.y, recoilRotation.z) * multiplier
        );
    }

    public void ApplyGunRecoil(float multiplier = 1f)
    {
        _targetPosition += recoilKickBack * multiplier;

        ApplyCamRecoil(multiplier);
    }

    public void ChangePosition(Vector3 changePosition)
    {
        _startPosition = changePosition;
    }

}
