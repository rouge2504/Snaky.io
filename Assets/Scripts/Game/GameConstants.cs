
using UnityEngine;

public class GameConstants
{
    public static int TOTAL_SNAKES = 70;
    public const float VISION_SNAKE = 0.2f;
    public const float VISION_FOOD = 0.5F;
    public const float VIS_DIST = 4;
    public const float SPEED = 30;
    public const float VIS_ANGLE = 30;
    public const float OFFSET_BODY_Y_POSITION = 0.02f;
    public const float ROTATION_IA = 1F;
    public const float OFFSET_FIELD = 10F;
    public const float FIELD_SCALE = 1500;
    public static float ROTATION_SPEED_IA {
        get {
            return Random.Range(40, 350);
        }
    }

    public static float TIME_TO_SPAWN_SNAKE
    {
        get
        {
            return Random.Range(2, 5);
        }
    }

    public static int LENGTH_SNAKE
    {
        get
        {
            return Random.Range(8, 40);
        }
    }

    public static float MAXSNAKESCALE = 100000;

    public readonly static float SNAKE_HEAD_SCALE = 10;

    public static float SNAKE_DIFF = 0.30F; //0.20


    public static float loadingMenuDelay = 1.5f;

    public static float searchiingMenuDelay {
        get
        {
            return Random.Range(1.5f, 2);
        }
    }


}
