using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Projectile
{
    [SerializeField] GameObject model;
    [SerializeField] ParticleSystem explosion;

    private float mineDamage;
    private float mineHitLaunchForce;
    float maxMineTime = 10;

    Fighter fighterRoot;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    public void SetVariables(float damage, float launchForce, float maxMineTime, Fighter fighterRoot)
    {
        mineDamage = damage;
        mineHitLaunchForce = launchForce;
        this.maxMineTime = maxMineTime;
        this.fighterRoot = fighterRoot;
    }

    public override void OnHitFighter()
    {
        model.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = true;

        Fighter hitFighter = GetHitFighter();

        Vector3 forceDirectionVector = (hitFighter.transform.position - transform.position).normalized;

        hitFighter.GetRigidBody().AddForceAtPosition((hitFighter.transform.up * 20) * (mineHitLaunchForce * 1.5f) * Mathf.Abs(Physics.gravity.y / 10), transform.position);
        hitFighter.GetRigidBody().AddForceAtPosition((forceDirectionVector * 20) * mineHitLaunchForce * Mathf.Abs(Physics.gravity.y / 10), transform.position);
        hitFighter.TakeDamage(mineDamage, fighterRoot);

        explosion.Play();
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(maxMineTime);
        Destroy(this.gameObject);
    }
}
