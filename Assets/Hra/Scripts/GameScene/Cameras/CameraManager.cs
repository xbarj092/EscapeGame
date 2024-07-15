using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _tutorial;
    [SerializeField] private CinemachineVirtualCamera _levelOnePreview;
    [SerializeField] private CinemachineVirtualCamera _levelOne;
    [SerializeField] private CinemachineVirtualCamera _levelTwoPreview;
    [SerializeField] private CinemachineVirtualCamera _levelTwo;
    [SerializeField] private CinemachineVirtualCamera _levelThreePreview;
    [SerializeField] private CinemachineVirtualCamera _levelThree;
    [SerializeField] private CinemachineVirtualCamera _levelFour;

    private void OnEnable()
    {
        CameraSwitcher.SwitchCamera(_tutorial);
        CameraSwitcher.Register(_tutorial);
        CameraSwitcher.Register(_levelOnePreview);
        CameraSwitcher.Register(_levelOne);
        CameraSwitcher.Register(_levelTwoPreview);
        CameraSwitcher.Register(_levelTwo);
        CameraSwitcher.Register(_levelThreePreview);
        CameraSwitcher.Register(_levelThree);
        CameraSwitcher.Register(_levelFour);
    }

    private void OnDisable()
    {
        CameraSwitcher.UnRegister(_tutorial);
        CameraSwitcher.UnRegister(_levelOnePreview);
        CameraSwitcher.UnRegister(_levelOne);
        CameraSwitcher.UnRegister(_levelTwoPreview);
        CameraSwitcher.UnRegister(_levelTwo);
        CameraSwitcher.UnRegister(_levelThreePreview);
        CameraSwitcher.UnRegister(_levelThree);
        CameraSwitcher.UnRegister(_levelFour);
    }

    public void EnterLevel(int level)
    {
        switch (level)
        {
            case 1:
                StartCoroutine(EnterLevelOne());
                break;
            case 2:
                StartCoroutine(EnterLevelTwo());
                break;
            case 3:
                StartCoroutine(EnterLevelThree());
                break;
            case 4:
                StartCoroutine(EnterLevelFour());
                break;
        }
    }

    private IEnumerator EnterLevelOne()
    {
        CameraSwitcher.SwitchCamera(_levelOnePreview);
        yield return new WaitForSeconds(5);
        CameraSwitcher.SwitchCamera(_levelOne);
        CinemachineFramingTransposer body = (CinemachineFramingTransposer)CameraSwitcher.ActiveCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        body.m_DeadZoneHeight = 0;
        yield return new WaitForSeconds(1);
        body.m_DeadZoneHeight = 2;
    }

    private IEnumerator EnterLevelTwo()
    {
        CameraSwitcher.SwitchCamera(_levelTwoPreview);
        yield return new WaitForSeconds(5);
        CameraSwitcher.SwitchCamera(_levelTwo);
        CinemachineFramingTransposer body = (CinemachineFramingTransposer)CameraSwitcher.ActiveCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        body.m_DeadZoneHeight = 0;
        yield return new WaitForSeconds(1);
        body.m_DeadZoneHeight = 2;
    }

    private IEnumerator EnterLevelThree()
    {
        CameraSwitcher.SwitchCamera(_levelThreePreview);
        yield return new WaitForSeconds(5);
        CameraSwitcher.SwitchCamera(_levelThree);
        CinemachineFramingTransposer body = (CinemachineFramingTransposer)CameraSwitcher.ActiveCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        body.m_DeadZoneHeight = 0;
        yield return new WaitForSeconds(1);
        body.m_DeadZoneHeight = 2;
    }

    private IEnumerator EnterLevelFour()
    {
        yield return new WaitForSeconds(0.5f);
        CameraSwitcher.SwitchCamera(_levelFour);
    }
}
