using UnityEngine;

public class TransformRecorder : Recorder<TransformValues>
{
    protected override TransformValues GetRecordValue()
        => new TransformValues(transform.position, transform.rotation);

    protected override bool IsDataValuesChanged(TransformValues lastValue) 
        => lastValue.Position != transform.position || lastValue.Rotation != transform.rotation;
}
