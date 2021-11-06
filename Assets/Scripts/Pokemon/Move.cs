using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    //can't use the short get method like below in the MoveBase class for example because it will now allow to be shown in ui/menu/insepctor in unity
    public MoveBase Base { get; set; }
    public int PP { get; set; }
    
    public Move(MoveBase Base)
    {
        this.Base = Base;
        this.PP = Base.PP;
    }
}
