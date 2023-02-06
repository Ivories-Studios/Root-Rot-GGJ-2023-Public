using UnityEngine;

public static class GameState
{
    public static bool IsPaused;
    public static Vector2 Checkpoint = new Vector2(0, 3);

    public static void Save()
    {
        PlayerPrefs.SetFloat("CheckpointX", Checkpoint.x);
        PlayerPrefs.SetFloat("CheckpointY", Checkpoint.y);
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        Checkpoint = new Vector2(PlayerPrefs.GetFloat("CheckpointX"), PlayerPrefs.GetFloat("CheckpointY"));
    }
}
