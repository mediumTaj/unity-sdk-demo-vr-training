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
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class reviews the last training set created.
	/// </summary>
	public class ReviewTrainingSetView : View
	{
		#region Private Data
		[SerializeField]
		private GameObject m_TrainingImagePreviewGameObject;
		[SerializeField]
		private RectTransform m_TrainingImagesHolderRectTransform;
		[SerializeField]
		private GridLayoutGroup m_TrainingImagesHolderGridLayoutGroup;

		private List<GameObject> m_PreviewImages = new List<GameObject>();
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		public ReviewTrainingSetView()
		{
			if (!m_ViewStates.Contains(AppState.REVIEW_TRAINING_SET))
				m_ViewStates.Add(AppState.REVIEW_TRAINING_SET);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
			PopulateImages();
		}
		#endregion

		#region Private Functions
		private void PopulateImages()
		{
			if (m_AppData.TempTrainingSet == null)
				return;

            m_TrainingImagesHolderGridLayoutGroup.cellSize = new Vector2(m_TrainingImagesHolderGridLayoutGroup.cellSize.x, m_TrainingImagesHolderGridLayoutGroup.cellSize.x / m_AppData.WebCameraDimensions.GetAspectRatio());

            foreach (Texture2D image in m_AppData.TempTrainingSet.images)
			{
				GameObject imageGO = Instantiate(m_TrainingImagePreviewGameObject, m_TrainingImagesHolderRectTransform) as GameObject;
				m_PreviewImages.Add(imageGO);

				RawImage rawImage = imageGO.GetComponent<RawImage>();
				rawImage.texture = image;
            }
		}

		private void ClearData()
		{
			while(m_PreviewImages.Count > 0)
			{
				Destroy(m_PreviewImages[0]);
				m_PreviewImages.RemoveAt(0);
			}
		}
		#endregion

		#region Public Functions
		/// <summary>
		/// UI click handler for discard button.
		/// </summary>
		public void OnDiscardButtonClicked()
		{
			m_AppData.TempTrainingSet = null;

			ClearData();

			if (m_AppData.AppState == AppState.REVIEW_TRAINING_SET)
				m_AppData.AppState = AppState.CREATE_TRAINING_SET;
		}

		/// <summary>
		/// UI click handler for accept button.
		/// </summary>
		public void OnAcceptButtonClicked()
		{
			m_AppData.TrainingSets.Add(m_AppData.TempTrainingSet);

			m_AppData.TempTrainingSet = null;

			ClearData();

			if (m_AppData.AppState == AppState.REVIEW_TRAINING_SET)
				m_AppData.AppState = AppState.TRAIN;
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
