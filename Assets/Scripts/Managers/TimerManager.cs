using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class TimerManager : Singleton<TimerManager>
    {
        public void StartTimer(float delay, Action action)
        {
            StartCoroutine(TimerCoroutine(delay, action));
        }
        
        private static IEnumerator TimerCoroutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}