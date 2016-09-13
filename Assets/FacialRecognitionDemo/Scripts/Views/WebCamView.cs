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
using System.Collections;
using IBM.Watson.DeveloperCloud.Widgets;
using IBM.Watson.DeveloperCloud.Utilities;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class WebCamView : View
	{
		#region Private Data
		[SerializeField]
		private WebCamWidget m_WebCamWidget;
		[SerializeField]
		private WebCamDisplayWidget m_WebCamDisplayWidget;
        [SerializeField]
        private AspectRatioFitter m_RawImageAspectRatioFitter;
		[SerializeField]
		private Text m_ClassifyResultText;
		[SerializeField]
		private GameObject m_ClassifyResultGameObject;
		[SerializeField]
		private RawImage m_ResultImage;
		[SerializeField]
		private AspectRatioFitter m_ResultImageAspectRatioFitter;
		[SerializeField]
		private GameObject m_ResultBlackBackground;
        #endregion

        #region Public Properties
        #endregion

        #region Constructor and Destructor
        /// <summary>
        /// WebCamView Constructor.
        /// </summary>
        public WebCamView()
		{
			if (!m_ViewStates.Contains(AppState.PHOTO))
				m_ViewStates.Add(AppState.PHOTO);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void Start()
		{
            EventManager.Instance.RegisterEventReceiver(Event.ON_WEB_CAMERA_DIMENSIONS_UPDATED, OnWebCameraDimensionsUpdated);
            Runnable.Run(DeactivateWebcam());
		}

		void OnEnable()
		{
            Runnable.Run(ActivateWebcam());
		}

		void OnDisable()
		{
			Runnable.Run(DeactivateWebcam());
		}
		#endregion

		#region Private Functions
		private IEnumerator ActivateWebcam()
		{
			yield return new WaitForSeconds(0.1f);
            m_AppData.WebCameraDimensions = new AppData.CameraDimensions(640, 480);
            m_WebCamWidget.ActivateWebCam();
		}

		private IEnumerator DeactivateWebcam()
		{
			yield return new WaitForSeconds(0.1f);
			m_WebCamWidget.DeactivateWebCam();
		}

        private void Classify(byte[] imageData)
        {
            string[] owners = { "IBM", "me" };
            m_AppData.VisualRecognition.Classify(OnClassify, imageData, owners, m_AppData.ClassifierIDsToClassifyWith.ToArray());
        }

        private void DetectFaces(byte[] imageData)
        {
            m_AppData.VisualRecognition.DetectFaces(OnDetectFaces, imageData);
        }

        private void RecognizeText(byte[] imageData)
        {
            m_AppData.VisualRecognition.RecognizeText(OnRecognizeText, imageData);
        }

		private void ClearClassifyResult()
		{
			m_ClassifyResultText.text = "";
			m_ClassifyResultGameObject.SetActive(false);
		}

		private IEnumerator TakePhoto()
		{
			yield return new WaitForEndOfFrame();

			m_ResultBlackBackground.SetActive(true);
			m_ResultImage.gameObject.SetActive(true);
			Texture2D image = new Texture2D(m_AppData.WebCameraDimensions.Width, m_AppData.WebCameraDimensions.Height, TextureFormat.RGB24, false);
			image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());
			image.Apply();

			byte[] imageData = image.EncodeToPNG();


			m_ResultImage.texture = image;

			if (m_AppData.Endpoints.Contains(Endpoint.CLASSIFY))
				Classify(imageData);

			if (m_AppData.Endpoints.Contains(Endpoint.DETECT_FACES))
				DetectFaces(imageData);

			if (m_AppData.Endpoints.Contains(Endpoint.RECOGNIZE_TEXT))
				RecognizeText(imageData);
		}
        #endregion

        #region Public Functions
        /// <summary>
        /// UI button handler for options button clicked.
        /// </summary>
        public void OnOptionsButtonClicked()
		{
			if (m_AppData.AppState == AppState.PHOTO)
				m_AppData.AppState = AppState.CONFIG;
		}

        /// <summary>
        /// UI button handler for take photo button clicked.
        /// </summary>
        public void OnTakePhotoButtonClicked()
        {
			Runnable.Run(TakePhoto());
        }

		/// <summary>
		/// UI button handler for closing the result image.
		/// </summary>
		public void OnResultImageCloseButtonClicked()
		{
			ClearClassifyResult();
			m_ResultImage.texture = null;
			m_ResultImage.gameObject.SetActive(false);
			m_ResultBlackBackground.SetActive(false);
		}
        #endregion

        #region Event Handlers
        private void OnWebCameraDimensionsUpdated(object[] args = null)
        {
            m_WebCamWidget.RequestedWidth = m_AppData.WebCameraDimensions.Width;
            m_WebCamWidget.RequestedHeight = m_AppData.WebCameraDimensions.Height;
            m_RawImageAspectRatioFitter.aspectRatio = m_ResultImageAspectRatioFitter.aspectRatio = m_AppData.WebCameraDimensions.GetAspectRatio();
        }

        private void OnClassify(ClassifyTopLevelMultiple classify, string data)
        {
            if (classify != null)
            {
				if(!m_ClassifyResultGameObject.activeSelf)
					m_ClassifyResultGameObject.SetActive(true);

                Log.Debug("WebCamRecognition", "\nimages processed: " + classify.images_processed);
				m_ClassifyResultText.text += "\nimages processed: " + classify.images_processed;
				foreach (ClassifyTopLevelSingle image in classify.images)
                {
                    Log.Debug("WebCamRecognition", "\tsource_url: " + image.source_url + ", resolved_url: " + image.resolved_url);
					m_ClassifyResultText.text += "\n\tsource_url: " + image.source_url + ", resolved_url: " + image.resolved_url;
					foreach (ClassifyPerClassifier classifier in image.classifiers)
                    {
                        Log.Debug("WebCamRecognition", "\t\tclassifier_id: " + classifier.classifier_id + ", name: " + classifier.name);
						m_ClassifyResultText.text += "\n\n\t\tclassifier_id: " + classifier.classifier_id + ", name: " + classifier.name;

						foreach (ClassResult classResult in classifier.classes)
						{
							Log.Debug("WebCamRecognition", "\t\t\tclass: " + classResult.m_class + ", score: " + classResult.score + ", type_hierarchy: " + classResult.type_hierarchy);
							m_ClassifyResultText.text += "\n\t\t\tclass: " + classResult.m_class + ", score: " + classResult.score + ", type_hierarchy: " + classResult.type_hierarchy;
						}
                    }
                }
            }
            else
            {
                Log.Debug("WebCamRecognition", "Classification failed!");
            }
        }

        private void OnDetectFaces(FacesTopLevelMultiple multipleImages, string data)
        {
            if (multipleImages != null)
            {
				if (!m_ClassifyResultGameObject.activeSelf)
					m_ClassifyResultGameObject.SetActive(true);

				Log.Debug("WebCamRecognition", "\nimages processed: {0}", multipleImages.images_processed);
				m_ClassifyResultText.text += string.Format("\nimages processed: {0}", multipleImages.images_processed);
				foreach (FacesTopLevelSingle faces in multipleImages.images)
                {
                    Log.Debug("WebCamRecognition", "\tsource_url: {0}, resolved_url: {1}", faces.source_url, faces.resolved_url);
					m_ClassifyResultText.text += string.Format("\n\n\tsource_url: {0}, resolved_url: {1}", faces.source_url, faces.resolved_url);

					foreach (OneFaceResult face in faces.faces)
					{
						if (face.face_location != null)
						{
							Log.Debug("WebCamRecognition", "\t\tFace location: {0}, {1}, {2}, {3}", face.face_location.left, face.face_location.top, face.face_location.width, face.face_location.height);
							m_ClassifyResultText.text += string.Format("\n\t\tFace location: {0}, {1}, {2}, {3}", face.face_location.left, face.face_location.top, face.face_location.width, face.face_location.height);
						}
						if (face.gender != null)
						{
							Log.Debug("WebCamRecognition", "\t\tGender: {0}, Score: {1}", face.gender.gender, face.gender.score);
							m_ClassifyResultText.text += string.Format("\n\t\tGender: {0}, Score: {1}", face.gender.gender, face.gender.score);
						}
						if (face.age != null)
						{
							Log.Debug("WebCamRecognition", "\t\tAge Min: {0}, Age Max: {1}, Score: {2}", face.age.min, face.age.max, face.age.score);
							m_ClassifyResultText.text += string.Format("\n\t\tAge Min: {0}, Age Max: {1}, Score: {2}", face.age.min, face.age.max, face.age.score);
						}
						if (face.identity != null)
						{
							Log.Debug("WebCamRecognition", "\t\tName: {0}, Score: {1}, Type Heiarchy: {2}", face.identity.name, face.identity.score, face.identity.type_hierarchy);
							m_ClassifyResultText.text += string.Format("\n\t\tName: {0}, Score: {1}, Type Heiarchy: {2}", face.identity.name, face.identity.score, face.identity.type_hierarchy);
						}
					}
                }
            }
            else
            {
                Log.Debug("WebCamRecognition", "Detect faces failed!");
            }
        }

        private void OnRecognizeText(TextRecogTopLevelMultiple multipleImages, string data)
        {
            if (multipleImages != null)
            {
				if (!m_ClassifyResultGameObject.activeSelf)
					m_ClassifyResultGameObject.SetActive(true);

				Log.Debug("WebCamRecognition", "\nimages processed: {0}", multipleImages.images_processed);
				m_ClassifyResultText.text += string.Format("\nimages processed: {0}", multipleImages.images_processed);
                foreach (TextRecogTopLevelSingle texts in multipleImages.images)
                {
                    Log.Debug("WebCamRecognition", "\tsource_url: {0}, resolved_url: {1}", texts.source_url, texts.resolved_url);
					m_ClassifyResultText.text += string.Format("\n\n\tsource_url: {0}, resolved_url: {1}", texts.source_url, texts.resolved_url);
                    Log.Debug("WebCamRecognition", "\ttext: {0}", texts.text);
					m_ClassifyResultText.text += string.Format("\n\ttext: {0}", texts.text);
                    foreach (TextRecogOneWord text in texts.words)
                    {
                        Log.Debug("WebCamRecognition", "\t\ttext location: {0}, {1}, {2}, {3}", text.location.left, text.location.top, text.location.width, text.location.height);
						m_ClassifyResultText.text += string.Format("\n\t\ttext location: {0}, {1}, {2}, {3}", text.location.left, text.location.top, text.location.width, text.location.height);
						Log.Debug("WebCamRecognition", "\t\tLine number: {0}", text.line_number);
						m_ClassifyResultText.text += string.Format("\n\t\tLine number: {0}", text.line_number);
						Log.Debug("WebCamRecognition", "\t\tword: {0}, Score: {1}", text.word, text.score);
						m_ClassifyResultText.text += string.Format("\n\t\tword: {0}, Score: {1}", text.word, text.score);
					}
				}
            }
            else
            {
                Log.Debug("WebCamRecognition", "RecognizeText failed!");
            }
        }
        #endregion
    }
}
