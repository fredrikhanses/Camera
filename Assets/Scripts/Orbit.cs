using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] private Controller playerController;
    [SerializeField, Range(0.5f, 1f)] private float offset;
    float alpha = 5f;
    void FixedUpdate()
    {
        transform.position = playerController.transform.position + new Vector3(offset * Mathf.Sin(Mathf.Deg2Rad * alpha), offset * Mathf.Cos(Mathf.Deg2Rad * alpha), offset * Mathf.Sin(Mathf.Deg2Rad * alpha) * Mathf.Cos(Mathf.Deg2Rad * alpha));
        alpha += Random.Range(1f, 3f);
        if(alpha >= 360)
        { 
            alpha = 0; 
        }
    
    //transform.RotateAround(playerController.transform.position, Vector3.forward, 20 * Time.deltaTime);
    }
}
