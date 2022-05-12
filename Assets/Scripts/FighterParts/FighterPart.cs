using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using DG.Tweening;
using TMPro;

public class FighterPart : MonoBehaviour
{
    public float healthPoints;
    public float weight;

    [SerializeField] protected GameObject damageText;
 
    public void TakeDamage(int damage, Vector3 hitPos)
    {
        healthPoints -= damage;
        GameObject damageTextObject = LeanPool.Spawn(damageText, hitPos, transform.rotation);
        damageTextObject.GetComponent<TMPro.TextMeshPro>().alpha = 1;
        damageTextObject.GetComponent<TMPro.TextMeshPro>().text = damage.ToString();
        damageTextObject.transform.DOMoveY(transform.position.y + damageTextObject.transform.position.y + 2, 3);
        LeanPool.Despawn(damageTextObject, 3);
    }
}
