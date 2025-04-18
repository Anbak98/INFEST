using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] protected UIScreen[] _screens;

    protected Dictionary<Type, UIScreen> _screenLookup;

    protected UIScreen _activeScreen;
    protected IPopup _popupHandler;

    protected virtual void Awake()
    {
        _screenLookup = new Dictionary<Type, UIScreen>();

        foreach (var screen in _screens)
        {
            screen.Controller = this;

            var t = screen.GetType();

            while (true)
            {
                _screenLookup.Add(t, screen);

                if (t.BaseType == null || typeof(UIScreen).IsAssignableFrom(t) == false || t.BaseType == typeof(UIScreen))
                {
                    break;
                }

                t = t.BaseType;
            }

            if (typeof(IPopup).IsAssignableFrom(t))
            {
                _popupHandler = (IPopup)screen;
            }
        }

        foreach (var screen in _screens)
        {
            screen.Init();
        }
    }

    protected virtual void Start()
    {
        ShowMain();
    }

    public void ShowMain()
    {
        if (_screens != null && _screens.Length > 0)
        {
            _screens[0].Show();
            _activeScreen = _screens[0];
        }
    }

    public virtual void Show<S>() where S : UIScreen
    {
        if (_screenLookup.TryGetValue(typeof(S), out var result))
        {
            if (result.IsModal == false && _activeScreen != result && _activeScreen)
            {
                _activeScreen.Hide();
            }
            if (_activeScreen != result)
            {
                result.Show();
            }
            if (result.IsModal == false)
            {
                _activeScreen = result;
            }
        }
        else
        {
            Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
        }
    }

    public void Hide()
    {
        if (_activeScreen != null)
        {
            _activeScreen.Hide();
            _activeScreen = null;
        }
    }

    public virtual S Get<S>() where S : UIScreen
    {
        if (_screenLookup.TryGetValue(typeof(S), out var result))
        {
            return result as S;
        }
        else
        {
            Debug.LogError($"Show() - Screen type '{typeof(S).Name}' not found");
            return null;
        }
    }

    public void Popup(string msg, string header = default)
    {
        if (_popupHandler == null)
        {
            Debug.LogError("Popup() - no popup handler found");
        }
        else
        {
            TooltipData tooltipData = new(msg, header);
            _popupHandler.OpenPopup(tooltipData);
        }
    }    
}
