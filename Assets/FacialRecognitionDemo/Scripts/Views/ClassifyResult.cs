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
	public class ClassifyResult : MonoBehaviour
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
			ClassifyTopLevelMultiple classifyResult = AppData.Instance.ClassifyResult;
			if (AppData.Instance.ClassifyResult!= null)
			{
				m_Text.text += "\nimages processed: " + classifyResult.images_processed;
				foreach (ClassifyTopLevelSingle image in classifyResult.images)
				{
					foreach (ClassifyPerClassifier classifier in image.classifiers)
					{
						m_Text.text += "\n\n\tclassifier_id: " + classifier.classifier_id;
						m_Text.text += "\n\tclassifer name: " + classifier.name;

						foreach (ClassResult classResult in classifier.classes)
						{
							m_Text.text += "\n\t\tclass: " + classResult.m_class + ", score: " + classResult.score;
							if (!string.IsNullOrEmpty(classResult.type_hierarchy))
								m_Text.text += "\n\t\t type_hierarchy: " + classResult.type_hierarchy;
						}
					}
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