using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class BlockSimpleTurretPower : BlockTurretPower
    {
        [SerializeField]
        private Transform turretPivot = null;
        [SerializeField]
        private float rangeOfSight = 5f;
        [SerializeField]
        private float projectileSpeed = 5f;
        [SerializeField]
        private float[] timeToWaitToShootPerBoostLevel = new float[5];
        [SerializeField]
        private Rigidbody2D projectilePrefab = null;
        [SerializeField]
        private float timeToDestroyProjectile = 1.5f;
        [SerializeField]
        private Transform[] shotTransforms = new Transform[1];

        private float timeWaitingToShoot = 0;

        public UnityEvent OnShot = new UnityEvent();

        public override void OnBlockConnected()
        {

        }

        public override void AimTarget(CoreBlock target, Vector2 targetPredectePosition, float timeElapsedBetweenTicks)
        {
            if (TheEnemyIsWithinRange(target))
            {
                LookTarget(targetPredectePosition);
                Shot(timeElapsedBetweenTicks);
            }
        }

        private bool TheEnemyIsWithinRange(CoreBlock target)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= rangeOfSight)
            {
                return true;
            }
            return false;
        }
        private void LookTarget(Vector2 targetPredectePosition)
        {
            turretPivot.transform.up = (targetPredectePosition - (Vector2)transform.position).normalized;
        }

        private void Shot(float timeElapsedBetweenTicks)
        {
            timeWaitingToShoot -= timeElapsedBetweenTicks;

            if (timeWaitingToShoot <= 0)
            {
                if (currentBoostLevel >= timeToWaitToShootPerBoostLevel.Length)
                {
                    currentBoostLevel = timeToWaitToShootPerBoostLevel.Length - 1;
                }

                timeWaitingToShoot = timeToWaitToShootPerBoostLevel[currentBoostLevel];
                
                ShootProjectiles(CreateProjectiles());
                OnShot.Invoke();
            }
        }

        //private void Update()
        //{
        //    float timeToWaitForShot = timeToWaitToShootPerBoostLevel[Mathf.Min(currentBoostLevel, timeToWaitToShootPerBoostLevel.Length - 1)];

        //    bool iAmPlayerOrEnemy = myBlock.CurrentAffiliation != Affiliation.Free;

        //    if (iAmPlayerOrEnemy)
        //    {
        //        Block nearestEnemy = GetNearestEnemyBlock(rangeOfSight);

        //        if(nearestEnemy)
        //        {
        //            LookAtNearestEnemy(nearestEnemy);
        //            WaitForShot(timeToWaitForShot);

        //            bool hasWaitedEnough = timeWaitingToShoot >= timeToWaitForShot;

        //            if (hasWaitedEnough)
        //            {
        //                ResetWaitTime();
        //                ShootProjectiles(CreateProjectiles());
        //            }
        //        }
        //        else
        //        {
        //            WaitForShot(timeToWaitForShot);
        //        }
        //    }
        //}

        private void LookAtNearestEnemy(Block nearestEnemy)
        {
            Rigidbody2D targetBody = nearestEnemy.GetComponentInParent<Rigidbody2D>();

            Vector2 targetDirection = targetBody.velocity.normalized;
            float distanceToTarget = Vector2.Distance(transform.position, nearestEnemy.transform.position);
            float targetSpeed = targetBody.velocity.magnitude * distanceToTarget * Time.deltaTime;
            targetSpeed *= 0.8f;

            Vector2 targetPredectePosition = (Vector2)nearestEnemy.transform.position + (targetDirection * targetSpeed);

            turretPivot.transform.up = (targetPredectePosition - (Vector2)transform.position).normalized;
        }

        private void WaitForShot(float timeToWaitForShot)
        {
            timeWaitingToShoot = Mathf.Clamp(timeWaitingToShoot + Time.deltaTime, 0f, timeToWaitForShot);
        }

        private void ResetWaitTime()
        {
            timeWaitingToShoot = 0f;
        }

        private List<Rigidbody2D> CreateProjectiles()
        {
            List<Rigidbody2D> projectilesCreated = new List<Rigidbody2D>();

            foreach (Transform t in shotTransforms)
            {
                Rigidbody2D projectile = Instantiate(projectilePrefab, t.position, Quaternion.identity);

                projectile.transform.up = t.up;
                projectile.gravityScale = 0f;
                projectile.drag = 0f;
                projectile.gameObject.layer = LayerMask.NameToLayer(myBlock.GetCore()?.CurrentAffiliation.ToString());

                projectilesCreated.Add(projectile);
            }

            return projectilesCreated;
        }

        private void ShootProjectiles(List<Rigidbody2D> projectiles)
        {
            foreach (Rigidbody2D p in projectiles)
            {
                p.AddForce(p.transform.up * projectileSpeed, ForceMode2D.Impulse);
                Destroy(p.gameObject, timeToDestroyProjectile);
            }
        }
    }
}
