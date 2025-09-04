using Unity.VisualScripting;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    private Camera _mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            return _mainCamera;
        }
    }

    public void SetCameraPosition(Vector3 pos)
    {
        Vector3 camPos = MainCamera.transform.position;

        camPos.x = pos.x;
        camPos.z = pos.z;

        MainCamera.transform.position = camPos;
    }
}