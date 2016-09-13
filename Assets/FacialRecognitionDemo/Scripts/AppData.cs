/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using IBM.Watson.DeveloperCloud.Connection;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class contains all application data.
	/// </summary>
	public class AppData
	{
        #region Private Data
        private string m_VisualRecognitionServiceID = "VisualRecognitionV3";
        private string m_VisualRecognitionServiceURL = "https://gateway-a.watsonplatform.net/visual-recognition/api";
        private string m_VisualRecognitionServiceNote = "This ApiKey was added at runtime.";
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// AppData Constructor
		/// </summary>
		public AppData()
		{
			ClassifierIDs.OnAdded += OnClassifierIDAdded;
			ClassifierIDs.OnRemoved += OnClassifierIDRemoved;
			ClassifiersVerbose.OnAdded += OnClassiferVerboseAdded;
			ClassifiersVerbose.OnRemoved += OnClassifierVerboseRemoved;
			ClassifierIDsToClassifyWith.OnAdded += OnClassifierIDsToClassifyWithAdded;
			ClassifierIDsToClassifyWith.OnRemoved += OnClassifierIDsToClassifyWithRemoved;
			Endpoints.OnAdded += OnEndpointAdded;
			Endpoints.OnRemoved += OnEndpointRemoved;

            if (VisualRecognition == null)
                VisualRecognition = new VisualRecognition();
		}

		~AppData()
		{
			ClassifierIDs.OnAdded -= OnClassifierIDAdded;
			ClassifierIDs.OnRemoved -= OnClassifierIDRemoved;
			ClassifiersVerbose.OnAdded -= OnClassiferVerboseAdded;
			ClassifiersVerbose.OnRemoved -= OnClassifierVerboseRemoved;
			ClassifierIDsToClassifyWith.OnAdded -= OnClassifierIDsToClassifyWithAdded;
			ClassifierIDsToClassifyWith.OnRemoved -= OnClassifierIDsToClassifyWithRemoved;
			Endpoints.OnAdded -= OnEndpointAdded;
			Endpoints.OnRemoved -= OnEndpointRemoved;
		}
		#endregion
		
		#region Instance
		/// <summary>
		/// Returns the singleton instance of AppData.
		/// </summary>
		public static AppData Instance { get { return Singleton<AppData>.Instance; } }
        #endregion

        #region Visual Recognition
        /// <summary>
        /// Visual Recognition instance.
        /// </summary>
        public VisualRecognition VisualRecognition 
        {
            get { return m_VisualRecognition; }
            set { m_VisualRecognition = value; }
        }
        private VisualRecognition m_VisualRecognition = null;
        #endregion

        #region Views
		/// <summary>
		/// A list of all views in the application. This is used for showing and hiding views depending on AppState.
		/// </summary>
        public List<View> Views = new List<View>();
		#endregion

		#region Application State
		/// <summary>
		/// The current state of the application. This shows and hides Views depending on their ViewStates.
		/// </summary>
		public int AppState
		{
			get { return m_AppState; }
			set
			{
				m_AppState = value;
				EventManager.Instance.SendEvent(Event.ON_UPDATE_APP_STATE);
			}
		}
		private int m_AppState = 0;
		#endregion

		#region Visual Recognition Classes
		/// <summary>
		/// List of Visual Recognition classes.
		/// </summary>
		public ObservedList<VRClass> VRClasses { get; set; }

		/// <summary>
		/// One Visual Recognition class.
		/// </summary>
		public class VRClass
		{
			/// <summary>
			/// Visual Recognition classname.
			/// </summary>
			public string className { get; set; }
			/// <summary>
			/// Visual Recognition Classifier Identifier.
			/// </summary>
			public string classifierID { get; set; }
			/// <summary>
			/// List of images as byteArrays.
			/// </summary>
			public ObservedList<byte[]> images = new ObservedList<byte[]>();

			/// <summary>
			/// VRClass Constructor.
			/// </summary>
			public VRClass()
			{
				images.OnAdded += OnImageAdded;
				images.OnRemoved += OnImageRemoved;
			}

			~VRClass()
			{
				images.OnAdded -= OnImageAdded;
				images.OnRemoved -= OnImageRemoved;
			}

			private void OnImageAdded(byte[] image)
			{
				EventManager.Instance.SendEvent(Event.ON_IMAGE_ADDED, image);
			}

			private void OnImageRemoved(byte[] image)
			{
				EventManager.Instance.SendEvent(Event.ON_IMAGE_REMOVED, image);
			}
		}
		#endregion

        #region Classifier IDs
        /// <summary>
        /// List of Classifier IDs
        /// </summary>
        public ObservedList<string> ClassifierIDs = new ObservedList<string>();

		private void OnClassifierIDAdded(string classifierID)
		{
			EventManager.Instance.SendEvent(Event.ON_CLASSIFIER_ADDED, classifierID);
		}

		private void OnClassifierIDRemoved(string classifierID)
		{
			EventManager.Instance.SendEvent(Event.ON_CLASSIFIER_ADDED, classifierID);
		}
		#endregion

		#region ClassifiersBrief
		/// <summary>
		/// An object containing references to all of the classifiers with their names and identifiers.
		/// </summary>
		public GetClassifiersTopLevelBrief ClassifiersBrief
		{
			get { return m_ClassifiersBrief; }
			set
			{
				m_ClassifiersBrief = value;

				foreach (GetClassifiersPerClassifierBrief classifier in ClassifiersBrief.classifiers)
				{
					ClassifierIDs.Add(classifier.classifier_id);
				}

				EventManager.Instance.SendEvent(Event.ON_CLASSIFIERS_UPDATED);
			}
		}
		private GetClassifiersTopLevelBrief m_ClassifiersBrief;
		#endregion

		#region ClassifiersVerbose
		/// <summary>
		/// List of Classifiers and their classes.
		/// </summary>
		public ObservedList<GetClassifiersPerClassifierVerbose> ClassifiersVerbose = new ObservedList<GetClassifiersPerClassifierVerbose>();

		private void OnClassiferVerboseAdded(GetClassifiersPerClassifierVerbose classifierVerbose)
		{
			EventManager.Instance.SendEvent(Event.ON_CLASSIFIER_VERBOSE_ADDED, classifierVerbose);
		}

		private void OnClassifierVerboseRemoved(GetClassifiersPerClassifierVerbose classifierVerbose)
		{
			EventManager.Instance.SendEvent(Event.ON_CLASSIFIER_VERBOSE_REMOVED, classifierVerbose);
		}

		/// <summary>
		/// List of ClassifierIDs to use for image classification.
		/// </summary>
		public ObservedList<string> ClassifierIDsToClassifyWith = new ObservedList<string>();

		private void OnClassifierIDsToClassifyWithAdded(string classifierID)
		{
			EventManager.Instance.SendEvent(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_ADDED, classifierID);
		}

		private void OnClassifierIDsToClassifyWithRemoved(string classifierID)
		{
			EventManager.Instance.SendEvent(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_REMOVED, classifierID);
		}
		#endregion

		#region APIKey
		/// <summary>
		/// The Visual Recognition APIKey
		/// </summary>
		public string APIKey
        {
            get { return m_APIKey; }
            set
            {
                m_APIKey = value;
                if (SetAPIKey(APIKey))
                {
                    VisualRecognition.ClearApiKey();
                    EventManager.Instance.SendEvent(Event.ON_API_KEY_UPDATED, APIKey);
                }
                else
                    Log.Warning("AppData", "Visual Recognition API Keys were not updated!");
            }
        }
        private string m_APIKey;

        private bool SetAPIKey(string apiKey)
        {
            Config.CredentialInfo visualRecognitionCredentials = new Config.CredentialInfo();

            visualRecognitionCredentials.m_ServiceID = m_VisualRecognitionServiceID;
            visualRecognitionCredentials.m_Apikey = apiKey;
            visualRecognitionCredentials.m_URL = m_VisualRecognitionServiceURL;
            visualRecognitionCredentials.m_Note = m_VisualRecognitionServiceNote;

            for (int i = 0; i < Config.Instance.Credentials.Count; i++)
            {
                if (Config.Instance.Credentials[i].m_ServiceID == visualRecognitionCredentials.m_ServiceID)
                {
                    if (Config.Instance.Credentials[i].m_Apikey != visualRecognitionCredentials.m_Apikey)
                    {
                        Log.Debug("VisualRecognitionUtilities", "Deleting existing visual recognition APIKEY");
                        Config.Instance.Credentials.RemoveAt(i);
                    }
                    else
                    {
                        Log.Debug("VisualRecognitionUtilities", "API Key matches - not replacing!");
                        return false;
                    }
                }
            }

            Log.Debug("VisualRecognitionUtilities", "Adding visual recognition APIKEY | serviceID: {0} | APIKey: {1} | URL: {2} | Note: {3}", visualRecognitionCredentials.m_ServiceID, visualRecognitionCredentials.m_Apikey, visualRecognitionCredentials.m_URL, visualRecognitionCredentials.m_Note);

            Config.Instance.Credentials.Add(visualRecognitionCredentials);

            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);
            File.WriteAllText(Application.streamingAssetsPath + "/Config.json", Config.Instance.SaveConfig());
            IsAPIKeyValid = false;
            RESTConnector.FlushConnectors();

            return true;
        }

        /// <summary>
        /// Is the current APIKey Valid.
        /// </summary>
		public bool IsAPIKeyValid
		{
			get { return m_IsAPIKeyValid; }
			set
			{
				m_IsAPIKeyValid = value;
				if (IsAPIKeyValid)
					EventManager.Instance.SendEvent(Event.ON_API_KEY_VALIDATED);
				else
					EventManager.Instance.SendEvent(Event.ON_API_KEY_INVALIDATED);
			}
		}
        private bool m_IsAPIKeyValid = false;

        /// <summary>
        /// Are we checking validity of API Key.
        /// </summary>
		private bool m_IsCheckingAPIKey = false;
		public bool IsCheckingAPIKey
		{
			get { return m_IsCheckingAPIKey; }
			set
			{
				m_IsCheckingAPIKey = value;
				if (IsCheckingAPIKey)
					EventManager.Instance.SendEvent(Event.CHECK_API_KEY);
				else
					EventManager.Instance.SendEvent(Event.API_KEY_CHECKED);
			}
		}
		#endregion

		#region Delete Classifier Confirmation
		/// <summary>
		/// The classifier identifier to confirm deletion of.
		/// </summary>
		public string ConfirmClassifierToDelete
		{
			get { return m_ConfirmClassifierToDelete; }
			set
			{
				m_ConfirmClassifierToDelete = value;

				if (!string.IsNullOrEmpty(ConfirmClassifierToDelete))
					EventManager.Instance.SendEvent(Event.ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION);
			}
		}
		private string m_ConfirmClassifierToDelete;
		#endregion

		#region Visual Recognition Endpoints
		/// <summary>
		/// List of endpoints to use for the call.
		/// </summary>
		public ObservedList<int> Endpoints = new ObservedList<int>();

		private void OnEndpointAdded(int endpoint)
		{
			EventManager.Instance.SendEvent(Event.ON_ENDPOINT_ADDED, endpoint);
		}

		private void OnEndpointRemoved(int endpoint)
		{
			EventManager.Instance.SendEvent(Event.ON_ENDPOINT_REMOVED, endpoint);
		}
        #endregion

        #region Web Camera Dimensions
        /// <summary>
        /// Controls the aspect ratio of the WebCamera.
        /// </summary>
        public CameraDimensions WebCameraDimensions
        {
            get { return m_WebCameraDimensions; }
            set
            {
                m_WebCameraDimensions = value;
                EventManager.Instance.SendEvent(Event.ON_WEB_CAMERA_DIMENSIONS_UPDATED);
            }
        }
        private CameraDimensions m_WebCameraDimensions;
        
        /// <summary>
        /// A class holding int values for WebCam width and height.
        /// </summary>
        public class CameraDimensions
        {
            /// <summary>
            /// The width of the WebCamera.
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// The height of the WebCamera.
            /// </summary>
            public int Height { get; set; }

            /// <summary>
            /// Default constructor
            /// </summary>
            public CameraDimensions()
            { }
            /// <summary>
            /// Constructor with initialization arguments.
            /// </summary>
            /// <param name="width">Width of thw WebCamera.</param>
            /// <param name="height">Height of the WebCamera.</param>
            public CameraDimensions(int width, int height)
            {
                Width = width;
                Height = height;
            }
            /// <summary>
            /// Returns the aspect ratio of the camera.
            /// </summary>
            /// <returns></returns>
            public float GetAspectRatio()
            {
                return (float)Width/(float)Height;
            }
        }
		#endregion

		#region Image
		/// <summary>
		/// Image texture to classify.
		/// </summary>
		public Texture2D Image
		{
			get { return m_Image; }
			set
			{
				m_Image = value;
				EventManager.Instance.SendEvent(Event.ON_IMAGE_TO_CLASSIFY);
			}
		}
		private Texture2D m_Image;

		/// <summary>
		/// Image byte data to classify.
		/// </summary>
		public byte[] ImageData
		{
			get { return m_Image.EncodeToPNG(); }
		}
		#endregion

		#region Scale Factor
		/// <summary>
		/// The scale factor between the requested WebCamTexture size and the RawTexture it is writing to. Used to adjust the position and size of face and text outlines.
		/// </summary>
		public float ScaleFactor
		{
			get { return m_ScaleFactor; }
			set
			{
				m_ScaleFactor = value;
				EventManager.Instance.SendEvent(Event.ON_UPDATE_SCALE_FACTOR);
			}
		}
		private float m_ScaleFactor;
		#endregion

		#region Results
		/// <summary>
		/// Stored resutls from Classify.
		/// </summary>
		public ClassifyTopLevelMultiple ClassifyResult
		{
			get { return m_ClassifyResult; }
			set
			{
				m_ClassifyResult = value;
				EventManager.Instance.SendEvent(Event.ON_CLASSIFICATION_RESULT);
			}
		}
		private ClassifyTopLevelMultiple m_ClassifyResult = null;

		/// <summary>
		/// Stored results from DetectFaces.
		/// </summary>
		public FacesTopLevelMultiple DetectFacesResult
		{
			get { return m_DetectFacesResult; }
			set
			{
				m_DetectFacesResult = value;
				EventManager.Instance.SendEvent(Event.ON_DETECT_FACES_RESULT);
			}
		}
		private FacesTopLevelMultiple m_DetectFacesResult = null;

		/// <summary>
		/// Stored results from RecognizeText.
		/// </summary>
		public TextRecogTopLevelMultiple RecognizeTextResult
		{
			get { return m_RecognizeTextResult; }
			set
			{
				m_RecognizeTextResult = value;
				EventManager.Instance.SendEvent(Event.ON_RECOGNIZE_TEXT_RESULT);
			}
		}
		private TextRecogTopLevelMultiple m_RecognizeTextResult = null;
		#endregion
	}
}
