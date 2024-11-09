using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;

    private void Awake()
    {
        RangeWeapon.onBulletShoot += Shake;
    }

    private void OnDestroy()
    {
        RangeWeapon.onBulletShoot -= Shake;
    }


    [NaughtyAttributes.Button]
    private void Shake()
    {
        Vector2 direction = Random.onUnitSphere.With(z: 0).normalized;
        transform.localPosition = Vector3.zero;

        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, direction * shakeMagnitude, shakeDuration).setEase(LeanTweenType.easeShake);
    }
}
