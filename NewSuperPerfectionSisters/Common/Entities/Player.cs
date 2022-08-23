using DataPanel;
using DataPanel.DataTypes;
using Engine.Controllers;
using Engine.Entities;
using Engine.Entities.Components;
using NewSuperPerfectionSisters.Common.Components;
using Raylib_cs;


namespace NewSuperPerfectionSisters.Common.Entities;

public class Player : Entity
{
    private Components.Player player;
    
    protected override void Construct()
    {
        AddComponent(new Transform2D(this));
        AddComponent(
            new PlatformerInputDetector(this),
            new Sprite2D(this),
            new Collider2D(this)
        );
        AddComponent(new Components.Player(this, new DataPanel<SpriteData>("resources/sprites/player.dp"), new DataPanel<AudioData>("resources/audio/sfx.dp")));
    }

    protected override void Init()
    {
        player = GetComponent<Components.Player>();
        player.OnJump += (_, _) =>
        {
            var _random = new Random();
            var _string = $"jump{_random.Next(1, 2)}";
            using var _dataPanel = new DataPanel<AudioData>("resources/audio/sfx.dp");
            var _sound = AssetController.GetSound(_dataPanel.GetData(_string));
            Raylib.PlaySound(_sound);
        };
    }

    public override void Render()
    {
    }

    public override void Collide()
    {
    }

    public override void Update()
    {
    }
}