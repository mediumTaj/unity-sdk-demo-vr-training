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
	public class TextOutline : MonoBehaviour
	{
		#region Private Data
		[SerializeField]
		private RectTransform m_TextOutlinePanelRectTransform;
		[SerializeField]
		private RectTransform m_OutlineRectTransform;
		[SerializeField]
		private RectTransform m_TextHolderRectTransform;
		[SerializeField]
		private Text m_Text;
		string textString = "Word: {0}\nScore: {1}\nLine Number: {2}";
		#endregion

		#region Public Propertiers
		/// <summary>
		/// The word data for this panel.
		/// </summary>
		private TextRecogOneWord m_TextResult;
		public TextRecogOneWord TextResult
		{
			get { return m_TextResult; }
			set
			{
				m_TextResult = value;

				UpdateText();
				UpdateRect();
			}
		}

		/// <summary>
		/// The position of this panel. This is adjusted for scale factor
		/// </summary>
		private Vector2 m_PanelPosition;
		public Vector2 PanelPosition
		{
			get { return m_PanelPosition; }
			set
			{
				m_PanelPosition = value * AppData.Instance.ScaleFactor;
				m_TextOutlinePanelRectTransform.anchoredPosition = PanelPosition;
			}
		}

		/// <summary>
		/// The dimensions of this panel. This is adjusted for scale factor.
		/// </summary>
		private Vector2 m_OutlineDimensions;
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
			PanelPosition = new Vector2((float)TextResult.location.left, -(float)TextResult.location.top);
			OutlineDimensions = new Vector2((float)TextResult.location.width, (float)TextResult.location.height);
		}

		private void UpdateText()
		{
			m_Text.text = string.Format(textString,
				TextResult.word,
				TextResult.score,
				TextResult.line_number.ToString()
				);
		}

		private void OnUpdateScaleFactor(object[] args = null)
		{
			Log.Debug("TextOutline", "OnUpdateScaleFactor: {0}", AppData.Instance.ScaleFactor);
		}
		#endregion

		#region Public Functions
		#endregion

		#region Event Handlers
		#endregion
	}
}
