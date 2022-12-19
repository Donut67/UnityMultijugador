using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionTimer{
    
    private Action _action;
    private float _time;

    public FunctionTimer(Action action, float time) {
        _action = action;
        _time = time;
    }

    public void Update() {
        _time -= Time.deltaTime;
        if(_time < 0) {
            _action();
        }
    }
}
