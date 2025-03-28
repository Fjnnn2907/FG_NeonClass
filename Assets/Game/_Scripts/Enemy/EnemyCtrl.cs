using UnityEngine;
using Photon.Pun;
using System.Collections;

public class EnemyCtrl : FinalStateMachine
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float roamTimer = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private EnemyStats enemyStats;

    private Vector2 moveDirection;
    private Transform targetPlayer;
    private bool isAttacking = false;
    private bool isDie = false;

    private SpawnEnemy spawner;

    public void SetSpawner(SpawnEnemy spawnEnemy)
    {
        spawner = spawnEnemy;
    }
    protected override void Init()
    {
        if (!photonView.IsMine) return;
        SetDefautlState();
        StartCoroutine(Roam());
    }
    protected override void FSMUpdate()
    {
        base.FSMUpdate();

        if (enemyStats.CurrentHealth <= 0 && !isDie)
            ChangeState(State.Die);
        
    }
    protected override void FSMFixedUpdate()
    {
        if (!photonView.IsMine) return;

        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Run:
                RunState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Die:
                DieState();
                break;
        }
    }

    private void FindToPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);
        if (player != null)
        {
            targetPlayer = player.transform;
            ChangeState(State.Attack);
        }
        else
        {
            targetPlayer = null;
            if (currentState == State.Attack) ChangeState(State.Idle);
        }
    }

    private IEnumerator Roam()
    {
        while (true)
        {
            if (!isDie)
            {
                if (currentState != State.Attack)
                {
                    moveDirection = Random.insideUnitCircle.normalized;
                    ChangeState(State.Run);
                }
                yield return new WaitForSeconds(roamTimer);
                ChangeState(State.Idle);
                yield return new WaitForSeconds(3f);
            }          
        }
    }

    private void IdleState()
    {
        PlayAnimation(Tag.IDLE);
        SetZeroVelocity();
    }

    private void RunState()
    {
        PlayAnimation(Tag.RUN);
        SetVelocity(moveDirection.x, moveDirection.y, speed);
    }

    private void AttackState()
    {
        if (targetPlayer == null) return;

        isAttacking = true;
        PlayAnimation(Tag.ATTACK);
        SetVelocity((targetPlayer.position.x - transform.position.x),
                    (targetPlayer.position.y - transform.position.y), speed * 1.5f);

        Invoke(nameof(ResetAttackState), 1f);
    }

    private void ResetAttackState()
    {
        isAttacking = false;
        ChangeState(State.Idle);
    }
    private void DieState()
    {
        if (!(photonView.IsMine)) return;

        PlayAnimation(Tag.DIE);
        isDie = true;
        PhotonNetwork.Destroy(gameObject);
        SetZeroVelocity();
    }

}

