﻿

using IBM.Watson.DeveloperCloud.Logging;
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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class TestView : MonoBehaviour
	{
		private VisualRecognitionController m_VisualRecognitionController = new VisualRecognitionController();

		[SerializeField]
		private InputField m_APIKeyField;

		[SerializeField]
		private InputField m_DeleteClassifierIDField;

        [SerializeField]
        private InputField m_GetClassiferIDField;

        /// <summary>
        /// Change credentials to the apikey input field.
        /// </summary>
		public void ChangeCredentials()
		{
			m_VisualRecognitionController.SetVisualRecognitionAPIKey(m_APIKeyField.text);
		}

        /// <summary>
        /// Get all classifiers (brief).
        /// </summary>
		public void GetClassifiers()
		{
			m_VisualRecognitionController.GetClassifiers();
		}

        /// <summary>
        /// Delete classifier in the delete input field.
        /// </summary>
		public void DeleteClassifier()
		{
			m_VisualRecognitionController.DeleteClassifier(m_DeleteClassifierIDField.text);
		}

        /// <summary>
        /// Gets verbose data from the classifier identifer in the input field.
        /// </summary>
        public void GetClassifierVerbose()
        {
            m_VisualRecognitionController.GetClassifier(m_GetClassiferIDField.text);
        }
	}
}
