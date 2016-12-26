using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AbstractTrapScript : MonoBehaviour
    {
        public int Number;

        public const string TagString = "trap";

        public void Start()
        {
            tag = TagString;
        }

        public bool IsCooldown
        {
            get { return cooldown; }
        }


        protected bool cooldown;

        public float CooldownTimer = 5f;

        public float TrapDuration = 1f;

        private void TrapReceiveSignal()
        {
            if (cooldown)
            {
                return;
            }

            cooldown = true;

            StartAction();
        }

        protected virtual void StartAction()
        {
            
        }

        protected virtual void EndAction()
        {


        }

        public virtual void ReceiveSignal(Button sender)
        {
            TrapReceiveSignal();

            sender.interactable = false;

            sender.gameObject.GetComponent<Image>().enabled = false;

            StartCoroutine(ButtonSenderCooldown(sender));
        }

        private IEnumerator ButtonSenderCooldown(Button sender)
        {
            yield return new WaitForSeconds(CooldownTimer);

            sender.interactable = true;

            sender.gameObject.GetComponent<Image>().enabled = true;
        }
    }
}
