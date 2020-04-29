using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class CustomBoxCollider2D : CustomCollider2D
{
    public Vector2 boxColliderSize = Vector2.one;
    [Tooltip("We will thin out the box collider horizontally when checking for collisions with our box collider")]
    public float HorizontalBuffer = .02f;
    [Tooltip("We will thin our box collider vertically to check our horizontal collisions")]
    public float VerticalBuffer = .02f;

    /// <summary>
    /// 
    /// </summary>
    public BoundsRect Bounds { get; set; }

    

    protected BoundsRect HorizontalCheckBounds;
    protected BoundsRect VerticalCheckBounds;

    private void OnValidate()
    {
        rigid = GetComponent<CustomPhysics2D>();
    }

    protected virtual void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            UpdateBoundsOfCollider();
        }

        Color colorToDraw = GIZMO_COLOR;

        DebugSettings.DrawLine(Bounds.BottomLeft, Bounds.BottomRight, colorToDraw);
        DebugSettings.DrawLine(Bounds.BottomRight, Bounds.TopRight, colorToDraw);
        DebugSettings.DrawLine(Bounds.TopRight, Bounds.TopLeft, colorToDraw);
        DebugSettings.DrawLine(Bounds.TopLeft, Bounds.BottomLeft, colorToDraw);
    }



    /// <summary>
    /// Updates the bounds of our box collider. This should be called every frame if you are using a nonstatic
    /// collider
    /// </summary>
    public override void UpdateBoundsOfCollider()
    {
        
        BoundsRect b = new BoundsRect();
        Vector2 localScale;
        if (!ignoreParentScale)
        {
            localScale = this.transform.localScale;
            localScale = new Vector2(Mathf.Abs(localScale.x), Mathf.Abs(localScale.y));
        }
        else
            localScale = Vector2.one;
        Vector2 scaledBoxColliderSize = new Vector2(boxColliderSize.x * localScale.x, boxColliderSize.y * localScale.y);
        Vector2 origin = this.transform.position + (isStatic ? Vector3.zero : Vector3.up * scaledBoxColliderSize.y / 2) + new Vector3(colliderOffset.x * localScale.x, colliderOffset.y * localScale.y);
        
        b.center = origin;
        


        b.TopLeft = origin + (Vector2.up * scaledBoxColliderSize.y / 2) - (Vector2.right * scaledBoxColliderSize.x / 2);
        b.TopRight = origin + (Vector2.up * scaledBoxColliderSize.y / 2) + (Vector2.right * scaledBoxColliderSize.x / 2);
        b.BottomLeft = origin - (Vector2.up * scaledBoxColliderSize.y / 2) - (Vector2.right * scaledBoxColliderSize.x / 2);
        b.BottomRight = origin - (Vector2.up * scaledBoxColliderSize.y / 2) + (Vector2.right * scaledBoxColliderSize.x / 2);

        this.Bounds = b;

        if (!isStatic)
        {
            VerticalCheckBounds = this.Bounds;
            HorizontalCheckBounds = this.Bounds;

            float verticalOffset = 0;
            float horizontalOffset = 0;

            VerticalCheckBounds.TopLeft.x += HorizontalBuffer / 2;
            VerticalCheckBounds.BottomLeft.x += HorizontalBuffer / 2;
            VerticalCheckBounds.TopRight.x -= HorizontalBuffer / 2;
            VerticalCheckBounds.BottomRight.x -= HorizontalBuffer / 2;

            HorizontalCheckBounds.TopLeft.y -= VerticalBuffer / 2;
            HorizontalCheckBounds.TopRight.y -= VerticalBuffer / 2;
            HorizontalCheckBounds.BottomLeft.y += VerticalBuffer / 2;
            HorizontalCheckBounds.BottomRight.y += VerticalBuffer / 2;

            if (Mathf.Abs(rigid.Velocity.y) > 0)
            {
                verticalOffset = Mathf.Sign(rigid.Velocity.y) * Mathf.Max(VerticalBuffer, Mathf.Abs(rigid.Velocity.y * GameOverseer.DELTA_TIME));
            }

            if (Mathf.Abs(rigid.Velocity.x) > 0)
            {
                horizontalOffset = Mathf.Sign(rigid.Velocity.x) * Mathf.Max(HorizontalBuffer, Mathf.Abs(rigid.Velocity.x * GameOverseer.DELTA_TIME));
            }
            VerticalCheckBounds.SetOffset(Vector2.up * verticalOffset);
            HorizontalCheckBounds.SetOffset(Vector2.right * horizontalOffset);

        }
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public override bool LineIntersectWithCollider(Vector2 origin, Vector2 direction, float length)
    {
        return LineIntersectRect(this.Bounds, origin, direction, length);
    }

    

   
   
    /// <summary>
    /// Whenever we intersect with a collider this method should be called to move the collider outside
    /// </summary>
    public override void PushObjectOutsideOfCollider(CustomCollider2D collider)
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public override Vector2 GetLowerBoundsAtXValue(float x)
    {
        return GetLowerBoundsAtXValueRect(this.Bounds, x);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public override Vector2 GetUpperBoundsAtXValue(float x)
    {
        return GetUpperBoundsAtXValueRect(this.Bounds, x);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public override Vector2 GetRightBoundAtYValue(float y)
    {
        return GetRighBoundAtYValueRect(this.Bounds, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public override Vector2 GetLeftBoundAtYValue(float y)
    {
        return GetLeftBoundAtYValueRect(this.Bounds, y);
    }

    public override Vector2 GetCenter()
    {
        return Bounds.center;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="colliderToCheck"></param>
    /// <returns></returns>
    public override bool ColliderIntersect(CustomCollider2D colliderToCheck)
    {
        return ColliderIntersectBounds(this.Bounds, colliderToCheck);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="boundsToCheck"></param>
    /// <param name="colliderToCheck"></param>
    /// <returns></returns>
    private bool ColliderIntersectBounds(BoundsRect boundsToCheck, CustomCollider2D colliderToCheck)
    {
        if (colliderToCheck is CustomBoxCollider2D)
        {
            return RectIntersectRect(boundsToCheck, ((CustomBoxCollider2D)colliderToCheck).Bounds);
        }
        else if (colliderToCheck is CustomCircleCollider2D)
        {
            return RectIntersectCircle(boundsToCheck, ((CustomCircleCollider2D)colliderToCheck).bounds);
        }
        else if (colliderToCheck is CustomCapsuleCollider2D)
        {
            return CapsuleIntersectRect(((CustomCapsuleCollider2D)colliderToCheck).bounds, boundsToCheck);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="colliderToCheck"></param>
    /// <param name="offsetDirection"></param>
    /// <returns></returns>
    public override bool ColliderIntersectVertically(CustomCollider2D colliderToCheck)
    {
        if (colliderToCheck == this || GetIngoreLayerCollision(colliderToCheck)) return false;


        if (rigid.Velocity.y == 0)
        {
            return false;
        }

        if (ColliderIntersectBounds(VerticalCheckBounds, colliderToCheck))
        {
            if (colliderToCheck is CustomBoxCollider2D)
            {
                float yPosition = IntersectionPointRectOnRect(this, (CustomBoxCollider2D)colliderToCheck, true).y;
                this.transform.position = new Vector3(this.transform.position.x, yPosition, this.transform.position.z);

            }
            else if (colliderToCheck is CustomCircleCollider2D)
            {
                Vector2 collisionPoint = IntersectionPointNonstaticRectOnStaticCircle(this, ((CustomCircleCollider2D)colliderToCheck), true);
                this.transform.position = new Vector3(this.transform.position.x, collisionPoint.y, this.transform.position.z);
            }
            else if (colliderToCheck is CustomCapsuleCollider2D)
            {
                Vector2 collisionPoint = IntersectionPointStaticCapsuleNonStaticRect((CustomCapsuleCollider2D)colliderToCheck, this);
                this.transform.position = new Vector3(this.transform.position.x, collisionPoint.y, this.transform.position.z);
            }
            return true;
        }
        return false;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="colliderToCheck"></param>
    /// <returns></returns>
    public override bool ColliderIntersectHorizontally(CustomCollider2D colliderToCheck)
    {
        if (colliderToCheck == this || GetIngoreLayerCollision(colliderToCheck)) return false;


        if (ColliderIntersectBounds(HorizontalCheckBounds, colliderToCheck))
        {
            if (colliderToCheck is CustomBoxCollider2D)
            {
                float xPosition = IntersectionPointRectOnRect(this, (CustomBoxCollider2D)colliderToCheck, false).x;
                this.transform.position = new Vector3(xPosition, this.transform.position.y, this.transform.position.z);

            }
            else if (colliderToCheck is CustomCircleCollider2D)
            {
                Vector2 collisionPoint = IntersectionPointNonstaticRectOnStaticCircle(this, ((CustomCircleCollider2D)colliderToCheck), false);
                transform.position = new Vector3(collisionPoint.x, this.transform.position.y, this.transform.position.z);
            }
            else if (colliderToCheck is CustomCapsuleCollider2D)
            {
                Vector2 collisionPoint = IntersectionPointStaticCapsuleNonStaticRect((CustomCapsuleCollider2D)colliderToCheck, this, false);
                transform.position = new Vector3(collisionPoint.x, this.transform.position.y, this.transform.position.z);
            }
            return true;
        }
        return false;
    }
}
