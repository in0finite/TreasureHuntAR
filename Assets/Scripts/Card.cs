using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunt
{

    public class Card : MonoBehaviour
    {
        public GameObject cover, card;

        public int MatchId { get; set; }
        public Texture2D CardTexture { get; set; }

        public bool IsRotating { get; set; } = false;



        public void AnimateFlip(float animationTime) {
            if (IsRotating) {
                return;
            }
            IsRotating = true;
            StartCoroutine(AnimateCoroutine(animationTime));
        }

        private IEnumerator AnimateCoroutine(float animationTime) {
            
            AssignTexture(this.CardTexture);

            float currentTime = Time.time;
            Vector3 currentAngles = this.transform.eulerAngles;
            float finalAngleX = currentAngles.x + 180;
            while (currentAngles.x < finalAngleX) {
                currentAngles.x += Time.deltaTime * 180 / animationTime;
                this.transform.eulerAngles = currentAngles;
                yield return null;
            } 
            currentAngles.x = finalAngleX;
            this.transform.eulerAngles = currentAngles;

            IsRotating = false;

            bool isVisible = this.transform.forward.y > 0;
            if (isVisible)
                AssignTexture(this.CardTexture);
            else
                AssignTexture(null);
            
        }

        void AssignTexture(Texture2D tex)
        {
            this.card.GetComponentInChildren<Renderer>().material.mainTexture = tex;
        }

        // Start is called before the first frame update
        void Start()
        {
            AssignTexture(null);
        }

    }

}
