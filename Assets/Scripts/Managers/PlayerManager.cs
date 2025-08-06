using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private GameObject player;

    // EFFECTS: returns the position of the player in the world
    public Vector2 getPosition()
    {
        return player.transform.position;
    }
}
