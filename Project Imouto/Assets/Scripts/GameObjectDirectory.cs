using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectDirectory {

    public static GameControllerScript GameController { get; set; }
    public static OverlayController OverlayController { get; set; }
    public static InteractionScript InteractionSystem { get; set; }
    public static PlayerHealthController PlayerHealthController { get; set; }
    public static PlayerInputController PlayerInputController { get; set; }
    public static PlayerAnimationController PlayerAnimationController { get; set; }
    public static PlayerMovementController PlayerMovementController { get; set; }
    public static PlayerAttackScript PlayerAttackScript { get; set; }
    public static PlayerXPController PlayerXPController { get; set; }
    public static PlayerProfileController PlayerProfileController { get; set; }
    public static FileHandler FileHandler { get; set; }
    public static MonsterSpawner MonsterSpawner { get; set; }
    public static MissionController MissionController { get; set; }
    public static ItemSpawnerScript ItemSpawner { get; set; }
    public static PlayerStatsController PlayerStatsController { get; set; }

    public static void ClearAll()
    {
        GameController = null;
        OverlayController = null;
    }

}
