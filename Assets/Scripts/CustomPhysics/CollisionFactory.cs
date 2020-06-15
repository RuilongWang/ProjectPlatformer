using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CollisionFactory
{
    public enum ECollisionShape
    {
        NONE,
        BOX,
    }

    /// <summary>
    /// Returns a Bounds Instance Based On desired bounds collision shap
    /// </summary>
    /// <returns></returns>
    public static Bounds GetNewBoundsInstance(ECollisionShape BoundsCollisionShape)
    {
        switch (BoundsCollisionShape)
        {
            case ECollisionShape.BOX:
                return new Box2DBounds();
            default:
                Debug.LogWarning("You passed in an unsupported Collision Shape.");
                return new Box2DBounds();
        }
    }


    #region collision bounds refs
    public abstract class Bounds
    {
        /// <summary>
        /// This value identifies the shape of our collider. This is primarily for optimization when checking what type of shape we are using rather than comparing against the class type
        /// </summary>
        public ECollisionShape CollisionShape { get; protected set; }

        public Vector2 CenterPoint { get { return GetCenterPoint(); } }
        public Vector2 MinBounds { get { return GetMinBounds(); } }
        public Vector2 MaxBounds { get { return GetMaxBounds(); } }

        /// <summary>
        /// This checks our bounds are currently overlapping with the bounds that
        /// are passed in
        /// </summary>
        /// <param name="BoundsToCheck"></param>
        /// <returns></returns>
        public bool IsOverlappingBounds(Bounds BoundsToCheck)
        {
            switch (BoundsToCheck.CollisionShape)
            {
                case ECollisionShape.BOX:
                    return IsOverlappingBox2DBounds((Box2DBounds)BoundsToCheck);
                default:
                    Debug.LogWarning("The Bounds shape you have passed in is not supported");
                    return false;
            }
        }
        /// <summary>
        /// This method will return whether or not we should push our our collider in the vertical direction or the horizontal direction. You can also check for overlaps
        /// with an overlap applied to it
        /// </summary>
        /// <param name="OtherBounds"></param>
        /// <param name="ShouldPushOutVertically"></param>
        /// <param name="ShouldPushOutHorizontally"></param>
        /// <param name="UseBufferForOverlap"></param>
        public void ShouldPushOutBounds(Bounds OtherBounds, out bool ShouldPushOutVertically, out bool ShouldPushOutHorizontally, bool UseBufferForOverlap = false)
        {
            switch (OtherBounds.CollisionShape)
            {
                case ECollisionShape.BOX:
                    ShouldPushOutBox2dBounds((Box2DBounds)OtherBounds, out ShouldPushOutVertically, out ShouldPushOutHorizontally, UseBufferForOverlap);
                    return;
                default:
                    Debug.LogWarning("The bounds shape that you have passed in is not supported");
                    ShouldPushOutVertically = false;
                    ShouldPushOutHorizontally = false;
                    return;
            }
        }

        protected abstract void ShouldPushOutBox2dBounds(Box2DBounds OtherBounds, out bool ShouldPushOutVertically, out bool ShouldPushOutHorizontally, bool UseBufferForOverlap = false);

        /// <summary>
        /// This method will find the nearest point on the collider that is passed in to snap to. 
        /// </summary>
        /// <param name="BoundsSnappingToUs"></param>
        public Vector2 GetOffsetToClosestHorizontalPointOnBounds(Bounds BoundsSnappingToUs)
        {
            switch (BoundsSnappingToUs.CollisionShape)
            {
                case ECollisionShape.BOX:
                    return GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds((Box2DBounds)BoundsSnappingToUs);
                default:
                    Debug.LogWarning("The bounds shape you have passed in is not currently supproted");
                    return Vector2.zero;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BoundsSnappingToUs"></param>
        /// <returns></returns>
        public Vector2 GetOffsetToClosestVerticalPointOnBounds(Bounds BoundsSnappingToUs)
        {
            switch (BoundsSnappingToUs.CollisionShape)
            {
                case ECollisionShape.BOX:
                    return GetOffsetForNearestVerticalPointOnBoundsForBox2DBounds((Box2DBounds)BoundsSnappingToUs);
                default:
                    Debug.LogWarning("The bounds shape that you have passed in is not currently supported");
                    return Vector2.zero;
            }
        }

        public void CopyBoundsFrom(Bounds BoundsToCopy) 
        {
            if (BoundsToCopy.CollisionShape != this.CollisionShape)
            {
                Debug.LogError("The bounds shape to copy does not match the this bounds shape. This Bounds: " + CollisionShape + " Other Bounds: " + BoundsToCopy.CollisionShape);
                return;
            }

            Vector2 CenterToCopy;
            Vector2 BoundsOfCopy;
            switch(CollisionShape)
            {
                case ECollisionShape.BOX:
                    CenterToCopy = BoundsToCopy.GetCenterPoint();
                    BoundsOfCopy = ((Box2DBounds)BoundsToCopy).BoxSize;
                    ((Box2DBounds)this).SetColliderBoundsForBox2D(ref CenterToCopy, ref BoundsOfCopy);
                    return;
                default:
                    Debug.LogWarning("The Collision type that was passed in is not supported. " + CollisionShape);
                    return;
            }
        }

        #region abstract methods
        protected abstract Vector2 GetMinBounds();
        protected abstract Vector2 GetMaxBounds();
        protected abstract Vector2 GetCenterPoint();
        /// <summary>
        /// Method to check whether or now we are overlapping a Box@DBounds object
        /// </summary>
        /// <param name="BoxBounds"></param>
        /// <returns></returns>
        protected abstract bool IsOverlappingBox2DBounds(Box2DBounds BoxBounds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BoxBounds"></param>
        /// <returns></returns>
        protected abstract Vector2 GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BoxBounds"></param>
        /// <returns></returns>
        protected abstract Vector2 GetOffsetForNearestVerticalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds);
        #endregion abstract methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class Box2DBounds : Bounds
    {
        public Vector2 UpLeft;
        public Vector2 UpRight;
        public Vector2 DownLeft;
        public Vector2 DownRight;
        public Vector2 BufferBounds;


        /// <summary>
        /// Returns a vector2 values that represents the Box size based on the 
        /// </summary>
        public Vector2 BoxSize { get { return UpRight - DownLeft; } }

        public Box2DBounds()
        {
            this.CollisionShape = ECollisionShape.BOX;
        }

        #region override methods
        protected override Vector2 GetMinBounds()
        {
            return DownLeft;
        }

        protected override Vector2 GetMaxBounds()
        {
            return UpRight;
        }

        protected override Vector2 GetCenterPoint()
        {
            return (DownLeft + UpRight) / 2f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OtherBoxBounds"></param>
        /// <returns></returns>
        protected override bool IsOverlappingBox2DBounds(Box2DBounds OtherBoxBounds)
        {
            bool ShouldPushOutVertically, ShouldPushOutHorizontally;
            ShouldPushOutBox2dBounds(OtherBoxBounds, out ShouldPushOutVertically, out ShouldPushOutHorizontally);

            return ShouldPushOutVertically && ShouldPushOutHorizontally;
                
        }

        protected override void ShouldPushOutBox2dBounds(Box2DBounds OtherBoxBounds, out bool ShouldPushOutVertically, out bool ShouldPushOutHorizontally, bool UseBufferForOverlap = false)
        {
            Vector2 CenterA = this.GetCenterPoint();
            Vector2 CenterB = OtherBoxBounds.GetCenterPoint();
            Vector2 SizeA = this.GetMaxBounds() - this.GetMinBounds();
            Vector2 SizeB = (OtherBoxBounds.GetMaxBounds() - OtherBoxBounds.GetMinBounds()) - OtherBoxBounds.BufferBounds;

            ShouldPushOutVertically = (Mathf.Abs(CenterA.x - CenterB.x) * 2) < (SizeA.x + SizeB.x);
            ShouldPushOutHorizontally = Mathf.Abs(CenterA.y - CenterB.y) * 2 < (SizeA.y + SizeB.y);
        }

        protected override Vector2 GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            if (this.MinBounds.x > BoxBounds.MinBounds.x)
            {
                return new Vector2(this.MinBounds.x - BoxBounds.MaxBounds.x, 0);
            }
            else if (this.MaxBounds.x < BoxBounds.MaxBounds.x)
            {
                return new Vector2(this.MaxBounds.x - BoxBounds.MinBounds.x, 0);
            }
            else if(this.CenterPoint.x < BoxBounds.CenterPoint.x)
            {
                return new Vector2(BoxBounds.MinBounds.x - this.MaxBounds.x, 0);
            }
            else
            {
                return new Vector2(BoxBounds.MaxBounds.x - this.MinBounds.x, 0);
            }
        }

        protected override Vector2 GetOffsetForNearestVerticalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            if (this.MinBounds.y > BoxBounds.MinBounds.y)
            {
                return new Vector2(0, this.MinBounds.y - BoxBounds.MaxBounds.y);
            }
            else if (this.MaxBounds.y < BoxBounds.MaxBounds.y)
            {
                return new Vector2(0, this.MaxBounds.y - BoxBounds.MinBounds.y);
            }
            else if (this.CenterPoint.y < BoxBounds.CenterPoint.y)
            {
                return new Vector2(0, BoxBounds.MinBounds.y - this.MaxBounds.y);
            }
            else
            {
                return new Vector2(0, BoxBounds.MaxBounds.y - this.MinBounds.y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CenterPoint"></param>
        /// <param name="Size"></param>
        public void SetColliderBoundsForBox2D(ref Vector2 CenterPoint, ref Vector2 Size)
        {
            UpLeft = CenterPoint + Vector2.up * Size.y / 2 + Vector2.left * Size.x / 2;
            UpRight = CenterPoint + Vector2.up * Size.y / 2 + Vector2.right * Size.x / 2;
            DownLeft = CenterPoint + Vector2.down * Size.y / 2 + Vector2.left * Size.x / 2;
            DownRight = CenterPoint + Vector2.down * Size.y / 2 + Vector2.right * Size.x / 2;
        }
        #endregion override methods

        public Vector3[] GetVerticies()
        {
            return new Vector3[]
            {
                UpLeft,
                UpRight,
                DownRight,
                DownLeft,
                UpLeft,
            };
        }
    }
    #endregion collision bounds refs
}
