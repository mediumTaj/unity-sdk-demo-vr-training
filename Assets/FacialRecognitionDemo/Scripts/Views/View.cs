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
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Logging;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class is the base class for all application views.
	/// </summary>
	public class View : MonoBehaviour
	{
		#region Private Data
		protected List<int> m_ViewStates = new List<int>();

		protected AppData m_AppData
        {
            get { return AppData.Instance; }
        }

        protected VisualRecognitionController m_Controller
        {
            get { return VisualRecognitionController.Instance; }
        }
		#endregion

		#region Constructor and Destructor
		public View()
		{}
		#endregion

		#region Awake / Start / Enable / Disable
		protected virtual void Awake()
		{
			if (!m_AppData.Views.Contains(this))
				m_AppData.Views.Add(this);
		}
		#endregion

		#region Private Functions
		public bool IsVisibleInCurrentAppState()
		{
			bool isVisible = false;
			if (m_ViewStates.Count == 0)
				Log.Warning("View", "View {0} does not have any view states!", gameObject.name);

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
