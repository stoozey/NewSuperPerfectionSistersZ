using DataPanel;
using DataPanel.DataTypes;
using Engine.Common.Structs;
using Engine.Controllers;
using Engine.Entities.Components;
using Engine.Levels;
using Engine.Loop;
using NewSuperPerfectionSisters.Common.Entities;
using Raylib_cs;


namespace NewSuperPerfectionSisters.Common.Levels;

public class W1L1 : Level
{
    public W1L1()
    {
        using var _dataPanel = new DataPanel<AudioData>("resources/audio/music.dp");
        Music = AssetController.GetSound(_dataPanel.GetData("theme"));
        Raylib.PlaySound(Music);
    }

    public override void OnStart()
    {
        for (var i = 0; i < Runner.WindowSize.X; i += 64)
        {
            var _groundBlock = new Block(BlockType.Ground);
            _groundBlock.GetComponent<Transform2D>().Position = new Vector2(i, (int) (Runner.WindowSize.Y - 64));
        }

        var _questionBlock = new Block(BlockType.Question);
        _questionBlock.GetComponent<Transform2D>().Position = new Vector2((int) Runner.WindowSize.X - 150, 128);
    }
}