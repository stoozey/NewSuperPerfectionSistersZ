using DataPanel;
using DataPanel.DataTypes;
using Engine.Controllers;
using Engine.Entities;
using Engine.Entities.Components;
using Engine.Levels;
using NewSuperPerfectionSisters.Common.Components;
using Raylib_cs;

namespace NewSuperPerfectionSisters.Common.Entities;

public enum BlockType
{
    Ground,
    Question,
    Explosion
}

public class Block : Entity
{
    private BlockType blockType;

    private double? closeTime;
    
    private DataPanel<SpriteData> dataPanel;
    private Collider2D collider2D;
    private Sprite2D sprite2D;

    private bool didTheThing = false;
    
    public Block(BlockType _blockType) : base()
    {
        blockType = _blockType;
    }
    
    protected override void Construct()
    {
        AddComponent( new Transform2D(this));
        AddComponent(
            new Sprite2D(this),
            new Collider2D(this)
        );
    }

    protected override void Init()
    {
        collider2D = GetComponent<Collider2D>();
        sprite2D = GetComponent<Sprite2D>();
        
        dataPanel = new DataPanel<SpriteData>("resources/sprites/blocks.dp");
    }

    public override void Render()
    {
        switch (blockType)
        {
            case BlockType.Ground: 
                sprite2D.SetSprite(dataPanel.GetData("cum_brick"));
                break;

            case BlockType.Question:
                sprite2D.SetSprite(dataPanel.GetData("question_brick"));
                break;
            
            case BlockType.Explosion:
                sprite2D.SetSprite(dataPanel.GetData("explosion"));
                break;
        }
        
        collider2D.Rectangle = sprite2D.Rectangle;
    }

    public override void Collide()
    {
        if (blockType != BlockType.Question) return;
        
        if (didTheThing) return;
        
        var _player = EntityController.GetCollidingEntity(this, collider2D.Rectangle);
        if (_player == null) return;

        didTheThing = true;
        blockType = BlockType.Explosion;

        Raylib.StopSound(LevelController.Level.Music);
        
        GetComponent<Transform2D>().Position.X -= 128;
        GetComponent<Transform2D>().Position.Y -= 128;
        
        using var _musicPanel = new DataPanel<AudioData>("resources/audio/music.dp");
        using var _cumPanel = new DataPanel<AudioData>("resources/audio/cum.dp");
        MusicController.SetMusic(_musicPanel.GetData("game_complete"));
        MusicController.Play();
        
        Raylib.PlaySound(AssetController.GetSound(_cumPanel.GetData("cummies")));

        closeTime = Raylib.GetTime() + 6.1;
    }

    public override void Update()
    {
        if ((closeTime != null) && (Raylib.GetTime() >= closeTime))
            Raylib.CloseWindow();
    }
}