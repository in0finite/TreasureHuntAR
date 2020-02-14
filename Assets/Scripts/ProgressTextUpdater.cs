using System.Collections;
using System.Collections.Generic;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace TreasureHunt
{
    public class ProgressTextUpdater : MonoBehaviour
    {
        public Text progressText;
        public int numLogMessagesToDisplay = 10;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            string logs = string.Join("\n", Logger.logMessages.Take(this.numLogMessagesToDisplay));

            float? progress = TreasureHuntManager.Singleton.CloudManager?.SessionStatus?.RecommendedForCreateProgress;

            this.progressText.text = "Progress: " + (progress * 100) + "%\n\n" + logs;

        }

    }

}
