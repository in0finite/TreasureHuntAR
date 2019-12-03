using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TreasureHunt
{

    public class CardGenerator : MonoBehaviour
    {
        public GameObject cardPrefab;
        public int mapWidth = 3;
        public int mapHeight = 4;

        public float spaceBetween = 0.3f;

        public float flipDuration = 2;

        public float pauseInterval = 2f;

        List<GameObject> m_cards = new List<GameObject>();

        //public List<Color> colors = new List<Color>();
        List<int> m_matchIds = Enumerable.Range(0, 6).ToList();

        Card m_activeCard, m_secondCard;

        bool m_isSelectionAllowed = true;

        public List<Texture2D> cardTextures = new List<Texture2D>();



        // Start is called before the first frame update
        void Start()
        {
            // for (int i = 0; i < mapWidth * mapHeight / 2; i++)
            // {
            //     var color = Random.ColorHSV();
            //     colors.Add(color);
            //     colors.Add(color);
            // }

            m_matchIds.AddRange(m_matchIds.ToArray());
            Shuffle(m_matchIds);
            
            

            for (int i = 0; i < this.mapWidth; i++)
            {
                for (int j = 0; j < this.mapHeight; j++)
                {
                    var go = Instantiate(this.cardPrefab);
                    var card = go.GetComponent<Card>();
                    go.transform.position = new Vector3(i * (card.cover.transform.lossyScale.x + spaceBetween), 0, j * (card.cover.transform.lossyScale.y + spaceBetween));
                    card.MatchId = m_matchIds[i * mapHeight + j];
                    card.CardTexture = this.cardTextures[card.MatchId];
                    m_cards.Add(go);
                }
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    var card = hit.transform.GetComponentInParent<Card>();
                    if (card != null)
                    {
                        OnCardHit(card);
                    }
                }
            }

        }

        void OnCardHit(Card card)
        {
            if (! m_isSelectionAllowed)
                return;
            
            if (null == m_activeCard)
            {
                m_activeCard = card;
                FlipCard(m_activeCard);
            }
            else
            {
                if (card == m_activeCard)
                {
                    // same card
                    // return the card
                    m_activeCard = null;
                    FlipCard(card);
                }
                else
                {
                    // flip the second card
                    FlipCard(card);
                    
                    // check if they match
                    if (card.MatchId == m_activeCard.MatchId)
                    {
                        // they match
                        // destroy them
                        Debug.LogFormat("cards match");
                        m_secondCard = card;
                        m_isSelectionAllowed = false;
                        Invoke(nameof(DestroyCards), this.pauseInterval);
                    }
                    else
                    {
                        // the don't match
                        // return both cards
                        Debug.LogFormat("cards dont match, id1 {0}, id2 {1}", m_activeCard.MatchId, card.MatchId);
                        m_secondCard = card;
                        m_isSelectionAllowed = false;
                        Invoke(nameof(ReturnCards), this.pauseInterval);
                    }

                }
            }
            
        }

        void FlipCard(Card card)
        {
            // Vector3 eulers = card.transform.eulerAngles;
            // eulers.x -= 180;
            // card.transform.eulerAngles = eulers;
            card.AnimateFlip(this.flipDuration);
        }

        void DestroyCards()
        {
            m_isSelectionAllowed = true;
            Destroy(m_activeCard.gameObject);
            Destroy(m_secondCard.gameObject);
        }

        void ReturnCards()
        {
            m_isSelectionAllowed = true;
            FlipCard(m_activeCard);
            FlipCard(m_secondCard);
            m_activeCard = null;
            m_secondCard = null;
        }


        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

    }

}