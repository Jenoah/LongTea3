using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Hammer : FighterWeapon, IWeapon
{
    [Header("Hammer behaviour properties")]
    [SerializeField] float hammerForce;
    [SerializeField, Range(-10,10)] float totalHammerLaunchForceMultiplier;
    [SerializeField] float cooldown;

    [Header("Hammer references")]
    [SerializeField] GameObject hammerTip;
    [SerializeField] LayerMask hammerTipMask;
    [SerializeField] LayerMask collidableLayers = Physics.AllLayers;

    bool isSwinging;
    bool canHit;

    Coroutine hammerSwingRotationRoutine;

    public override void ActivateWeapon(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            if (transform.localEulerAngles.x > -1 && transform.localEulerAngles.x < 1)
            {
                float hammerTime = (100 / hammerForce) / 2;
                hammerSwingRotationRoutine = RotateObject.instance.RotateObjectToAngle(this.transform.gameObject, new Vector3(90, 0, 0), hammerTime);
                isSwinging = true;
                canHit = true;
                OnAttack.Invoke();
                if (fighterRoot) fighterRoot.onAttack();
                StartCoroutine(ResetHammerWhenDone(hammerTime));
            }
        }
    }

    public void FixedUpdate()
    {
        if (isSwinging) CheckCollision();
    }

    public override void CheckCollision()
    {
        List<Collider> tipHits = new List<Collider>();
        List<Collider> hits = new List<Collider>();

        foreach (Collider childCollider in GetComponentsInChildren<Collider>())
        {
            if (childCollider.gameObject != hammerTip.gameObject)
            {
                hits.AddRange(Physics.OverlapBox(childCollider.transform.position, childCollider.transform.lossyScale / 2, childCollider.transform.rotation, collidableLayers, QueryTriggerInteraction.Collide).ToList());
            }
        }
        tipHits.AddRange(Physics.OverlapBox(hammerTip.transform.position, hammerTip.transform.lossyScale / 2, hammerTip.transform.rotation, collidableLayers, QueryTriggerInteraction.Collide).ToList());

        if (hits.Count > 0)
        {
            foreach (Collider hit in hits)
            {
                Fighter otherFighter = hit.GetComponentInParent<Fighter>();
                if (otherFighter != fighterRoot)
                {
                    RotateObject.instance.StopCoroutine(hammerSwingRotationRoutine);
                    isSwinging = false;
                    hammerSwingRotationRoutine = RotateObject.instance.RotateObjectToAngle(this.transform.gameObject, new Vector3(0, 0, 0), transform.localEulerAngles.x / 90);
                }               
            }
        }

        if (tipHits.Count > 0)
        {
            foreach (Collider hit in tipHits)
            {
                if (hit.GetComponentInParent<Fighter>())
                {
                    Fighter otherFighter = hit.GetComponentInParent<Fighter>();
                    if (otherFighter != fighterRoot && canHit)
                    {
                        otherFighter.GetComponent<Rigidbody>().AddForceAtPosition(((transform.up * 2)) * ((hammerForce) * (totalHammerLaunchForceMultiplier * 20)) * Mathf.Abs(Physics.gravity.y / 10), hit.gameObject.transform.position);
                        RotateObject.instance.StopCoroutine(hammerSwingRotationRoutine);
                        isSwinging = false;
                        hammerSwingRotationRoutine = RotateObject.instance.RotateObjectToAngle(this.transform.gameObject, new Vector3(0, 0, 0), transform.localEulerAngles.x / 90);
                        otherFighter.TakeDamage(damage, otherFighter);
                        canHit = false;
                    }
                }
            }
        }
    }

    private IEnumerator ResetHammerWhenDone(float hammerTime)
    {
        yield return new WaitForSeconds(hammerTime);
        isSwinging = false;
        hammerSwingRotationRoutine = RotateObject.instance.RotateObjectToAngle(this.transform.gameObject, new Vector3(0, 0, 0), cooldown);
    }
}
