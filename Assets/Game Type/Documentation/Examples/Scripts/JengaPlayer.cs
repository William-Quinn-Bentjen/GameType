using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JengaPlayer : Teams.TeamMember {
    public Color personalColor;
    public enum InputType
    {
        keyboard,
        controller1,
        controller2,
        controller3,
        controller4
    }
    public InputType input;
}
