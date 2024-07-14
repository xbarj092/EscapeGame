using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ChosenActionType
{
    Default,
    Custom
}

public class TutorialPlayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [field: SerializeField] public TutorialID TutorialID { get; private set; }
    [field: SerializeField, Space(5)] public bool DarkenBackground { get; private set; }
    [field: SerializeField] public Image BgImage { get; private set; }

    public TutorialAction Action;
    [field: SerializeField] public StringStorage MainTexts { get; private set; }

    [SerializeField] private int _startingTextIndex = -1;
    private int _currentMainTextIndex = -1;

    public event Action<TutorialID> OnTutorialEnd;

    public void OnEnable()
    {
        Action.Init(this);
        Action.StartAction();
        Action.OnActionFinished += OnCurrentActionFinished;
        SetBlackBackground(DarkenBackground);
    }

    public void SetBlackBackground(bool visible, bool ableToClickThrough = false)
    {
        BgImage.color = new Color(BgImage.color.r, BgImage.color.b, BgImage.color.g, visible ? 1 : 0);
        BgImage.raycastTarget = !ableToClickThrough;
    }

    public void MoveToNextNarratorText()
    {
        _currentMainTextIndex++;
        UpdateNarratorFrameText(MainTexts.Strings[_currentMainTextIndex]);
    }

    private void OnCurrentActionFinished()
    {
        Action.Exit();
        Action.OnActionFinished -= OnCurrentActionFinished;
        OnTutorialEnd?.Invoke(TutorialID);
        Destroy(gameObject);
    }

    public void IncreaseMainTextIndex()
    {
        _currentMainTextIndex++;
    }

    private void UpdateNarratorFrameText(string text)
    {
        _text.text = text;
    }
}
