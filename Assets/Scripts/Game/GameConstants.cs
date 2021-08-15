
using UnityEngine;

public class GameConstants
{
    public const int TOTAL_SNAKES = 80;
    public const float VISION_SNAKE = 0.2f;
    public const float VISION_FOOD = 0.5F;
    public const float VIS_DIST = 4;
    public const float VIS_ANGLE = 30;
    public const float OFFSET_BODY_Y_POSITION = 0.02f;
    public const float ROTATION_IA = 1F;
    public const float OFFSET_FIELD = 10F;
    public static float ROTATION_SPEED_IA {
        get {
            return Random.Range(40, 350);
        }
    }

    public static float TIME_TO_SPAWN_SNAKE
    {
        get
        {
            return Random.Range(2, 60);
        }
    }

    public static int LENGTH_SNAKE
    {
        get
        {
            return Random.Range(8, 40);
        }
    }

    public static float MAXSNAKESCALE = 100F;

    public static float SNAKE_HEAD_SCALE = 10;

    public static float SNAKE_DIFF = 0.15F;
}
