using System.Collections;
using System.Collections.Generic;
using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace TreasureHunt
{
    public class ProgressTextUpdater : MonoBehaviour
    {
        public Text progressText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float? progress = TreasureHuntManager.Singleton.CloudManager?.SessionStatus?.RecommendedForCreateProgress;
            this.progressText.text = "Progress: " + progress;
        }

    }

}
