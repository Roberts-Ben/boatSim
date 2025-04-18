using UnityEngine;
using System.Collections.Generic;

public class Buoyancy : MonoBehaviour
{
    [SerializeField] private List<Transform> floatPoints = new();

    [SerializeField] private float buoyancy = 400f;
    [SerializeField] private float waterHeightAtPoint = 0f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach(Transform point in floatPoints) // See how much of our boat is below the surface
        {
            if (point == null) continue;

            waterHeightAtPoint = Waves.instance.GetWaveHeight(point.transform.position.x, point.transform.position.z); // Get the simulated wave height at this location

            float diff = point.position.y - waterHeightAtPoint;

            if (diff < 0)
            {
                rb.AddForceAtPosition(Vector3.up * buoyancy * Mathf.Abs(diff), point.position, ForceMode.Force); // Push that point back up, based on how far under it is
            }
        }
    }
}
