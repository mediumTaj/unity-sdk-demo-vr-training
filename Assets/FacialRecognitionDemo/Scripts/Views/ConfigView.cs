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
		private string m_CheckingMessage = "Checking API Key Validity...";
		private string m_FailMessage = "API Key check failed! Please try again.";
		private string m_SuccessMessage = "The API Key is valid!";
		private string m_EnterAPIKeyMessage = "Please enter Visual Recognition API Key.";
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// The ConfigView Constructor.
		/// </summary>
		public ConfigView()
		{
			m_ViewStates.Add(AppState.CONFIG);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
            EventManager.Instance.RegisterEventReceiver(Event.CHECK_API_KEY, HandleCheckAPIKey);
			EventManager.Instance.RegisterEventReceiver(Event.API_KEY_CHECKED, HandleAPIKeyChecked);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_UPDATED, OnAPIKeyUpdated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_VALIDATED, m_Controller.GetAllClassifierData);
			EventManager.Instance.RegisterEventReceiver(Event.ON_API_KEY_INVALIDATED, m_Controller.ClearClassifierData);
			EventManager.Instance.RegisterEventReceiver(Event.ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION, OnRequestClassifierDeleteConfirmation);
		}

        void OnDisable()
		{
            EventManager.Instance.UnregisterEventReceiver(Event.CHECK_API_KEY, HandleCheckAPIKey);
			EventManager.Instance.UnregisterEventReceiver(Event.API_KEY_CHECKED, HandleAPIKeyChecked);
            EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_UPDATED, OnAPIKeyUpdated);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_VALIDATED, m_Controller.GetAllClassifierData);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_API_KEY_INVALIDATED, m_Controller.ClearClassifierData);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_REQUEST_CLASSIFIER_DELETE_CONFIRMATION, OnRequestClassifierDeleteConfirmation);
		}

		void Start()
		{
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

		}
		#endregion
	}
}
