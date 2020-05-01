using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Math : MonoBehaviour
{
    public float radius = 1f;

    public Transform testObj;

    Vector3[] localPoints = new Vector3[1];

    [SerializeField, Range(0f, 1f)]
    float sphereRadius = 1f;

    private void OnDrawGizmos()
    {
        Vector3 triggerCenter = transform.position;
        Vector3 testPt = testObj.position;

        Vector3 triggerToPoint = testPt - triggerCenter;
        float distSq = triggerToPoint.x * triggerToPoint.x + triggerToPoint.z * triggerToPoint.z;

        bool inside = distSq <= radius * radius;

        Vector3 triggerToPointDir = triggerToPoint / triggerToPoint.magnitude;
        Gizmos.DrawLine(triggerCenter, triggerCenter + triggerToPointDir);

        Gizmos.color = inside ? Color.green : Color.red;
        Gizmos.DrawWireSphere(triggerCenter, radius);
        Gizmos.color = Color.white;

        for (int i = 0; i < localPoints.Length; i++)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoints[i]);
            Gizmos.DrawSphere(localPoints[i], sphereRadius);
        }
    }
}

//using UnityEngine;

//public class TrigTest : MonoBehaviour
//{

//    [Range(0, 360)]
//    public float angleDeg;
//    public int circleDetail = 3;

//    Vector2 AngToDir(float angRad)
//    {
//        float x = Mathf.Cos(angRad);
//        float y = Mathf.Sin(angRad);
//        return new Vector2(x, y);
//    }

//    float DirToAng(Vector2 vec => Mathf.ATan2(vec.y, vec.x));
    
//    public void OnDrawGizmos()
//    {
//        float angleRad = angleDeg * Mathf.Deg2Rad;
//        Gizmos.DrawLine(transform.position, transform.TransformPoint(AngToDir(angleRad)));

//        Vector3[] circlePts = new Vector3[circleDetail];
//        for (int i = 0; i < circleDetail; i++)
//        {
//            float t = i / (float)circleDetail;
//            float angDeg = t * 360;
//            float angRad = angDeg * Mathf.Deg2Rad;
//            circlePts[i] = transform.TransformPoint((Vector3)AngToDir(angRad));
//            Gizmos.color = Color.HSVToRGB(t, 1, 1);
//            Gizmos.DrawSphere(circlePts[i], 0.05f);
//        }


//        Gizmos.color = Color.white;
//        for (int i = 0; i < circleDetail; i++)
//        {
//            Vector3 startPt = circlePts[i];
//            Vector3 endPt = circlePts[(i + 1) % circleDetail];
//            Gizmos.DrawLine(startPt, endPt);
//        }
//    }


//}

//using UnityEngine;

//public class Placer : MonoBehaviour
//{

//    public Transform objectToPlace;

//    void OnDrawGizmos()
//    {
//        Transform tf = transform;
//        Vector3 rayDir = tf.forward;
//        Vector3 origin = tf.position;
//        Ray ray = new Ray(origin, rayDir);

//        if (Physics.Raycast(ray, out RaycastHit hit))
//        {
//            Gizmos.color = Color.white;
//            Gizmos.DrawLine(origin, hit.point);
//            objectToPlace.position = hit.point;

//            Vector3 upDir = hit.normal;
//            Gizmos.color = Color.green;
//            Gizmos.DrawLine(hit.point, hit.point + upDir);

//            Vector3 rightDir = Vector3.Cross(upDir, rayDir).normalized;
//            Gizmos.color = Color.red;
//            Gizmos.DrawLine(hit.point, hit.point + rightDir);

//            Gizmos.color = Color.cyan;
//            Vector3 forwardDir = Vector3.Cross(rightDir, upDir);
//            Gizmos.DrawLine(hit.point, hit.point + forwardDir);

//            Quaternion objRot = Quaternion.LookRotation(forwardDir, hit.normal);
//            objectToPlace.rotation = objRot;
//        }
//    }

//}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Timeline;
//using UnityEngine;

//public class TriggerLook : MonoBehaviour
//{

//    public Transform player;

//    public float threshold = 0.1f;

