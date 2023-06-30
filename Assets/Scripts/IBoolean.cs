using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoolean
{
    event Action<bool> OnValueChanged;
    bool Value { get; set; }
}

public abstract class BooleanBase : MonoBehaviour
{
    public event Action<bool> OnValueChanged;

    [SerializeField] private bool _value;
    public bool booleanValue
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }
}