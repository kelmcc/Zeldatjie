using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Extension methods for Colliders.
    /// </summary>
    public static class ColliderExtensions
    {
        public static BoundingSphere GetBoundingSphere(this Collider collider)
        {
            return collider.bounds.GetBoundingSphere();
        }

        public static Vector3 GetCenter(this Collider collider)
        {
            Type type = collider.GetType();

            if (type == typeof(BoxCollider)) return ((BoxCollider)collider).GetCenter();
            if (type == typeof(SphereCollider)) return ((SphereCollider)collider).GetCenter();
            if (type == typeof(CapsuleCollider)) return ((CapsuleCollider)collider).GetCenter();
            if (type == typeof(MeshCollider)) return ((MeshCollider)collider).GetCenter();

            throw new NotImplementedException();
        }

        public static Vector3 GetCenter(this CapsuleCollider collider)
        {
            return collider.transform.TransformPoint(collider.center);
        }

        public static Vector3 GetCenter(this SphereCollider collider)
        {
            return collider.transform.TransformPoint(collider.center);
        }

        public static Vector3 GetCenter(this BoxCollider collider)
        {
            return collider.transform.TransformPoint(collider.center);
        }

        public static Vector3 GetCenter(this MeshCollider collider)
        {
            return collider.bounds.center;
        }

        public static bool ContainsPoint(this Collider collider, Vector3 point)
        {
            Type type = collider.GetType();

            if (type == typeof(BoxCollider)) return ((BoxCollider)collider).ContainsPoint(point);
            if (type == typeof(SphereCollider)) return ((SphereCollider)collider).ContainsPoint(point);
            if (type == typeof(CapsuleCollider)) return ((CapsuleCollider)collider).ContainsPoint(point);
            if (type == typeof(MeshCollider)) return ((MeshCollider)collider).ContainsPoint(point);

            throw new NotImplementedException();
        }

        public static bool ContainsPoint(this CapsuleCollider collider, Vector3 point)
        {
            return new Capsule3(collider).ContainsPoint(point);
        }

        public static bool ContainsPoint(this SphereCollider collider, Vector3 point)
        {
            return new Sphere3(collider).ContainsPoint(point);
        }


        public static bool ContainsPoint(this BoxCollider collider, Vector3 point)
        {
            return new Box3(collider).ContainsPoint(point);
        }


        public static bool ContainsPoint(this MeshCollider collider, Vector3 point)
        {
            if (collider.bounds.Contains(point))
            {
                Collider[] colliders = Physics.OverlapSphere(point, 1 << collider.gameObject.layer);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] == collider)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        public static List<Collider> GetIntersectingColliders(this Collider collider)
        {
            Type type = collider.GetType();

            if (type == typeof(BoxCollider)) return ((BoxCollider)collider).GetIntersectingColliders();
            if (type == typeof(SphereCollider)) return ((SphereCollider)collider).GetIntersectingColliders();
            if (type == typeof(CapsuleCollider)) return ((CapsuleCollider)collider).GetIntersectingColliders();
            if (type == typeof(MeshCollider)) return ((MeshCollider)collider).GetIntersectingColliders();

            throw new NotImplementedException();
        }


        public static List<Collider> GetIntersectingColliders(this CapsuleCollider collider)
        {
            Capsule3 capsule = new Capsule3(collider);
            List<Collider> colliders = new List<Collider>(Physics.OverlapCapsule(capsule.InnerStartPoint, capsule.InnerEndPoint, capsule.Radius));
            colliders.Remove(collider);
            return colliders;
        }

        public static List<Collider> GetIntersectingColliders(this SphereCollider collider)
        {
            Sphere3 sphere = new Sphere3(collider);
            List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(sphere.Center, sphere.Radius));
            colliders.Remove(collider);
            return colliders;
        }

        public static List<Collider> GetIntersectingColliders(this BoxCollider collider)
        {
            Box3 box = new Box3(collider);
            List<Collider> colliders = new List<Collider>(Physics.OverlapBox(box.Center, box.Extents, box.Rotation));  
            colliders.Remove(collider);
            return colliders;
        }
        public static List<Collider> GetIntersectingColliders(this BoxCollider collider, LayerMask mask)
        {
            Box3 box = new Box3(collider);
            List<Collider> colliders = new List<Collider>(Physics.OverlapBox(box.Center, box.Extents, box.Rotation, mask));
            colliders.Remove(collider);
            return colliders;
        }

        public static List<Collider> GetIntersectingColliders(this MeshCollider collider)
        {
            Box3 box = new Box3(collider.bounds);
            Collider[] nearbyColliders = Physics.OverlapBox(box.Center, box.Extents, box.Rotation);
            List<Collider> intersectingColliders = new List<Collider>();

            for (int i = 0; i < nearbyColliders.Length; i++)
            {
                if (nearbyColliders[i] != collider && IsColliding(collider, nearbyColliders[i]))
                {
                    intersectingColliders.Add(nearbyColliders[i]);
                }
            }

            return intersectingColliders;
        }

        public static bool IsColliding(this Collider collider, Collider otherCollider)
        {
            float distance;
            Vector3 direction;
            return Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation, otherCollider, otherCollider.transform.position, otherCollider.transform.rotation, out direction, out distance);
        }

        public static bool ComputePenetration(this Collider collider, Collider otherCollider, out float distance, out Vector3 direction)
        {
            return Physics.ComputePenetration(collider, collider.transform.position, collider.transform.rotation, otherCollider, otherCollider.transform.position, otherCollider.transform.rotation, out direction, out distance);
        }

        public static void IgnoreCollisions(this IList<Collider> colliders, IList<Collider> otherColliders, bool ignore = true)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                for (int j = 0; j < otherColliders.Count; j++)
                {
                    if (colliders[i] != null && otherColliders[j] != null)
                    {
                        Physics.IgnoreCollision(colliders[i], otherColliders[j], ignore);
                    }
                }
            }
        }

        public static void IgnoreCollisions(this IList<Collider> colliders, Collider otherCollider, bool ignore = true)
        {
            for (int j = 0; j < colliders.Count; j++)
            {
                if (colliders[j] != null && otherCollider != null)
                {
                    Physics.IgnoreCollision(colliders[j], otherCollider, ignore);
                }
            }
        }

        public static void IgnoreCollisions(this Collider collider, IList<Collider> otherColliders, bool ignore = true)
        {
            for (int j = 0; j < otherColliders.Count; j++)
            {
                if (otherColliders[j] != null && collider != null)
                {
                    Physics.IgnoreCollision(collider, otherColliders[j], ignore);
                }
            }
        }

        public static void IgnoreCollisions(this Collider collider, Collider otherCollider, bool ignore = true)
        {
            Physics.IgnoreCollision(collider, otherCollider, ignore);
        }

        public static int GetColliderLayer(this Collision collision)
        {
            return collision.collider.gameObject.layer;
        }

        public static string GetColliderLayerName(this Collision collision)
        {
            return LayerMask.LayerToName(collision.collider.gameObject.layer);
        }

    }

}
