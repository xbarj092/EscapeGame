using UnityEngine;

public class BreakableCube : BaseCube
{
    protected override void HandleAction()
    {
        Destroy(gameObject);
    }
}
