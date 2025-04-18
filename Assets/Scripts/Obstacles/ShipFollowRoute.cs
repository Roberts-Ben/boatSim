using System.Collections.Generic;
using UnityEngine;

public class ShipFollowRoute : MonoBehaviour
{
    [SerializeField] private List<Route> routes;
    [SerializeField] private List<float> timeToTraverseRoutes; // Allows certain legs to be faster than others, or to normalise travel time

    private float timePassed;
    private float progress;
    private Vector3 waypointPos;

    private int activeRoute = 0;
    // Initialise based on where the ship starts in relation to it's assigned route. N.B ship doesn't snap to that location so it would instead move towards that assigned location until it reaches it
    [SerializeField] private int startingRoute;
    [SerializeField] private float startingProgress;

    [SerializeField] private float acceleration = 200f;
    [SerializeField] private float turnForce = 0.5f;

    [SerializeField] private float rotationOffset = 90f;

    // The 4 points of each curve on the current route
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        activeRoute = startingRoute;
        timePassed = startingProgress;

        ProcessCurrentRoute();
    }

    void FixedUpdate()
    {
        if (routes.Count == 0 || activeRoute >= routes.Count || routes[activeRoute].waypoints.Count < 4)
        {
            Debug.LogWarning("Invalid route data: " + activeRoute);
            return;
        }

        timePassed += Time.deltaTime;
        progress = Mathf.Clamp01(timePassed / timeToTraverseRoutes[activeRoute]); // Step through the current curve and set the next destination
        waypointPos = Route.CalculateBezierCurvePoint(progress, p0, p1, p2, p3);

        ApplyForwardForce();
        ApplyTurningForce();

        if (progress >= 1)
        {
            timePassed = 0;
            activeRoute++;

            if (activeRoute >= routes.Count)
            {
                activeRoute = 0;
            }
            ProcessCurrentRoute();
        }
    }

    /// <summary>
    /// Grabs the current route nodes as the route is started
    /// </summary>
    public void ProcessCurrentRoute()
    {
        Route currentRoute = routes[activeRoute];

        p0 = currentRoute.waypoints[0].position;
        p1 = currentRoute.waypoints[1].position;
        p2 = currentRoute.waypoints[2].position;
        p3 = currentRoute.waypoints[3].position;
    }

    public void ApplyForwardForce()
    {
        // Move towards the current target
        Vector3 direction = waypointPos - transform.position;
        rb.AddForce(direction * acceleration, ForceMode.Force);
    }

    public void ApplyTurningForce()
    {
        // Sets desired angle to rotate towards
        var localTarget = transform.InverseTransformPoint(waypointPos);
        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        Vector3 eulerAngleVelocity = new(0, angle + rotationOffset, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * turnForce * Time.deltaTime);

        rb.MoveRotation(rb.rotation * deltaRotation); // Avoiding torque approach to keep the ship tighter to the curve
    }
}