//    void OnDrawGizmos()
//    {
//        Vector3 posTrigger = transform.position;
//        Vector3 posPlayer = player.position;
//        Vector3 playerLookDir = player.forward;

//        Vector3 playerToTrigger = posTrigger - posPlayer;
//        Vector3 playerToTriggerDir = playerToTrigger.normalized;

//        // if this is 1, the player is looking straight at it
//        // if this is 0, the player is looking exactly sideways from the trigger
//        // if this is -1, the player is looking straight away from it
//        float looktowardness = Vector3.Dot(playerLookDir, playerToTriggerDir);

//        bool lookingAt = looktowardness >= threshold;

//        Gizmos.color = Color.cyan;
//        Gizmos.DrawLine(posPlayer, posPlayer + playerLookDir);

//        Gizmos.color = lookingAt ? Color.green : Color.red;
//        Gizmos.DrawLine(posPlayer, posPlayer + playerToTriggerDir);
//        Gizmos.color = Color.white;
//    }

//}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.Timeline;
//using UnityEngine;

//public class TriggerLook : MonoBehaviour
//{


//    public enum Type
//    {
//        LookAt,
//        LeftRight
//    }

//    public Type lookType = Type.LeftRight;
//    public Transform player;

//    public float threshold = 0.1f;

//    void OnDrawGizmos()
//    {
//        Vector3 posTrigger = transform.position;
//        Vector3 posPlayer = player.position;
//        Vector3 playerLookDir = player.forward;

//        Vector3 playerToTrigger = posTrigger - posPlayer;
//        Vector3 playerToTriggerDir = playerToTrigger.normalized;

//        bool valid = false;
//        if (lookType == Type.LookAt)
//        {
//            // if this is 1, the player is looking straight at it
//            // if this is 0, the player is looking exactly sideways from the trigger
//            // if this is -1, the player is looking straight away from it
//            float looktowardness = Vector3.Dot(playerLookDir, playerToTriggerDir);
//            valid = looktowardness >= threshold;
//        }
//        else
//        {
//            // top down projected space version
//            // Vector2 triggerPosXZ = transform.position.XZ();
//            // Vector2 playerPosXZ = player.position.XZ();
//            // Vector2 playerLookDirXZ = player.forward.XZ().normalized;
//            // Vector2 dirPlayerToTriggerXZ = (triggerPosXZ - playerPosXZ).normalized;
//            // Vector2 playerPerpLookDirXZ = playerLookDirXZ.Rot90CW();
//            // float leftandrightness = Vector2.Dot( playerPerpLookDirXZ, dirPlayerToTriggerXZ );
//            // Gizmos.color = Color.yellow;
//            // Vector3 playerPerpLookDirWorld = new Vector3( playerPerpLookDirXZ.x, 0f, playerPerpLookDirXZ.y );
//            // Gizmos.DrawLine( posPlayer, posPlayer + playerPerpLookDirWorld );

//            // cross product version
//            // this assumes world space up is your personal left/right reference up vector!
//            float leftandrightness = Vector3.Cross(playerLookDir, playerToTriggerDir).y;

//            valid = leftandrightness < 0;
//        }


//        Gizmos.color = Color.cyan;
//        Gizmos.DrawLine(posPlayer, posPlayer + playerLookDir);

//        Gizmos.color = valid ? Color.green : Color.red;
//        Gizmos.DrawLine(posPlayer, posPlayer + playerToTriggerDir);
//        Gizmos.color = Color.white;
//    }


//}

//public static class Vec2Extensions
//{
//    public static Vector2 Rot90CW(this Vector2 v)
//    {
//        return new Vector2(v.y, -v.x);
//    }
//    public static Vector2 XZ(this Vector3 v)
//    {
//        return new Vector2(v.x, v.z);
//    }
//}
//public class TrigTest : MonoBehaviour
//{

//    [Range(0, 360)]
//    public float angleDeg;
//    public int circleDetail = 3;
//    [Range(0.01f, 4f)]
//    public float radius = 1f;

//    const float TAU = 6.28318530718f; // full turn in radians

