using Godot;
using System;
using System.Linq;
using vansur.script;
using vansur.script.Singles;

public partial class UnitPick : Control
{
    [Export] public Button[] ChooseButtons = []; 
    public Label InfoLabel, SecondaryInfoLabel;
    [Export] Button startButton;

    private string[] info =
    [
       "tank FH\nspeed: 160px/s\naccel: 100px/s\nhp: 100\narmor: 400u\nrotation speed: 4rad/sec",
       "tank LA\nspeed: 180px/s\naccel: 80px/s\nhp: 120\narmor: 290u\nrotation speed: 3rad/sec",
       "tank Emir\nspeed: 220px/s\naccel: 60px/s\nhp:200\narmor: 200u\nrotation speed: 2.5rad/sec",
       "tank TL-3", //TrippleLauncher-3
       "tank IH-30" //Ierihon30
    ];

    private Tank[] tanks;
    private Sprite2D[] bodies;
    private int[] tankIndex = [0, 0];

    private (Texture2D bodyTexture, PackedScene[] heads, float speed, float accel, float hp, float armor, float rotationSpeed)
       [] tankinfo;

    void LoadTI()
    {
    
       var defaultTex = GD.Load<Texture2D>("res://assets/tank.png");
       var gl = StatInfo.HeadScenes;

       tankinfo =
       [
          (defaultTex, [gl[0]], 160, 100, 100, 400, 1.64f),
          (defaultTex, [gl[1]], 180, 80, 120, 290, 1.17f),
          (defaultTex, [gl[2]], 220, 60, 200, 200, 1.0f),
          (defaultTex, [gl[3]], 300, 200, 130, 130, 2.0f),
          (defaultTex, [gl[4]], 120, 200, 150, 500, 0.7f)
       ];

       string[] names = ["tank FH", "tank LA", "tank Emir", "tank TL-3", "tank IH-30"];
       info = new string[tankinfo.Length];

       for (int i = 0; i < tankinfo.Length; i++)
       {
          var d = tankinfo[i];
          info[i] = $"{names[i]}\nspeed: {d.Item3}px/s\naccel: {d.Item4}px/s\nhp: {d.Item5}\narmor: {d.Item6}u\nrotation speed: {d.Item7}rad/sec";
       }
    
    }

    public override void _Ready()
    {
       LoadTI();
       startButton = GetNode<Button>("Button");
       startButton.Pressed += Start;
       InfoLabel = GetNode<Label>("Label");
       SecondaryInfoLabel = GetNode<Label>("SecondaryInfoLabel");
       
       tanks = [GetNode<Tank>("Control/tank"), GetNode<Tank>("Control2/tank")];
       bodies = [tanks[0].GetNode<Sprite2D>("Tank"), tanks[1].GetNode<Sprite2D>("Tank")];

       UpdateInfoText(0, 0);
       UpdateInfoText(0, 1);

       for (int i = 0; i < ChooseButtons.Length; i++)
       {
          var btn = ChooseButtons[i];
          int playerIndex = i / 2;
          int direction = (i % 2 == 0) ? -1 : 1;
          
          btn.Pressed += () => SetButton(bodies[playerIndex], direction, playerIndex);
       }
    }

    void SetButton(Sprite2D sprite, int direction, int absI)
    {
       int newIndex = tankIndex[absI] + direction;

       if (newIndex >= 0 && newIndex < tankinfo.Length)
       {
           tankIndex[absI] = newIndex;
           var currentInfo = tankinfo[tankIndex[absI]];
           
           sprite.Texture = currentInfo.bodyTexture;
           tanks[absI].ClearHeads();
           foreach (var t in currentInfo.heads)
              tanks[absI].AddHead(t);
           
           UpdateInfoText(tankIndex[absI], absI);
       }
    }

    private void UpdateInfoText(int unitId, int playerIndex)
    {
       if (unitId < info.Length)
       {
          if (playerIndex == 0)
             InfoLabel.Text = info[unitId];
          else
             SecondaryInfoLabel.Text = info[unitId];
       }
    }

    void Start()
    {
       var map = GD.Load<PackedScene>("res://scene/duels.tscn").Instantiate<Duels>();
       var tankScene = GD.Load<PackedScene>("res://scene/tank.tscn");

       for (var o = 0; o < 2; o++) 
       {
          var newTank = tankScene.Instantiate<Tank>();
          newTank.Team = o; 
          var tinfo = tankinfo[tankIndex[o]];
          
          newTank.Position = new Vector2((newTank.Team == 0) ? 300 : 1200, 100 + 100 * o / 2f);
          
          foreach (var t in tinfo.heads)
             newTank.AddHead(t);
             
          newTank.GetNode<Sprite2D>("Tank").Texture = tinfo.bodyTexture;
          newTank.MaxSpeed = tinfo.speed;
          newTank.Acceleration = tinfo.accel;
          newTank.MaxHp = tinfo.hp;
          newTank.MaxArmor = tinfo.armor;
          newTank.RotationSpeed = tinfo.rotationSpeed;
          var move = GD.Load<PackedScene>("res://scene/user_move.tscn").Instantiate<UserMove>();
          move.IsJoySitck = o == 1; 
          if (move.IsJoySitck) map.Tank1 = newTank;
          else map.Tank2 = newTank;
          newTank.AddChild(move);
          move.myTank = newTank;
          map.AddChild(newTank);
       }

       map.Init();
       GetTree().Root.AddChild(map);
       CallDeferred("queue_free");
    }
}