using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTile : MonoBehaviour {
    private HashSet<CustomPhysics2D> currentlyAttachedRigidbodies = new HashSet<CustomPhysics2D>();


    public virtual void OnTileEntered(CustomPhysics2D rigid)
    {
        currentlyAttachedRigidbodies.Add(rigid);
    }

    public virtual void OnTileExited(CustomPhysics2D rigid)
    {
        if (currentlyAttachedRigidbodies.Contains(rigid))
        {
            currentlyAttachedRigidbodies.Remove(rigid);
        }
    }
}
