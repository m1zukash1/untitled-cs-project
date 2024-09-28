using Godot;
using System;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Linq;

public partial class MenuController : Control
{
    private Panel loginPanel;
    private Panel registerPanel;
    private Vector2 screenSize;
    private AnimationController animationController;

    private Button loginPanelRegisterButton;
    private Button registerPanelLoginButton;
    private Button registerButton;

    private LineEdit registerUsernameInput;
    private LineEdit registerPasswordInput;
    private LineEdit registerEmailInput;

    AppContext context = new AppContext();

    public override void _Ready()
    {
        // Get references to the panels and the animation controller
        loginPanel = GetNode<Panel>("LoginPanel");
        registerPanel = GetNode<Panel>("RegisterPanel");
        animationController = GetNode<AnimationController>("AnimationController");

        // Get references to the buttons
        loginPanelRegisterButton = GetNode<Button>("LoginPanel/RegisterButton");
        registerPanelLoginButton = GetNode<Button>("RegisterPanel/LoginButton");
        registerButton = GetNode<Button>("RegisterPanel/RegisterButton");

        // Get references to the LineEdit inputs in the register panel
        registerUsernameInput = GetNode<LineEdit>("RegisterPanel/UsernameInput");
        registerPasswordInput = GetNode<LineEdit>("RegisterPanel/PasswordInput");
        registerEmailInput = GetNode<LineEdit>("RegisterPanel/EmailInput");

        // Connect the button signals
        loginPanelRegisterButton.Pressed += _OnLoginPanelRegisterButtonPressed;
        registerPanelLoginButton.Pressed += _OnRegisterPanelLoginButtonPressed;
        registerButton.Pressed += _OnRegisterButtonPressed;

        GetViewport().SizeChanged += _OnScreenSizeChanged;

        // Get the initial screen size and update panels
        UpdateScreenSize();

        // Initialize panel positions and visibility
        loginPanel.Position = new Vector2(0, loginPanel.Position.Y);
        registerPanel.Position = new Vector2(screenSize.X, registerPanel.Position.Y);
        loginPanel.Visible = true;
        registerPanel.Visible = false;
    }

    private void UpdateScreenSize()
    {
        screenSize = GetViewport().GetVisibleRect().Size;
        animationController.UpdateScreenSize(screenSize, loginPanel, registerPanel);
    }

    private void _OnScreenSizeChanged()
    {
        UpdateScreenSize();
    }

    private void _OnLoginPanelRegisterButtonPressed()
    {
        animationController.AnimateToRegister(loginPanel, registerPanel);
    }

    private void _OnRegisterPanelLoginButtonPressed()
    {
        animationController.AnimateToLogin(loginPanel, registerPanel);
    }

private bool IsValidEmail(string email)
    {
        string emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailRegex);
    }
private void _OnRegisterButtonPressed()
    {
        // Collect input data from the registration panel
        string username = registerUsernameInput.Text.Trim();
        string password = registerPasswordInput.Text.Trim();
        string email = registerEmailInput.Text.Trim();

        // Check if any of the fields are empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
        {
            GD.Print("All fields must be filled out");
            return;
        }

        // Validate email format
        if (!IsValidEmail(email))
        {
            GD.Print("Invalid email format");
            return;
        }

        // Check if the user already exists based on username or email
        var existingUser = context.Users.FirstOrDefault(user => user.Username == username || user.Email == email);
        if (existingUser != null)
        {
            GD.Print("User with the same username or email already exists.");
            return;
        }

        // Proceed with the user creation and saving to the database
        var newUser = new User
        {
            Username = username,
            Password = password,
            Email = email
        };

        try
        {
            context.Users.Add(newUser);
            context.SaveChanges();
            GD.Print("User registered successfully!");
        }
        catch (Exception ex)
        {
            GD.Print("Error registering user: " + ex.Message);
        }
    }
}
