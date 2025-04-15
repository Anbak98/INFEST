using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil")]
    public Vector3 recoilKickBack = new Vector3(0f, 0f, -0.1f); // 뒤로 물러남
    public Vector3 recoilRotation = new Vector3(5f, 2f, 2f);    // 회전 반동

    [Header("Recovery")]
    public float returnSpeed = 5f;
    public float snappiness = 10f;

    private Vector3 _startPosition; // 시작 위치
    private Vector3 _currentPosition; // 현재 위치
    private Vector3 _targetPosition; // 반동으로 옮겨지는 위치
    private Vector3 _currentRotation; // 현재 회전
    private Vector3 _targetRotation; // 반동으로 옮겨지는 회전


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

    public void ApplyCamRecoil(float multiplier = 1f) // 화면 반동
    {
         _targetRotation += new Vector3(
            recoilRotation.x * multiplier,
            Random.Range(-recoilRotation.y, recoilRotation.y) * multiplier,
            Random.Range(-recoilRotation.y, recoilRotation.z) * multiplier
        );
    }

    public void ApplyGunRecoil(float multiplier = 1f) //총기 반동
    {
        _targetPosition += recoilKickBack * multiplier;

        ApplyCamRecoil(multiplier);
    }

    public void ChangePosition(Vector3 changePosition) // 초기화 위치 변경
    {
        _startPosition = changePosition;
    }

}
