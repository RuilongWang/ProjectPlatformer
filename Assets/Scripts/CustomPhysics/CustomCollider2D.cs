﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base class of our custom collider. This will check to see if there are any points where our collider intersects
/// with other colliders.
/// </summary>
public abstract class CustomCollider2D : MonoBehaviour {

    #region const variables
    protected readonly Color GIZMO_COLOR = Color.green;
    #endregion const variables
    public Vector2 colliderOffset;

    
    [Tooltip("Mark this value true if you would like to treat this value as a trigger")]
    public bool isTrigger;


    [SerializeField]
    [Tooltip("This should be removed in later iterations of the game. Adding this just so that we do not have to redo hitboxes that have already been setup")]
    protected bool ignoreParentScale;

    public int colliderLayer;
    
    /// <summary>
    /// The attached Custom physics component that is attached to our custom collider
    /// This is not required for components that are static.
    /// </summary>
    public CustomPhysics2D rigid { get; set; }

    /// <summary>
    /// Due to the fact that non static colliders will have the chance to have their velocity adjusted when colliding with other non static colliders
    /// it is good to keep a reference of the original velocity
    /// </summary>
    public Vector2 originalVelocity;

    /// <summary>
    /// IMPORTANT: If there is a Custom Physics object attached to the gameobject, this collider will be registered as a nonstatic collider
    /// </summary>
    public bool isStatic
    {
        get
        {
            return rigid == null;
        }
    }

    

    protected virtual void Awake()
    {
        colliderLayer = gameObject.layer;
        UpdateBoundsOfCollider();
        rigid = GetComponent<CustomPhysics2D>();
        
        GameOverseer.Instance.PhysicsManager.AddColliderToManager(this);
    }


    protected virtual void OnDestroy()
    {
        if (GameOverseer.Instance && GameOverseer.Instance.PhysicsManager)
        {
            GameOverseer.Instance.PhysicsManager.RemoveColliderFromManager(this);
        }
    }
    /// <summary>
    /// Be sure to call this method
    /// </summary>
    public abstract void UpdateBoundsOfCollider();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public abstract bool LineIntersectWithCollider(Vector2 origin, Vector2 direction, float length);


    public abstract void PushObjectOutsideOfCollider(CustomCollider2D collider);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public abstract Vector2 GetLowerBoundsAtXValue(float x);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public abstract Vector2 GetUpperBoundsAtXValue(float x);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public abstract Vector2 GetRightBoundAtYValue(float y);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public abstract Vector2 GetLeftBoundAtYValue(float y);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public abstract Vector2 GetCenter();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="colliderToCheck"></param>
    /// <returns></returns>
    public abstract bool ColliderIntersect(CustomCollider2D colliderToCheck);

    public abstract bool ColliderIntersectHorizontally(CustomCollider2D colliderToCheck);

    public abstract bool ColliderIntersectVertically(CustomCollider2D colliderToCheck);


    public virtual CustomCollider2D[] GetAllTilesHitFromRayCasts(Vector2 v1, Vector2 v2, Vector2 direction, float distance, int rayCount)
    {
        Vector2 offset = (v2 - v1) / (rayCount - 1);
        List<CustomCollider2D> lineColliders;
        HashSet<CustomCollider2D> allLines = new HashSet<CustomCollider2D>();
        for (int i = 0; i < rayCount; i++)
        {
            GameOverseer.Instance.PhysicsManager.CheckLineIntersectWithCollider(v1 + offset * i, direction, distance, out lineColliders);
            foreach (CustomCollider2D c in lineColliders)
            {
                if (c != this)
                {
                    allLines.Add(c);
                }
            }
        }

        CustomCollider2D[] allValidColliderList = new CustomCollider2D[allLines.Count];
        allLines.CopyTo(allValidColliderList);
        return allValidColliderList;
    }

    /// <summary>
    /// 
    /// </summary>
    public struct BoundsRect
    {
        public Vector2 TopLeft;
        public Vector2 TopRight;
        public Vector2 BottomLeft;
        public Vector2 BottomRight;
        public Vector2 center;

