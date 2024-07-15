using Cinemachine;
using System.Collections.Generic;

public static class CameraSwitcher
{
    private static List<CinemachineVirtualCamera> _cameras = new();

    public static CinemachineVirtualCamera ActiveCamera = null;

    public static bool IsActiveCamera(CinemachineVirtualCamera cam) => cam == ActiveCamera;

    public static void SwitchCamera(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        ActiveCamera = cam;

        foreach (CinemachineVirtualCamera camera in _cameras)
        {
            if (camera != ActiveCamera)
            {
                camera.Priority = 0;
            }
        }
    }

    public static void Register(CinemachineVirtualCamera cam)
    {
        _cameras.Add(cam);
    }

    public static void UnRegister(CinemachineVirtualCamera cam)
    {
        _cameras.Remove(cam);
    }
}
