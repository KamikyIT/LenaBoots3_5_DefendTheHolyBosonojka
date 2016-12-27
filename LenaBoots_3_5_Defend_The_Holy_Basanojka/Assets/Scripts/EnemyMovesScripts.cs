using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovesScripts : MonoBehaviour
    {
        public float MoveSpeed;

        public float MiningTime;

        public float DyingTime;

        private float MoveSpeedStep;

        private Vector2[] PathPoints;

        private bool instanciated;

        private bool mining;

        private Action MoveDelegate;

        private DiamondSingleScript DiamondChildScript;

        private DiamondScript DiamondsPoint;

        private int next_index;
        private bool dying;
        private Animator _anim;

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

                if (next_index + 1 == PathPoints.Length)
                {
                    OnDestory(true);
                }

                next_index = (next_index + 1) % PathPoints.Length;

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

                DiamondChildScript.gameObject.transform.SetParent(transform);

                DiamondChildScript.gameObject.transform.localPosition = new Vector3(0.2f, 0f);

                DiamondChildScript.CathedByMonster();

                StartCoroutine(StartMining());
            }
        }

        public IEnumerator StartMining()
        {
            yield return new WaitForSeconds(MiningTime);



            MoveDelegate = Move;
        }

        public void SetMovePoints(Vector2[] pathPoints)
        {
            this.PathPoints = pathPoints;

            instanciated = true;

            MoveDelegate = Move;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(DiamondScript.TagString))
            {
                MoveDelegate = MiningDiamond;

                DiamondsPoint = other.gameObject.GetComponent<DiamondScript>();

                if (DiamondsPoint == null)
                {
                    Debug.Log("other.gameObject.GetComponent<DiamondScript>() == null");
                }
            }

            if (other.gameObject.CompareTag(AbstractTrapScript.TagString))
            {
                MoveDelegate = DyingMethod;
            }
            
        }

        private void DyingMethod()
        {
            this.dying = true;

            //anim.SetTrigger("dying");

            anim.Play("enemy_dying");

            StartCoroutine(DyingCoroutine());
        }

        private IEnumerator DyingCoroutine()
        {
            yield return new WaitForSeconds(DyingTime);

            OnDestory();
        }

        private void OnDestory(bool finished = false)
        {
            var hasDiamond = DiamondChildScript != null;

            if (finished)
            {
                var playerStatsGameObject = GameObject.FindGameObjectWithTag(PlayerLevelStats.TagString);

                var playerStats = playerStatsGameObject.GetComponent<PlayerLevelStats>();

                if (playerStats != null && hasDiamond) playerStats.MinersGotDiamond();
            }
            else
            {
                if (hasDiamond)
                {
                    DiamondChildScript.DropedFromMonster();
                }
            }

            Destroy(gameObject);
        }
    }
}
