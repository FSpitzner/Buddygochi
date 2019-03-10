using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Healthbar : MonoBehaviour
{

    #region Variable Declarations
    [Space]
    [SerializeField] Gradient hpColor = null;

    [Header("Tweening")]
    #pragma warning disable CS0414
    [SerializeField] bool punchOnUpdate = true;
    #pragma warning restore CS0414
    [ConditionalHide("punchOnUpdate", false)]
    [SerializeField] float punchAmount = 0.1f;
    [ConditionalHide("punchOnUpdate", false)]
    [SerializeField] float punchDuration = 0.3f;

    [Header("References")]
    [SerializeField] Transform healthbarForeground = null;
    #endregion



    #region Unity Event Functions

    #endregion



    #region Public Functions
    public void UpdateHealthbar(float percentage)
    {
        // Error handling
        if (percentage > 1f || percentage < -0.01f)
        {
            Debug.LogError("Tried to update healthbar with invalid percentage value: " + percentage);
            return;
        }

        // Set new color of the healthbar
        healthbarForeground.GetComponent<UnityEngine.UI.Image>().color = hpColor.Evaluate(percentage);

        // Rescale the green part of the healthbar
        Vector3 newScale = new Vector3(percentage, healthbarForeground.localScale.y, healthbarForeground.localScale.z);
        healthbarForeground.transform.localScale = newScale;

        // Add Juiciness
        LeanTween.scale(healthbarForeground.gameObject, newScale + Vector3.one * punchAmount, punchDuration).setEase(LeanTweenType.punch);
    }


    public void UpdateHealthBarSpecial(float value, float min, float max)
    {
        float normal = Mathf.InverseLerp(min, max, value);
        float bValue = Mathf.Lerp(0, 1, normal);
        UpdateHealthbar(bValue);
    }
    #endregion
}
