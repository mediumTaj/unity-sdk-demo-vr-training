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
*/

using UnityEngine;
using System.Collections;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class reviews the last training set created.
	/// </summary>
	public class ReviewTrainingSetView : View
	{
		#region Private Data
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		public ReviewTrainingSetView()
		{
			if (!m_ViewStates.Contains(AppState.REVIEW_TRAINING_SET))
				m_ViewStates.Add(AppState.REVIEW_TRAINING_SET);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		/// <summary>
		/// UI click handler for discard button.
		/// </summary>
		public void OnDiscardButtonClicked()
		{
			m_AppData.TrainingSets.RemoveAt(m_AppData.TrainingSets.Count);

			if (m_AppData.AppState == AppState.REVIEW_TRAINING_SET)
				m_AppData.AppState = AppState.CREATE_TRAINING_SET;
		}

		/// <summary>
		/// UI click handler for accept button.
		/// </summary>
		public void OnAcceptButtonClicked()
		{
			if (m_AppData.AppState == AppState.REVIEW_TRAINING_SET)
				m_AppData.AppState = AppState.TRAIN;
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
