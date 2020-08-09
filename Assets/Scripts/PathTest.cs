using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTest : MonoBehaviour
{
    public float PathSpeed;
    public Transform target;
    public LayerMask lm;
    public float sight;
    bool rayset = false;
    float angleToTarget = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Pathfind((Vector2)target.position);
    }
    void Pathfind(Vector2 target)
    {
        if (RaycastInDirection(angleToTarget,sight).hit.collider == null){
            rayset = false;
        }
        Vector2 normal = (target - (Vector2)transform.position).normalized;
        if (!rayset)
        {
            angleToTarget = Mathf.Atan2(normal.x, -normal.y) * Mathf.Rad2Deg;
        }
        DirectionalRaycastResult r = RaycastInDirection(angleToTarget - 90, sight);
        List<DirectionalRaycastResult> rays = new List<DirectionalRaycastResult>(0);
        if (r.hit.collider != null)
        {
            for (int a = -180; a < 180; a += 8)
            {
                DirectionalRaycastResult r2d = RaycastInDirection(angleToTarget - 90 + a, 1000);
                if (r2d.hit.collider == null)
                {
                    rays.Add(r2d);
                }
            }
            if (rays.Count > 0)
            {
                rayset = true;
                Invoke("ClearRays", 2f);
                float greatestDist = 19999;
                float greatestAngle = 0;
                for (int i = 0; i < rays.Count; i++)
                {
                    if ((((Vector2)transform.position + reverseAtan(rays[i].angle) * 20) - target).magnitude < greatestDist)
                    {
                        greatestAngle = rays[i].angle + 90;
                        greatestDist = (((Vector2)transform.position + reverseAtan(rays[i].angle) * 20) - target).magnitude;
                    }
                }
                angleToTarget = greatestAngle;
            }
        }
        transform.position += (Vector3)reverseAtan(angleToTarget - 90) * PathSpeed;// * (target - (Vector2)transform.position).magnitude;
    }
    Vector2 reverseAtan(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }
    DirectionalRaycastResult RaycastInDirection(float angle, float dist)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, reverseAtan(angle), dist, lm);
        if (hit.collider == null)
        {
            Debug.DrawRay(transform.position, reverseAtan(angle) * dist, Color.yellow);
        }
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, reverseAtan(angle) * hit.distance, Color.yellow);
        }
        return new DirectionalRaycastResult(hit, angle);
    }
    void ClearRays()
    {
        rayset = false;
    }
}
public class DirectionalRaycastResult
{
    public RaycastHit2D hit;
    public float angle;
    public DirectionalRaycastResult(RaycastHit2D _hit, float _angle)
    {
        hit = _hit;
        angle = _angle;
    }
}