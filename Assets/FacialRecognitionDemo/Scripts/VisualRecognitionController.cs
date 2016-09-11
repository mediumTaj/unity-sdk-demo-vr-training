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
using IBM.Watson.DeveloperCloud.Utilities;
using System;
using System.Collections.Generic;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class VisualRecognitionController
	{
		private VisualRecognition m_VisualRecognition = new VisualRecognition();

        #region Instance
        /// <summary>
        /// Returns the singleton instance of VisualRecognitionController.
        /// </summary>
        public static VisualRecognitionController Instance { get { return Singleton<VisualRecognitionController>.Instance; } }
        #endregion

        #region Private Data
        private AppData m_AppData
		{
			get { return AppData.Instance; }
		}
		#endregion

		#region API Key
		/// <summary>
		/// Sets the APIKey in the Data.
		/// </summary>
		/// <param name="apiKey">The Visual Recognition service API Key.</param>
		public void SetVisualRecognitionAPIKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("apiKey");

            m_AppData.APIKey = apiKey;
        }

        /// <summary>
        /// Check API Key validity
        /// </summary>
        public void CheckAPIKey()
        {
            m_VisualRecognition.GetServiceStatus(HandleGetServiceStatus);
        }

        private void HandleGetServiceStatus(string serviceID, bool active)
		{
			if (active)
				m_AppData.IsAPIKeyValid = true;

			m_AppData.IsCheckingAPIKey = false;
		}
		#endregion

		#region Get Classifiers
		/// <summary>
		/// Gets all classifiers.
		/// </summary>
		public void GetClassifiers()
		{
			if (!m_VisualRecognition.GetClassifiers(OnGetClassifiers))
				Log.Debug("VisualRecognitionController", "Failed to get classifiers!");
		}

		private void OnGetClassifiers(GetClassifiersTopLevelBrief classifiers, string data)
		{
			if (classifiers != null)
			{
				Log.Debug("VisualRecognitionController", "GetClassifiers succeeded!");
				m_AppData.ClassifiersBrief = classifiers;

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnGetClassifiers();", "data: {0}", data);
			}
			else
			{
				Log.Debug("VisualRecognitionController", "GetClassifiers failed!");
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
				Log.Debug("VisualRecognitionController", "Failed to delete classifier!");
		}

		private void OnDeleteClassifier(bool success, string data)
		{
			if (success)
			{
				Log.Debug("VisualRecognitionController", "Deleted classifier!");

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnDeleteClassifier();", "data: {0}", data);
			}
			else
				Log.Debug("VisualRecognitionController", "Failed to delete classifier!");
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
                Log.Debug("VisualRecognitionController", "Failed to get classifier!");
        }

        private void OnGetClassifier(GetClassifiersPerClassifierVerbose classifier, string data)
        {
            if(classifier != null)
            {
                bool classifierFound = false;
                bool replaceClassifier = false;
                int classifierIndexToRemove = default(int);

                foreach (GetClassifiersPerClassifierVerbose ClassifierVerbose in m_AppData.ClassifiersVerbose)
                    if (ClassifierVerbose.classifier_id == classifier.classifier_id)
                    {
                        classifierFound = true;

                        if (ClassifierVerbose.classes != classifier.classes)
                        {
                            replaceClassifier = true;
                            classifierIndexToRemove = m_AppData.ClassifiersVerbose.IndexOf(ClassifierVerbose);
                        }
                    }

                if (!classifierFound)
                    m_AppData.ClassifiersVerbose.Add(classifier);

                if (classifierFound && replaceClassifier)
                {
                    m_AppData.ClassifiersVerbose.RemoveAt(classifierIndexToRemove);
                    m_AppData.ClassifiersVerbose.Add(classifier);
                }

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnGetClassifier();", "data: {0}", data);
			}
			else
			{
				Log.Warning("VisualRecognitionController", "Failed to get classifier!");
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
                    Log.Debug("VisualRecognitionController", "Failed to get classifier {0}!", classifier.classifier_id);
        }
		#endregion

		#region Get All Classifier Data
		/// <summary>
		/// Retrieves all classifierIDs and then gets all classifier data from each classifierID.
		/// </summary>
		public void GetAllClassifierData(object[] args = null)
		{
			if (!m_VisualRecognition.GetClassifiers(OnGetAllClassifierData))
				Log.Debug("VisualRecognitionController", "Failed to get classifiers!");
		}

		private void OnGetAllClassifierData(GetClassifiersTopLevelBrief classifiers, string data)
		{
			if (classifiers != null)
			{
				int numClassifiers = classifiers.classifiers.Length;
				Log.Debug("VisualRecogntionController", "Classifier data received. NumClassifiers: {0}", numClassifiers);
				m_AppData.ClassifiersBrief = classifiers;

				if (!string.IsNullOrEmpty(data))
					Log.Debug("VisualRecognitionController | OnGetAllClassifierData();", "data: {0}", data);

				for (int i = 0; i < numClassifiers; i++)
					if (!string.IsNullOrEmpty(classifiers.classifiers[i].classifier_id))
						if (!m_VisualRecognition.GetClassifier(OnGetClassifier, classifiers.classifiers[i].classifier_id, string.Format("classifier{0}/{1}", i, numClassifiers)))
							Log.Debug("VisualRecognitionController", "Failed to get classifier {0}!", classifiers.classifiers[i].classifier_id);
			}
			else
			{
				Log.Warning("VisualRecognitionController", "Failed to get all classifier data!");
			}
		}
		#endregion

		#region Clear Classifier Data
		/// <summary>
		/// Clears all classifier data.
		/// </summary>
		/// <param name="args">Optional event arguments.</param>
		public void ClearClassifierData(object[] args = null)
		{
			if(m_AppData.ClassifiersVerbose.Count > 0)
			{
				List<GetClassifiersPerClassifierVerbose> classifierList = new List<GetClassifiersPerClassifierVerbose>();
				foreach (GetClassifiersPerClassifierVerbose classifierVerbose in m_AppData.ClassifiersVerbose)
					classifierList.Add(classifierVerbose);

				foreach (GetClassifiersPerClassifierVerbose classifierVerbose in classifierList)
					m_AppData.ClassifiersVerbose.Remove(classifierVerbose);
			}

			if(m_AppData.ClassifierIDs.Count > 0)
			{
				List<string> classifierIDList = new List<string>();
				foreach (string classifierID in m_AppData.ClassifierIDs)
					classifierIDList.Add(classifierID);

				foreach (string classifierID in classifierIDList)
					m_AppData.ClassifierIDs.Remove(classifierID);
			}
		}
		#endregion

		#region Classify
		/// <summary>
		/// Classifies an image by ByteArray.
		/// </summary>
		/// <param name="imageData">Byte array of image data.</param>
		/// <param name="classifierIDs">Array of classifier identifiers to use.</param>
		/// <param name="owners">Array of owners. Usually "IBM" and "me"</param>
		/// <param name="threshold">Threshold for omitting classification results.</param>
		public void Classify(byte[] imageData, string[] classifierIDs = default(string[]), string[] owners = default(string[]), float threshold = 1)
		{
			if (!m_VisualRecognition.Classify(OnClassify, imageData, owners, classifierIDs, threshold))
				Log.Warning("VisualRecognitionController", "Failed to classify image!");
		}

		private void OnClassify(ClassifyTopLevelMultiple classify, string data)
		{
			if(classify != null)
			{
				foreach (ClassifyTopLevelSingle image in classify.images)
					foreach (ClassifyPerClassifier classifier in image.classifiers)
					{
						Log.Debug("VisualRecognitionController", "Classifier Name: {0} | ID: {1}", classifier.name, classifier.classifier_id);
						foreach (ClassResult classResult in classifier.classes)
						{
						
						Log.Debug("VisualRecogntionController", "Class: {0} | Score: {1}", classResult.m_class, classResult.score);
							if (!string.IsNullOrEmpty(classResult.type_hierarchy))
								Log.Debug("VisualRecogntionController", "type_hierarchy: {0}", classResult.type_hierarchy);
						}
					}
			}
			else
			{
				Log.Debug("VisualRecognitionController", "Failed to classify image!");
			}
		}
        #endregion

        #region UpdateClassifier
        public void UpdateClassifier(string classifierID, byte[] positiveExamplesData, byte[] negativeExamplesData = default(byte[]))
        {

        }
		#endregion

		#region Start Application
		public void StartApplication()
		{
			if (m_AppData.AppState == AppState.START)
				m_AppData.AppState = AppState.CONFIG;
			else
				Log.Error("VisualRecognitionController", "Application not in start state!");
		}
		#endregion
	}
}
