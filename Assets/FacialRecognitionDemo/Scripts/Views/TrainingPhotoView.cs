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
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class TrainingPhotoView : View
	{
		#region Private Data
		[SerializeField]
		private WebCamWidget m_WebCamWidget;
		[SerializeField]
		private WebCamDisplayWidget m_WebCamDisplayWidget;
		[SerializeField]
		private AspectRatioFitter m_RawImageAspectRatioFitter;
		[SerializeField]
		private RectTransform m_RawImageRectTransform;
		[SerializeField]
		private RawImage m_RawImage;
        [SerializeField]
        private Button m_TakePhotoButton;

		private AppData.TrainingSet m_TrainingSet;
        private float m_PhotoInterval = 0.5f;
        #endregion

        #region Public Properties
        #endregion

        #region Constructor and Destructor
        /// <summary>
        /// TrainingPhotoView Constructor.
        /// </summary>
        public TrainingPhotoView()
		{
			if (!m_ViewStates.Contains(AppState.CREATE_TRAINING_SET))
				m_ViewStates.Add(AppState.CREATE_TRAINING_SET);
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
			Runnable.Run(ActivateWebcam());

			m_WebCamDisplayWidget.RawImage = m_RawImage;
            m_TakePhotoButton.interactable = true;

            //EventManager.Instance.RegisterEventReceiver(Event.ON_TRAINING_SET_ADDED, OnTrainingSetAdded);
            //EventManager.Instance.RegisterEventReceiver(Event.ON_TRAINING_SET_REMOVED, OnTrainingSetRemoved);
        }

		void OnDisable()
		{
			Runnable.Run(DeactivateWebcam());
            m_TakePhotoButton.interactable = false;

            //EventManager.Instance.UnregisterEventReceiver(Event.ON_TRAINING_SET_ADDED, OnTrainingSetAdded);
            //EventManager.Instance.UnregisterEventReceiver(Event.ON_TRAINING_SET_REMOVED, OnTrainingSetRemoved);
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

		private IEnumerator TakeTrainingPhotos(float intervalTime)
		{
			yield return new WaitForEndOfFrame();
			while (m_TrainingSet.imagesData.Count < 12)
			{
				yield return new WaitForSeconds(intervalTime);
				Log.Debug("TrainingPhotoView", "Taking photo!");
				Texture2D image = new Texture2D(m_AppData.WebCameraDimensions.Width, m_AppData.WebCameraDimensions.Height, TextureFormat.RGB24, false);
				image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());
				image.Apply();

				m_TrainingSet.images.Add(image);
				m_TrainingSet.imagesData.Add(image.EncodeToPNG());
			}

			Log.Debug("TrainingPhotoView", "Photo session complete!");
			//m_AppData.TrainingSets.Add(m_TrainingSet);
			m_AppData.TempTrainingSet = m_TrainingSet;

			if (m_AppData.AppState == AppState.CREATE_TRAINING_SET)
				m_AppData.AppState = AppState.REVIEW_TRAINING_SET;
		}
		#endregion

		#region Public Functions
		/// <summary>
		/// UI button handler for cancel button clicked.
		/// </summary>
		public void OnCancelButtonClicked()
		{
			if (m_AppData.AppState == AppState.CREATE_TRAINING_SET)
				m_AppData.AppState = AppState.TRAIN;
		}

		/// <summary>
		/// UI button handler for take photo button clicked.
		/// </summary>
		public void OnTakePhotoButtonClicked()
		{
            m_TakePhotoButton.interactable = false;
			m_TrainingSet = new AppData.TrainingSet();
            Runnable.Run(TakeTrainingPhotos(m_PhotoInterval));
		}
		#endregion

		#region Event Handlers
		private void OnWebCameraDimensionsUpdated(object[] args = null)
		{
			m_WebCamWidget.RequestedWidth = m_AppData.WebCameraDimensions.Width;
			m_WebCamWidget.RequestedHeight = m_AppData.WebCameraDimensions.Height;
			m_RawImageAspectRatioFitter.aspectRatio = m_AppData.WebCameraDimensions.GetAspectRatio();

			m_AppData.ScaleFactor = m_RawImageRectTransform.rect.width / (float)m_WebCamWidget.RequestedWidth;
		}

		private void OnImageToClassify(object[] args = null)
		{
			if (m_AppData.AppState == AppState.PHOTO)
				m_AppData.AppState = AppState.CLASSIFY_RESULT;
		}

		//private void OnTrainingSetAdded(object[] args)
		//{
		//	if(args[0] is AppData.TrainingSet)
		//	{
				
		//	}
		//}

		//private void OnTrainingSetRemoved(object[] args)
		//{
		//	if (args[0] is AppData.TrainingSet)
		//	{

		//	}
		//}
		#endregion
	}
}
