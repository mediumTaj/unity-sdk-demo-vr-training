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
using System.Collections;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
	public class ClassifiersView : View
	{
		#region Private Data
		[SerializeField]
		private GameObject m_ClassifierViewPrefab;
		#endregion

		#region Public Properties
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_ADDED, OnClassifierVerboseAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_REMOVED, OnClassifierVerboseRemoved);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_ADDED, OnClassifierVerboseAdded);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_VERBOSE_REMOVED, OnClassifierVerboseRemoved);
		}
		#endregion

		#region Event Handlers
		private void OnClassifierVerboseAdded(object[] args)
		{
			if(args[0] is GetClassifiersPerClassifierVerbose)
			{
				GameObject classifierVerboseGameObject = GameObject.Instantiate(m_ClassifierViewPrefab, gameObject.GetComponent<RectTransform>()) as GameObject;
				ClassifierView classifierView = classifierVerboseGameObject.GetComponent<ClassifierView>();
				classifierView.ClassifierVerbose = args[0] as GetClassifiersPerClassifierVerbose;
			}
		}

		private void OnClassifierVerboseRemoved(object[] args)
		{
			if (args[0] is GetClassifiersPerClassifierVerbose)
			{

			}
		}
		#endregion
	}
}
