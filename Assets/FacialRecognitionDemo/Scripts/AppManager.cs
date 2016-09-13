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
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;
using System.Collections.Generic;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class manages the application states.
	/// </summary>
	public class AppManager : MonoBehaviour
	{
		#region Private Data
		private AppData m_AppData
		{
			get { return AppData.Instance; }
		}
		#endregion

		#region Awake / Enable / Disable
		void Awake()
		{
			m_AppData.AppState = AppState.NONE;
			LogSystem.InstallDefaultReactors();
		}

		void Start()
		{
			m_AppData.AppState = AppState.START;
		}

		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_UPDATE_APP_STATE, OnUpdateAppState);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.ON_UPDATE_APP_STATE, OnUpdateAppState);
		}
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		#endregion

		#region Event Handlers
		private void OnUpdateAppState(object[] args)
		{
			Log.Debug("AppManager", "App state has been updated to {0}!", m_AppData.AppState);

			List<View> viewList = new List<View>();
			foreach (View view in m_AppData.Views)
				viewList.Add(view);

			foreach(View view in viewList)
			{
				bool isVisible = view.IsVisibleInCurrentAppState();
				view.gameObject.SetActive(isVisible);
			}
		}
		#endregion
	}
}
