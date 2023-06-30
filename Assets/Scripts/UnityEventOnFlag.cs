using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UnityEventOnFlag : MonoBehaviour
{
    public UnityEvent eventToRun;
    public string flag;

    void Start()
    {
       Flags.AddListener(flag,FinishScene);
    }

    // Update is called once per frame
    void FinishScene(bool value)
    {
        if(!value)return;
        if(eventToRun!=null)
            eventToRun.Invoke();
    }
}
