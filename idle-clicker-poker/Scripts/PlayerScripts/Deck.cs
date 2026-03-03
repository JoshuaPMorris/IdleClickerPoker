using Godot;
using System;
using System.Collections.Generic;

public partial class Deck : Area2D
{
    public override void _InputEvent(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);

        if (Input.IsActionJustPressed("Click"))
        {
            CardManager.singleton.DrawFromDeck();
        }
    }
}
