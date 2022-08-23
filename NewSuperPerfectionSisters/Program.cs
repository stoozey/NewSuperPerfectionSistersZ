using Engine.Common.Structs;
using Engine.Controllers;
using Engine.Entities.Components;
using Engine.Loop;
using NewSuperPerfectionSisters.Common.Entities;
using NewSuperPerfectionSisters.Common.Levels;
using Raylib_cs;


Runner.Initialize();

Runner.WindowTitle = "New Super Perfection Sisters Z";
Runner.WindowSize = new System.Numerics.Vector2(640, 360);
Runner.BackgroundColour = Color.BLUE;

var _player = new Player();
_player.GetComponent<Transform2D>().Position =
    new Vector2(32, (int) Runner.WindowSize.Y - 92 - 64);

var _level = new W1L1();
LevelController.SetLevel(_level);

Runner.Run();