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
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class outlines faces from DetectFaces and shows results data.
	/// </summary>
	public class FaceOutline : MonoBehaviour
	{
		#region Private Data
		[SerializeField]
		private RectTransform m_FaceOutlinePanelRectTransform;
		[SerializeField]
		private RectTransform m_OutlineRectTransform;
		[SerializeField]
		private RectTransform m_TextHolderRectTransform;
		[SerializeField]
		private Text m_Text;
		string textString = "Age: {0} to {1}, Score: {2}\nGender: {3}, Score:{4}\nIdentity: {5}, Score: {6}\nTypeHierarchy: {7}";
		#endregion

		#region Public Propertiers
		/// <summary>
		/// The face data for this panel.
		/// </summary>
		public OneFaceResult FaceResult
		{
			get { return m_FaceResult; }
			set
			{
				m_FaceResult = value;

				UpdateText();
				UpdateRect();
			}
		}
		private OneFaceResult m_FaceResult;

		/// <summary>
		/// The position of this panel. This is adjusted for scale factor
		/// </summary>
		public Vector2 PanelPosition
		{
			get { return m_PanelPosition; }
			set
			{
				m_PanelPosition = value * AppData.Instance.ScaleFactor;
				m_FaceOutlinePanelRectTransform.anchoredPosition = PanelPosition;
			}
		}
		private Vector2 m_PanelPosition;

		/// <summary>
		/// The dimensions of this panel. This is adjusted for scale factor.
		/// </summary>
		public Vector2 OutlineDimensions
		{
			get { return m_OutlineDimensions; }
			set
			{
				m_OutlineDimensions = value * AppData.Instance.ScaleFactor;
				m_OutlineRectTransform.sizeDelta = OutlineDimensions;

				m_TextHolderRectTransform.localPosition = new Vector2(m_TextHolderRectTransform.anchoredPosition.x, -m_OutlineRectTransform.sizeDelta.y);
				m_TextHolderRectTransform.sizeDelta = new Vector2(m_OutlineRectTransform.sizeDelta.x, m_TextHolderRectTransform.sizeDelta.y);
			}
		}
		private Vector2 m_OutlineDimensions;
		#endregion

		#region Constructor and Destructor
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_UPDATE_SCALE_FACTOR, OnUpdateScaleFactor);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.ON_UPDATE_SCALE_FACTOR, OnUpdateScaleFactor);

		}
		#endregion
		
		#region Private Functions
		private void UpdateRect()
		{
			PanelPosition = new Vector2((float)FaceResult.face_location.left, -(float)FaceResult.face_location.top);
			OutlineDimensions = new Vector2((float)FaceResult.face_location.width, (float)FaceResult.face_location.height);
		}

		private void UpdateText()
		{
			m_Text.text = string.Format(textString,
				FaceResult.age.min.ToString(),
				FaceResult.age.max.ToString(),
				FaceResult.age.score.ToString("F2"),
				FaceResult.gender.gender,
				FaceResult.gender.score.ToString("F2"),
				FaceResult.identity != null && !string.IsNullOrEmpty(FaceResult.identity.name) ? FaceResult.identity.name : "?",
				FaceResult.identity != null && FaceResult.identity.score != default(double) ? FaceResult.identity.score.ToString("F2") : "?",
				FaceResult.identity != null && !string.IsNullOrEmpty(FaceResult.identity.type_hierarchy) ? FaceResult.identity.type_hierarchy : "?"
				);
		}

		private void OnUpdateScaleFactor(object[] args = null)
		{
			Log.Debug("FaceOutline", "OnUpdateScaleFactor: {0}", AppData.Instance.ScaleFactor);
		}
		#endregion

		#region Public Functions
		#endregion

		#region Event Handlers
		#endregion
	}
}
