using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerLevelStats : MonoBehaviour
    {
        public const string TagString = "player_level_stats";

        public LayerMask DiamondsLayerMask;

        public Text ScoreText;

        public Text DiamondsLeftText;

        public Text TimeText;

        private int kills_score
        {
            get { return _killsScore; }
            set
            {
                _killsScore = value;
                DisplayKillsScore();
            }
        }

        

        private int diamonds_left_score
        {
            get { return _diamondsLeftScore; }
            set
            {
                if (value == 0)
                {
                    LoseGame();
                    
                    return;
                }

                _diamondsLeftScore = value;

                DisplayDiamondsLeft();
            }
        }

        private void LoseGame()
        {

            PlayerLevelResults.Kills = kills_score;

            PlayerLevelResults.Time = GetTimeString();

            SceneManager.LoadScene("results_scene");
        }


        private int _killsScore;
        private int _diamondsLeftScore;

        private int diamonds_total;

        private float time_played;

        // Use this for initialization
        void Start ()
        {
            tag = TagString;

            kills_score = 0;

            var diamonds = GameObject.FindObjectsOfType<DiamondSingleScript>();

            if (diamonds == null || diamonds.Length == 0)
            {
                Debug.LogError("diamonds == null || diamonds.Length == 0");
            }

            diamonds_total = diamonds.Length;

            diamonds_left_score = diamonds_total;

            time_played = 0f;
        }
	
        // Update is called once per frame
        void Update ()
        {
            #region Нажатие на даймонды

            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                var hitsAll = Physics2D.BoxCastAll((Vector2) mousePos, new Vector2(0.5f, 0.5f), 0f, Vector2.down, 1f,
                    DiamondsLayerMask);

                if (hitsAll == null || hitsAll.Any() == false)
                {
                    return;
                }

                foreach (var hit in hitsAll)
                {
                    if (hit.collider == null)
                    {
                        return;
                    }

                    var diamondGameObject = hit.collider.gameObject;

                    if (diamondGameObject.tag == DiamondSingleScript.TagString)
                    {
                        var diamondSingleScript = diamondGameObject.GetComponent<DiamondSingleScript>();

                        if (diamondSingleScript != null)
                        {
                            diamondSingleScript.PlayerClickedOn();
                        }
                    }
                }
            }

            #endregion

            TimeText.text = GetTimeString();
        }

        

        void FixedUpdate()
        {
            time_played += Time.fixedDeltaTime;
        }

        public void EnemyKilled(EnemyMovesScripts enemy)
        {
            kills_score++;
        }

        private void DisplayKillsScore()
        {
            ScoreText.text = kills_score.ToString();
        }

        private void DisplayDiamondsLeft()
        {
            DiamondsLeftText.text = string.Format("{0} / {1}", diamonds_left_score, diamonds_total);

            DiamondsLeftText.color = CalculateDiamondsLeftTextboxColor();
        }

        private Color CalculateDiamondsLeftTextboxColor()
        {
            var persent = (float)diamonds_left_score/diamonds_total;

            if (persent <= 0.1f)
            {
                return Color.red;
            }

            if ((persent > 0.1f) && (persent < 0.3f))
            {
                //FF0044FF
                return new Color(1, 0, 0x44 / 0xFF);
            }

            if ((persent >= 0.3f) && (persent < 0.6f))
            {
                //0060FFFF   
                return new Color(0, 0x60 / 0xFF, 1);
            }

            if ((persent >= 0.6f) && (persent < 0.8f))
            {
                //00FF36FF
                return new Color(0, 1, 0x36 / 0xFF);
            }

            //00FFBFFF
            return new Color(0, 1, 0xFB/ 0xFF);
        }

        public void MinersGotDiamond()
        {
            Debug.Log("MinersGot");

            diamonds_left_score--;
        }

        private string GetTimeString()
        {
            var time = (int)time_played;

            var milisec = (int)(((float) time_played - (float)time) * 1000f);
            var milisec_string = string.Empty;

            if (milisec < 10)
            {
                milisec_string = "00" + milisec.ToString();
            }
            else if (milisec < 100)
            {
                milisec_string = "0" + milisec.ToString();
            }
            else
            {
                milisec_string = milisec.ToString();
            }



            var sec = time%60;

            time -= sec;

            var mins = time/60;

            return string.Format("{0}:{1}.{2}", mins, sec, milisec_string);
        }

        public static PlayerLevelStats FindPlayerStats()
        {
            var player_stats_gameObject = GameObject.FindGameObjectWithTag(PlayerLevelStats.TagString);
            if (player_stats_gameObject != null)
            {
                var player_stats = player_stats_gameObject.GetComponent<PlayerLevelStats>();
                if (player_stats != null)
                {
                    return player_stats;
                }
                else
                {
                    Debug.LogError("player_stats == null");

                    return null;
                }
            }
            else
            {
                Debug.LogError("player_stats_gameObject == null");
                return null;
            }
        }
    }

    public static class PlayerLevelResults
    {
        public static int Kills;

        public static string Time = "0 : 0.000";
    }
}
