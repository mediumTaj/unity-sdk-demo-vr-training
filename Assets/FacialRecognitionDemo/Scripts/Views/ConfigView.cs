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

using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Utilities;
using System.Collections;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class controls the Config View.
	/// </summary>
	public class ConfigView : View
	{
		#region Private Data
		[SerializeField]
		private InputField m_APIKeyInputField;
		[SerializeField]
		private Text m_StatusText;
		[SerializeField]
		private GameObject m_DeleteClassifierConfirmationPanel;
		[SerializeField]
		private Text m_DeleteConfirmationText;
		[SerializeField]
		private GameObject m_ContinueButton;
		[SerializeField]
		private Text m_ClassifierIDsToClassifyWithText;
		private string m_CheckingMessage = "Checking API Key Validity...";
		private string m_FailMessage = "API Key check failed! Please try again.";
		private string m_SuccessMessage = "The API Key is valid!";
		private string m_EnterAPIKeyMessage = "Please enter Visual Recognition API Key.";
		private string m_DeleteConfirmationMessage = "Are you sure you would like to delete classifier {0}?";
		private string m_ClassifierIDsToClassifyWithString = "{0}\n";
		
		private bool m_IsDeleteClassifierConfirmationVisible = false;
		private bool IsDeleteClassifierConfirmationVisible
		{
			get { return m_IsDeleteClassifierConfirmationVisible; }
			set
			{
				m_IsDeleteClassifierConfirmationVisible = value;
				m_DeleteClassifierConfirmationPanel.SetActive(m_IsDeleteClassifierConfirmationVisible);
			}
		}

		private bool m_IsContinueButtonVisible = false;
		private bool IsContinueButtonVisible
		{
			get { return m_IsContinueButtonVisible; }
			set
			{
				m_IsContinueButtonVisible = value;
				m_ContinueButton.SetActive(IsContinueButtonVisible);
			}
		}
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// The ConfigView Constructor.
		/// </summary>
		public ConfigView()
		{
			if (!m_ViewStates.Contains(AppState.CONFIG))
				m_ViewStates.Add(AppState.CONFIG);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
            EventManager.Instance.RegisterEventReceiver(Event.CHECK_API_KEY, HandleCheckAPIKey);
			EventManager.Instance.RegisterEventReceiver(Event.API_KEY_CHECKED, HandleAPIKeyChecked);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_UPDATED, OnAPIKeyUpdated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_VALIDATED, OnAPIKeyValidated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_INVALIDATED, OnAPIKeyInvalidated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION, OnRequestClassifierDeleteConfirmation);
		}

        void OnDisable()
		{
            EventManager.Instance.UnregisterEventReceiver(Event.CHECK_API_KEY, HandleCheckAPIKey);
			EventManager.Instance.UnregisterEventReceiver(Event.API_KEY_CHECKED, HandleAPIKeyChecked);
            EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_UPDATED, OnAPIKeyUpdated);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_VALIDATED, OnAPIKeyValidated);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_INVALIDATED, OnAPIKeyInvalidated);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION, OnRequestClassifierDeleteConfirmation);
		}

		void Start()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_ADDED, OnClassifierIDToClassifyWithAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_REMOVED, OnClassifierIDToClassifyWithRemoved);

			if (!m_AppData.ClassifierIDsToClassifyWith.Contains("default"))
				m_AppData.ClassifierIDsToClassifyWith.Add("default");

			CheckAPIKey();
		}
		#endregion

		#region Private Functions
		private void CheckAPIKey()
        {
            string apiKey = Config.Instance.GetAPIKey("VisualRecognitionV3");

            if (!string.IsNullOrEmpty(apiKey))
            {
                m_APIKeyInputField.text = Config.Instance.GetAPIKey("VisualRecognitionV3");

                if (!m_AppData.IsAPIKeyValid)
                {
                    m_AppData.IsCheckingAPIKey = true;
                    m_StatusText.text = m_CheckingMessage;
                }
                else
                {
                    m_StatusText.text = m_SuccessMessage;
                }
            }
            else
                m_StatusText.text = m_EnterAPIKeyMessage;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// UI Click handler for Check API Key button.
        /// </summary>
        public void OnCheckAPIKeyButtonClicked()
        {
			m_AppData.IsAPIKeyValid = false;
            m_AppData.APIKey = m_APIKeyInputField.text;
        }

		/// <summary>
		/// UI Click handler for cancelling clasifier deletion.
		/// </summary>
		public void OnCancelDeleteClassifierClicked()
		{
			IsDeleteClassifierConfirmationVisible = false;
		}

		/// <summary>
		/// UI Click handler for confirming classifier deletion.
		/// </summary>
		public void OnDeleteClassifierConfirmatioClicked()
		{
			IsDeleteClassifierConfirmationVisible = false;
			m_Controller.DeleteClassifier(m_AppData.ConfirmClassifierToDelete);
		}

		/// <summary>
		/// UI Click Handler for the continue button.
		/// </summary>
		public void OnContinueButtonClicked()
		{
			m_AppData.AppState = AppState.PHOTO;
		}

		public void OnUseDefaultClassifierValueChanged(bool val)
		{
			if (val)
			{
				if (!m_AppData.ClassifierIDsToClassifyWith.Contains("default"))
					m_AppData.ClassifierIDsToClassifyWith.Add("default");
			}
			else
			{
				if (m_AppData.ClassifierIDsToClassifyWith.Contains("default"))
					m_AppData.ClassifierIDsToClassifyWith.Remove("default");
			}
		}
        #endregion

        #region Event Handlers
        private void HandleCheckAPIKey(object[] args = null)
        {
            m_Controller.CheckAPIKey();
        }

        private void HandleAPIKeyChecked(object[] args = null)
		{
			if (m_AppData.IsAPIKeyValid)
				m_StatusText.text = m_SuccessMessage;
			else
				m_StatusText.text = m_FailMessage;
		}

        private void OnAPIKeyUpdated(object[] args = null)
        {
            Runnable.Run(LoadConfig());
        }

        private IEnumerator LoadConfig()
        {
            Config.Instance.ConfigLoaded = false;
            Config.Instance.LoadConfig();
            while (!Config.Instance.ConfigLoaded)
                yield return null;

            CheckAPIKey();
        }

		private void OnRequestClassifierDeleteConfirmation(object[] args = null)
		{
			m_DeleteConfirmationText.text = string.Format(m_DeleteConfirmationMessage, m_AppData.ConfirmClassifierToDelete);
			IsDeleteClassifierConfirmationVisible = true;
		}

		private void OnAPIKeyValidated(object[] args = null)
		{
			m_Controller.GetAllClassifierData();
			IsContinueButtonVisible = true;
		}

		private void OnAPIKeyInvalidated(object[] args = null)
		{
			m_Controller.ClearClassifierData();
			IsContinueButtonVisible = false;
		}

		private void OnClassifierIDToClassifyWithAdded(object[] args = null)
		{
			if (args[0] is string && m_ClassifierIDsToClassifyWithText.text != null)
			{
				m_ClassifierIDsToClassifyWithText.text += string.Format(m_ClassifierIDsToClassifyWithString, args[0] as string);
			}
		}

		private void OnClassifierIDToClassifyWithRemoved(object[] args = null)
		{
			if(args[0] is string && m_ClassifierIDsToClassifyWithText.text != null)
			{
				if (m_ClassifierIDsToClassifyWithText.text.Contains(args[0] as string))
					m_ClassifierIDsToClassifyWithText.text = m_ClassifierIDsToClassifyWithText.text.Replace(string.Format(m_ClassifierIDsToClassifyWithString, args[0] as string), "");
			}
		}
		#endregion
	}
}
