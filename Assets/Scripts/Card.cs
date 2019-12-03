using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreasureHunt
{

    public class Card : MonoBehaviour
    {
        public GameObject cover, card;

        Color m_color;

        public Color CardColor { get => m_color; set { m_color = value; card.GetComponentInChildren<Renderer>().material.color = value; } }


        // Start is called before the first frame update
        void Start()
        {
            
        }

    }

}
