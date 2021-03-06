using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayOnBottom : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.down;
    [SerializeField] private LayerMask hitLayers = Physics.AllLayers;
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.05f, 0f);
    [SerializeField] private Transform castOrigin = null;
    [SerializeField] private Vector3 castOffset = new Vector3(0f, 0.1f, 0f);

    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    // Update is called once per frame
    void Update()
    {
        if(castOrigin && Physics.Raycast(castOrigin.position + castOffset, direction, out RaycastHit hit, maxDistance, hitLayers))
        {
            targetRotation = Quaternion.LookRotation(hit.normal);
            targetPosition = hit.point;
            targetPosition += transform.TransformDirection(offset);
        }

        transform.SetPositionAndRotation(targetPosition, targetRotation);
    }
}
