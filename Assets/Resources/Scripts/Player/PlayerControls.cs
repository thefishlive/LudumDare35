using UnityEngine;
using System.Collections;
using InControl;
using System;

public class PlayerControls : PlayerActionSet
{
    public PlayerAction MoveUp { get; private set; }
    public PlayerAction MoveDown { get; private set; }
    public PlayerAction MoveLeft { get; private set; }
    public PlayerAction MoveRight { get; private set; }
    public PlayerTwoAxisAction Move { get; private set; }

    public PlayerAction Shift { get; private set; }

    public PlayerControls()
    {
        MoveUp = CreatePlayerAction("Move up");
        MoveDown = CreatePlayerAction("Move down");
        MoveLeft = CreatePlayerAction("Move left");
        MoveRight = CreatePlayerAction("Move Right");

        Move = CreateTwoAxisPlayerAction(MoveLeft, MoveRight, MoveDown, MoveUp);

        Shift = CreatePlayerAction("Shapeshift");

        SetupKeyboardDefaults();
    }

    private void SetupKeyboardDefaults()
    {
        MoveUp.AddDefaultBinding(Key.W);
        MoveDown.AddDefaultBinding(Key.S);
        MoveLeft.AddDefaultBinding(Key.A);
        MoveRight.AddDefaultBinding(Key.D);

        Shift.AddDefaultBinding(Key.Space);
    }
}
