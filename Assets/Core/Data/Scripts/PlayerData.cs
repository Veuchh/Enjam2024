using UnityEngine;

public static class PlayerData
{
    public static Vector2 currentMoveInput;
    public static Orientation currentOrientation = Orientation.South;

    public static bool IsAttacking;

    public static bool CanMove => !IsAttacking;

    public static bool CanAttack => !IsAttacking;
}