//    Vector2 AngToDir(float angRad)
//    {
//        float x = Mathf.Cos(angRad);
//        float y = Mathf.Sin(angRad);
//        return new Vector2(x, y);
//    }





//    public void OnDrawGizmos()
//    {

//        float angleRad = angleDeg * Mathf.Deg2Rad;
//        Vector2 localSpaceDir = AngToDir(angleRad);
//        Vector3 worldSpaceDir = transform.TransformPoint(localSpaceDir);
//        Gizmos.DrawLine(transform.position, worldSpaceDir);

//        Vector3[] circlePts = new Vector3[circleDetail];
//        for (int i = 0; i < circleDetail; i++)
//        {
//            float t = i / (float)circleDetail;
//            float angRad = t * TAU;
//            circlePts[i] = transform.TransformPoint(AngToDir(angRad) * radius);
//            Gizmos.color = Color.HSVToRGB(t, 1, 1);
//            Gizmos.DrawSphere(circlePts[i], 0.05f);
//        }

//        Gizmos.color = Color.white;
//        for (int i = 0; i < circleDetail; i++)
//        {
//            Vector3 startPt = circlePts[i];
//            Vector3 endPt = circlePts[(i + 1) % circleDetail];
//            Gizmos.DrawLine(startPt, endPt);
//        }
//    }


//}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[ExecuteAlways]
//public class Lerper : MonoBehaviour
//{

//    public Transform a;
//    public Transform b;

//    [Range(-1, 2)]
//    public float t = 0;

//    void OnDrawGizmos()
//    {
//        Gizmos.DrawLine(a.position, b.position);
//    }

//    void Update()
//    {
//        Vector3 aPos = a.position;
//        Vector3 bPos = b.position;
//        transform.position = Vector3.LerpUnclamped(aPos, bPos, t);
//    }

//}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[ExecuteAlways]
//public class Lerper : MonoBehaviour
//{

//    public Transform a;
//    public Transform b;

//    [Range(0, 1)]
//    public float t = 0;

//    void OnDrawGizmos()
//    {
//        Gizmos.DrawLine(a.position, b.position);
//    }

//    void Update()
//    {
//        Vector3 aPos = a.position;
//        Vector3 bPos = b.position;
//        transform.position = Vector3.Lerp(aPos, bPos, t);

//        Quaternion aRot = a.rotation;
//        Quaternion bRot = b.rotation;
//        transform.rotation = Quaternion.Slerp(aRot, bRot, t);

//    }

//}

//static float Lerp(float a, float b, float t) => (1 - t) * a + b * t;
//static float InverseLerp(float a, float b, float v) => (v - a) / (b - a);
//static float Remap(float iMin, float iMax, float oMin, float oMax, float v)
//{
//    float t = InverseLerp(iMin, iMax, v);
//    return Lerp(oMin, oMax, t);
//} 

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//[ExecuteAlways]
//public class Lerper : MonoBehaviour
//{

//    [Range(0, 1)]
//    public float t = 0;

//    public Transform[] p;
//    public Camera cam;

//    void OnDrawGizmos()
//    {
//        Vector3[] pts = p.Select(x => x.position).ToArray();

//        Gizmos.DrawLine(pts[0], pts[1]);
//        Gizmos.DrawLine(pts[2], pts[3]);

//        DrawBezier(pts);

//        Vector3 curveTangent = GetTangent(t, pts);
//        cam.transform.position = GetPoint(t, pts);
//        cam.transform.rotation = Quaternion.LookRotation(curveTangent, Vector3.up);
//    }

//    static Vector3 GetTangent(float t, Vector3[] pts)
//    {
//        Vector3 a = Vector3.Lerp(pts[0], pts[1], t);
//        Vector3 b = Vector3.Lerp(pts[1], pts[2], t);
//        Vector3 c = Vector3.Lerp(pts[2], pts[3], t);
//        Vector3 d = Vector3.Lerp(a, b, t);
//        Vector3 e = Vector3.Lerp(b, c, t);
//        return (e - d).normalized;
//    }

