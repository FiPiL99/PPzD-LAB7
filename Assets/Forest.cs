using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{
    [SerializeField, EventRef] private string forestSnapshot;
    [SerializeField, EventRef] private string emptySpaceSnapshot;
    private EventInstance forestSnapshotInstance;
    private EventInstance emptySpaceSnapshotInstance;
    private void Awake()
    {
        emptySpaceSnapshotInstance = RuntimeManager.CreateInstance(emptySpaceSnapshot);
        forestSnapshotInstance = RuntimeManager.CreateInstance(forestSnapshot);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            emptySpaceSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            forestSnapshotInstance.start();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            forestSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            emptySpaceSnapshotInstance.start();
        }
    }
}
