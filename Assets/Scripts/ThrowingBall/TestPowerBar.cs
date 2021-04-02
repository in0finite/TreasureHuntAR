using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TestPowerBar : MonoBehaviour
{
    [SerializeField] private Image _powerFillImage;
    [SerializeField] private float _fillSpeed = 0.5f;

    private Coroutine _fillCoroutine;

    public float FillAmount => _powerFillImage.fillAmount;

    public void StartFill()
    {
        _fillCoroutine = StartCoroutine(StartFillCoroutine());
    }

    public void StopFill()
    {
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
            _fillCoroutine = null;
        }
    }

    public void ResetFill()
    {
        StopFill();
        _powerFillImage.fillAmount = 0;
    }

    private IEnumerator StartFillCoroutine()
    {
        while (_powerFillImage.fillAmount != 1)
        {
            _powerFillImage.fillAmount += Time.deltaTime * _fillSpeed;
            yield return null;
        }
    }
}
