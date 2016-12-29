using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovesScripts : MonoBehaviour
    {
        public float MoveSpeed;

        public float MiningTime;

        public GameObject EnemyHandForDiamond;

        public float DyingTime;

        private float MoveSpeedStep;

        private Vector2[] PathPoints;

        public Transform DiamondPositionGameObject;
        
        private bool instanciated;

        private bool mining;

        private Action MoveDelegate;

        private DiamondSingleScript DiamondChildScript
        {
            get { return diamondChildScript; }
            set
            {
                diamondChildScript = value;

                if (diamondChildScript != null)
                {
                    DiamondChildScript.CathedByMonster(this);

                    DiamondChildScript.gameObject.transform.position = EnemyHandForDiamond.transform.position;

                    DiamondChildScript.gameObject.transform.SetParent(EnemyHandForDiamond.transform, true);
                }
            }
        }

        private DiamondScript DiamondsPoint;

        private int next_index;
        private bool dying;
        private Animator _anim;
        private bool pickuping;
        private DiamondSingleScript diamondChildScript;
        private bool repeatMoves;

        private Animator anim
        {
            get
            {
                if (_anim == null)
                {
                    _anim = GetComponent<Animator>();
                }
                return _anim;
            }
        }

        // Use this for initialization
        void Start ()
        {
            MoveSpeedStep = MoveSpeed*Time.fixedDeltaTime;
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (MoveDelegate == null)
            {
                return;
            }

            MoveDelegate();

        }

        private void Move()
        {
            var posNow = (Vector2) transform.position;

            var nextPoint = PathPoints[next_index];

            if ((posNow - nextPoint).magnitude < MoveSpeedStep)
            {
                // TODO: if (repeatMoves)
                if (next_index + 1 == PathPoints.Length)
                {
                    OnDestory(true);
                }

                next_index = (next_index + 1)%PathPoints.Length;

                nextPoint = PathPoints[next_index];
            }

            var nextStepVectorDirection = (nextPoint - posNow).normalized;

            var nextStep = nextStepVectorDirection*MoveSpeedStep;

            var scale = transform.localScale;

            if (nextStep.x > 0f)
            {
                scale.x = Mathf.Abs(transform.localScale.x);
            }
            else if (nextStep.x < 0f)
            {
                scale.x =  - Mathf.Abs(transform.localScale.x);
            }

            transform.localScale = scale;

            transform.Translate(nextStep, Space.World);
        }

        private void MiningDiamond()
        {
            if (mining)
            {
                return;
            }

            mining = true;

            var nextDiamond = DiamondsPoint.StealOneDiamond();

            if (nextDiamond != null)
            {
                DiamondChildScript = nextDiamond;

                StartCoroutine(StartMining());
            }
        }

        private void PickUpDiamond()
        {
            if (pickuping)
            {
                return;
            }

            pickuping = true;

            //StartCoroutine(StartMining());
            StartCoroutine(StartPickuping());
        }

        private IEnumerator StartPickuping()
        {
            yield return new WaitForSeconds(0.1f);
            
            MoveDelegate = Move;
        }

        public IEnumerator StartMining()
        {
            yield return new WaitForSeconds(MiningTime);

            MoveDelegate = Move;
        }


        public void SetMovePoints(Vector2[] pathPoints, bool repeatMoves)
        {
            this.repeatMoves = true;

            if (repeatMoves)
            {

                var pathPointsList = new List<Vector2>(pathPoints);

                for (int i = pathPoints.Length - 1; i >= 0; i--)
                {
                    pathPointsList.Add(pathPoints[i]);
                }

                this.PathPoints = pathPointsList.ToArray();
            }
            else
            {
                this.PathPoints = pathPoints;
            }

            instanciated = true;

            MoveDelegate = Move;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(AbstractTrapScript.TagString))
            {
                MoveDelegate = DyingMethod;
            }

            if (DiamondChildScript == null)
            {
                if (other.gameObject.CompareTag(DiamondScript.TagString))
                {
                    Debug.Log("if (other.gameObject.CompareTag(DiamondScript.TagString))");

                    DiamondsPoint = other.gameObject.GetComponent<DiamondScript>();

                    if (DiamondsPoint == null)
                    {
                        Debug.Log("other.gameObject.GetComponent<DiamondScript>() == null");
                    }

                    MoveDelegate = MiningDiamond;

                    
                }

                if (other.gameObject.CompareTag(DiamondSingleScript.TagString))
                {
                    var found_diamond = other.gameObject.GetComponent<DiamondSingleScript>();

                    if (found_diamond == null)
                    {
                        Debug.Log("found_diamond == null");

                        return;
                    }

                    if (found_diamond.IsCathedAlready)
                    {
                        return;
                    }

                    DiamondChildScript = found_diamond;

                    MoveDelegate = PickUpDiamond;
                }
            }
            
        }

        private void DyingMethod()
        {
            if (dying)
            {
                return;
            }

            this.dying = true;

            anim.Play("enemy_dying");

            if (DiamondChildScript != null)
            {
                DiamondChildScript.DropedFromMonster(this);
            }

            StartCoroutine(DyingCoroutine());
        }

        private IEnumerator DyingCoroutine()
        {
            yield return new WaitForSeconds(DyingTime);

            OnDestory();
        }

        private void OnDestory(bool finished = false)
        {
            var player_stats = PlayerLevelStats.FindPlayerStats();

            if (player_stats != null)
            {
                player_stats.EnemyKilled(this);
            }

            var hasDiamond = DiamondChildScript != null;

            if (finished)
            {
                var playerStatsGameObject = GameObject.FindGameObjectWithTag(PlayerLevelStats.TagString);

                var playerStats = playerStatsGameObject.GetComponent<PlayerLevelStats>();

                if (playerStats != null && hasDiamond) playerStats.MinersGotDiamond();
            }

            DiamondChildScript = null;

            Destroy(gameObject);
        }
    }
}
