using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class EnemyAudioController : EnemySubsystem
{
    [SerializeField, EventRef] private string footsteps;
    [SerializeField, EventRef] private string attack;
    [SerializeField, EventRef] private string death;
    [SerializeField, EventRef] private string landing;
    [SerializeField, EventRef] private string jumping;
    [SerializeField, EventRef] private string block;
    [SerializeField, EventRef] private string hit;
    public override void HandleEvent(EnemyEventTypes eventType)
    {
        switch (eventType)
        {
            case EnemyEventTypes.Jump:
                OnJumping();
                break;
            case EnemyEventTypes.Landing:
                OnLanding();
                break;
            case EnemyEventTypes.Death:
                OnDeath();
                break;
            case EnemyEventTypes.Attack:
                OnAttack();
                break;
            case EnemyEventTypes.Footstep:
                OnFootstep();
                break;
            case EnemyEventTypes.Block:
                OnBlock();
                break;
            case EnemyEventTypes.Hit:
                OnHit();
                break;
        }
    }

    private void OnHit() => RuntimeManager.PlayOneShot(hit);

    private void OnBlock() => RuntimeManager.PlayOneShot(block);

    private void OnJumping() => RuntimeManager.PlayOneShot(landing);

    private void OnLanding() => RuntimeManager.PlayOneShot(jumping);

    private void OnDeath() => RuntimeManager.PlayOneShot(death);

    private void OnAttack() => RuntimeManager.PlayOneShot(attack);

    private void OnFootstep() => RuntimeManager.PlayOneShot(footsteps);
}

