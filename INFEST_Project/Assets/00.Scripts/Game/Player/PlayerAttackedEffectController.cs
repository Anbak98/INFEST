using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackedEffectController : MonoBehaviour
{
    [Header("Attack Effect Components")]
    [SerializeField] private GameObject _uiAttackEffect;
    [SerializeField] private float _fadeDuration = 1.0f;

    [Header("Camera Components")]
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeIntensity = 2.0f;

    private CinemachineBasicMultiChannelPerlin _cameraNoise;

    private void Awake()
    {
        if (_virtualCamera != null)
        {
            _cameraNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void CalledWhenPlayerAttacked()
    {
        //StartCoroutine(ShowCameraEffectAttacked());
        StartCoroutine(ShowUIEffectAttacked());
    }

    private IEnumerator ShowCameraEffectAttacked()
    {
        // 카메라 쉐이킹 시작
        if (_cameraNoise != null)
        {
            _cameraNoise.m_AmplitudeGain = _shakeIntensity;
            yield return new WaitForSeconds(_shakeDuration);
            _cameraNoise.m_AmplitudeGain = 0f;
        }
    }

    private IEnumerator ShowUIEffectAttacked()
    {
        if (!_uiAttackEffect.activeSelf)
        {
            _uiAttackEffect.SetActive(true);

            // 페이드 아웃 시작
            CanvasGroup canvasGroup = _uiAttackEffect.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = _uiAttackEffect.AddComponent<CanvasGroup>();
            }

            canvasGroup.alpha = 1.0f;

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, elapsed / _fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = 0f;
            _uiAttackEffect.SetActive(false);
        }
    }
}