        public Vector3[] GetVertices()
        {
            return new Vector3[]
            {
                TopLeft,
                TopRight,
                BottomRight,
                BottomLeft,
            };
        }

        public void SetOffset(Vector2 offset)
        {
            TopLeft += offset;
            TopRight += offset;
            BottomLeft += offset;
            BottomRight += offset;
            center += offset;
        }

        public override string ToString()
        {
            return "TL: " + TopLeft + "\nTR: " + TopRight + "\nBL: " + BottomLeft + "\nBR: " + BottomRight;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct BoundsCircle
    {
        public Vector2 center;
        public float radius;
    }

    /// <summary>
    /// Bounds collider for our capsule collider simply contains two circles and a rect collider
    /// </summary>
    public struct BoundsCapsule
    {
        public BoundsRect rectBounds;
        public BoundsCircle topCircleBounds;
        public BoundsCircle bottomCircleBounds;

        public void SetOffset(Vector2 offset)
        {
            rectBounds.SetOffset(offset);
            topCircleBounds.center += offset;
            bottomCircleBounds.center += offset;
        }
    }

    #region static methods
    /// <summary>
    /// Use this method to check if a rect bounds intersects another rect bound
    /// </summary>
    /// <returns></returns>
    public static bool RectIntersectRect(BoundsRect r1, BoundsRect r2)
    {
        Vector2 tl1 = r1.TopLeft;
        Vector2 br1 = r1.BottomRight;
        Vector2 tl2 = r2.TopLeft;
        Vector2 br2 = r2.BottomRight;

        if (tl1.x > br2.x || tl2.x > br1.x)
        {
            return false;
        }
        if (tl1.y < br2.y || tl2.y < br1.y)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Use this method to check if a rect bounds intersects a circle bounds
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <param name="intersectionPoint"></param>
    /// <returns></returns>
    public static bool RectIntersectCircle(BoundsRect r, BoundsCircle c)
    {

        Vector2 point = c.center;

        Vector2 A = r.TopLeft;
        Vector2 B = r.TopRight;
        Vector2 D = r.BottomLeft;
        float height = r.TopLeft.y - r.BottomLeft.y;
        float width = r.TopRight.x - r.TopRight.x;
        float APdotAB = Vector2.Dot(point - A, B - A);
        float ABdotAB = Vector2.Dot(B - A, B - A);
        float APdotAD = Vector2.Dot(point - A, D - A);
        float ADdotAD = Vector2.Dot(D - A, D - A);
        if (0 <= APdotAB && APdotAB <= ABdotAB && 0 <= APdotAD && APdotAD < ADdotAD)
        {
            return true;

        }
        
        return LineIntersectCircle(c, r.BottomLeft, r.TopRight);
        //float rectX = r.bottomLeft.x;
        //float recty = r.bottomLeft.y;

        //float nearestX = Mathf.Max(rectX, Mathf.Min(point.x, rectX + width));
        //float nearestY = Mathf.Max(recty, Mathf.Min(point.y, recty + height));

        //float dX = point.x - nearestX;
        //float dY = point.y - nearestY;

        //return (dX * dX + dY * dY) < c.radius * c.radius;
    }

    /// <summary>
    /// Use this method to check if two circle bounds are intersecting with each other
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    /// <param name="intersectionPoint"></param>
    /// <returns></returns>
    public static bool CircleIntersectCircle(BoundsCircle c1, BoundsCircle c2)
    {
        float distanceMax = c1.radius + c2.radius;
        float distance = Vector2.Distance(c1.center, c2.center);

        return distance <= distanceMax;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static bool CapsuleIntersectCapsule(BoundsCapsule c, BoundsCapsule r)
    {
        if (CapsuleIntersectRect(c, r.rectBounds))
        {
            return true;
        }
        if (CapsuleIntersectCircle(c, r.topCircleBounds))
        {
            return true;
        }
        if (CapsuleIntersectCircle(c, r.bottomCircleBounds))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="cir"></param>
    /// <returns></returns>
    public static bool CapsuleIntersectCircle(BoundsCapsule cap, BoundsCircle cir)
    {
        if (CircleIntersectCircle(cap.bottomCircleBounds, cir))
        {
            return true;
        }
        if (CircleIntersectCircle(cap.topCircleBounds, cir))
        {
            return true;
        }
        if (RectIntersectCircle(cap.rectBounds, cir))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static bool CapsuleIntersectRect(BoundsCapsule c, BoundsRect r)
    {
        if (RectIntersectCircle(r, c.bottomCircleBounds))
        {
            return true;
        }
        if (RectIntersectCircle(r, c.bottomCircleBounds))
        {
            return true;
        }
        if (RectIntersectRect(r, c.rectBounds))
        {
            return true;
        }

        return false;
    }

    #region Line Intersection methods

    /// <summary>
    /// Returns true if the line that was passed in intersect with the given circle
    /// </summary>
    /// <param name="c"></param>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool LineIntersectCircle(BoundsCircle c, Vector2 pointA, Vector2 pointB)
    {
        
        Vector2 point = c.center;

        float rectX = pointA.x;
        float recty = pointA.y;

        float nearestX = Mathf.Max(rectX, Mathf.Min(point.x, pointB.x));
        float nearestY = Mathf.Max(recty, Mathf.Min(point.y, pointB.y));

        float dX = point.x - nearestX;
        float dY = point.y - nearestY;

        return (dX * dX + dY * dY) < c.radius * c.radius;
    }

    /// <summary>
    /// Overload method of our line intersect method
    /// </summary>
    /// <param name="c"></param>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool LineIntersectCircle(BoundsCircle c, Vector2 origin, Vector2 direction, float length)
    {
        Vector2 pointA = origin;
        Vector2 pointB = origin + direction * length;
        return LineIntersectCircle(c, pointA, pointB);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool LineIntersectRect(BoundsRect bounds, Vector2 origin, Vector2 direction, float length)
    {
        Vector2 v0 = direction * length;
        Vector2 endpoint = origin + v0;

        Vector2 tr = bounds.TopRight;
        Vector2 bl = bounds.BottomLeft;

        if (bl.x < origin.x && origin.x < tr.x && bl.y < origin.y && origin.y < tr.y)
        {
            return true;
        }


        if (LineCrossLine(origin, v0, bounds.BottomLeft, (bounds.BottomRight - bounds.BottomLeft)))
        {
            return true;
        }
        if (LineCrossLine(origin, v0, bounds.BottomRight, (bounds.TopRight - bounds.BottomRight)))
        {
            return true;
        }
        if (LineCrossLine(origin, v0, bounds.TopRight, (bounds.TopLeft - bounds.TopRight)))
        {
            return true;
        }
        if (LineCrossLine(origin, v0, bounds.TopLeft, (bounds.BottomLeft - bounds.TopLeft)))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns true if the line passes through the given capsule
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static bool LineIntersectsCapsule(BoundsCapsule bounds, Vector2 origin, Vector2 direction, float length)
    {
        if (LineIntersectCircle(bounds.topCircleBounds, origin, direction, length))
        {
            return true;
        }
        if (LineIntersectCircle(bounds.bottomCircleBounds, origin, direction, length))
        {
            return true;
        }
        if (LineIntersectRect(bounds.rectBounds, origin, direction, length))
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Return true if the two lines intersect. u0 and v0 are line 1 and u1 and v1 are line 2
    /// </summary>
    public static bool LineCrossLine(Vector2 u0, Vector2 v0, Vector2 u1, Vector2 v1)
    {
        float d1 = GetDeterminant(v1, v0);
        if (d1 == 0)
        {
            return false;
        }


        float s = (1 / d1) * (((u0.x - u1.x) * v0.y) - ((u0.y - u1.y) * v0.x));
        float t = (1 / d1) * -((-(u0.x - u1.x) * v1.y) + ((u0.y - u1.y) * v1.x));

        return s > 0 && s < 1 && t > 0 && t < 1;
    }

    /// <summary>
    /// Returns the determinant of the two 2D vectors
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static float GetDeterminant(Vector2 v1, Vector2 v2)
    {
        return -v2.x * v1.y + v1.x * v2.y;
    }
    #endregion line intersection methods

    #region intersection point methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
    public static Vector2 IntersectionPointRectOnRect(CustomBoxCollider2D nonstaticCollider, CustomBoxCollider2D staticCollider, bool collidedVertically = true)
    {
        Vector2 intersedctionPoint = Vector2.zero;
        float yPoint;
        float xPoint;
        if (collidedVertically)
        {
            xPoint = nonstaticCollider.Bounds.TopLeft.x;
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCollider, staticCollider, xPoint);
        }
        else
        {
            yPoint = nonstaticCollider.Bounds.BottomRight.y;
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCollider, staticCollider, yPoint);
        }
    }

    /// <summary>
    /// Returns the collision point of the of the two collider bounds
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="c1"></param>
    /// <returns></returns>
    public static Vector2 IntersectionPointNonstaticRectOnStaticCircle(CustomBoxCollider2D nonstaticRectCollider, CustomCircleCollider2D staticCircleCollider, bool collidedVertically = true)
    {
        BoundsRect r1 = nonstaticRectCollider.Bounds;
        BoundsCircle c1 = staticCircleCollider.bounds;

        Vector2 centerPointOfCircle = c1.center;
        Vector2 collisionPoint;

        if (collidedVertically)
        {
            if (centerPointOfCircle.x < r1.BottomLeft.x)
            {
                collisionPoint.x = r1.BottomLeft.x;
            }
            else if (centerPointOfCircle.x > r1.BottomRight.x)
            {
                collisionPoint.x = r1.BottomRight.x;
            }
            else
            {
                collisionPoint.x = centerPointOfCircle.x;
            }

            return IntersectionPointColliderVerticalAtXPoint(nonstaticRectCollider, staticCircleCollider, collisionPoint.x);
        }
        else
        {
            if (centerPointOfCircle.y < r1.BottomLeft.y)
            {
                collisionPoint.y = r1.BottomLeft.y;
            }
            else if (centerPointOfCircle.y > r1.TopLeft.y)
            {
                collisionPoint.y = r1.TopLeft.y;
            }
            else
            {
                collisionPoint.y = centerPointOfCircle.y;
            }
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticRectCollider, staticCircleCollider, collisionPoint.y);
        }
    }

    public Vector2 IntersectionPointStaticRectOnNonstaticCircle(CustomBoxCollider2D staticRect, CustomCircleCollider2D nonstaticCircle, bool collidedVertically)
    {
        BoundsRect r1 = staticRect.Bounds;
        BoundsCircle c1 = nonstaticCircle.bounds;

        Vector2 centerPointOfCircle = c1.center;
        Vector2 collisionPoint;

        if (collidedVertically)
        {
            if (centerPointOfCircle.x < r1.BottomLeft.x)
            {
                collisionPoint.x = r1.BottomLeft.x;
            }
            else if (centerPointOfCircle.x > r1.BottomRight.x)
            {
                collisionPoint.x = r1.BottomRight.x;
            }
            else
            {
                collisionPoint.x = centerPointOfCircle.x;
            }
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCircle, staticRect, collisionPoint.x);
        }
        else
        {
            if (centerPointOfCircle.y < r1.BottomLeft.y)
            {
                collisionPoint.y = r1.BottomLeft.y;
            }
            else if (centerPointOfCircle.y > r1.TopLeft.y)
            {
                collisionPoint.y = r1.TopLeft.y;
            }
            else
            {
                collisionPoint.y = centerPointOfCircle.y;
            }
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCircle, staticRect, collisionPoint.y);
        }
    }

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="upperCircle"></param>
    /// <param name="lowerCircle"></param>
    /// <param name="collidedVertically"></param>
    /// <returns></returns>
    public static Vector2 IntersectionPointCircleOnCircle(CustomCircleCollider2D nonstaticCircle, CustomCircleCollider2D staticCircle, bool collidedVertically = true)
    {
        Vector2 collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCircle.bounds, staticCircle.bounds);

        if (collidedVertically)
        {
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCircle, staticCircle, collisionPoint.x);
        }
        else
        {
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCircle, staticCircle, collisionPoint.y);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nonstaticCapsule"></param>
    /// <param name="staticCircle"></param>
    /// <param name="collidedVertically"></param>
    /// <returns></returns>
    public static Vector2 IntersectionPointNonstaticCapsuleStaticCircle(CustomCapsuleCollider2D nonstaticCapsule, CustomCircleCollider2D staticCircle, bool collidedVertically = true)
    {
        Vector2 cCenter = staticCircle.GetCenter();
        Vector2 capCenter = nonstaticCapsule.GetCenter();
        Vector2 collisionPoint;

        if (collidedVertically)
        {
            if (cCenter.y < capCenter.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.bottomCircleBounds, staticCircle.bounds);
            }
            else
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.topCircleBounds, staticCircle.bounds);
            }
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCapsule, staticCircle, collisionPoint.x);
        }
        else
        {
            if (cCenter.y > nonstaticCapsule.bounds.rectBounds.TopLeft.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.topCircleBounds, staticCircle.bounds);
            }
            else if (cCenter.y < nonstaticCapsule.bounds.rectBounds.BottomLeft.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.bottomCircleBounds, staticCircle.bounds);
            }
            else
            {
                collisionPoint = cCenter;
            }
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCapsule, staticCircle, collisionPoint.y);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="staticCapsule"></param>
    /// <param name="nonstaticCircle"></param>
    /// <param name="collidedVertically"></param>
    /// <returns></returns>
    public static Vector2 IntersectionPointStaticCapsuleNonstaticCircle(CustomCapsuleCollider2D staticCapsule, CustomCircleCollider2D nonstaticCircle, bool collidedVertically = true)
    {
        Vector2 capCenter = staticCapsule.GetCenter();
        Vector2 cCenter = nonstaticCircle.GetCenter();

        Vector2 collisionPoint;
        if (collidedVertically)
        {
            if (cCenter.y < capCenter.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(staticCapsule.bounds.bottomCircleBounds, nonstaticCircle.bounds);
            }
            else
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(staticCapsule.bounds.topCircleBounds, nonstaticCircle.bounds);
            }
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCircle, staticCapsule, collisionPoint.x);
        }
        else
        {
            if (cCenter.y > staticCapsule.bounds.rectBounds.TopLeft.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(staticCapsule.bounds.topCircleBounds, nonstaticCircle.bounds);
            }
            else if (cCenter.y < staticCapsule.bounds.rectBounds.BottomLeft.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(staticCapsule.bounds.bottomCircleBounds, nonstaticCircle.bounds);
            }
            else
            {
                collisionPoint = cCenter;
            }
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCircle, staticCapsule, collisionPoint.y);
        }
    }


    public static Vector2 IntersectionPointNonstaticCapsuleStaticRect(CustomCapsuleCollider2D nonstaticCapsule, CustomBoxCollider2D staticRect, bool collidedVertically = true)
    {
        Vector2 intersectionPoint = Vector2.zero;
        Vector2 capCenter = nonstaticCapsule.GetCenter();
        Vector2 rCenter = staticRect.GetCenter();
        if (collidedVertically)
        {
            float xPoint = capCenter.x;
            if (staticRect.Bounds.TopRight.x < capCenter.x)
            {
                xPoint = staticRect.Bounds.TopRight.x;
            }
            else if (staticRect.Bounds.TopLeft.x > capCenter.x)
            {
                xPoint = staticRect.Bounds.TopLeft.x;
            }
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCapsule, staticRect, xPoint);
        }
        else
        {
            float yPoint = capCenter.y;
            if (staticRect.Bounds.TopLeft.y < nonstaticCapsule.bounds.rectBounds.BottomLeft.y)
            {
                yPoint = staticRect.Bounds.TopLeft.y;
            }
            else if (staticRect.Bounds.BottomLeft.y > nonstaticCapsule.bounds.rectBounds.TopLeft.y)
            {
                yPoint = staticRect.Bounds.BottomLeft.y;
            }

            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCapsule, staticRect, yPoint);
        }
    }

    public static Vector2 IntersectionPointStaticCapsuleNonStaticRect(CustomCapsuleCollider2D staticCapsule, CustomBoxCollider2D nonstaticRect, bool collidedVertically = true)
    {
        Vector2 rCenter = nonstaticRect.GetCenter();
        Vector2 capCenter = staticCapsule.GetCenter();
        if (collidedVertically)
        {
            float xPoint = rCenter.x;
            if (nonstaticRect.Bounds.TopRight.x < rCenter.x)
            {
                xPoint = nonstaticRect.Bounds.TopRight.x;
            }
            else if (nonstaticRect.Bounds.TopLeft.x > rCenter.x)
            {
                xPoint = nonstaticRect.Bounds.TopLeft.x;
            }

            return IntersectionPointColliderVerticalAtXPoint(nonstaticRect, staticCapsule, xPoint);
        }
        else
        {
            float yPoint = rCenter.y;
            if (nonstaticRect.Bounds.TopLeft.y < staticCapsule.bounds.rectBounds.BottomLeft.y)
            {
                yPoint = nonstaticRect.Bounds.TopLeft.y;
            }
            else if (nonstaticRect.Bounds.BottomLeft.y > staticCapsule.bounds.rectBounds.TopLeft.y)
            {
                yPoint = nonstaticRect.Bounds.BottomLeft.y;
            }

            return IntersectionPointColliderHorizontalAtYPoint(nonstaticRect, staticCapsule, yPoint);
        }
    }

    public static Vector2 IntersectionPointCapsuleOnCapsule(CustomCapsuleCollider2D nonstaticCapsule, CustomCapsuleCollider2D staticCapsule, bool collidedVertically = true)
    {
        Vector2 capCenter1 = nonstaticCapsule.GetCenter();
        Vector2 capCenter2 = staticCapsule.GetCenter();

        Vector2 collisionPoint;
        if (collidedVertically)
        {
            if (capCenter1.y < capCenter2.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.topCircleBounds, staticCapsule.bounds.bottomCircleBounds);
            }
            else
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.bottomCircleBounds, staticCapsule.bounds.topCircleBounds);
            }
            return IntersectionPointColliderVerticalAtXPoint(nonstaticCapsule, staticCapsule, collisionPoint.x);
        }
        else
        {
            if (nonstaticCapsule.bounds.bottomCircleBounds.center.y > staticCapsule.bounds.topCircleBounds.center.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.bottomCircleBounds, staticCapsule.bounds.topCircleBounds);
            }
            else if (nonstaticCapsule.bounds.topCircleBounds.center.y < staticCapsule.bounds.bottomCircleBounds.center.y)
            {
                collisionPoint = CollisionPointCircleOnCircleBounds(nonstaticCapsule.bounds.topCircleBounds, staticCapsule.bounds.bottomCircleBounds);
            }
            else if (nonstaticCapsule.bounds.topCircleBounds.center.y < staticCapsule.bounds.topCircleBounds.center.y)
            {
                collisionPoint = nonstaticCapsule.bounds.topCircleBounds.center;
            }
            else
            {
                collisionPoint = nonstaticCapsule.bounds.bottomCircleBounds.center;
            }
            return IntersectionPointColliderHorizontalAtYPoint(nonstaticCapsule, staticCapsule, collisionPoint.y);
        }
    }


    private static Vector2 IntersectionPointColliderVerticalAtXPoint(CustomCollider2D nonstaticCollider, CustomCollider2D staticCollider, float xPoint)
    {
        Vector2 intersectionPoint;
        Vector2 nonstaticCenter = nonstaticCollider.GetCenter();
        Vector2 staticCenter = staticCollider.GetCenter();
        if (nonstaticCenter.y < staticCenter.y)
        {
            intersectionPoint = staticCollider.GetLowerBoundsAtXValue(xPoint);
            intersectionPoint.y -= (nonstaticCollider.GetUpperBoundsAtXValue(xPoint).y - nonstaticCollider.transform.position.y);
        }
        else
        {
            intersectionPoint = staticCollider.GetUpperBoundsAtXValue(xPoint);
            intersectionPoint.y -= (nonstaticCollider.GetLowerBoundsAtXValue(xPoint).y - nonstaticCollider.transform.position.y);
        }

        return intersectionPoint;
    }

    

    private static Vector2 IntersectionPointColliderHorizontalAtYPoint(CustomCollider2D nonstaticCollider, CustomCollider2D staticCollider, float yPoint)
    {
        Vector2 intersectionPoint = Vector2.zero;
        Vector2 nonstaticCenter = nonstaticCollider.GetCenter();
        Vector2 staticCenter = staticCollider.GetCenter();

        if (nonstaticCenter.x < staticCenter.x)
        {
            intersectionPoint = staticCollider.GetLeftBoundAtYValue(yPoint);
            intersectionPoint.x -= (nonstaticCollider.GetRightBoundAtYValue(yPoint).x - nonstaticCollider.transform.position.x);
        }
        else
        {
            intersectionPoint = staticCollider.GetRightBoundAtYValue(yPoint);
            intersectionPoint.x -= (nonstaticCollider.GetLeftBoundAtYValue(yPoint).x - nonstaticCollider.transform.position.x);
        }

        return intersectionPoint;
    }

    private static Vector2 CollisionPointCircleOnCircleBounds(BoundsCircle c1, BoundsCircle c2)
    {
        float totalRadius = c1.radius + c2.radius;
        return c1.center + (c2.center - c1.center) * c1.radius / totalRadius;
    }
    #endregion intersection point methods

    #region layer methods

    /// <summary>
    /// Return whether or not we ignore the collider based ignorelayer value. If a collider is ignored any collision calculations shoudld be skipped
    /// </summary>
    /// <param name="colliderToCheck"></param>
    /// <returns></returns>
    protected bool GetIngoreLayerCollision(CustomCollider2D colliderToCheck)
    {
        return Physics2D.GetIgnoreLayerCollision(this.colliderLayer, colliderToCheck.colliderLayer);
    }
    #endregion layer methods


    #region get outter bounds of collider

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector2 GetLowerBoundsAtXValueCircle(BoundsCircle cBounds, float x)
    {
        float adjustedX = x - cBounds.center.x;

        float angle = Mathf.Acos(adjustedX / cBounds.radius);
        return new Vector2(x, -Mathf.Sin(angle) * cBounds.radius + cBounds.center.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector2 GetUpperBoundsAtXValueCircle(BoundsCircle cBounds, float x)
    {
        float adjustedX = x - cBounds.center.x;

        float angle = Mathf.Acos(adjustedX / cBounds.radius);
        return new Vector2(x, Mathf.Sin(angle) * cBounds.radius + cBounds.center.y);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 GetRighBoundAtYValueCircle(BoundsCircle cBounds, float y)
    {
        float adjustedY = y - cBounds.center.y;
        float angle = Mathf.Asin(adjustedY / cBounds.radius);
        return new Vector2(Mathf.Cos(angle) * cBounds.radius + cBounds.center.x, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 GetLeftBoundAtYValueCircle(BoundsCircle cBounds, float y)
    {
        float adjustedY = y - cBounds.center.y;
        float angle = Mathf.Asin(adjustedY / cBounds.radius);
        return new Vector2(-Mathf.Cos(angle) * cBounds.radius + cBounds.center.x, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector2 GetLowerBoundsAtXValueRect(BoundsRect rBounds, float x)
    {
        return new Vector2(x, rBounds.BottomLeft.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Vector2 GetUpperBoundsAtXValueRect(BoundsRect rBounds, float x)
    {
        return new Vector2(x, rBounds.TopRight.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 GetRighBoundAtYValueRect(BoundsRect rBounds, float y)
    {
        return new Vector2(rBounds.TopRight.x, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 GetLeftBoundAtYValueRect(BoundsRect rBounds, float y)
    {
        return new Vector2(rBounds.BottomLeft.x, y);
    }
    #endregion get outter bounds of collider
    #endregion static methods
}
