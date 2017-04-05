using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Will scale the camera to whatever resolution is provided. Sets the camera's project matrix to the orthographic size
 multiplied by the aspect.*/
public class CameraScaling : MonoBehaviour
{
    private Camera cam;

    public float orthSize = 6;
    public float aspect = 1.33333f;

    void Start()
    {
        cam = GetComponent<Camera>();
        Camera.main.projectionMatrix = Matrix4x4.Ortho(-orthSize * aspect, orthSize * aspect, -orthSize, orthSize, cam.nearClipPlane, cam.farClipPlane);
    }
}
