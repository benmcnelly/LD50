using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

    public static FollowCamera _camera_controll;

    public float FollowSpeed = 2f;
    public float regular_zoom = -15f;
    public float far_zoom = -35f;
    public float current_zoom = -15f;
    public Transform Target;

    void Start ()
    {
        if (_camera_controll == null)
        {
            DontDestroyOnLoad(gameObject);
            _camera_controll = this;
        } else if (_camera_controll != this)
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        Vector3 cam_placement = Target.position;
        cam_placement.z = current_zoom;
        transform.position = Vector3.Slerp(transform.position, cam_placement, FollowSpeed * Time.deltaTime);
    }

    
    public void ZoomOut()
    {
        current_zoom = far_zoom;
    }

    public void ZoomRegular()
    {
        current_zoom = regular_zoom;
    }    


}
