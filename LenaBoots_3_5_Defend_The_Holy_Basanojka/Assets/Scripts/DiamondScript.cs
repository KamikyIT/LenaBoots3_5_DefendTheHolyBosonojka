using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class DiamondScript : MonoBehaviour {
        public const string TagString = "diamond";

        private List<GameObject> DiamondInstances;

        // Use this for initialization
        void Start ()
        {
            gameObject.tag = TagString;

            DiamondInstances = new List<GameObject>();

            foreach (Transform child in transform)
            {
                DiamondInstances.Add(child.gameObject);
            }
        }
	
        // Update is called once per frame
        void Update ()
        {

	
        }

        public DiamondSingleScript StealOneDiamond()
        {
            if (DiamondInstances.Any())
            {
                var nextDiamond = DiamondInstances.FirstOrDefault();

                if (nextDiamond == null)
                {
                    return null;
                }

                var diamondScript = nextDiamond.GetComponent<DiamondSingleScript>();

                DiamondInstances.Remove(nextDiamond);

                return diamondScript;
            }

            return null;
        }

        public void DiamondComesBack(GameObject diamondChild)
        {
            DiamondInstances.Add(diamondChild);
        }
    }
}
