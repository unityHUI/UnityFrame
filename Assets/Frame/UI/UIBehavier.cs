using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UIBehavier : MonoBehaviour
{

    void Awake()
    {
        UIManager.Instance.AddGameObject(name, gameObject);
    }
    void OnDestroy()
    {
        UIManager.Instance.RemoveGameObejct(name);
    }

    public void AddButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            btn.onClick.AddListener(action);
        }
    }
    public void RemoveButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            btn.onClick.RemoveListener(action);
        }
    }
    public void AddToggleListener(UnityAction<bool> action)
    {
        if (action != null)
        {
            Toggle toggle = transform.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(action);
        }
    }
    public void RemoveToggleListener(UnityAction<bool> action)
    {
        if (action != null)
        {
            Toggle toggle = transform.GetComponent<Toggle>();
            toggle.onValueChanged.RemoveListener(action);
        }
    }

    public void AddSliderListener(UnityAction<float> action)
    {
        if (action != null)
        {
            Slider slider = transform.GetComponent<Slider>();
            slider.onValueChanged.AddListener(action);
        }
    }
    public void RemoveSliderListener(UnityAction<float> action)
    {
        if (action != null)
        {
            Slider slider = transform.GetComponent<Slider>();
            slider.onValueChanged.RemoveListener(action);
        }
    }
    public void AddInputChangeListener(UnityAction<string> action)
    {
        if (action != null)
        {
            InputField field = transform.GetComponent<InputField>();
            field.onValueChanged.AddListener(action);
        }
    }
    public void RemoveInputChangeListener(UnityAction<string> action)
    {
        if (action != null)
        {
            InputField field = transform.GetComponent<InputField>();
            field.onValueChanged.RemoveListener(action);
        }
    }

    public void AddInputEditEndListener(UnityAction<string> action)
    {
        if (action != null)
        {
            InputField field = transform.GetComponent<InputField>();
            field.onEndEdit.AddListener(action);
        }
    }

    public void RemoveInputEditEndListener(UnityAction<string> action)
    {
        if (action != null)
        {
            InputField field = transform.GetComponent<InputField>();
            field.onEndEdit.RemoveListener(action);
        }
    }

}
