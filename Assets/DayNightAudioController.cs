using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightAudioController : MonoBehaviour
{
    [SerializeField, EventRef] private string daySnapshot;
    [SerializeField, EventRef] private string sunsetSnapshot;
    [SerializeField, EventRef] private string nightSnapshot;
    private EventInstance daySnapshotInstance;
    private EventInstance sunsetSnapshotInstance;
    private EventInstance nightSnapshotInstance;
    private void Awake()
    {
        daySnapshotInstance = RuntimeManager.CreateInstance(daySnapshot);
        daySnapshotInstance.start();
        sunsetSnapshotInstance = RuntimeManager.CreateInstance(sunsetSnapshot);
        nightSnapshotInstance = RuntimeManager.CreateInstance(nightSnapshot);
        DayNightController.TimeOfDayChangedEvent += ChangeAudio;
    }
    private void ChangeAudio(TimeOfDay timeOfDay)
    {
        ResetInstances();
        switch (timeOfDay)
        {
            case TimeOfDay.Day:
                daySnapshotInstance.start();
                break;
            case TimeOfDay.Sunset:
                sunsetSnapshotInstance.start();
                break;
            case TimeOfDay.Night:
                nightSnapshotInstance.start();
                break;
        }
    }

    private void ResetInstances()
    {
        daySnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        sunsetSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        nightSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
