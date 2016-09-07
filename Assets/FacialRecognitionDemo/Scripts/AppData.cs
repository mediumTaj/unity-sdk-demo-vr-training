﻿

using IBM.Watson.DeveloperCloud.Connection;
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
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using System;
using System.IO;
using UnityEngine;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
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
			ClassifierIDs.Added += OnClassifierIDAdded;
			ClassifierIDs.Removed += OnClassifierIDRemoved;
			ClassifiersVerbose.Added += OnClassiferVerboseAdded;
			ClassifiersVerbose.Removed += OnClassifierVerboseRemoved;
		}

		~AppData()
		{
			ClassifierIDs.Added -= OnClassifierIDAdded;
			ClassifierIDs.Removed -= OnClassifierIDRemoved;
			ClassifiersVerbose.Added -= OnClassiferVerboseAdded;
			ClassifiersVerbose.Removed -= OnClassifierVerboseRemoved;
		}
		#endregion

		#region Instance
		/// <summary>
		/// Returns the singleton instance of AppData.
		/// </summary>
		public static AppData Instance { get { return Singleton<AppData>.Instance; } }
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
				images.Added += OnImageAdded;
				images.Removed += OnImageRemoved;
			}

			~VRClass()
			{
				images.Added -= OnImageAdded;
				images.Removed -= OnImageRemoved;
			}

			private void OnImageAdded()
			{
				EventManager.Instance.SendEvent(Constants.ON_IMAGE_ADDED);
			}

			private void OnImageRemoved()
			{
				EventManager.Instance.SendEvent(Constants.ON_IMAGE_REMOVED);
			}
		}
		#endregion

		#region API Key
		/// <summary>
		/// The Visual Recognition APIKey
		/// </summary>
		public string APIKey
		{
			get { return m_APIKey; }
			set
			{
				m_APIKey = value;
                if (!SetAPIKey(APIKey))
                    Log.Warning("AppData", "Visual Recognition API Keys were not updated!");
                else
                    EventManager.Instance.SendEvent(Constants.ON_API_KEY_UPDATED);
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
            RESTConnector.FlushConnectors();

            return true;
        }
        #endregion

        #region Classifier IDs
        /// <summary>
        /// List of Classifier IDs
        /// </summary>
        public ObservedList<string> ClassifierIDs = new ObservedList<string>();

		private void OnClassifierIDAdded()
		{
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_ADDED);
		}

		private void OnClassifierIDRemoved()
		{
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_ADDED);
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

				EventManager.Instance.SendEvent(Constants.ON_CLASSIFIERS_UPDATED);
			}
		}
		private GetClassifiersTopLevelBrief m_ClassifiersBrief;
		#endregion

		#region ClassifiersVerbose
		/// <summary>
		/// List of Classifiers and their classes.
		/// </summary>
		public ObservedList<GetClassifiersPerClassifierVerbose> ClassifiersVerbose = new ObservedList<GetClassifiersPerClassifierVerbose>();

		private void OnClassiferVerboseAdded()
		{
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_VERBOSE_ADDED);
		}

		private void OnClassifierVerboseRemoved()
		{
			EventManager.Instance.SendEvent(Constants.ON_CLASSIFIER_VERBOSE_REMOVED);
		}
		#endregion
	}
}
