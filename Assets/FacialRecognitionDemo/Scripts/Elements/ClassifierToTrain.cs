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
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using UnityEngine.UI;
using System.Collections.Generic;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class ClassifierToTrain : MonoBehaviour
	{
		#region Private Data
		[SerializeField]
		private Text m_ClassifierName;
		[SerializeField]
		private Text m_ClassifierID;
		[SerializeField]
		private Text m_Classes;
		[SerializeField]
		private Toggle m_TrainingToggle;
		#endregion

		#region Public Properties
		/// <summary>
		/// The verbose classifier data for this object.
		/// </summary>
		public GetClassifiersPerClassifierVerbose ClassifierVerbose
		{
			get { return m_ClassifierVerbose; }
			set
			{
				m_ClassifierVerbose = value;
				m_ClassifierName.text = ClassifierVerbose.name;
				m_ClassifierID.text = ClassifierVerbose.classifier_id;

				if (ClassifierVerbose.classes == null)
				{
					m_Classes.text = "";
				}
				else
				{
					List<string> classList = new List<string>();
					foreach (Class classifierClass in ClassifierVerbose.classes)
					{
						classList.Add(classifierClass.m_Class);
					}

					m_Classes.text = string.Join(", ", classList.ToArray());
				}
			}
		}
		private GetClassifiersPerClassifierVerbose m_ClassifierVerbose;

		/// <summary>
		/// Is this classifier available for training.
		/// </summary>
		public bool CanTrain
		{
			get { return m_CanTrain; }
			set
			{
				m_CanTrain = value;
				m_TrainingToggle.interactable = CanTrain;
			}
		}
		private bool m_CanTrain;
		#endregion

		#region Constructor and Destructor
		#endregion

		#region Awake / Start / Enable / Disable
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		public void OnToggle(bool val)
		{
			if (val)
			{
				if (!AppData.Instance.ClassifierIDsToTrain.Contains(ClassifierVerbose.classifier_id))
					AppData.Instance.ClassifierIDsToTrain.Add(ClassifierVerbose.classifier_id);
			}
			else
			{
				if (AppData.Instance.ClassifierIDsToTrain.Contains(ClassifierVerbose.classifier_id))
					AppData.Instance.ClassifierIDsToTrain.Remove(ClassifierVerbose.classifier_id);
			}
		}
		#endregion

		#region Event Handlers
		#endregion
	}
}
