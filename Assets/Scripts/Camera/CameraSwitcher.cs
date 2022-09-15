using System.Collections.Generic;
using UnityEngine;

public sealed class CameraSwitcher
{
    private List<Camera> _cameras;

    private int _index;
    private int _previousIndex;
    private float _previousPriority;

    public void RegisterCamera(Camera camera)
    {
        _cameras.Add(camera);
    }

    public void UnRegisterCamera(Camera camera)
    {
        _cameras.Remove(camera);
    }
    
    public void SwitchCamera()
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            if (_index != i)
            {
                continue;
            }

            _cameras[_previousIndex].depth = _previousPriority;

            _previousPriority = _cameras[i].depth;
            _cameras[i].depth = 100f;
        }

        _previousIndex = _index;

        _index++;
        _index %= _cameras.Count;
    }
}