//    static Vector3 GetPoint(float t, Vector3[] pts)
//    {
//        Vector3 a = Vector3.Lerp(pts[0], pts[1], t);
//        Vector3 b = Vector3.Lerp(pts[1], pts[2], t);
//        Vector3 c = Vector3.Lerp(pts[2], pts[3], t);
//        Vector3 d = Vector3.Lerp(a, b, t);
//        Vector3 e = Vector3.Lerp(b, c, t);
//        return Vector3.Lerp(d, e, t);
//    }

//    void DrawBezier(Vector3[] pts)
//    {
//        const int DETAIL = 32;
//        Vector3[] drawPts = new Vector3[DETAIL];
//        for (int i = 0; i < DETAIL; i++)
//        {
//            float tDraw = i / (DETAIL - 1f);
//            drawPts[i] = GetPoint(tDraw, pts);
//        }
//        Handles.DrawAAPolyLine(drawPts);
//    }


//}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CameraMovement : MonoBehaviour
//{

//    public Vector3 velocity;
//    public Vector3 acceleration;

//    public float lookSensitivity = 1f;
//    public float accForce = 1f;
//    public float dragCoefficient = 0.9f;


//    void Update()
//    {
//        // mouselook
//        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
//        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;
//        transform.Rotate(Vector3.up, mouseX, Space.World);
//        transform.Rotate(Vector3.right, -mouseY, Space.Self);

//        Vector3 inputVec = Vector3.zero;

//        void DoInputAdd(KeyCode key, Vector3 dir)
//        {
//            if (Input.GetKey(key))
//                inputVec += dir;
//        }

//        DoInputAdd(KeyCode.W, transform.forward);
//        DoInputAdd(KeyCode.S, -transform.forward);
//        DoInputAdd(KeyCode.A, -transform.right);
//        DoInputAdd(KeyCode.D, transform.right);
//        DoInputAdd(KeyCode.Space, Vector3.up);
//        DoInputAdd(KeyCode.LeftControl, Vector3.down);

//        if (inputVec.sqrMagnitude > 0.001f)
//            inputVec.Normalize();

//        // set acceleration
//        acceleration = inputVec * accForce;

//        // calculate velocity change
//        velocity += acceleration * Time.deltaTime;

//        // to dampen the velocity vector
//        velocity *= Mathf.Pow(dragCoefficient, -Time.deltaTime); // this is a hack, not physically accurate. don't tell jonathan blow

//        // move position based on velocity
//        transform.position += velocity * Time.deltaTime;
//    }


//}

//using UnityEngine;

//[RequireComponent(typeof(Camera))]
//public class FlyCamera : MonoBehaviour
//{
//    public float acceleration = 50; // how fast you accelerate
//    public float accSprintMultiplier = 4; // how much faster you go when "sprinting"
//    public float lookSensitivity = 1; // mouse look sensitivity
//    public float dampingCoefficient = 5; // how quickly you break to a halt after you stop your input
//    public bool focusOnEnable = true; // whether or not to focus and lock cursor immediately on enable

//    Vector3 velocity; // current velocity

//    bool Focused
//    {
//        get => Cursor.lockState == CursorLockMode.Locked;
//        set
//        {
//            Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
//            Cursor.visible = value == false;
//        }
//    }

//    void OnEnable()
//    {
//        if (focusOnEnable) Focused = true;
//    }

//    void OnDisable() => Focused = false;

//    void Update()
//    {
//        // Input
//        if (Focused)
//            UpdateInput();
//        else if (Input.GetMouseButtonDown(0))
//            Focused = true;

//        // Physics
//        velocity = Vector3.Lerp(velocity, Vector3.zero, dampingCoefficient * Time.deltaTime);
//        transform.position += velocity * Time.deltaTime;
//    }

//    void UpdateInput()
//    {
//        // Position
//        velocity += GetAccelerationVector() * Time.deltaTime;

//        // Rotation
//        Vector2 mouseDelta = lookSensitivity * new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
//        transform.Rotate(Vector3.up, mouseDelta.x, Space.World);
//        transform.Rotate(Vector3.right, mouseDelta.y, Space.Self);

//        // Leave cursor lock
//        if (Input.GetKeyDown(KeyCode.Escape))
//            Focused = false;
//    }

