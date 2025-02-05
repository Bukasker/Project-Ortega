using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAnimation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float amplitude = 1.0f;
    [SerializeField] private float frequency = 0.5f;
    [SerializeField] private float pointerYPosition = 1f;

    private void Update()
    {
        FloatAndRotatePointer();
    }
    private void FloatAndRotatePointer()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x,
                                         (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude) + pointerYPosition,
                                         transform.position.z);
    }
}
