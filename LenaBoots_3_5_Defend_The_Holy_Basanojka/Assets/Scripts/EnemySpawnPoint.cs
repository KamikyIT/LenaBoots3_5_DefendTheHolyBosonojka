using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        private int enemy_spawned_count;

        public EnemyMovesScripts EnemyPrefab;

        public bool RepeatMoves;


        public List<GameObject> PathPointsGameObjects;
        private Vector2[] PathPoints;

        //public List<SpawnTimes> SpawnTimes;

        public float Timer;
        private bool cooldown;

        // Use this for initialization
        void Start ()
        {
            enemy_spawned_count = 0;

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

            StartCoroutine(SpawnEnemy(Timer));
        }

        private IEnumerator SpawnEnemy(float cooldown_time)
        {
            yield return new WaitForSeconds(cooldown_time);

            enemy_spawned_count++;

            var enemy = (GameObject) GameObject.Instantiate(EnemyPrefab.gameObject, transform.position, Quaternion.identity);

            enemy.name = "[" + enemy_spawned_count + "] " + EnemyPrefab.gameObject.name;

            var enemyMoves = enemy.GetComponent<EnemyMovesScripts>();

            enemyMoves.SetMovePoints(PathPoints, RepeatMoves);

            this.cooldown = false;

        }
    }
}
