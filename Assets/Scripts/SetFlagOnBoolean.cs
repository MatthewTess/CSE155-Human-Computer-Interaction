using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFlagOnBoolean : MonoBehaviour
{
    public BooleanBase boolean;
    public bool stateToCheckFor = true;
    public string flagToSet;
    public bool flagStateToSet, reversible;
    void Start()
    {
        boolean.OnValueChanged += SetFlag;
    }
    void SetFlag(bool value)
    {
        if (value == stateToCheckFor)
            Flags.Update(flagToSet, flagStateToSet);
        else if (reversible)
            Flags.Update(flagToSet, value);
    }
}
