
using UnityEngine;

public class GameConstants
{
    public const int TOTAL_SNAKES = 30;
    public const float VISION_SNAKE = 0.8f;
    public const float VIS_DIST = 4;
    public const float VIS_ANGLE = 30;
    public const float OFFSET_BODY_Y_POSITION = 0.02f;
    public const float ROTATION_IA = 1F;
    public static float ROTATION_SPEED_IA {
        get {
            return Random.Range(40, 350);
        }
        }
}
