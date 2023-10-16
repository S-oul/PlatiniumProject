using UnityEngine;

public interface Interactable
{
    //The collider that checks if the player is in range. 
    Collider2D InteractionRangeCollider { get; }

    //On IteractionRange collider enter
    void OnEnterInteractionRange(Player player);

    //When the player activates/interacts with the object
    void Interact(Player player);
}