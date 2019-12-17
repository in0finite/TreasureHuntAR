using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace TreasureHunt
{

    public class TreasureHuntManager : InputInteractionBase
    {
        public SpatialAnchorManager CloudManager = null;

        private string currentAnchorId = "";

        public GameObject AnchoredObjectPrefab;

        protected CloudSpatialAnchor currentCloudAnchor;

        protected GameObject spawnedObject;
        private Material spawnedObjectMat;
        protected AnchorLocateCriteria anchorLocateCriteria;

        protected CloudSpatialAnchorWatcher currentWatcher;

        // Start is called before the first frame update
        async new void Start()
        {
            
            base.Start();

            Debug.LogFormat("Creating session");

            if (CloudManager.Session == null)
            {
                await CloudManager.CreateSessionAsync();
            }

            Debug.LogFormat("Session created");

            currentAnchorId = "";
            currentCloudAnchor = null;

            anchorLocateCriteria = new AnchorLocateCriteria();

            anchorLocateCriteria.Identifiers = new string[0];

            await CloudManager.StartSessionAsync();

            Debug.LogFormat("Started session");

            currentWatcher = CreateWatcher();

            Debug.LogFormat("Created watcher");

            CloudManager.AnchorLocated += CloudManager_AnchorLocated;

        }

        private void CloudManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
        {
            Debug.LogFormat("Anchor recognized as a possible anchor {0} {1}", args.Identifier, args.Status);
            if (args.Status == LocateAnchorStatus.Located)
            {
                OnCloudAnchorLocated(args);
            }
        }

        protected void OnCloudAnchorLocated(AnchorLocatedEventArgs args)
        {

            if (args.Status == LocateAnchorStatus.Located)
            {
                currentCloudAnchor = args.Anchor;

                UnityDispatcher.InvokeOnAppThread(() =>
                {
                    Pose anchorPose = Pose.identity;

#if UNITY_ANDROID || UNITY_IOS
                    anchorPose = currentCloudAnchor.GetPose();
#endif
                    // HoloLens: The position will be set based on the unityARUserAnchor that was located.
                    SpawnOrMoveCurrentAnchoredObject(anchorPose.position, anchorPose.rotation);
                });
            }
        }

        protected virtual void SpawnOrMoveCurrentAnchoredObject(Vector3 worldPos, Quaternion worldRot)
        {
            // Create the object if we need to, and attach the platform appropriate
            // Anchor behavior to the spawned object
            if (spawnedObject == null)
            {
                // Use factory method to create
                spawnedObject = SpawnNewAnchoredObject(worldPos, worldRot, currentCloudAnchor);

                // Update color
                spawnedObjectMat = spawnedObject.GetComponent<MeshRenderer>().material;
            }
            else
            {
                // Use factory method to move
                MoveAnchoredObject(spawnedObject, worldPos, worldRot, currentCloudAnchor);
            }
        }

        protected virtual void MoveAnchoredObject(GameObject objectToMove, Vector3 worldPos, Quaternion worldRot, CloudSpatialAnchor cloudSpatialAnchor = null)
        {
            // Get the cloud-native anchor behavior
            CloudNativeAnchor cna = spawnedObject.GetComponent<CloudNativeAnchor>();

            // Warn and exit if the behavior is missing
            if (cna == null)
            {
                Debug.LogWarning($"The object {objectToMove.name} is missing the {nameof(CloudNativeAnchor)} behavior.");
                return;
            }

            // Is there a cloud anchor to apply
            if (cloudSpatialAnchor != null)
            {
                // Yes. Apply the cloud anchor, which also sets the pose.
                cna.CloudToNative(cloudSpatialAnchor);
            }
            else
            {
                // No. Just set the pose.
                cna.SetPose(worldPos, worldRot);
            }
        }

        protected virtual GameObject SpawnNewAnchoredObject(Vector3 worldPos, Quaternion worldRot, CloudSpatialAnchor cloudSpatialAnchor)
        {
            // Create the object like usual
            GameObject newGameObject = SpawnNewAnchoredObject(worldPos, worldRot);

            // If a cloud anchor is passed, apply it to the native anchor
            if (cloudSpatialAnchor != null)
            {
                CloudNativeAnchor cloudNativeAnchor = newGameObject.GetComponent<CloudNativeAnchor>();
                cloudNativeAnchor.CloudToNative(cloudSpatialAnchor);
            }

            // Set color
            newGameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

            // Return newly created object
            return newGameObject;
        }

        protected virtual GameObject SpawnNewAnchoredObject(Vector3 worldPos, Quaternion worldRot)
        {
            // Create the prefab
            GameObject newGameObject = GameObject.Instantiate(AnchoredObjectPrefab, worldPos, worldRot);

            // Attach a cloud-native anchor behavior to help keep cloud
            // and native anchors in sync.
            newGameObject.AddComponent<CloudNativeAnchor>();

            // Set the color
            newGameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

            // Return created object
            return newGameObject;
        }

        protected CloudSpatialAnchorWatcher CreateWatcher()
        {
            if ((CloudManager != null) && (CloudManager.Session != null))
            {
                return CloudManager.Session.CreateWatcher(anchorLocateCriteria);
            }
            else
            {
                return null;
            }
        }



        protected override void OnSelectObjectInteraction(Vector3 hitPoint, object target)
        {
            if (IsPlacingObject())
            {
                Quaternion rotation = Quaternion.AngleAxis(0, Vector3.up);

                SpawnOrMoveCurrentAnchoredObject(hitPoint, rotation);

                Debug.LogFormat("saving to cloud");

                SaveCurrentObjectAnchorToCloudAsync().Wait();
            }
        }

        public bool IsPlacingObject()
        {
            return true;
        }


        protected virtual async Task SaveCurrentObjectAnchorToCloudAsync()
        {
            // Get the cloud-native anchor behavior
            CloudNativeAnchor cna = spawnedObject.GetComponent<CloudNativeAnchor>();

            // If the cloud portion of the anchor hasn't been created yet, create it
            if (cna.CloudAnchor == null) { cna.NativeToCloud(); }

            // Get the cloud portion of the anchor
            CloudSpatialAnchor cloudAnchor = cna.CloudAnchor;

            // In this sample app we delete the cloud anchor explicitly, but here we show how to set an anchor to expire automatically
            // cloudAnchor.Expiration = DateTimeOffset.Now.AddDays(7);

            while (!CloudManager.IsReadyForCreate)
            {
                await Task.Delay(330);
                float createProgress = CloudManager.SessionStatus.RecommendedForCreateProgress;
                //feedbackBox.text = $"Move your device to capture more environment data: {createProgress:0%}";
            }

            bool success = false;

            //feedbackBox.text = "Saving...";

            try
            {
                // Actually save
                await CloudManager.CreateAnchorAsync(cloudAnchor);

                // Store
                currentCloudAnchor = cloudAnchor;

                // Success?
                success = currentCloudAnchor != null;

                if (success)
                {
                    // Await override, which may perform additional tasks
                    // such as storing the key in the AnchorExchanger
                //    await OnSaveCloudAnchorSuccessfulAsync();
                }
                else
                {
                //    OnSaveCloudAnchorFailed(new Exception("Failed to save, but no exception was thrown."));
                    Debug.LogError("Failed to save, but no exception was thrown.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }


    }

}
