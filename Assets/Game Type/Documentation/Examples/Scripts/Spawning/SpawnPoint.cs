using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spawning
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(SphereCollider))]
    public class SpawnPoint : MonoBehaviour
    {
        public enum ColliderTypeEnum
        {
            Capsule,
            Box,
            Sphere
        }
        public bool active = true;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                if (value != active)
                {
                    if (value)
                    {
                        Activate();
                    }
                    else
                    {
                        Deactivate();
                    }
                }
            }
        }
        [Header("Team Settings")]
        [Tooltip("Neutral spawns points can spawn any team")]
        public bool neutral = true;

        public List<Teams.Team> whiteList;
        [Header("Blocked Check Settings")]
        [Tooltip("Time in seconds that the spawnpoint should make sure nothing is inside it")]
        public float BlockedCheckInterval = 5f;

        public ColliderTypeEnum _colliderType = ColliderTypeEnum.Capsule;
        public ColliderTypeEnum ColliderType
        {
            get
            {
                return _colliderType;
            }
            set
            {
                switch (value)
                {
                    case ColliderTypeEnum.Capsule:
                        colliders.boxCollider.enabled = false;
                        colliders.sphereCollider.enabled = false;
                        colliders.capsuleCollider.enabled = true;
                        break;
                    case ColliderTypeEnum.Box:
                        colliders.capsuleCollider.enabled = false;
                        colliders.sphereCollider.enabled = false;
                        colliders.boxCollider.enabled = true;
                        break;
                    case ColliderTypeEnum.Sphere:
                        colliders.capsuleCollider.enabled = false;
                        colliders.boxCollider.enabled = false;
                        colliders.sphereCollider.enabled = true;
                        break;
                }
            }
        }
        public LayerMask blockedCheckLayerMask = new LayerMask();
        public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
        public struct Colliders
        {
            public BoxCollider boxCollider;
            public CapsuleCollider capsuleCollider;
            public SphereCollider sphereCollider;
            public Colliders(MonoBehaviour monoBehaviour)
            {
                boxCollider = monoBehaviour.GetComponent<BoxCollider>();
                capsuleCollider = monoBehaviour.GetComponent<CapsuleCollider>();
                sphereCollider = monoBehaviour.GetComponent<SphereCollider>();
            }
        }
        public Colliders colliders;
        public List<Collider> _blockingSpawnZone = new List<Collider>();
        public List<Collider> BlockingSpawnZone
        {
            get
            {
                return _blockingSpawnZone;
            }
            set
            {
                if (value != null)
                {
                    if (value.Count > 0)
                    {
                        Deactivate();
                    }
                    else
                    {
                        if (enabled) Activate();
                    }
                    _blockingSpawnZone = value;
                }
                else
                {
                    _blockingSpawnZone = new List<Collider>();
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (BlockingSpawnZone.Contains(other) == false) BlockingSpawnZone.Add(other);
        }
        private void OnTriggerStay(Collider other)
        {
            if (BlockingSpawnZone.Contains(other) == false) BlockingSpawnZone.Add(other);
        }
        private void OnTriggerExit(Collider other)
        {
            BlockingSpawnZone.Remove(other);
        }
        public virtual List<Collider> BlockedCheck()
        {
            List<Collider> cols = new List<Collider>();
            switch (_colliderType)
            {
                case ColliderTypeEnum.Capsule:
                    cols = new List<Collider>(Physics.OverlapCapsule(transform.TransformPoint(colliders.capsuleCollider.center + new Vector3(0, -colliders.capsuleCollider.height / 2, 0)), transform.TransformPoint(colliders.capsuleCollider.center + new Vector3(0, colliders.capsuleCollider.height / 2, 0)), colliders.capsuleCollider.radius, blockedCheckLayerMask.value, triggerInteraction));
                    cols.Remove(colliders.capsuleCollider);
                    break;
                case ColliderTypeEnum.Box:
                    cols = new List<Collider>(Physics.OverlapBox(transform.TransformPoint(colliders.boxCollider.center), colliders.boxCollider.size / 2, transform.rotation, blockedCheckLayerMask.value, triggerInteraction));
                    cols.Remove(colliders.boxCollider);
                    break;
                case ColliderTypeEnum.Sphere:
                    cols = new List<Collider>(Physics.OverlapSphere(transform.TransformPoint(colliders.sphereCollider.center), colliders.sphereCollider.radius, blockedCheckLayerMask.value, triggerInteraction));
                    cols.Remove(colliders.sphereCollider);
                    break;
            }
            return cols;
        }
        public IEnumerator CheckSpawnZone()
        {
            while (true)
            {
                BlockingSpawnZone = BlockedCheck();
                yield return new WaitForSecondsRealtime(BlockedCheckInterval);
            }
        }
        [ContextMenu("Setup Colliders")]
        private void SetColliders()
        {
            colliders = new Colliders(this);
            colliders.boxCollider.isTrigger = true;
            colliders.sphereCollider.isTrigger = true;
            colliders.capsuleCollider.isTrigger = true;
            ColliderType = _colliderType;
        }
        private void Awake()
        {
            _blockingSpawnZone = new List<Collider>();
            SetColliders();
            StartCoroutine(CheckSpawnZone());
        }
        public virtual void Activate()
        {
            active = true;
        }
        public virtual void Deactivate()
        {
            active = false;
        }
        private void Reset()
        {
            blockedCheckLayerMask.value = ~0;
            SetColliders();
        }
    }
}