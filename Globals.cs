using Godot;

public partial class Globals : Node
{
    // === Constantes ===
    public const int LIFE_MIN = -1;
    public const int LIFE_MAX = 2;

    // === Estados Gerais ===
    public static int note = 0;
    public static int score = 0;
    public static int life = LIFE_MAX;
    public static int flyes = 0;
    public static int scene = 0;
    public static int animal = 1;
    public static int unlock = 4;

    // === Flags de Controle ===
    public static bool pause = false;
    public static bool Sswitch = false;
    public static bool help = false;
    public static bool transform = false;
    public static bool sound = true;
    public static bool options = false;
    public static bool control = true;
    public static bool checkpoint = false;
    public static bool notehud = false;
    public static bool lifehud = false;
    public static bool lockFlag = false;
    public static bool exit = false;
    public static bool released = false;
    public static bool cloud = false;
    public static bool gameover = false;
    public static bool forget = false;
    public static bool totem = false;

    // === Dados do Player ===
    public static CharacterBody2D player = null;
    public static Training save = null;
    public static string ability = "";
    public static Vector2 posit = Vector2.Zero;

    public override void _Process(double delta)
    {
        life = Mathf.Clamp(life, LIFE_MIN, LIFE_MAX);

        if (life > -1 || forget)
            gameover = false;
        else
            gameover = true;
    }
}

