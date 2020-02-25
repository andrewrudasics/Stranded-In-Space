using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTimer : MonoBehaviour
{
    static IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void WaitForSeconds(float time)
    {
        StartCoroutine(ExecuteAfterTime(time));
    }
}
