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
            EventManager.Instance.RegisterEventReceiver(Event.ON_IMAGE_TO_CLASSIFY, OnImageToClassify);
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
		
		private IEnumerator TakePhoto()
		{
			yield return new WaitForEndOfFrame();
			Texture2D image = new Texture2D(m_AppData.WebCameraDimensions.Width, m_AppData.WebCameraDimensions.Height, TextureFormat.RGB24, false);
			image.SetPixels32(m_WebCamWidget.WebCamTexture.GetPixels32());
			image.Apply();

			m_AppData.Image = image;
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
        #endregion

        #region Event Handlers
        private void OnWebCameraDimensionsUpdated(object[] args = null)
        {
            m_WebCamWidget.RequestedWidth = m_AppData.WebCameraDimensions.Width;
            m_WebCamWidget.RequestedHeight = m_AppData.WebCameraDimensions.Height;
            m_RawImageAspectRatioFitter.aspectRatio = m_AppData.WebCameraDimensions.GetAspectRatio();
        }

		private void OnImageToClassify(object[] args = null)
		{
			if (m_AppData.AppState == AppState.PHOTO)
				m_AppData.AppState = AppState.CLASSIFY_RESULT;
		}
        #endregion
    }
}
