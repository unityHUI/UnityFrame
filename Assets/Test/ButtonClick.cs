using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonClick : MonoBehaviour
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
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(action);
    }
    public void AddSliderListener(UnityAction<float> action)
    {
        Slider slider = transform.GetComponent<Slider>();
        slider.onValueChanged.AddListener(action);
    }
}
