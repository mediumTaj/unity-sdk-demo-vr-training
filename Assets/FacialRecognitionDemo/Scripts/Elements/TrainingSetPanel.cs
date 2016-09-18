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
using System.Collections;
using IBM.Watson.DeveloperCloud.Logging;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class TrainingSetPanel : MonoBehaviour
	{
		#region Private Data
		[SerializeField]
		private RawImage m_RawImage;
		[SerializeField]
		private InputField m_ClassField;
        private TrainingView m_TrainingView;
        #endregion

        #region Public Properties
        /// <summary>
        /// The panel's trainign set. Sets the image of the panel to the fist training image.
        /// </summary>
        public AppData.TrainingSet TrainingSet
        {
            get { return m_TrainingSet; }
            set
            {
                m_TrainingSet = value;
                m_RawImage.texture = TrainingSet.images[0];
            }
        }
        private AppData.TrainingSet m_TrainingSet;

        /// <summary>
        /// Returns the contents of the class input field.
        /// </summary>
        public string ClassName
        {
            get
            {
                return m_ClassField.text;
            }
        }
		#endregion

		#region Constructor and Destructor
		#endregion

		#region Awake / Start / Enable / Disable
        void Start()
        {
            m_TrainingView = GetComponentInParent<TrainingView>();

            if (m_TrainingView == null)
                Log.Warning("TrainingSetPanel", "No training view was found!");
        }
        #endregion

        #region Private Functions
        #endregion

        #region Public Functions
        /// <summary>
        /// UI Handler when Classnames are added.
        /// </summary>
        public void OnChangeClassname()
        {
            //m_TrainingView.set();
            //TODO attach this to triggger when we edit the classname. Controller changes variable in model and triggers event to be sent. TrainingView listens for that event.
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
