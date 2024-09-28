using Godot;
using System;

public partial class AnimationController : Node
{
    // Exported variables for animation settings
    [Export] private float animationDuration = 0.275f; // Animation duration in seconds
    [Export] private Tween.TransitionType transitionType = Tween.TransitionType.Sine; // Transition type
    [Export] private Tween.EaseType easeType = Tween.EaseType.InOut; // Ease type

    private Vector2 screenSize;

    public void UpdateScreenSize(Vector2 newScreenSize, Panel loginPanel, Panel registerPanel)
    {
        screenSize = newScreenSize;

        if (loginPanel.Visible)
        {
            loginPanel.Position = new Vector2(0, loginPanel.Position.Y);
            registerPanel.Position = new Vector2(screenSize.X, registerPanel.Position.Y);
        }
        else
        {
            loginPanel.Position = new Vector2(-screenSize.X, loginPanel.Position.Y);
            registerPanel.Position = new Vector2(0, registerPanel.Position.Y);
        }
    }

    public void AnimateToRegister(Panel loginPanel, Panel registerPanel)
    {
        loginPanel.Position = new Vector2(0, loginPanel.Position.Y);
        registerPanel.Position = new Vector2(screenSize.X, registerPanel.Position.Y);
        registerPanel.Visible = true;

        Tween tween = loginPanel.GetTree().CreateTween();

        // Animate LoginPanel sliding left and RegisterPanel sliding in from the right
        tween.TweenProperty(loginPanel, "position", new Vector2(-screenSize.X, loginPanel.Position.Y), animationDuration)
             .SetTrans(transitionType)
             .SetEase(easeType);

        tween.TweenProperty(registerPanel, "position", new Vector2(0, registerPanel.Position.Y), animationDuration)
             .SetTrans(transitionType)
             .SetEase(easeType);

        tween.Finished += () => loginPanel.Visible = false;
    }

    public void AnimateToLogin(Panel loginPanel, Panel registerPanel)
    {
        loginPanel.Position = new Vector2(-screenSize.X, loginPanel.Position.Y);
        registerPanel.Position = new Vector2(0, registerPanel.Position.Y);
        loginPanel.Visible = true;

        Tween tween = registerPanel.GetTree().CreateTween();

        // Animate RegisterPanel sliding right and LoginPanel sliding in from the left
        tween.TweenProperty(registerPanel, "position", new Vector2(screenSize.X, registerPanel.Position.Y), animationDuration)
             .SetTrans(transitionType)
             .SetEase(easeType);

        tween.TweenProperty(loginPanel, "position", new Vector2(0, loginPanel.Position.Y), animationDuration)
             .SetTrans(transitionType)
             .SetEase(easeType);

        tween.Finished += () => registerPanel.Visible = false;
    }
}
