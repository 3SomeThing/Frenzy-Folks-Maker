using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditObjectSlider : MonoBehaviour
{
    [SerializeField] public TMP_Text sliderValueText;
    [SerializeField] public TMP_Text labelText;
    [SerializeField] public Slider slider;

    public void SetSliderValue()
    {
        sliderValueText.text = slider.value.ToString("0.00");
    }
}
