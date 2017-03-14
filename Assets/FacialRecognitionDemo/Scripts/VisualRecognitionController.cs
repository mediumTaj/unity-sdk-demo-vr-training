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
	/// <summary>
	/// This class handles all functionality from views and user interaction. The controller directly sets values in the AppData (Model).
	/// </summary>
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
		/// Confirms the deletion of a Visual Recognition classifier.
		/// </summary>
		/// <param name="classifierID"></param>
		public void ConfirmDeleteClassifier(string classifierID)
		{
			if (string.IsNullOrEmpty(classifierID))
				throw new ArgumentNullException(classifierID);

			m_AppData.ConfirmClassifierToDelete = classifierID;
		}

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

				DeleteClassifierFromAppData(m_AppData.ConfirmClassifierToDelete);

				m_AppData.ConfirmClassifierToDelete = default(string);
			}
			else
				Log.Debug("VisualRecognitionController", "Failed to delete classifier!");
		}

		/// <summary>
		/// Deletes a classifier from AppData.
		/// </summary>
		/// <param name="classifierID">The classifier identifier to delete.</param>
		public void DeleteClassifierFromAppData(string classifierID)
		{
			if (string.IsNullOrEmpty(classifierID))
				throw new ArgumentNullException(classifierID);

			if (m_AppData.ClassifiersVerbose.Count > 0)
			{
				List<GetClassifiersPerClassifierVerbose> classifierList = new List<GetClassifiersPerClassifierVerbose>();
				foreach (GetClassifiersPerClassifierVerbose classifierVerbose in m_AppData.ClassifiersVerbose)
					classifierList.Add(classifierVerbose);

				foreach (GetClassifiersPerClassifierVerbose classifierVerbose in classifierList)
					if(classifierID == classifierVerbose.classifier_id)
						m_AppData.ClassifiersVerbose.Remove(classifierVerbose);
			}

			if (m_AppData.ClassifierIDs.Count > 0)
			{
				List<string> classifierIDList = new List<string>();
				foreach (string cID in m_AppData.ClassifierIDs)
					classifierIDList.Add(cID);

				foreach (string cID in classifierIDList)
					if(classifierID == cID)
						m_AppData.ClassifierIDs.Remove(cID);
			}
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

			if(m_AppData.ClassifiersBrief != null && m_AppData.ClassifiersBrief.classifiers.Length > 0)
			{
				Array.Clear(m_AppData.ClassifiersBrief.classifiers, 0, m_AppData.ClassifiersBrief.classifiers.Length);
			}

			if (m_AppData.ClassifierIDsToClassifyWith.Count > 0)
				m_AppData.ClassifierIDsToClassifyWith.Clear();
		}
		#endregion

		#region Select / Deselect classifiers
		/// <summary>
		/// Selects all classifiers to use for classification. This includes the default classifier.
		/// </summary>
		public void SelectAllClassifiers()
		{
			if(!m_AppData.ClassifierIDsToClassifyWith.Contains("default"))
				m_AppData.ClassifierIDsToClassifyWith.Add("default");

			foreach (GetClassifiersPerClassifierVerbose classifierVerbose in m_AppData.ClassifiersVerbose)
				if (!m_AppData.ClassifierIDsToClassifyWith.Contains(classifierVerbose.classifier_id))
					m_AppData.ClassifierIDsToClassifyWith.Add(classifierVerbose.classifier_id);
		}
		
		/// <summary>
		/// Deselets all classifiers to use for clasification. This includes the default classifier.
		/// </summary>
		public void DeselectAllClassifiers()
		{
			if (m_AppData.ClassifierIDsToClassifyWith.Contains("default"))
				m_AppData.ClassifierIDsToClassifyWith.Remove("default");

			List<string> classiferIDsToClassifyWithList = new List<string>();
			foreach (string classifierID in m_AppData.ClassifierIDsToClassifyWith)
				classiferIDsToClassifyWithList.Add(classifierID);

			foreach (string classifierID in classiferIDsToClassifyWithList)
				m_AppData.ClassifierIDsToClassifyWith.Remove(classifierID);
		}
		#endregion

        #region UpdateClassifier
        public void UpdateClassifier(string classifierID, byte[] positiveExamplesData, byte[] negativeExamplesData = default(byte[]))
        {

        }
		#endregion

		#region TrainClassifier
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

		#region Classify
		/// <summary>
		/// Classifys a byteArray of imagedata using the Visual Recognition service.
		/// </summary>
		/// <param name="imageData">A byte[] of image data.</param>
		public void Classify(byte[] imageData)
		{
			string[] owners = { "IBM", "me" };
			m_AppData.VisualRecognition.Classify(OnClassify, imageData, owners, m_AppData.ClassifierIDsToClassifyWith.ToArray());
		}

		private void OnClassify(ClassifyTopLevelMultiple classify, string data)
		{
			if (classify != null)
				m_AppData.ClassifyResult = classify;
		}
		#endregion

		#region Detect Faces
		/// <summary>
		/// Detects faces in a byteArray of imagedata using the Visual Recognition service.
		/// </summary>
		/// <param name="imageData">A byte[] of image data.</param>
		public void DetectFaces(byte[] imageData)
		{
			m_AppData.VisualRecognition.DetectFaces(OnDetectFaces, imageData);
		}

		private void OnDetectFaces(FacesTopLevelMultiple multipleImages, string data)
		{
			if (multipleImages != null)
				m_AppData.DetectFacesResult = multipleImages;
		}
		#endregion

		#region RecognizeText
		/// <summary>
		/// Recognizes text in a byteArray of imagedata using the Visual Recognition service.
		/// </summary>
		/// <param name="imageData">A byte[] of image data.</param>
		public void RecognizeText(byte[] imageData)
		{
			m_AppData.VisualRecognition.RecognizeText(OnRecognizeText, imageData);
		}

		private void OnRecognizeText(TextRecogTopLevelMultiple multipleImages, string data)
		{
			if (multipleImages != null)
				m_AppData.RecognizeTextResult = multipleImages;
		}
		#endregion
	}
}
