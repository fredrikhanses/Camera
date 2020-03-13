using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField]
    Transform focus = default;

    [SerializeField, Range(1.0f, 20.0f)]
    float distance = 5.0f;

    [SerializeField, Min(0.0f)]
    float focusRadius = 1.0f;

    [SerializeField, Range(0.0f, 1.0f)]
    float focusCentering = 0.75f;

    [SerializeField, Range(1.0f, 360.0f)]
    float rotationSpeed = 90.0f;

    [SerializeField, Range(-89.0f, 89.0f)]
    float minVerticalAngle = -30.0f, maxVerticalAngle = 60.0f;

    [SerializeField, Min(0.0f)]
    float alignDelay = 5.0f;

    Vector3 focusPoint, previousFocusPoint;

    Vector2 orbitAngles = new Vector2(45.0f, 0.0f);

    float lastManualRotationTime;

    void OnValidate()
    {
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    void Awake()
    {
        focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);
    }

    void LateUpdate()
    {
        UpdateFocusPoint();
        ManualRotation();
        Quaternion lookRotation;
        if (ManualRotation() || AutomaticRotation())
        {
            ConstrainAngles();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
        else
        {
            lookRotation = transform.localRotation;
        }
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    void UpdateFocusPoint()
    {        
        Vector3 targetPoint = focus.position;
        if(focusRadius > 0.0f)
        {
            float distance = Vector3.Distance(targetPoint, focusPoint);
            if(distance > focusRadius)
            {
                focusPoint = Vector3.Lerp(targetPoint, focusPoint, focusRadius / distance);
            }
        }
        if (distance > 0.01f && focusCentering > 0.0f)
        {
            focusPoint = Vector3.Lerp(targetPoint, focusPoint, Mathf.Pow(1.0f - focusCentering, Time.unscaledDeltaTime));
        }
    }

    bool ManualRotation()
    {
        Vector2 input = new Vector2(Input.GetAxis("Vertical Camera"), Input.GetAxis("Horizontal Camera"));
        const float e = 0.001f;
        if(input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            lastManualRotationTime = Time.unscaledTime;
            return true;
        }
        return false;
    }

    void ConstrainAngles()
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    bool AutomaticRotation()
    {
        if(Time.unscaledTime - lastManualRotationTime < alignDelay)
        {
            return false;
        }
        return true;
    }
}
