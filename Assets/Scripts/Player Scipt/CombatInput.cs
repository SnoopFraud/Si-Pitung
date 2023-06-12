using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ComboState
{
    None,
    Attack1,
    Attack2,
    Attack3
}

public class CombatInput : MonoBehaviour
{
    #region Variables
    public static CombatInput instance;

    public LayerMask enemyLayers;
    public Transform attackPoint;

    public float attackRange = 0.5f;

    // Push variables
    public Transform player;
    private Rigidbody rb;

    // Combo stuff
    private ComboState currentComboState;
    private float currentComboTimer;
    private float defaultComboTime = 0.7f;
    private bool activateReseter;
    private float attackCooldown = 1f;

    // Dashing
    public float dashCooldown;
    private int enemyLayerNumber = 7;

    // Trail renderer
    [SerializeField] private TrailRenderer trailRenderer;
    #endregion

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        InitializeCombo();
        InitializeDashing();
    }

    private void Update()
    {
        ResetCombo();
    }

    #region Combat Actions
    public void ComboAttack()
    {
        if (currentComboState == ComboState.Attack3)
            return;

        currentComboState++;
        PlayerVar.isAttacking = true;
        activateReseter = true;
        currentComboTimer = defaultComboTime;
        PlayerInput.instance._moveSpeed = 0;

        int damage = GetComboDamage();

        switch (currentComboState)
        {
            case ComboState.Attack1:
                HitEnemies(damage);
                StartCoroutine(MovePlayerForward(0.1f, 3f));
                PlayerAnim.instance.AnimationPlay(PlayerAnim.instance.AnimAttackName[0]);
                Debug.Log("Attack 1");
                break;
            case ComboState.Attack2:
                HitEnemies(damage);
                StartCoroutine(MovePlayerForward(0.1f, 20f));
                PlayerAnim.instance.AnimationPlay(PlayerAnim.instance.AnimAttackName[1]);
                Debug.Log("Attack 2");
                break;
            case ComboState.Attack3:
                HitEnemies(damage);
                StartCoroutine(MovePlayerForward(0.2f, 20f));
                PlayerAnim.instance.AnimationPlay(PlayerAnim.instance.AnimAttackName[2]);
                Debug.Log("Attack 3");
                StartCoroutine(AttackCooldown());
                break;
        }
    }

    private int GetComboDamage()
    {
        switch (currentComboState)
        {
            case ComboState.Attack1:
                return 5;
            case ComboState.Attack2:
                return 10;
            case ComboState.Attack3:
                return 20;
            default:
                return 0;
        }
    }

    private void ResetCombo()
    {
        if (activateReseter)
        {
            currentComboTimer -= Time.deltaTime;

            if (currentComboTimer <= 0)
            {
                InitializeCombo();
                PlayerVar.isAttacking = false;
                activateReseter = false;
                PlayerInput.instance._moveSpeed = PlayerInput.instance.BaseSpeed;
            }
        }
    }

    private IEnumerator AttackCooldown()
    {
        PlayerVar.onAtkCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        PlayerVar.onAtkCooldown = false;
    }

    private void HitEnemies(int damage)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }

    public IEnumerator MovePlayerForward(float duration, float speed)
    {
        Vector3 direction = player.right * (PlayerVar.isFacingRight ? 1f : -1f);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            rb.AddForce(Vector3.Lerp(Vector3.zero, direction * speed, t), ForceMode.VelocityChange);
            yield return null;
        }
    }

    public IEnumerator PerformDash()
    {
        PlayerVar.CanDash = false;
        PlayerVar.isDashing = true;
        PlayerInput.instance._moveSpeed = 0;

        Physics.IgnoreLayerCollision(gameObject.layer, enemyLayerNumber, true);
        trailRenderer.emitting = true;

        PlayerAnim.instance.AnimationPlay(PlayerAnim.instance.DashAnim);

        yield return new WaitForSeconds(dashCooldown);

        Physics.IgnoreLayerCollision(gameObject.layer, enemyLayerNumber, false);
        trailRenderer.emitting = false;

        PlayerInput.instance._moveSpeed = PlayerInput.instance.BaseSpeed;
        PlayerVar.isDashing = false;
        PlayerVar.CanDash = true;
    }
    #endregion

    #region Initialization
    private void InitializeCombo()
    {
        currentComboState = ComboState.None;
        currentComboTimer = defaultComboTime;
        PlayerVar.isAttacking = false;
        PlayerVar.onAtkCooldown = false;
    }

    private void InitializeDashing()
    {
        PlayerVar.CanDash = true;
        PlayerVar.isDashing = false;
        trailRenderer.emitting = false;
    }
    #endregion

    // Draw the attack range for visualization purposes
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
