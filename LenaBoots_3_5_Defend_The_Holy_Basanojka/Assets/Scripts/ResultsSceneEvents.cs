using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ResultsSceneEvents : MonoBehaviour
    {
        public Text KillsText;

        public Text TimeText;


        // Use this for initialization
        void Start ()
        {

            KillsText.text = "KILLS : " + PlayerLevelResults.Kills.ToString();

            TimeText.text = "Survived Time : " + PlayerLevelResults.Time.ToString();

        }
	
        // Update is called once per frame
        void Update ()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene("level_1");
            }
	
        }
    }
}
