# if UNITY_EDITOR
using UnityEngine;

public class TutorialTester : MonoBehaviour
{
    public TutorialID TutorialToTest;

    public void TestTutorialSpawn()
    {
        TutorialManager.Instance.InstantiateTutorial(TutorialToTest);
    }
}
#endif