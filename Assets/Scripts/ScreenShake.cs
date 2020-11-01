using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    bool notShaking = true;
    
    public IEnumerator Shake(float _duration, float _magnitude)
    {
        if (notShaking)
        {
            notShaking = false;
            Vector3 _originPos = transform.localPosition;
            float _elapsed = 0;

            while (_elapsed < _duration)
            {
                float _x = Random.Range(-1f, 1f) * _magnitude;
                float _y = Random.Range(-1f, 1f) * _magnitude;

                transform.localPosition = new Vector3(_x, _y, 0) + _originPos;

                _elapsed += Time.deltaTime;

                yield return null;
            }

            yield return new WaitForEndOfFrame();
            transform.localPosition = _originPos;
            notShaking = true;
        }
    }
}
