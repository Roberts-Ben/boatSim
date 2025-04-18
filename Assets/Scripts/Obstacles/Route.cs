using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public List<Transform> waypoints;

    private Vector3 gizmoPos;

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Count < 4)
        {
            Debug.LogWarning("Invalid route data - not enough nodes");
            return;
        }

        for (int i = 0; i < waypoints.Count; i++)
        {
            // Visuals for each of the 4 nodes
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(waypoints[i].position, Vector3.one);
        }

        for (float time = 0; time <= 1; time += 0.05f)
        {
            // Drop visuals at intervals along the path of the curve
            gizmoPos = CalculateBezierCurvePoint(time, waypoints[0].position, waypoints[1].position, waypoints[2].position, waypoints[3].position);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gizmoPos, 0.25f);
        }

        Gizmos.DrawLine(new Vector3(waypoints[0].position.x, waypoints[0].position.y, waypoints[0].position.z), // Draw lines between start/end node and it's director
            new Vector3(waypoints[1].position.x, waypoints[1].position.y, waypoints[1].position.z));
        Gizmos.DrawLine(new Vector3(waypoints[2].position.x, waypoints[2].position.y, waypoints[2].position.z),
            new Vector3(waypoints[3].position.x, waypoints[3].position.y, waypoints[3].position.z));

    }

    public static Vector3 CalculateBezierCurvePoint(float progress, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return Mathf.Pow(1 - progress, 3) * p0 + 3
            * Mathf.Pow(1 - progress, 2) * progress * p1 + 3
            * (1 - progress) * Mathf.Pow(progress, 2) * p2 + Mathf.Pow(progress, 3) * p3;
    }
}
