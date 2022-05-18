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

    protected Fighter fighterRoot;
    protected Rigidbody fighterRigidBody;

    [SerializeField] protected GameObject damageText;
 
    public void SetReferences(Fighter fighter, Rigidbody rb)
    {
        fighterRoot = fighter;
        fighterRigidBody = rb;
    }

    public Rigidbody GetRigidBodyFighter()
    {
        return fighterRigidBody;
    }

    public void TakeDamage(int damage, Vector3 hitPos)
    {
        if (fighterRoot.isDead) return;
        healthPoints -= damage;
        GameObject damageTextObject = LeanPool.Spawn(damageText, hitPos, transform.rotation);
        damageTextObject.GetComponent<TextMeshPro>().alpha = 1;
        damageTextObject.GetComponent<TextMeshPro>().text = damage.ToString();
        //damageTextObject.GetComponent<TMPro.TextMeshPro>().text = "Bruh";
        damageTextObject.transform.DOMoveY(transform.position.y + damageTextObject.transform.position.y + 2, 3);
        LeanPool.Despawn(damageTextObject, 3);
        fighterRoot.onTakeDamage();
        fighterRoot.CheckDeath();
    }
}
