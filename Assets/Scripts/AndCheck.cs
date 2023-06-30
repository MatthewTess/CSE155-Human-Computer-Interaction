using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndCheck : BooleanBase
{
    public List<BooleanBase> bools;
    private void Start()
    {
        foreach (BooleanBase b in bools)
        {
            b.OnValueChanged += HandleValueChanged;
        }
    }
    private void HandleValueChanged(bool value)
    {
        bool result = true;
        foreach (BooleanBase b in bools)
        {
            result = (b.booleanValue == false) ? false : result;
        }
        booleanValue = result;
    }
}
