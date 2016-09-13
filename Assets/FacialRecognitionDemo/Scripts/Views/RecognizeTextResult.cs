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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	/// <summary>
	/// This class displays recognize text results in the RecognizeTextResult prefab.
	/// </summary>
	public class RecognizeTextResult : MonoBehaviour
	{
		#region Private Data
		[SerializeField]
		public Text m_Text;
		#endregion

		#region Public Properties
		#endregion

		#region Constructor and Destructor
		#endregion

		#region Awake / Start / Enable / Disable
		void Awake()
		{
			TextRecogTopLevelMultiple multipleImages = AppData.Instance.RecognizeTextResult;
			if (multipleImages != null)
			{
				m_Text.text += string.Format("\nimages processed: {0}", multipleImages.images_processed);
				foreach (TextRecogTopLevelSingle texts in multipleImages.images)
				{
					if (!string.IsNullOrEmpty(texts.text))
					{
						m_Text.text += string.Format("\n\ttext: {0}", texts.text);

						foreach (TextRecogOneWord text in texts.words)
						{
							m_Text.text += string.Format("\n\t\ttext location: {0}, {1}, {2}, {3}", text.location.left, text.location.top, text.location.width, text.location.height);
							m_Text.text += string.Format("\n\t\tLine number: {0}", text.line_number);
							m_Text.text += string.Format("\n\t\tword: {0}, Score: {1}", text.word, text.score);
						}
					}
					else
						m_Text.text += "\n\tNo text detected!";
				}
			}
		}
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		#endregion

		#region Event Handlers
		#endregion
	}
}
