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
using IBM.Watson.DeveloperCloud.Utilities;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class TestView : View
	{
		private VisualRecognitionController m_VisualRecognitionController = new VisualRecognitionController();

		[SerializeField]
		private InputField m_APIKeyField;

		[SerializeField]
		private InputField m_DeleteClassifierIDField;

        [SerializeField]
        private InputField m_GetClassiferIDField;

		#region Enable and Disable
		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_ADDED, OnClassifierAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_REMOVED, OnClassiferRemoved);
			EventManager.Instance.RegisterEventReceiver(Event.ON_IMAGE_ADDED, OnImageAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_IMAGE_REMOVED, OnImageRemoved);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_UPDATED, OnAPIKeyUpdated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIERS_UPDATED, OnClassifiersUpdated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_ADDED, OnClassifierVerboseAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_REMOVED, OnClassifierVerboseRemoved);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_ADDED, OnClassifierAdded);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_REMOVED, OnClassiferRemoved);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_IMAGE_ADDED, OnImageAdded);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_IMAGE_REMOVED, OnImageRemoved);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_UPDATED, OnAPIKeyUpdated);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIERS_UPDATED, OnClassifiersUpdated);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_ADDED, OnClassifierVerboseAdded);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_REMOVED, OnClassifierVerboseRemoved);
		}
		#endregion

		#region UI Event Handlers
		/// <summary>
		/// Change credentials to the apikey input field.
		/// </summary>
		public void ChangeCredentials()
		{
			m_VisualRecognitionController.SetVisualRecognitionAPIKey(m_APIKeyField.text);
		}

        /// <summary>
        /// Get all classifiers (brief).
        /// </summary>
		public void GetClassifiers()
		{
			m_VisualRecognitionController.GetClassifiers();
		}

        /// <summary>
        /// Delete classifier in the delete input field.
        /// </summary>
		public void DeleteClassifier()
		{
			m_VisualRecognitionController.DeleteClassifier(m_DeleteClassifierIDField.text);
		}

        /// <summary>
        /// Gets verbose data from the classifier identifer in the input field.
        /// </summary>
        public void GetClassifierVerbose()
        {
            m_VisualRecognitionController.GetClassifier(m_GetClassiferIDField.text);
        }

		/// <summary>
		/// Gets all verbose data from all classifiers.
		/// </summary>
		public void GetAllClassifierData()
		{
			Log.Debug("TestView", "Attempting to get all classifier data!");
			m_VisualRecognitionController.GetAllClassifierData();
		}
		#endregion

		#region Event Handlers
		private void OnClassifierVerboseRemoved(object[] args)
		{
			Log.Debug("TestView", "Verbose classifier was removed {0}.", (args[0] as GetClassifiersPerClassifierVerbose).classifier_id);
		}

		private void OnClassifierVerboseAdded(object[] args)
		{
			Log.Debug("TestView", "Verbose classifier was added {0}.", (args[0] as GetClassifiersPerClassifierVerbose).classifier_id);
		}

		private void OnClassifiersUpdated(object[] args)
		{
            Log.Debug("TestView", "Classifier was updated.");
		}

		private void OnAPIKeyUpdated(object[] args)
		{
			Log.Debug("TestView", "API Key was updated {0}.", args[0] as string);
		}

		private void OnImageRemoved(object[] args)
		{
			Log.Debug("TestView", "Image was removed.");
		}

		private void OnImageAdded(object[] args)
		{
			Log.Debug("TestView", "Image was added.");
		}

		private void OnClassiferRemoved(object[] args)
		{
			Log.Debug("TestView", "Classifier was removed {0}.", args[0]);
		}

		private void OnClassifierAdded(object[] args)
		{
			Log.Debug("TestView", "Classifier was added {0}.", args[0]);
		}
		#endregion
	}
}
