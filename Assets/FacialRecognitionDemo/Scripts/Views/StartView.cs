﻿/**
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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class is the Start view.
	/// </summary>
	public class StartView : View
	{
		#region Private Data
		private AppData m_AppData
		{
			get { return AppData.Instance; }
		}
		#endregion

		#region Public Properties
		#endregion

		#region Awake / Start / Enable / Disable
		void Awake()
		{
			m_ViewStates.Add(AppState.START);
		}
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		public bool IsVisibleInCurrentAppState()
		{
			bool isVisible = false;
			foreach (int ViewState in m_ViewStates)
				if (ViewState == m_AppData.AppState)
					isVisible = true;

			return isVisible;
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
