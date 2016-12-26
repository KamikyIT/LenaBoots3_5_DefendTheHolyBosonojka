using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PikaTrapScript : AbstractTrapScript
    {

        private BoxCollider2D boxCollider2D;

        private SpriteRenderer spriteChild;
        // Use this for initialization
        void Start ()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();

            spriteChild = GetComponentInChildren<SpriteRenderer>();

            spriteChild.enabled = false;

            boxCollider2D.enabled = false;
        }

        // Update is called once per frame
        void Update () {
	
        }

        protected override void StartAction()
        {
            this.cooldown = true;

            boxCollider2D.enabled = true;

            spriteChild.enabled = true;

            StartCoroutine(ActionCoroutine());

            StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator ActionCoroutine()
        {
            yield return new WaitForSeconds(TrapDuration);

            boxCollider2D.enabled = false;

            spriteChild.enabled = false;
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(CooldownTimer);

            EndAction();
        }

        protected override void EndAction()
        {
            cooldown = false;
        }
    }
}
