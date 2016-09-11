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
using System.Collections;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class WebCamView : View
	{
		#region Private Data
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// WebCamView Constructor.
		/// </summary>
		public WebCamView()
		{
			m_ViewStates.Add(AppState.PHOTO);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		public void OnOptionsButtonClicked()
		{
			if (m_AppData.AppState == AppState.PHOTO)
				m_AppData.AppState = AppState.CONFIG;
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
