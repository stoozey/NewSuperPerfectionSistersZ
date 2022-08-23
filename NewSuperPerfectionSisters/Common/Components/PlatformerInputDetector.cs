using Engine.Entities;
using Engine.Entities.Components;
using Raylib_cs;


namespace NewSuperPerfectionSisters.Common.Components;

public class PlatformerInputDetector : Component
{
    public System.Numerics.Vector2 Axis;
    public bool JumpDown;
    public bool MenuDown;
    
    public event EventHandler? OnJumpPressed;
    public event EventHandler? OnJumpReleased;

    public event EventHandler? OnMenuPressed;
    public event EventHandler? OnMenuReleased;
    
    public event EventHandler? OnUpPressed;
    public event EventHandler? OnUpReleased;
    public event EventHandler? OnDownPressed;
    public event EventHandler? OnDownReleased;
    public event EventHandler? OnLeftPressed;
    public event EventHandler? OnLeftReleased;
    public event EventHandler? OnRightPressed;
    public event EventHandler? OnRightReleased;
    
    private const KeyboardKey KEY_LEFT = KeyboardKey.KEY_A;
    private const KeyboardKey KEY_RIGHT = KeyboardKey.KEY_D;
    private const KeyboardKey KEY_UP = KeyboardKey.KEY_W;
    private const KeyboardKey KEY_DOWN = KeyboardKey.KEY_S;
    
    private const KeyboardKey KEY_JUMP = KeyboardKey.KEY_SPACE;
    private const KeyboardKey KEY_MENU = KeyboardKey.KEY_ESCAPE;

    public PlatformerInputDetector(Entity _owner) : base(_owner)
    {
    }

    private static int GetInputValue(KeyboardKey _keyboardKey)
        => (Raylib.IsKeyDown(_keyboardKey) ? 1 : 0);

    private bool HandleEvent(KeyboardKey _keyboardKey, EventHandler? _onPressed, EventHandler? _onReleased)
    {
        if (Raylib.IsKeyPressed(_keyboardKey))
            _onPressed?.Invoke(this, EventArgs.Empty);
        else if (Raylib.IsKeyReleased(_keyboardKey))
            _onReleased?.Invoke(this, EventArgs.Empty);

        return Raylib.IsKeyDown(_keyboardKey);
    }
    
    private void HandleAxis()
    {
        Axis.X = (GetInputValue(KEY_RIGHT) - GetInputValue(KEY_LEFT));
        Axis.Y = (GetInputValue(KEY_DOWN) - GetInputValue(KEY_UP));
    }

    private void HandleEvents()
    {
        JumpDown = HandleEvent(KEY_JUMP, OnJumpPressed, OnJumpReleased);
        MenuDown = HandleEvent(KEY_MENU, OnMenuPressed, OnMenuReleased);

        HandleEvent(KEY_UP, OnUpPressed, OnUpReleased);
        HandleEvent(KEY_DOWN, OnDownPressed, OnDownReleased);
        HandleEvent(KEY_LEFT, OnLeftPressed, OnLeftReleased);
        HandleEvent(KEY_RIGHT, OnRightPressed, OnRightReleased);
    }
    
    public override void Render()
    {
    }

    public override void Update()
    { 
        HandleAxis();
        HandleEvents();
    }
}