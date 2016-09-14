﻿/**
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
using System.Collections;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class TrainingSetPanel : MonoBehaviour
	{
		#region Private Data
		[SerializeField]
		private RawImage m_RawImage;
		[SerializeField]
		private InputField m_ClassField;
		#endregion

		#region Public Properties
		public AppData.TrainingSet TrainingSet
		{
			get { return m_TrainingSet; }
			set
			{
				m_TrainingSet = value;
				m_RawImage.texture = TrainingSet.images[0];
			}
		}
		private AppData.TrainingSet m_TrainingSet;
		#endregion

		#region Constructor and Destructor
		#endregion

		#region Awake / Start / Enable / Disable
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		#endregion

		#region Event Handlers
		#endregion
	}
}