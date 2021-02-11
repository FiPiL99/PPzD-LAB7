using FMODUnity;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudioController : PlayerSubsystem {
    [SerializeField, EventRef] private string footsteps;
    [SerializeField, EventRef] private string attack;
    [SerializeField, EventRef] private string death;
    [SerializeField, EventRef] private string landing;
    [SerializeField, EventRef] private string jumping;
    [SerializeField, EventRef] private string block;
    [SerializeField, EventRef] private string hit;
    [SerializeField, EventRef] private string hearthBeat;
    bool isHearthPlaying = false;
    FMOD.Studio.EventInstance playerState;


    private void Start()
    {
        playerState = RuntimeManager.CreateInstance(hearthBeat);
    }
    public override void HandleEvent(PlayerEventType eventType) {
        switch (eventType) {
            case PlayerEventType.Jump:
                OnJumping();
                break;
            case PlayerEventType.Landing:
                OnLanding();
                break;
            case PlayerEventType.Death:
                OnDeath();
                break;
            case PlayerEventType.Attack:
                OnAttack();
                break;
            case PlayerEventType.Footstep:
                OnFootstep();
                break;
            case PlayerEventType.Block:
                OnBlock();
                break;
            case PlayerEventType.Hit:
                OnHit();
                break;
        }
    }

    private void OnHit()
    {
        RuntimeManager.PlayOneShot(hit);
        ManageHearthbeat();
    }

    private void OnBlock() => RuntimeManager.PlayOneShot(block);

    private void OnJumping() => RuntimeManager.PlayOneShot(landing);

    private void OnLanding() => RuntimeManager.PlayOneShot(jumping);

    private void OnDeath()
    {
        RuntimeManager.PlayOneShot(death);
        playerState.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnAttack() => RuntimeManager.PlayOneShot(attack);

    private void OnFootstep() => RuntimeManager.PlayOneShot(footsteps);

    private void ManageHearthbeat()
    {
        var max = 30f;
        var min = 0;
        if (player.PlayerHealthPercentage < 30)
        {
            if (!isHearthPlaying)
            {
                playerState.start();
                isHearthPlaying = true;
            }
            float t = 1 - MapTo01(min, max, player.PlayerHealthPercentage);
            float volume = Mathf.Lerp(min, max, t);
            RuntimeManager.StudioSystem.setParameterByName("LowpassValue", t);
        }

    }

    float MapTo01(float min, float max, float x)
    {
        return ((x + Mathf.Abs(min)) / (max - min));
    }
}
