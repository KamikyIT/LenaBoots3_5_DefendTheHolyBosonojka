using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class TrapsGuiClickEvents : MonoBehaviour
    {

        public List<PikaTrapScript> Pikas;

        // Use this for initialization
        void Start ()
        {
            Pikas = new List<PikaTrapScript>();

            var pikas = GameObject.FindObjectsOfType<PikaTrapScript>();

            if (pikas != null && pikas.Any())
            {
                Pikas.AddRange(pikas);
            }

        }
	
        // Update is called once per frame
        void Update ()
        {
            var btnGo = GameObject.Find("Button Pika 1");

            

            var pika = Pikas.FirstOrDefault();

            if (pika == null)
            {
                
                return;
            }

            var btn = btnGo.GetComponent<Button>();

            

            btnGo.GetComponent<Button>().enabled = pika.IsCooldown == false;
        }

        public void PikaButtonClick(int pika_number)
        {
            var pika = Pikas.FirstOrDefault(x => x.Number == pika_number);

            if (pika == null)
            {
                Debug.Log("pika == null : pika_number = " + pika_number);

                return;
            }

            var sender_name = EventSystem.current.currentSelectedGameObject.name;

            var sender = GameObject.Find(sender_name);

            if (sender == null)
            {
                Debug.Log("sender == null : sender_name = " + sender_name);
            }

            var sender_btn = sender.GetComponent<Button>();
            

            pika.ReceiveSignal(sender_btn);
        }
    }
}
