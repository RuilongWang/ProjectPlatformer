﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionFactory
{
    public enum ECollisionShape
    {
        Box,
        Circle,//NOT IMPLEMENTED
        Capsule,//NOT IMPLEMENTED
    }

    /// <summary>
    /// Returns a Bounds Instance Based On desired bounds collision shap
    /// </summary>
    /// <returns></returns>
    public static Bounds GetNewBoundsInstance(ECollisionShape BoundsCollisionShape)
    {
        switch (BoundsCollisionShape)
        {
            case ECollisionShape.Box:
                return new Box2DBounds();
            case ECollisionShape.Circle:
                return new CircleBounds();
            case ECollisionShape.Capsule:
                return new Capsule2DBounds();
            default:
                return new Box2DBounds();
        }
    }


    #region collision bounds refs
    public abstract class Bounds
    {
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
            if (BoundsToCheck is Box2DBounds)
            {
                return IsOverlappingBox2DBounds((Box2DBounds)BoundsToCheck);
            }

            return false;
        }

        /// <summary>
        /// You can cast or 'stretch' our collider to see if anything in the direction and magnitude of our CastOffset is in our way.
        /// If you are using physics collisions, CastOffset should equal the value of how much we are going to move in the next frame
        /// which would be DeltaTime * Velocity
        /// </summary>
        /// <param name="BoundsToCheck"></param>
        /// <param name="CastOffset"></param>
        /// <param name="BufferAmount"></param>
        /// <returns></returns>
        public bool CastCollisionForPhysicsOverlap(Bounds BoundsToCheck, Vector2 CastOffset, Vector2 BufferAmount)
        {
            if (BoundsToCheck is Box2DBounds)
            {
                 
            }

            return false;
        }

        /// <summary>
        /// This method will find the nearest point on the collider that is passed in to snap to. 
        /// </summary>
        /// <param name="BoundsSnappingToUs"></param>
        public Vector2 GetOffsetToClosestHorizontalPointOnBounds(Bounds BoundsSnappingToUs)
        {
            if (BoundsSnappingToUs is Box2DBounds)
            {
                return GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds((Box2DBounds)BoundsSnappingToUs);
            }
            return Vector2.zero;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BoundsSnappingToUs"></param>
        /// <returns></returns>
        public Vector2 GetOffsetToClosestVerticalPointOnBounds(Bounds BoundsSnappingToUs)
        {
            if (BoundsSnappingToUs is Box2DBounds)
            {
                return GetOffsetForNearestVerticalPointOnBoundsForBox2DBounds((Box2DBounds)BoundsSnappingToUs);
            }
            return Vector2.zero;
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

        /// <summary>
        /// Returns a vector2 values that represents the Box size based on the 
        /// </summary>
        public Vector2 BoxSize { get { return UpRight - DownLeft; } }

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

        protected override bool IsOverlappingBox2DBounds(Box2DBounds BoxBounds)
        {
            Vector2 CenterA = this.GetCenterPoint();
            Vector2 CenterB = BoxBounds.GetCenterPoint();
            Vector2 SizeA = this.GetMaxBounds() - this.GetMinBounds();
            Vector2 SizeB = BoxBounds.GetMaxBounds() - BoxBounds.GetMinBounds();

            return (Mathf.Abs(CenterA.x - CenterB.x) * 2) < (SizeA.x + SizeB.x) &&
                Mathf.Abs(CenterA.y - CenterB.y) * 2 < (SizeA.y + SizeB.y);
        }

        protected override Vector2 GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            if (this.MinBounds.x > BoxBounds.MinBounds.x)
            {
                return new Vector2(BoxBounds.MaxBounds.x - this.MinBounds.x, 0);
            }
            else if (this.MaxBounds.x < BoxBounds.MaxBounds.x)
            {
                return new Vector2(BoxBounds.MinBounds.x - this.MaxBounds.x, 0);
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

    /// <summary>
    /// 
    /// </summary>
    public class CircleBounds : Bounds
    {
        public float Radius;
        private Vector2 Center;

        #region override methods
        protected override Vector2 GetCenterPoint()
        {
            return CenterPoint;
        }

        protected override Vector2 GetMaxBounds()
        {
            return CenterPoint + Vector2.one * Radius;
        }

        protected override Vector2 GetMinBounds()
        {
            return CenterPoint - Vector2.one * Radius;
        }

        public void UpdateColliderBounds(Vector2 CenterPoint, float Radius)
        {
            this.Center = CenterPoint;
            this.Radius = Radius;
        }

        protected override bool IsOverlappingBox2DBounds(Box2DBounds BoxBounds)
        {
            throw new System.NotImplementedException();
        }

        protected override Vector2 GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            throw new NotImplementedException();
        }

        protected override Vector2 GetOffsetForNearestVerticalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            throw new NotImplementedException();
        }
        #endregion override methods
    }

    public class Capsule2DBounds : Bounds
    {
        #region override methods
        protected override Vector2 GetCenterPoint()
        {
            throw new System.NotImplementedException();
        }

        protected override Vector2 GetMaxBounds()
        {
            throw new System.NotImplementedException();
        }

        protected override Vector2 GetMinBounds()
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsOverlappingBox2DBounds(Box2DBounds BoxBounds)
        {
            throw new System.NotImplementedException();
        }

        protected override Vector2 GetOffsetForNearestHorizontalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            throw new NotImplementedException();
        }

        protected override Vector2 GetOffsetForNearestVerticalPointOnBoundsForBox2DBounds(Box2DBounds BoxBounds)
        {
            throw new NotImplementedException();
        }
        #endregion override methods
    }

    #endregion collision bounds refs
}
