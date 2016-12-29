using UnityEngine;

namespace Assets.Scripts
{
    public class DiamondSingleScript : MonoBehaviour
    {
        public const string TagString = "diamond_single";

        private Vector3 start_position;

        private DiamondScript DiamondParent;

        [SerializeField()]
        private DiamondStateEnum diamondStateEnum;

        // Use this for initialization
        void Start ()
        {
            tag = TagString;

            start_position = transform.position;

            diamondStateEnum = DiamondStateEnum.InDiamondPyramid;

            DiamondParent = transform.parent.gameObject.GetComponent<DiamondScript>();
        }
	
        // Update is called once per frame
        void Update ()
        {


	
        }

        public void DropedFromMonster(EnemyMovesScripts enemy)
        {
            Debug.Log(name + " CathedByMonster " + enemy.name);

            transform.SetParent(null, true);

            var collider = GetComponent<CircleCollider2D>();

            collider.enabled = true;

            diamondStateEnum = DiamondStateEnum.Droped;
        }


        public void PlayerClickedOn()
        {
            if (diamondStateEnum == DiamondStateEnum.Droped)
            {
                transform.position = start_position;
                
                DiamondParent.DiamondComesBack(gameObject);

                diamondStateEnum = DiamondStateEnum.InDiamondPyramid;
            }
        }

        public void CathedByMonster(EnemyMovesScripts enemy)
        {
            Debug.Log(name +  " CathedByMonster " + enemy.name );

            var collider = GetComponent<CircleCollider2D>();

            collider.enabled = false;

            diamondStateEnum = DiamondStateEnum.InMonster;
        }

        public bool IsCathedAlready
        {
            get
            {
                return diamondStateEnum == DiamondStateEnum.InMonster;
            }
        } 
    }

    public enum DiamondStateEnum
    {
        InDiamondPyramid = 1,
        InMonster = 2 ,
        Droped = 4,
    }
}
