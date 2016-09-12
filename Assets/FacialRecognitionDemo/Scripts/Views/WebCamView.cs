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

        private IEnumerator Classify()
        {
            yield return new WaitForEndOfFrame();

            Texture2D image = new Texture2D(m_AppData.WebCameraDimensions.Width, m_AppData.WebCameraDimensions.Height, TextureFormat.RGB24, false);
            image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());

            byte[] imageData = image.EncodeToPNG();
            string[] owners = { "IBM", "me" };
            m_AppData.VisualRecognition.Classify(OnClassify, imageData, owners, m_AppData.ClassifierIDsToClassifyWith.ToArray());
        }

        private IEnumerator DetectFaces()
        {
            yield return new WaitForEndOfFrame();

            Texture2D image = new Texture2D(m_WebCamWidget.WebCamTexture.width, m_WebCamWidget.WebCamTexture.height, TextureFormat.RGB24, false);
            image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());

            byte[] imageData = image.EncodeToPNG();

            m_AppData.VisualRecognition.DetectFaces(OnDetectFaces, imageData);
        }

        private IEnumerator RecognizeText()
        {
            yield return new WaitForEndOfFrame();

            Texture2D image = new Texture2D(m_WebCamWidget.WebCamTexture.width, m_WebCamWidget.WebCamTexture.height, TextureFormat.RGB24, false);
            image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());

            byte[] imageData = image.EncodeToPNG();

            m_AppData.VisualRecognition.RecognizeText(OnRecognizeText, imageData);
        }

        //private TopClass GetTopClass(ClassifyTopLevelMultiple classResults)
        //{
        //    TopClass topClass = new TopClass();
        //    foreach(ClassifyTopLevelSingle classifyToplevelSingle in classResults.images)
        //        foreach(ClassifyPerClassifier classifier in classifyToplevelSingle.classifiers)
        //            foreach(ClassResult classResult in classifier.classes)


        //    return topClass;
        //}

        //private class TopClass
        //{
        //    public string ClassifierName { get; set; }
        //    public string ClassifierID { get; set; }
        //    public string Class { get; set; }
        //    public float Score { get; set; }
        //}
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
            if (m_AppData.Endpoints.Contains(Endpoint.CLASSIFY))
                Runnable.Run(Classify());

            if (m_AppData.Endpoints.Contains(Endpoint.DETECT_FACES))
                Runnable.Run(DetectFaces());

            if (m_AppData.Endpoints.Contains(Endpoint.RECOGNIZE_TEXT))
                Runnable.Run(RecognizeText());
        }
        #endregion

        #region Event Handlers
        private void OnWebCameraDimensionsUpdated(object[] args = null)
        {
            m_WebCamWidget.RequestedWidth = m_AppData.WebCameraDimensions.Width;
            m_WebCamWidget.RequestedHeight = m_AppData.WebCameraDimensions.Height;
            m_RawImageAspectRatioFitter.aspectRatio = m_AppData.WebCameraDimensions.GetAspectRatio();
        }

        private void OnClassify(ClassifyTopLevelMultiple classify, string data)
        {
            if (classify != null)
            {
                Log.Debug("WebCamRecognition", "images processed: " + classify.images_processed);
                foreach (ClassifyTopLevelSingle image in classify.images)
                {
                    Log.Debug("WebCamRecognition", "\tsource_url: " + image.source_url + ", resolved_url: " + image.resolved_url);
                    foreach (ClassifyPerClassifier classifier in image.classifiers)
                    {
                        Log.Debug("WebCamRecognition", "\t\tclassifier_id: " + classifier.classifier_id + ", name: " + classifier.name);
                        foreach (ClassResult classResult in classifier.classes)
                            Log.Debug("WebCamRecognition", "\t\t\tclass: " + classResult.m_class + ", score: " + classResult.score + ", type_hierarchy: " + classResult.type_hierarchy);
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
                Log.Debug("WebCamRecognition", "images processed: {0}", multipleImages.images_processed);
                foreach (FacesTopLevelSingle faces in multipleImages.images)
                {
                    Log.Debug("WebCamRecognition", "\tsource_url: {0}, resolved_url: {1}", faces.source_url, faces.resolved_url);
                    foreach (OneFaceResult face in faces.faces)
                    {
                        if (face.face_location != null)
                            Log.Debug("WebCamRecognition", "\t\tFace location: {0}, {1}, {2}, {3}", face.face_location.left, face.face_location.top, face.face_location.width, face.face_location.height);
                        if (face.gender != null)
                            Log.Debug("WebCamRecognition", "\t\tGender: {0}, Score: {1}", face.gender.gender, face.gender.score);
                        if (face.age != null)
                            Log.Debug("WebCamRecognition", "\t\tAge Min: {0}, Age Max: {1}, Score: {2}", face.age.min, face.age.max, face.age.score);

                        if (face.identity != null)
                            Log.Debug("WebCamRecognition", "\t\tName: {0}, Score: {1}, Type Heiarchy: {2}", face.identity.name, face.identity.score, face.identity.type_hierarchy);
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
                Log.Debug("WebCamRecognition", "images processed: {0}", multipleImages.images_processed);
                foreach (TextRecogTopLevelSingle texts in multipleImages.images)
                {
                    Log.Debug("WebCamRecognition", "\tsource_url: {0}, resolved_url: {1}", texts.source_url, texts.resolved_url);
                    Log.Debug("WebCamRecognition", "\ttext: {0}", texts.text);
                    foreach (TextRecogOneWord text in texts.words)
                    {
                        Log.Debug("WebCamRecognition", "\t\ttext location: {0}, {1}, {2}, {3}", text.location.left, text.location.top, text.location.width, text.location.height);
                        Log.Debug("WebCamRecognition", "\t\tLine number: {0}", text.line_number);
                        Log.Debug("WebCamRecognition", "\t\tword: {0}, Score: {1}", text.word, text.score);
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
