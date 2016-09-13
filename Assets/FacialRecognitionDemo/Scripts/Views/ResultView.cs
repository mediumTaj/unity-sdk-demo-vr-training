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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	using UnityEngine;
	using System.Collections;
	using UnityEngine.UI;
	using Utilities;
	using System.Collections.Generic;
	using Services.VisualRecognition.v3;
	using Logging;

	/// <summary>
	/// This class shows the results from Classification, DetectFaces and RecognizeText.
	/// </summary>
	public class ResultView : View
	{
		#region Private Data
		[SerializeField]
		private RectTransform m_ResultContentRectTransform;
		//[SerializeField]
		//private GameObject m_ClassifyResultGameObject;
		[SerializeField]
		private RawImage m_ResultImage;
		[SerializeField]
		private AspectRatioFitter m_ResultImageAspectRatioFitter;
		[SerializeField]
		private GameObject m_ClassifyResultsPrefab;
		[SerializeField]
		private GameObject m_DetectFacesResultsPrefab;
		[SerializeField]
		private GameObject m_RecognizeTextReultsPrefab;
		[SerializeField]
		private GameObject m_FaceOutlinePanel;
		[SerializeField]
		private GameObject m_TextOutlinePanel;
		private List<GameObject> m_ResultGameObjectList = new List<GameObject>();
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// The ResultView constructor.
		/// </summary>
		public ResultView()
		{
			if (!m_ViewStates.Contains(AppState.CLASSIFY_RESULT))
				m_ViewStates.Add(AppState.CLASSIFY_RESULT);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		protected override void Awake()
		{
			base.Awake();
			EventManager.Instance.RegisterEventReceiver(Event.ON_WEB_CAMERA_DIMENSIONS_UPDATED, OnWebCameraDimensionsUpdated);
			EventManager.Instance.RegisterEventReceiver(Event.ON_IMAGE_TO_CLASSIFY, OnImageToClassify);
		}

		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFICATION_RESULT, OnClassificationResult);
			EventManager.Instance.RegisterEventReceiver(Event.ON_DETECT_FACES_RESULT, OnDetectFacesResult);
			EventManager.Instance.RegisterEventReceiver(Event.ON_RECOGNIZE_TEXT_RESULT, OnRecognizeTextResult);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFICATION_RESULT, OnClassificationResult);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_DETECT_FACES_RESULT, OnDetectFacesResult);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_RECOGNIZE_TEXT_RESULT, OnRecognizeTextResult);
		}
		#endregion

		#region Private Functions
		private void ClearClassifyResult()
		{
			m_AppData.Image = null;
			m_AppData.ClassifyResult = null;
			m_AppData.DetectFacesResult = null;
			m_AppData.RecognizeTextResult = null;
			m_ResultImage.texture = null;
			while (m_ResultGameObjectList.Count > 0)
			{
				Destroy(m_ResultGameObjectList[0]);
				m_ResultGameObjectList.RemoveAt(0);
			}
			//m_ClassifyResultGameObject.SetActive(false);
		}
		#endregion

		#region Public Functions
		/// <summary>
		/// UI button handler for closing the result image.
		/// </summary>
		public void OnCloseButtonClicked()
		{
			ClearClassifyResult();
			if (m_AppData.AppState == AppState.CLASSIFY_RESULT)
				m_AppData.AppState = AppState.PHOTO;
		}
		#endregion

		#region Event Handlers
		private void OnWebCameraDimensionsUpdated(object[] args = null)
		{
			m_ResultImageAspectRatioFitter.aspectRatio = m_AppData.WebCameraDimensions.GetAspectRatio();
		}

		private void OnImageToClassify(object[] args = null)
		{
			if (m_AppData.Image != null)
			{
				m_ResultImage.texture = m_AppData.Image;

				if (m_AppData.Endpoints.Contains(Endpoint.CLASSIFY))
					m_Controller.Classify(m_AppData.ImageData);

				if (m_AppData.Endpoints.Contains(Endpoint.DETECT_FACES))
					m_Controller.DetectFaces(m_AppData.ImageData);

				if (m_AppData.Endpoints.Contains(Endpoint.RECOGNIZE_TEXT))
					m_Controller.RecognizeText(m_AppData.ImageData);
			}
		}

		private void OnClassificationResult(object[] args = null)
		{
			//if (!m_ClassifyResultGameObject.activeSelf)
			//	m_ClassifyResultGameObject.SetActive(true);

			GameObject classifyResult = Instantiate(m_ClassifyResultsPrefab, m_ResultContentRectTransform) as GameObject;
			m_ResultGameObjectList.Add(classifyResult);

		}

		private void OnDetectFacesResult(object[] args = null)
		{
			if (m_AppData.DetectFacesResult == null)
				return;

			//if (!m_ClassifyResultGameObject.activeSelf)
			//	m_ClassifyResultGameObject.SetActive(true);

			GameObject detectFacesResult = Instantiate(m_DetectFacesResultsPrefab, m_ResultContentRectTransform) as GameObject;
			m_ResultGameObjectList.Add(detectFacesResult);

			RectTransform resultRectTransform = m_ResultImage.GetComponent<RectTransform>();

			if(m_AppData.DetectFacesResult.images != null && m_AppData.DetectFacesResult.images.Length > 0 && m_AppData.DetectFacesResult.images[0].faces != null &&  m_AppData.DetectFacesResult.images[0].faces.Length > 0)
				foreach (OneFaceResult faceResult in m_AppData.DetectFacesResult.images[0].faces)
				{
					GameObject detectFacesOutline = Instantiate(m_FaceOutlinePanel, resultRectTransform) as GameObject;
					FaceOutline faceOutline = detectFacesOutline.GetComponent<FaceOutline>();
					if (faceOutline != null)
						faceOutline.FaceResult = faceResult;
					else
						Log.Warning("ResultView", "No face outline script found!");
					m_ResultGameObjectList.Add(detectFacesOutline);
				}
		}
		
		private void OnRecognizeTextResult(object[] args = null)
		{
			if (m_AppData.RecognizeTextResult == null)
				return;

			//if (!m_ClassifyResultGameObject.activeSelf)
			//	m_ClassifyResultGameObject.SetActive(true);

			GameObject recognizeTextResult = Instantiate(m_RecognizeTextReultsPrefab, m_ResultContentRectTransform) as GameObject;
			m_ResultGameObjectList.Add(recognizeTextResult);

			RectTransform resultRectTransform = m_ResultImage.GetComponent<RectTransform>();

			if (m_AppData.RecognizeTextResult.images != null && m_AppData.RecognizeTextResult.images.Length > 0 && m_AppData.RecognizeTextResult.images[0].words != null && m_AppData.RecognizeTextResult.images[0].words.Length > 0)
				foreach (TextRecogOneWord textResult in m_AppData.RecognizeTextResult.images[0].words)
				{
					GameObject detectTextOutline = Instantiate(m_TextOutlinePanel, resultRectTransform) as GameObject;
					TextOutline textOutline = detectTextOutline.GetComponent<TextOutline>();
					if (textOutline != null)
						textOutline.TextResult = textResult;
					else
						Log.Warning("ResultView", "No text outline script found!");
					m_ResultGameObjectList.Add(detectTextOutline);
				}
		}
		#endregion
	}
}
