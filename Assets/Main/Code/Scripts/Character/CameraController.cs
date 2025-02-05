using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour
{
    [Header("CameraSettings")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform character;
    Vector3 previousPos = Vector3.zero;
    Vector3 deltaPos = Vector3.zero;

    private void Update()
    {
        CamControl();
    }
    void CamControl()
    {
        Vector3 targetPos = new Vector3(character.position.x, cam.transform.position.y, character.position.z - 12f);
        cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 5f);
    }

}