using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAction {

    List<string> actionNames { get; }

    void TriggerAction();
}
