using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefabs;

    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += InstantiateDamageText;
        PlayerHealth.onAttackDodged += AttackDodgedCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken -= InstantiateDamageText;
        PlayerHealth.onAttackDodged -= AttackDodgedCallback;
    }

    private void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private DamageText CreateFunction() => Instantiate(damageTextPrefabs, transform);

    private void ActionOnGet(DamageText text) => text.gameObject.SetActive(true);

    private void ActionOnRelease(DamageText text)
    {
        if (text == null) return;
        text.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(DamageText text) => Destroy(text.gameObject);

    private void InstantiateDamageText(int damage, Vector2 pos, bool isCriticalHit)
    {
        DamageText damageTextIns = damageTextPool.Get();

        Vector3 spawnPosition = pos + Vector2.up * 0;
        damageTextIns.transform.position = spawnPosition;

        damageTextIns.PlayAnim(damage.ToString(), isCriticalHit);
        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextIns));
    }

    private void AttackDodgedCallback(Vector2 pos)
    {
        DamageText damageTextIns = damageTextPool.Get();

        Vector3 spawnPosition = pos + Vector2.up * 0;
        damageTextIns.transform.position = spawnPosition;

        damageTextIns.PlayAnim("Dodge", false);
        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextIns));
    }

}
