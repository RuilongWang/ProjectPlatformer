using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TileCollider : MonoBehaviour {
    public bool canHitWhileMovingRight = true;
    public bool canHitWhileMovingLeft = true;
    public bool canHitWhileMovingUp = true;
    public bool canHitWhileMovingDown = true;

    private Collider2D tileCollider;

    #region monobehaviour methods
    private void Start()
    {
        tileCollider = GetComponent<Collider2D>();
    }
    #endregion monobehaviour methods

    #region get point methods
    /// <summary>
    /// Returns a Vector2 point that is the closest to the right of the collider
    /// from the point that is passed in.
    /// </summary>
    /// <param name="originPoint"></param>
    /// <returns></returns>
    public virtual Vector2 GetPointToRight(Vector2 point)
    {
        return new Vector2(tileCollider.bounds.max.x, point.y);
    }

    /// <summary>
    /// Returns a Vector2 point that is closest to the left of the collider
    /// from the point that is passed into the method
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public virtual Vector2 GetPointToLeft(Vector2 point)
    {
        return new Vector2(tileCollider.bounds.min.x, point.y);
    }

    /// <summary>
    /// Returns a Vector2 point that is closest to the top of the collider
    /// from the point that is passed into the method
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public virtual Vector2 GetPointToTop(Vector2 point)
    {
        return new Vector2(point.x, tileCollider.bounds.max.y);
    }

    /// <summary>
    /// Returns a Vector2 point that is closes to the bottom of the collider
    /// from the point that is passed into the method
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public virtual Vector2 GetPointToBottom(Vector2 point)
    {
        return new Vector2(point.x, tileCollider.bounds.min.y);
    }
    #endregion get point methods

    #region trigger methods
    /// <summary>
    /// Whenever a tile object as been entered, we should call this method
    /// </summary>
    /// <param name="collider"></param>
    public virtual void OnTileEntered(CustomCollider2D collider)
    {
    }

    /// <summary>
    /// Whenever a custom collider object exits this tile, this method should be called
    /// </summary>
    /// <param name="collider"></param>
    public virtual void OnTileExited(CustomCollider2D collider)
    {

    }
    #endregion trigger methods
}
