using UnityEngine;

public enum GameScreenType
{
    None = 0,
    Death = 1
}

public class GameScreen : MonoBehaviour
{
    [SerializeField] private bool _destroyOnClose;

    [field: SerializeField] public GameScreenType GameScreenType { get; private set; }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        if (_destroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
