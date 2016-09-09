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
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Utilities;

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
		private bool m_APIKeyChecked = false;

		private AppData m_AppData
		{
			get { return AppData.Instance; }
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
			m_ViewStates.Add(AppState.CONFIG);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		protected override void Awake()
		{
			base.Awake();

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

		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.API_KEY_CHECKED, HandleAPIKeyChecked);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.API_KEY_CHECKED, HandleAPIKeyChecked);
		}
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		#endregion

		#region Event Handlers
		private void HandleAPIKeyChecked(object[] args)
		{
			if (m_AppData.IsAPIKeyValid)
				m_StatusText.text = m_SuccessMessage;
			else
				m_StatusText.text = m_FailMessage;
		}
		#endregion
	}
}
