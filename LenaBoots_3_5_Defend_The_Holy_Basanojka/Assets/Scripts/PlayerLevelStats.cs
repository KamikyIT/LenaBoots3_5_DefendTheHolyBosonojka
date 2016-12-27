using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerLevelStats : MonoBehaviour
    {
        public const string TagString = "player_level_stats";

        public LayerMask DiamondsLayerMask;

        private Camera mainCamera;


        // Use this for initialization
        void Start ()
        {
            tag = TagString;

            mainCamera = Camera.main;
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, DiamondsLayerMask);

                if (hit == null || hit.collider == null)
                {
                    return;
                }

                var diamondGameObject = hit.collider.gameObject;

                if (diamondGameObject.tag ==  DiamondSingleScript.TagString)
                {
                    var diamondSingleScript = diamondGameObject.GetComponent<DiamondSingleScript>();

                    if (diamondSingleScript != null)
                    {
                        diamondSingleScript.PlayerClickedOn();
                    }
                }
            }

        }

        public void MinersGotDiamond()
        {
            Debug.Log("MinersGot");
            
        }
    }
}
