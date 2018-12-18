using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayer : Teams.TeamMember {
    public Color personalColor;
    public enum InputType
    {
        keyboard,
        controller1,
        controller2,
        controller3,
        controller4,
        none
    }
    public InputType input;
}
