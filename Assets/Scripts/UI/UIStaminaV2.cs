using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStaminaV2 : MonoBehaviour
{
    [SerializeField] PlayerStaminaScriptableObject _staminaSO;
    private Material _materialStamina;
    private void Awake()
    {
        _materialStamina = GetComponent<SpriteRenderer>().material;
    }
    private void OnEnable()
    {
        _staminaSO.OnValueChanged += SetMaterial;
        _staminaSO.FailUseStamina += FailUseStamina;
    }

    private void SetMaterial(float value)
    {
        _materialStamina.SetFloat("_FillAmount", _staminaSO.Value01);
    }
    private void FailUseStamina()
    {
        StartCoroutine(Flick());
    }

    IEnumerator Flick()
    {
        _materialStamina.SetFloat("_FailUseStamina", 1);
        yield return new WaitForSeconds(0.5f);
        _materialStamina.SetFloat("_FailUseStamina", 0);
    }
}
