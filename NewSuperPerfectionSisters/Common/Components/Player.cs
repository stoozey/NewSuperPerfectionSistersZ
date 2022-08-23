using System.Numerics;
using DataPanel;
using DataPanel.DataTypes;
using Engine.Controllers;
using Engine.Entities;
using Engine.Entities.Components;
using Raylib_cs;


namespace NewSuperPerfectionSisters.Common.Components;

public class Player : Component
{
    public DataPanel<SpriteData> Sprites;

    public EventHandler? OnJump;

    private const int JUMP_POWER = 15;
    private const int JUMP_CACHE_MAX = 4;
    
    private int walkSpeed;
    private int runSpeed;

    private DataPanel<SpriteData> playerSprites;

    private PlatformerInputDetector inputDetector;
    private Transform2D transform;
    private Sprite2D sprite2D;
    private Collider2D collider2D;

    private Vector2 inputAxis;

    private int jumpState = 0;
    private int jumpPower = 0;
    private bool jumpCached = false;
    
    public Player(Entity _owner, DataPanel<SpriteData> _sprites, DataPanel<AudioData> _sounds, int _walkSpeed = 2, int _runSpeed = 3) : base(_owner)
    {
        Sprites = _sprites;

        walkSpeed = _walkSpeed;
        runSpeed = _runSpeed;
        
        inputDetector = Require<PlatformerInputDetector>();
        transform = Require<Transform2D>();
        sprite2D = Require<Sprite2D>();
        collider2D = Require<Collider2D>();
        
        var _startingTexture = Sprites.GetData("idle");
        sprite2D.SetSprite(_startingTexture);

        inputDetector.OnJumpPressed += (_, _) => Jump();

    }

    private void Jump()
    {
        if (jumpState > 0) return;
            
        OnJump?.Invoke(this, EventArgs.Empty);
        jumpState = 1;
        jumpPower = -JUMP_POWER;
    }
    
    private void CheckCollisions()
    {
        inputAxis = inputDetector.Axis;

        var _colliderRect = collider2D.Rectangle;
        if (EntityController.GetCollidingEntity(Owner, new Rectangle(_colliderRect.x + inputAxis.X, _colliderRect.y, _colliderRect.width + inputAxis.X, _colliderRect.height)) != null)
            inputAxis.X = 0;
        
        if (EntityController.GetCollidingEntity(Owner, new Rectangle(_colliderRect.x, _colliderRect.y + inputAxis.Y, _colliderRect.width, _colliderRect.height + inputAxis.Y)) != null)
            inputAxis.Y = 0;
    }
    
    private void HandleMovement()
    {
        switch (jumpState)
        {
            case 0:
            {
                
                // if (jumpCached)
                // {
                //     jumpCached = false;
                //     Jump();
                // }

                break;
            }
                
            case 1:
            {
                jumpPower += 1;
                if (jumpPower >= 0)
                    jumpState = 2;
                
                break;
            }
            
            case 2:
            {
                jumpPower += 1;
                if ((inputDetector.JumpDown) && (JUMP_POWER - jumpPower) <= JUMP_CACHE_MAX)
                    jumpCached = true;
                
                if (jumpPower >= JUMP_POWER)
                {
                    jumpPower = 0;
                    jumpState = 0;
                }

                break;
            }
        }

        var _moveSpeed = walkSpeed;
        transform.Position.X += ((int) inputAxis.X * _moveSpeed);
        transform.Position.Y += jumpPower;
        
        collider2D.Rectangle = sprite2D.Rectangle;
    }
    
    public override void Render()
    {
    }

    public override void Update()
    {
        CheckCollisions();
        HandleMovement();
    }
}