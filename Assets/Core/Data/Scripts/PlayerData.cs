using UnityEngine;

public static class PlayerData
{
    public static Vector2 currentMoveInput;
    public static Orientation currentOrientation = Orientation.South;

    public static int seed ;
    public static bool IsAttacking;
    public static float bestTime;

    public static bool CanMove => !IsAttacking;

    public static bool CanAttack => !IsAttacking;
}
