using System;
using UnityEngine;

public interface IRecorder
{
    event Action RestoredValue;
    event Action DataHasEnded;
    
    void StartRecord();

    void StopRecord();

    void StartReplay(); }
