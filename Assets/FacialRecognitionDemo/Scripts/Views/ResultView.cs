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
	public class ResultView : View
	{
		#region Private Data
		[SerializeField]
		private Text m_ClassifyResultText;
		[SerializeField]
		private GameObject m_ClassifyResultGameObject;
		[SerializeField]
		private RawImage m_ResultImage;
		[SerializeField]
		private AspectRatioFitter m_ResultImageAspectRatioFitter;
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		public ResultView()
		{
			if (!m_ViewStates.Contains(AppState.CLASSIFY_RESULT))
				m_ViewStates.Add(AppState.CLASSIFY_RESULT);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void Start()
		{
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
			m_ClassifyResultText.text = "";
			m_AppData.Image = null;
			m_AppData.ClassifyResult = null;
			m_AppData.DetectFacesResult = null;
			m_AppData.RecognizeTextResult = null;
			m_ResultImage.texture = null;
			m_ClassifyResultGameObject.SetActive(false);
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
			if (!m_ClassifyResultGameObject.activeSelf)
				m_ClassifyResultGameObject.SetActive(true);
		}

		private void OnDetectFacesResult(object[] args = null)
		{
			if (!m_ClassifyResultGameObject.activeSelf)
				m_ClassifyResultGameObject.SetActive(true);
		}

		private void OnRecognizeTextResult(object[] args = null)
		{
			if (!m_ClassifyResultGameObject.activeSelf)
				m_ClassifyResultGameObject.SetActive(true);
		}
		#endregion
	}
}
