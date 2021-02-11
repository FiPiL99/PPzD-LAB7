using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] Bandit banditController;
    [SerializeField] float speed = .5f;
    [SerializeField] float attackRange = .25f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float damage = 10;
    [SerializeField] EnemySubsystem[] subsystems;

    EnemyInput input;
    PlayerController target;
    float attackCooldownTimer;
    float distanceToTarget;
    bool initialised = false;

    void Awake() {
        Initialise();
        input = new EnemyInput();
        banditController.Setup(input);
        banditController.AttackHitEvent += CheckIfTargetInRange;
        banditController.DiedEvent += Despawn;
        banditController.DiedEvent += HandleDeath;
    }

    void Initialise()
    {
        if (initialised)
            return;
        foreach (var subsystem in subsystems)
            subsystem.Initialise(this);
        banditController.JumpedEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Jump);
        banditController.LandedEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Landing);
        banditController.AttackedEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Attack);
        banditController.DiedEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Death);
        banditController.FootstepEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Footstep);
        banditController.BlockEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Block);
        banditController.HitEvent += () => NotifySubsystemsAboutNewEvent(EnemyEventTypes.Hit);
        initialised = true;
    }

    void Despawn() {
        Destroy(gameObject, 3);
    }

    void CheckIfTargetInRange() {
        if (distanceToTarget <= attackRange)
            target.DealDamage(damage);
    }

    void Update() {
        if (banditController.IsDead) {
            input.horizontalMove = 0;
            return;
        }
        UpdateTarget();
        UpdateMovement();
        UpdateAttack();
    }

    void UpdateTarget() {
        if (!target)
            target = FindObjectOfType<PlayerController>();
        if(target)
            distanceToTarget = (target.transform.position - transform.position).magnitude;
    }

    void UpdateMovement() {
        if (target && !target.IsDead) {
            var targetPosition = target.transform.position;
            var movementDirection = (targetPosition - transform.position).normalized;
            input.horizontalMove = movementDirection.x * speed;
        } else
            input.horizontalMove = 0;
    }

    void UpdateAttack() {
        input.isAttacking = false;
        if (attackCooldownTimer > 0) {
            attackCooldownTimer -= Time.deltaTime;
            return;
        }
        if (target && !target.IsDead) {
            if (distanceToTarget <= attackRange) {
                input.isAttacking = true;
                attackCooldownTimer = attackCooldown;
            }
        }
    }

    public void DealDamage(float damage) {
        banditController.TakeDamage(damage);
    }

    void HandleDeath() {
        GetComponent<Collider2D>().enabled = false;
    }

    void NotifySubsystemsAboutNewEvent(EnemyEventTypes eventType)
    {
        foreach (var enemySubsystem in subsystems)
            enemySubsystem.HandleEvent(eventType);
    }

    public abstract class EnemySubsystem : MonoBehaviour
    {
        protected EnemyController enemy;

        public void Initialise(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public abstract void HandleEvent(EnemyEventTypes eventType);
    }

    public enum EnemyEventTypes
    {
        Jump,
        Landing,
        Death,
        Attack,
        Footstep,
        Block,
        Hit
    }
}