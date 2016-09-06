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

using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using System;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class VisualRecognitionController
	{
		private VisualRecognition m_VisualRecognition = new VisualRecognition();
        
        /// <summary>
        /// Sets teh APIKey in the Data.
        /// </summary>
        /// <param name="apiKey">The Visual Recognition service API Key.</param>
        public void SetVisualRecognitionAPIKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("apiKey");

            AppData.Instance.APIKey = apiKey;
        }

        /// <summary>
        /// UIHandler for getting all classifiers.
        /// </summary>
		public void GetClassifiers()
		{
			if (!m_VisualRecognition.GetClassifiers(OnGetClassifiers))
				Log.Debug("VisualRecognitionUtilities", "Failed to get classifiers!");
		}

        /// <summary>
        /// Get all classifiers handler.
        /// </summary>
        /// <param name="classifiers">Classifer object.</param>
        /// <param name="data">Custom data.</param>
		private void OnGetClassifiers(GetClassifiersTopLevelBrief classifiers, string data)
		{
			if (classifiers != null)
			{
				Log.Debug("VisualRecognitionUtilities", "GetClassifiers succeeded!");
				AppData.Instance.ClassifiersBrief = classifiers;
			}
			else
			{
				Log.Debug("VisualRecognitionUtilities", "GetClassifiers failed!");
			}
		}

        /// <summary>
        /// UIHandler for deleting all classifiers.
        /// </summary>
        /// <param name="classifierID"></param>
		public void DeleteClassifier(string classifierID)
		{
			if (string.IsNullOrEmpty(classifierID))
				throw new ArgumentNullException("ClassifierID");

			if (!m_VisualRecognition.DeleteClassifier(OnDeleteClassifier, classifierID))
				Log.Debug("VisualRecognitionUtilities", "Failed to delete classifier!");
		}

        /// <summary>
        /// Delete classifier handler.
        /// </summary>
        /// <param name="success">Delete success.</param>
        /// <param name="data">Custom data.</param>
		private void OnDeleteClassifier(bool success, string data)
		{
			if (success)
				Log.Debug("VisualRecognitionUtilities", "Deleted classifier!");
			else
				Log.Debug("VisualRecognitionUtilities", "Failed to delete classifier!");
		}
	}
}
