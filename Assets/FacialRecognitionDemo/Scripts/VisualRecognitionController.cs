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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public void SelectAllClassifiers()
		{
			if(!m_AppData.ClassifierIDsToClassifyWith.Contains("default"))
				m_AppData.ClassifierIDsToClassifyWith.Add("default");

			foreach (GetClassifiersPerClassifierVerbose classifierVerbose in m_AppData.ClassifiersVerbose)
				if (!m_AppData.ClassifierIDsToClassifyWith.Contains(classifierVerbose.classifier_id))
					m_AppData.ClassifierIDsToClassifyWith.Add(classifierVerbose.classifier_id);
		}
		
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

		//#region Classify
		///// <summary>
		///// Classifies an image by ByteArray.
		///// </summary>
		///// <param name="imageData">Byte array of image data.</param>
		///// <param name="classifierIDs">Array of classifier identifiers to use.</param>
		///// <param name="owners">Array of owners. Usually "IBM" and "me"</param>
		///// <param name="threshold">Threshold for omitting classification results.</param>
		//public void Classify(byte[] imageData, string[] classifierIDs = default(string[]), string[] owners = default(string[]), float threshold = 1)
		//{
		//	if (!m_VisualRecognition.Classify(OnClassify, imageData, owners, classifierIDs, threshold))
		//		Log.Warning("VisualRecognitionController", "Failed to classify image!");
		//}

		//private void OnClassify(ClassifyTopLevelMultiple classify, string data)
		//{
		//	if(classify != null)
		//	{
		//		foreach (ClassifyTopLevelSingle image in classify.images)
		//			foreach (ClassifyPerClassifier classifier in image.classifiers)
		//			{
		//				Log.Debug("VisualRecognitionController", "Classifier Name: {0} | ID: {1}", classifier.name, classifier.classifier_id);
		//				foreach (ClassResult classResult in classifier.classes)
		//				{
						
		//				Log.Debug("VisualRecogntionController", "Class: {0} | Score: {1}", classResult.m_class, classResult.score);
		//					if (!string.IsNullOrEmpty(classResult.type_hierarchy))
		//						Log.Debug("VisualRecogntionController", "type_hierarchy: {0}", classResult.type_hierarchy);
		//				}
		//			}
		//	}
		//	else
		//	{
		//		Log.Debug("VisualRecognitionController", "Failed to classify image!");
		//	}
		//}
  //      #endregion

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

		#region Classify
		public void Classify(byte[] imageData)
		{
			string[] owners = { "IBM", "me" };
			m_AppData.VisualRecognition.Classify(OnClassify, imageData, owners, m_AppData.ClassifierIDsToClassifyWith.ToArray());
		}

		private void OnClassify(ClassifyTopLevelMultiple classify, string data)
		{
			if (classify != null)
				m_AppData.ClassifyResult = classify;

			//if (classify != null)
			//{
			//	if (!m_ClassifyResultGameObject.activeSelf)
			//		m_ClassifyResultGameObject.SetActive(true);

			//	Log.Debug("WebCamRecognition", "\nimages processed: " + classify.images_processed);
			//	m_ClassifyResultText.text += "\nimages processed: " + classify.images_processed;
			//	foreach (ClassifyTopLevelSingle image in classify.images)
			//	{
			//		Log.Debug("WebCamRecognition", "\tsource_url: " + image.source_url + ", resolved_url: " + image.resolved_url);
			//		m_ClassifyResultText.text += "\n\tsource_url: " + image.source_url + ", resolved_url: " + image.resolved_url;
			//		foreach (ClassifyPerClassifier classifier in image.classifiers)
			//		{
			//			Log.Debug("WebCamRecognition", "\t\tclassifier_id: " + classifier.classifier_id + ", name: " + classifier.name);
			//			m_ClassifyResultText.text += "\n\n\t\tclassifier_id: " + classifier.classifier_id + ", name: " + classifier.name;

			//			foreach (ClassResult classResult in classifier.classes)
			//			{
			//				Log.Debug("WebCamRecognition", "\t\t\tclass: " + classResult.m_class + ", score: " + classResult.score + ", type_hierarchy: " + classResult.type_hierarchy);
			//				m_ClassifyResultText.text += "\n\t\t\tclass: " + classResult.m_class + ", score: " + classResult.score + ", type_hierarchy: " + classResult.type_hierarchy;
			//			}
			//		}
			//	}
			//}
			//else
			//{
			//	Log.Debug("WebCamRecognition", "Classification failed!");
			//}
		}
		#endregion

		#region Detect Faces
		public void DetectFaces(byte[] imageData)
		{
			m_AppData.VisualRecognition.DetectFaces(OnDetectFaces, imageData);
		}

		private void OnDetectFaces(FacesTopLevelMultiple multipleImages, string data)
		{
			if (multipleImages != null)
				m_AppData.DetectFacesResult = multipleImages;

			//if (multipleImages != null)
			//{
			//	if (!m_ClassifyResultGameObject.activeSelf)
			//		m_ClassifyResultGameObject.SetActive(true);

			//	Log.Debug("WebCamRecognition", "\nimages processed: {0}", multipleImages.images_processed);
			//	m_ClassifyResultText.text += string.Format("\nimages processed: {0}", multipleImages.images_processed);
			//	foreach (FacesTopLevelSingle faces in multipleImages.images)
			//	{
			//		Log.Debug("WebCamRecognition", "\tsource_url: {0}, resolved_url: {1}", faces.source_url, faces.resolved_url);
			//		m_ClassifyResultText.text += string.Format("\n\n\tsource_url: {0}, resolved_url: {1}", faces.source_url, faces.resolved_url);

			//		foreach (OneFaceResult face in faces.faces)
			//		{
			//			if (face.face_location != null)
			//			{
			//				Log.Debug("WebCamRecognition", "\t\tFace location: {0}, {1}, {2}, {3}", face.face_location.left, face.face_location.top, face.face_location.width, face.face_location.height);
			//				m_ClassifyResultText.text += string.Format("\n\t\tFace location: {0}, {1}, {2}, {3}", face.face_location.left, face.face_location.top, face.face_location.width, face.face_location.height);
			//			}
			//			if (face.gender != null)
			//			{
			//				Log.Debug("WebCamRecognition", "\t\tGender: {0}, Score: {1}", face.gender.gender, face.gender.score);
			//				m_ClassifyResultText.text += string.Format("\n\t\tGender: {0}, Score: {1}", face.gender.gender, face.gender.score);
			//			}
			//			if (face.age != null)
			//			{
			//				Log.Debug("WebCamRecognition", "\t\tAge Min: {0}, Age Max: {1}, Score: {2}", face.age.min, face.age.max, face.age.score);
			//				m_ClassifyResultText.text += string.Format("\n\t\tAge Min: {0}, Age Max: {1}, Score: {2}", face.age.min, face.age.max, face.age.score);
			//			}
			//			if (face.identity != null)
			//			{
			//				Log.Debug("WebCamRecognition", "\t\tName: {0}, Score: {1}, Type Heiarchy: {2}", face.identity.name, face.identity.score, face.identity.type_hierarchy);
			//				m_ClassifyResultText.text += string.Format("\n\t\tName: {0}, Score: {1}, Type Heiarchy: {2}", face.identity.name, face.identity.score, face.identity.type_hierarchy);
			//			}
			//		}
			//	}
			//}
			//else
			//{
			//	Log.Debug("WebCamRecognition", "Detect faces failed!");
			//}
		}
		#endregion

		#region RecognizeText
		public void RecognizeText(byte[] imageData)
		{
			m_AppData.VisualRecognition.RecognizeText(OnRecognizeText, imageData);
		}

		private void OnRecognizeText(TextRecogTopLevelMultiple multipleImages, string data)
		{
			if (multipleImages != null)
				m_AppData.RecognizeTextResult = multipleImages;

			//if (multipleImages != null)
			//{
			//	if (!m_ClassifyResultGameObject.activeSelf)
			//		m_ClassifyResultGameObject.SetActive(true);

			//	Log.Debug("WebCamRecognition", "\nimages processed: {0}", multipleImages.images_processed);
			//	m_ClassifyResultText.text += string.Format("\nimages processed: {0}", multipleImages.images_processed);
			//	foreach (TextRecogTopLevelSingle texts in multipleImages.images)
			//	{
			//		Log.Debug("WebCamRecognition", "\tsource_url: {0}, resolved_url: {1}", texts.source_url, texts.resolved_url);
			//		m_ClassifyResultText.text += string.Format("\n\n\tsource_url: {0}, resolved_url: {1}", texts.source_url, texts.resolved_url);
			//		Log.Debug("WebCamRecognition", "\ttext: {0}", texts.text);
			//		m_ClassifyResultText.text += string.Format("\n\ttext: {0}", texts.text);
			//		foreach (TextRecogOneWord text in texts.words)
			//		{
			//			Log.Debug("WebCamRecognition", "\t\ttext location: {0}, {1}, {2}, {3}", text.location.left, text.location.top, text.location.width, text.location.height);
			//			m_ClassifyResultText.text += string.Format("\n\t\ttext location: {0}, {1}, {2}, {3}", text.location.left, text.location.top, text.location.width, text.location.height);
			//			Log.Debug("WebCamRecognition", "\t\tLine number: {0}", text.line_number);
			//			m_ClassifyResultText.text += string.Format("\n\t\tLine number: {0}", text.line_number);
			//			Log.Debug("WebCamRecognition", "\t\tword: {0}, Score: {1}", text.word, text.score);
			//			m_ClassifyResultText.text += string.Format("\n\t\tword: {0}, Score: {1}", text.word, text.score);
			//		}
			//	}
			//}
			//else
			//{
			//	Log.Debug("WebCamRecognition", "RecognizeText failed!");
			//}
		}
		#endregion
	}
}
