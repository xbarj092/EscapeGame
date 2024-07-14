using System.Collections;
using UnityEngine;

public class DashHandler
{
    public bool IsDashing { get; private set; }

    private readonly CharacterController2D _controller;
    private float _cooldownTimer = 0f;
    private bool _dashPossible = true;

    private const float DASHING_POWER = 40f;
    private const float DASHING_TIME = 0.2f;
    private const float DASHING_COOLDOWN = 1f;

    public DashHandler(CharacterController2D controller)
    {
        _controller = controller;
    }

    public void DashPressed()
    {
        if (IsDashPossible())
        {
            _controller.StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        _dashPossible = false;
        IsDashing = true;

        float originalGravity = _controller.Rigidbody2D.gravityScale;
        _controller.Rigidbody2D.gravityScale = 0f;
        _controller.Rigidbody2D.velocity = new Vector2(_controller.transform.localScale.x * DASHING_POWER, 0f);

        yield return new WaitForSeconds(DASHING_TIME);

        _controller.Rigidbody2D.gravityScale = originalGravity;
        IsDashing = false;

        while (_cooldownTimer < DASHING_COOLDOWN)
        {
            _cooldownTimer += Time.deltaTime;
            yield return null;
        }

        _cooldownTimer = 0f;
        _dashPossible = true;
    }

    public bool IsDashPossible() => _dashPossible;

    public float TimeToNextDash() => _cooldownTimer / DASHING_COOLDOWN;
}
