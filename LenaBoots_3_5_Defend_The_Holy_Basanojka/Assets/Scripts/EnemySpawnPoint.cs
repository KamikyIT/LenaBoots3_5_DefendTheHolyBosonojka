using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public EnemyMovesScripts EnemyPrefab;

        public List<GameObject> PathPointsGameObjects;
        private Vector2[] PathPoints;

        //public List<SpawnTimes> SpawnTimes;

        public float Timer;
        private bool cooldown;

        // Use this for initialization
        void Start ()
        {
            var newPathPoints = new List<Vector2>();

            PathPointsGameObjects.ForEach(p =>
            {
                newPathPoints.Add((Vector2) p.transform.position);
            }
            );

            PathPoints = newPathPoints.ToArray();
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (cooldown)
            {
                return;
            }

            cooldown = true;

            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            yield return new WaitForSeconds(Timer);

            var enemy = (GameObject) GameObject.Instantiate(EnemyPrefab.gameObject, transform.position, Quaternion.identity);

            

            var enemyMoves = enemy.GetComponent<EnemyMovesScripts>();

            enemyMoves.SetMovePoints(PathPoints);



            this.cooldown = false;

        }
    }

    //[Serializable]
    //public class SpawnTimes
    //{
    //    public float Time;

    //    public int Count;
    //}
}
