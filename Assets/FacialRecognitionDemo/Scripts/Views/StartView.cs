

using System;
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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class contorls the Start View.
	/// </summary>
	public class StartView : View
	{
        #region Private Data
        #endregion

        #region Public Properties
        #endregion

        #region Constructor and Destructor
        /// <summary>
        /// StartView Constrtuctor.
        /// </summary>
        public StartView()
		{
			m_ViewStates.Add(AppState.START);
		}
        #endregion

        #region Awake / Start / Enable / Disable
        #endregion

        #region Private Functions
        #endregion

        #region Public Functions
        /// <summary>
        /// UI Handler for clicking the Start button.
        /// </summary>
        public void OnStartButtonClicked()
        {
            m_Controller.StartApplication();
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
