using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ReplayConfig", menuName = "Record/ReplayConfig")]
public class ReplayConfig : ScriptableObject
{
    [SerializeField, Range(0, 10)] private float _maximumSecondsToRecord;
    [SerializeField, Range(1, 10)] private float _numberOfSkippedFrames;
    public float MaximumSecondsToRecord => _maximumSecondsToRecord;
    public float NumberOfSkippedFrames => _numberOfSkippedFrames;
}