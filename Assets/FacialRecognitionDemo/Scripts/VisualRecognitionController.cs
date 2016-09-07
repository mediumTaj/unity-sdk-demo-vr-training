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

        #region API Key
        /// <summary>
        /// Sets the APIKey in the Data.
        /// </summary>
        /// <param name="apiKey">The Visual Recognition service API Key.</param>
        public void SetVisualRecognitionAPIKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("apiKey");

            AppData.Instance.APIKey = apiKey;
        }
        #endregion

        #region Get Classifiers
        /// <summary>
        /// Gets all classifiers.
        /// </summary>
        public void GetClassifiers()
		{
			if (!m_VisualRecognition.GetClassifiers(OnGetClassifiers))
				Log.Debug("VisualRecognitionUtilities", "Failed to get classifiers!");
		}

		private void OnGetClassifiers(GetClassifiersTopLevelBrief classifiers, string data)
		{
			if (classifiers != null)
			{
				Log.Debug("VisualRecognitionUtilities", "GetClassifiers succeeded!");
				AppData.Instance.ClassifiersBrief = classifiers;

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnGetClassifiers();", "data: {0}", data);
			}
			else
			{
				Log.Debug("VisualRecognitionUtilities", "GetClassifiers failed!");
			}
		}
        #endregion

        #region Delete Classifier
        /// <summary>
        /// Deletes a Visual Recognition classifier.
        /// </summary>
        /// <param name="classifierID">The Visual Recognition classifier being deleted.</param>
        public void DeleteClassifier(string classifierID)
		{
			if (string.IsNullOrEmpty(classifierID))
				throw new ArgumentNullException("ClassifierID");

			if (!m_VisualRecognition.DeleteClassifier(OnDeleteClassifier, classifierID))
				Log.Debug("VisualRecognitionUtilities", "Failed to delete classifier!");
		}

		private void OnDeleteClassifier(bool success, string data)
		{
			if (success)
			{
				Log.Debug("VisualRecognitionUtilities", "Deleted classifier!");

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnDeleteClassifier();", "data: {0}", data);
			}
			else
				Log.Debug("VisualRecognitionUtilities", "Failed to delete classifier!");
		}
        #endregion

        #region Get Classifier
        /// <summary>
        /// Gets verbose data for a Visual Recognition classifier.
        /// </summary>
        /// <param name="classifierID">The classifier identifier being queried.</param>
        public void GetClassifier(string classifierID)
        {
            if (string.IsNullOrEmpty(classifierID))
                throw new ArgumentNullException("ClassifierID");

            if (!m_VisualRecognition.GetClassifier(OnGetClassifier, classifierID))
                Log.Debug("VisualRecognitionUtilities", "Failed to get classifier!");
        }

        private void OnGetClassifier(GetClassifiersPerClassifierVerbose classifier, string data)
        {
            if(classifier != null)
            {
                bool classifierFound = false;
                bool replaceClassifier = false;
                int classifierIndexToRemove = default(int);

                foreach (GetClassifiersPerClassifierVerbose ClassifierVerbose in AppData.Instance.ClassifiersVerbose)
                    if (ClassifierVerbose.classifier_id == classifier.classifier_id)
                    {
                        classifierFound = true;

                        if (ClassifierVerbose.classes != classifier.classes)
                        {
                            replaceClassifier = true;
                            classifierIndexToRemove = AppData.Instance.ClassifiersVerbose.IndexOf(ClassifierVerbose);
                        }
                    }

                if (!classifierFound)
                    AppData.Instance.ClassifiersVerbose.Add(classifier);

                if (classifierFound && replaceClassifier)
                {
                    AppData.Instance.ClassifiersVerbose.RemoveAt(classifierIndexToRemove);
                    AppData.Instance.ClassifiersVerbose.Add(classifier);
                }

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnGetClassifier();", "data: {0}", data);
			}
			else
			{
				Log.Warning("VisualRecognitionUtilities", "Failed to get classifier!");
			}
		}
        #endregion

        #region Get Classifiers Verbose
        /// <summary>
        /// Gets verbose data from all classifiers.
        /// </summary>
        /// <param name="classifiers"></param>
        public void GetClassifiers(GetClassifiersTopLevelBrief classifiers)
        {
            if (classifiers == null)
                throw new ArgumentNullException("classifiers");

            foreach(GetClassifiersPerClassifierBrief classifier in classifiers.classifiers)
                if (!m_VisualRecognition.GetClassifier(OnGetClassifier, classifier.classifier_id))
                    Log.Debug("VisualRecognitionUtilities", "Failed to get classifier {0}!", classifier.classifier_id);
        }
		#endregion

		#region Get All Classifier Data
		/// <summary>
		/// Retrieves all classifierIDs and then gets all classifier data from each classifierID.
		/// </summary>
		public void GetAllClassifierData()
		{
			if (!m_VisualRecognition.GetClassifiers(OnGetAllClassifierData))
				Log.Debug("VisualRecognitionUtilities", "Failed to get classifiers!");
		}

		private void OnGetAllClassifierData(GetClassifiersTopLevelBrief classifiers, string data)
		{
			if (classifiers != null)
			{
				int numClassifiers = classifiers.classifiers.Length;
				Log.Debug("VisualRecogntionController", "Classifier data received. NumClassifiers: {0}", numClassifiers);
				AppData.Instance.ClassifiersBrief = classifiers;

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnGetAllClassifierData();", "data: {0}", data);

				for (int i = 0; i < numClassifiers; i++)
					if (!string.IsNullOrEmpty(classifiers.classifiers[i].classifier_id))
						if (!m_VisualRecognition.GetClassifier(OnGetClassifier, classifiers.classifiers[i].classifier_id, string.Format("classifier{0}/{1}", i, numClassifiers)))
							Log.Debug("VisualRecognitionUtilities", "Failed to get classifier {0}!", classifiers.classifiers[i].classifier_id);
			}
			else
			{
				Log.Warning("VisualRecognitionUtilities", "Failed to get all classifier data!");
			}
		}
		#endregion
	}
}
