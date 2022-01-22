using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySliderManager : MonoBehaviour
{
    [SerializeField]
    private GameObject go_background;
    [SerializeField]
    private RocketLauncher launcher;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private int i_defaultSensitivity = 200;

    public delegate void SensitivityChange(float value);
    public static SensitivityChange onSensitivityChanged;

    private bool isDisplayed = false;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = i_defaultSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            isDisplayed = !isDisplayed;

            go_background.SetActive(isDisplayed);
            launcher.Enable(!isDisplayed);

            Cursor.lockState = isDisplayed ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isDisplayed;
        }
    }

    public void OnSliderValueChanged()
    {
        onSensitivityChanged?.Invoke(slider.value);
    }
}
