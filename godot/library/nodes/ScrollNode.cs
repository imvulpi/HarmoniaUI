using Godot;
using System;

namespace HarmoniaUI.Library.Nodes
{
    public partial class ScrollNode : Control
    {
        public Vector2 Overflows { get; set; } = new();
        public Vector2 NegativeOverflows { get; set; } = new();
        public Vector2 ScrollStep { get; set; } = new(20, 20);
        public Vector2 CurrentScrollStep = new();

        public Vector2 CurrentScroll = new();
        public Vector2 LastOffset = Vector2.Zero;
        public Vector2 CurrentOffset = new();

        public double ScrollSpeed = 7f;
        public Vector2 CurrentScrollPercent = new();
        public float ScrollCheckingSizeX = 0.2f;

        public override void _PhysicsProcess(double delta)
        {
            CurrentOffset = CurrentOffset.Lerp(CurrentScroll, (float)(delta * ScrollSpeed));
            Vector2 offsetDelta = CurrentOffset - LastOffset;

            var children = GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is Control control)
                {
                    //offsetDelta.Min(Overflows);
                    //offsetDelta.Max(Vector2.Zero);
                    control.Position += offsetDelta;
                }
            }
            LastOffset = CurrentOffset;
        }

        public override void _GuiInput(InputEvent @event)
        {
            if (Overflows == Vector2.Zero) return;
            if(@event is InputEventMouseButton mouseButton)
            {
                if (mouseButton.IsPressed())
                {
                    if(mouseButton.ButtonIndex == MouseButton.WheelUp)
                    {
                        bool YPriority = CalculateYPriority();
                        if (YPriority)
                        {
                            float stepY = ScrollStep.Y;
                            if(stepY + CurrentScroll.Y > 0)
                            {
                                stepY = -CurrentScroll.Y;
                            }

                            CurrentScroll.Y += stepY;
                        }
                        else
                        {
                            CurrentScroll.X += ScrollStep.X;
                        }
                        
                        if (CurrentScroll.X != 0 && Overflows.X != 0) CurrentScrollPercent.X = CurrentScroll.X / Overflows.X;
                        else CurrentScrollPercent.X = 0;

                        if (CurrentScroll.Y != 0 && Overflows.Y != 0) CurrentScrollPercent.Y = CurrentScroll.Y / Overflows.Y;
                        else CurrentScrollPercent.Y = 0;
                    }
                    else if(mouseButton.ButtonIndex == MouseButton.WheelDown)
                    {
                        bool YPriority = CalculateYPriority();
                        if (YPriority)
                        {
                            float stepY = -ScrollStep.Y;
                            if (MathF.Abs(stepY + CurrentScroll.Y) > Overflows.Y)
                            {
                                stepY = (MathF.Abs(CurrentScroll.Y) - Overflows.Y);
                            }
                            CurrentScroll.Y += stepY;
                        }
                        else
                        {
                            CurrentScroll.X -= ScrollStep.X;
                        }

                        if (CurrentScroll.X != 0 && Overflows.X != 0) CurrentScrollPercent.X = CurrentScroll.X / Overflows.X;
                        else CurrentScrollPercent.X = 0;

                        if (CurrentScroll.Y != 0 && Overflows.Y != 0) CurrentScrollPercent.Y = CurrentScroll.Y / Overflows.Y;
                        else CurrentScrollPercent.Y = 0;
                    }
                }
            }

            base._GuiInput(@event);
        }

        public void UpdateLayout()
        {
            RecoverScrollPosition();
        }

        public void RecoverScrollPosition()
        {
            CurrentScroll = Overflows * CurrentScrollPercent;
            CurrentOffset = CurrentScroll;
            var children = GetChildren();
            for (int i = 0; i < children.Count; i++)
            {
                if (children[i] is Control control)
                {
                    control.Position -= NegativeOverflows;
                    control.Position += CurrentScroll;
                }
            }
            LastOffset = CurrentScroll;
        }

        public bool CalculateYPriority()
        {
            if (Overflows.Y <= 0) return false;
            if(Overflows.X > 0)
            {
                float scrollCheck = Size.Y * ScrollCheckingSizeX;
                Vector2 mousePos = GetLocalMousePosition();
                Rect2 rect = GetRect();

                float bottomY = rect.Position.Y + rect.Size.Y * (1 - ScrollCheckingSizeX);
                float topY = rect.Position.Y + rect.Size.Y;

                return mousePos.Y >= bottomY && mousePos.Y <= topY;
            }
            return true;
        }
    }
}
