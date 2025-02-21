using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraWallCollison : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public LayerMask wallLayer;
    public float minDistance = 2f;
    public float maxDistance = 30f;
    public float damping = 0.2f;

    private Transform cameraTransform;
    private Transform followTarget;
    private float currentDistance;

    void Start() {
        cameraTransform = virtualCamera.VirtualCameraGameObject.transform;
        followTarget = virtualCamera.Follow;
        currentDistance = maxDistance;
    }

    void LateUpdate() {
        Vector3 direction = (cameraTransform.position - followTarget.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(followTarget.position, direction + new Vector3(0.5f,0,0), out hit, maxDistance, wallLayer)) {
            currentDistance = Mathf.Lerp(currentDistance, Mathf.Clamp(hit.distance, minDistance, maxDistance), damping);
        }
        else if(Physics.Raycast(followTarget.position, direction + new Vector3(-0.5f, 0, 0), out hit, maxDistance, wallLayer)) {
            currentDistance = Mathf.Lerp(currentDistance, Mathf.Clamp(hit.distance, minDistance, maxDistance), damping);
        }
        else {
            currentDistance = Mathf.Lerp(currentDistance, maxDistance, damping);
        }

        cameraTransform.position = followTarget.position + direction * currentDistance;
    }
}
