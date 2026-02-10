using Godot;
using System;
using System.Runtime.InteropServices.JavaScript;

public partial class CardGhost : Area2D
{
    public CardObj subject;

    public bool hoverSwitch;

    public override void _Ready()
    {
        base._Ready();

        MouseEntered += OnMouseEntered;
        MouseExited += OnMouseExited;
    }

    // Hover effect
    private void OnMouseEntered()
    {
        hoverSwitch = false;
        Vector2 pos = subject.Position;
        pos.Y = Position.Y - CardManager.singleton.raiseHeight;
        subject.Position = pos;
    }
    private void OnMouseExited()
    {
        // If the mouse is still on the card then dont lower the card
        if (subject.isHovered)
        {
            hoverSwitch = true;
            return;
        }
        
        subject.Position = Vector2.Zero;
    }
}