//    Vector3 GetAccelerationVector()
//    {
//        Vector3 moveInput = default;

//        void AddMovement(KeyCode key, Vector3 dir)
//        {
//            if (Input.GetKey(key))
//                moveInput += dir;
//        }

//        AddMovement(KeyCode.W, Vector3.forward);
//        AddMovement(KeyCode.S, Vector3.back);
//        AddMovement(KeyCode.D, Vector3.right);
//        AddMovement(KeyCode.A, Vector3.left);
//        AddMovement(KeyCode.Space, Vector3.up);
//        AddMovement(KeyCode.LeftControl, Vector3.down);
//        Vector3 direction = transform.TransformVector(moveInput.normalized);

//        if (Input.GetKey(KeyCode.LeftShift))
//            return direction * (acceleration * accSprintMultiplier); // "sprinting"
//        return direction * acceleration; // "walking"
//    }
//}

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class TrajectoryTest : MonoBehaviour
//{

//    public Camera playerCamera;

//    [Range(0f, 8)]
//    public float launchSpeed;

//    Vector3 mouseGroundPosition;

//    void OnDrawGizmos()
//    {
//        // calc launch angle
//        Vector3 dirPlayerToTarget = (mouseGroundPosition - transform.position).normalized;
//        float distToTarget = Vector3.Distance(transform.position, mouseGroundPosition);

//        (float angleLow, float angleHigh) launchAngles = GetLaunchAngle(distToTarget, launchSpeed);


//        Gizmos.color = Color.white;
//        const int pointCount = 32;
//        const float dt = 1f / 20f;
//        for (int i = 0; i < pointCount; i++)
//        {
//            float time = i * dt;
//            Vector2 pt2DA = GetPointInTrajectory(transform.position, launchSpeed, launchAngles.angleLow, time);
//            Vector2 pt2DB = GetPointInTrajectory(transform.position, launchSpeed, launchAngles.angleHigh, time);

//            void TryDrawTrajectoryPoint(Vector2 point2D)
//            {
//                if (point2D.y < 0)
//                    return;
//                Vector3 pt = dirPlayerToTarget * point2D.x; // lateral offset
//                pt.y = point2D.y; // vertical offset
//                Gizmos.DrawSphere(pt, 0.1f);
//            }

//            TryDrawTrajectoryPoint(pt2DA);
//            TryDrawTrajectoryPoint(pt2DB);
//        }


//        Gizmos.color = Color.red;
//        Gizmos.DrawSphere(mouseGroundPosition, 0.15f);
//    }


//    void Update()
//    {
//        mouseGroundPosition = GetCursorGroundPosition();
//    }

//    // d = s*s*sin(2*angle) / g
//    // d * g = s*s*sin(2*angle)
//    // (d * g) / (s*s) = sin(2*angle)
//    // asin( (d*g) / (s*s) ) = 2*angle
//    // asin( (d*g) / (s*s) ) / 2 = angle
//    // angle = asin( (d*g) / (s*s) ) / 2 
//    static (float, float) GetLaunchAngle(float dist, float speed)
//    {
//        float gravity = Physics.gravity.y;
//        float asinContent = Mathf.Clamp((dist * -gravity) / (speed * speed), -1, 1);
//        return
//            (Mathf.Asin(asinContent) / 2,
//                (Mathf.Asin(-asinContent) + Mathf.PI) / 2);
//    }

//    static Vector2 GetPointInTrajectory(Vector2 startPoint, float launchSpeed, float launchAngRad, float time)
//    {
//        float gravity = Physics.gravity.y;
//        float xDisp = launchSpeed * time * Mathf.Cos(launchAngRad);
//        float yDisp = launchSpeed * time * Mathf.Sin(launchAngRad) + .5f * gravity * time * time;
//        return startPoint + new Vector2(xDisp, yDisp);
//    }

//    Vector3 GetCursorGroundPosition()
//    {
//        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
//        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
//        if (groundPlane.Raycast(ray, out float hitDist))
//        {
//            Vector3 groundPos = ray.GetPoint(hitDist);
//            groundPos.y = 0f;
//            return groundPos;
//        }

//        return default;
//    }


//}

