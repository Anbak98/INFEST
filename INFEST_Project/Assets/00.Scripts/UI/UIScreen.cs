using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    protected static readonly int HideAnimHash = Animator.StringToHash("Hide");
    protected static readonly int ShowAnimHash = Animator.StringToHash("Show");

    [SerializeField] private bool _isModal;
    [SerializeField] private List<ScreenPlugin> _plugins;

    private Animator _animator;
    private Coroutine _hideCoroutine;

    public List<ScreenPlugin> Plugins => _plugins;
    public bool IsModal => _isModal;
    public bool IsShowing { get; private set; }
    public UIController Controller { get; set; }

    protected virtual void Start()
    {
        TryGetComponent(out _animator);
    }

    public virtual void Awake() { }

    public virtual void Init() { }

    public virtual void Hide()
    {
        if (_animator)
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
            }

            _hideCoroutine = StartCoroutine(HideAnimCoroutine());
            return;
        }

        IsShowing = false;

        foreach (var p in _plugins)
        {
            p.Hide(this);
        }

        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        if (_hideCoroutine != null)
        {
            StopCoroutine(_hideCoroutine);
            if (_animator.gameObject.activeInHierarchy && _animator.HasState(0, ShowAnimHash))
            {
                _animator.Play(ShowAnimHash, 0, 0);
            }
        }

        gameObject.SetActive(true);

        IsShowing = true;

        foreach (var p in _plugins)
        {
            p.Show(this);
        }
    }

    private IEnumerator HideAnimCoroutine()
    {
#if UNITY_IOS || UNITY_ANDROID
      var changedFramerate = false;
      if (Config.AdaptFramerateForMobilePlatform) {
        if (Application.targetFrameRate < 60) {
          Application.targetFrameRate = 60;
          changedFramerate = true;
        }
      }
#endif

        _animator.Play(HideAnimHash);
        yield return null;
        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

#if UNITY_IOS || UNITY_ANDROID
      if (changedFramerate) {
        new FusionMenuGraphicsSettings().Apply();
      }
#endif

        gameObject.SetActive(false);
    }
}
