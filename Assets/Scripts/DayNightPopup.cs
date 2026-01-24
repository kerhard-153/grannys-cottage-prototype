using TMPro;
using UnityEngine;
using System.Collections;

public class DayNightPopup : MonoBehaviour
{
    [Header("UI Refs")]
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    [Header("Settings")]
    public float popupDuration = 2f;

    private void OnEnable()
    {
        DayNightCycle.OnTimeChanged += ShowPopup;
    }

    private void OnDisable()
    {
        DayNightCycle.OnTimeChanged -= ShowPopup;
    }

    void ShowPopup(DayNightCycle.TimeOfDay time)
    {
        StopAllCoroutines();
        StartCoroutine(PopupRoutine(time));
    }

    IEnumerator PopupRoutine(DayNightCycle.TimeOfDay time)
    {
        popupPanel.SetActive(true);
        popupText.text = time == DayNightCycle.TimeOfDay.Day ? "DAYTIME" : "NIGHTFALL";

        yield return new WaitForSeconds(popupDuration);

        popupPanel.SetActive(false);
    }
}
