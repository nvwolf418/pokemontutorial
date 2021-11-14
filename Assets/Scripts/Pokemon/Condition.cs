using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }

    public Action<Pokemon> OnStart { get; set; }
    //if you want to do action but retun a value, use Func instead of Action. Need to returna  bool.
    //The return type comes after all paramters like on the line below, we return bool so it is the alst one.
    public Func<Pokemon, bool> OnBeforeMove { get; set; }
    public Action<Pokemon> OnAfterTurn { get; set; }
}
