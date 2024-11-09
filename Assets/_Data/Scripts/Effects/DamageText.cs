using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro txtDamage;
    [SerializeField] private Animator anim;

    [NaughtyAttributes.Button]
    public void PlayAnim(string damage, bool isCriticalHit)
    {
        anim.Play("TextFading");
        txtDamage.text = damage;
        txtDamage.color = isCriticalHit ? Color.red : Color.white;
    }
}
