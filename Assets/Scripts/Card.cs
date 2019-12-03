using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunt
{

    public class Card : MonoBehaviour
    {
        public GameObject cover, card;


        //Color m_color;

       // public Color CardColor { get => m_color; set { m_color = value; card.GetComponentInChildren<Renderer>().material.color = value; } }

        public int MatchId { get; set; }
        public Texture2D CardTexture { set { card.GetComponentInChildren<Renderer>().material.mainTexture = value; } }

        private bool isRotating = false;

        public void AnimateFlip(float animationTime) {
            if (isRotating) {
                return;
            }
            isRotating = true;
            StartCoroutine(AnimateCoroutine(animationTime));
        }

        private IEnumerator AnimateCoroutine(float animationTime) {
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
            isRotating = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

    }

}
