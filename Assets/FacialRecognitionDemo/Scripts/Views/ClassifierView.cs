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
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using System;

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
    public class ClassifierView : View
    {
		#region Private Data
		[SerializeField]
		private GameObject m_ClassItemPrefab;
		[SerializeField]
		private RectTransform m_ClassListRectTransform;
		[SerializeField]
		private Text m_ClassifierIDLabel;
		[SerializeField]
		private Text m_ClassifierNameLabel;
		[SerializeField]
		private Text m_ClassifierStatusLabel;
		[SerializeField]
		private Text m_ClassifierCreatedLabel;
		[SerializeField]
		private Toggle m_UseClassiferToClassifyToggle;
		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets this ClassifierView's verbose data. Initiates population of UI data fields.
		/// </summary>
		public GetClassifiersPerClassifierVerbose ClassifierVerbose
		{
			get { return m_ClassifierVerbose; }
			set
			{
				m_ClassifierVerbose = value;
				ClassifierID = ClassifierVerbose.classifier_id;
				ClassifierName = ClassifierVerbose.name;
				Status = ClassifierVerbose.status;
				Created = ClassifierVerbose.created;
				Classes = ClassifierVerbose.classes;
			}
		}
		private GetClassifiersPerClassifierVerbose m_ClassifierVerbose;

		/// <summary>
		/// Gets or sets this classifier's identifier.
		/// </summary>
		public string ClassifierID
		{
			get { return m_ClassifierID; }
			set
			{
				m_ClassifierID = value;
				m_ClassifierIDLabel.text = ClassifierID;
			}
		}
		private string m_ClassifierID;

		/// <summary>
		/// Gets or sets thhis classifier's name.
		/// </summary>
		public string ClassifierName
		{
			get { return m_ClassifierName; }
			set
			{
				m_ClassifierName = value;
				m_ClassifierNameLabel.text = ClassifierName;
			}
		}
		private string m_ClassifierName;

		/// <summary>
		/// Gets or sets this classifier's status.
		/// </summary>
		public string Status
		{
			get { return m_Status; }
			set
			{
				m_Status = value;
				m_ClassifierStatusLabel.text = Status;
			}
		}
		private string m_Status;

		/// <summary>
		/// Gets or sets this classifier's created date.
		/// </summary>
		public string Created
		{
			get { return m_Created; }
			set
			{
				m_Created = value;
				m_ClassifierCreatedLabel.text = Created;
			}
		}
		private string m_Created;

		/// <summary>
		/// Gets or sets this classifier's custom classes.
		/// </summary>
		public Class[] Classes
		{
			get { return m_Classes; }
			set
			{
				m_Classes = value;
				foreach(Class classifierClass in Classes)
				{
					GameObject classItem = Instantiate(m_ClassItemPrefab, m_ClassListRectTransform) as GameObject;
					Text classItemText = classItem.GetComponent<Text>();
					classItemText.text = classifierClass.m_Class;
				}
			}
		}
		private Class[] m_Classes;
		
		/// <summary>
		/// UI Event handler for delete classifier button clicked.
		/// </summary>
		public void OnDeleteClassifierButtonClicked()
		{
			m_Controller.ConfirmDeleteClassifier(ClassifierID);
		}
		#endregion

		#region Constructor and Destructor
		/// <summary>
		/// The ClassifierView Constructor.
		/// </summary>
		public ClassifierView()
		{
			if (!m_ViewStates.Contains(AppState.CONFIG))
				m_ViewStates.Add(AppState.CONFIG);
		}
		#endregion

		#region Awake / Start / Enable / Disable
		void OnEnable()
		{
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_ADDED, OnClassifierIDToClassifyWithAdded);
			EventManager.Instance.RegisterEventReceiver(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_REMOVED, OnClassifierIDToClassifyWithRemoved);
		}

		void OnDisable()
		{
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_ADDED, OnClassifierIDToClassifyWithAdded);
			EventManager.Instance.UnregisterEventReceiver(Event.ON_CLASSIFIER_ID_TO_CLASSIFY_WITH_REMOVED, OnClassifierIDToClassifyWithRemoved);
		}
		#endregion

		#region Private Functions
		#endregion

		#region Public Functions
		/// <summary>
		/// UI Event handler for flagging this classifier to use for classification.
		/// </summary>
		/// <param name="val"></param>
		public void OnUseClassifierValueChanged(bool val)
		{
			if (val)
			{
				if (!m_AppData.ClassifierIDsToClassifyWith.Contains(ClassifierID))
					m_AppData.ClassifierIDsToClassifyWith.Add(ClassifierID);
			}
			else
			{
				if (m_AppData.ClassifierIDsToClassifyWith.Contains(ClassifierID))
					m_AppData.ClassifierIDsToClassifyWith.Remove(ClassifierID);
			}
		}
		#endregion

		#region Event Handlers
		private void OnClassifierIDToClassifyWithAdded(object[] args)
		{
			if(args[0] is string)
			{
				if (args[0] as string == ClassifierID)
					m_UseClassiferToClassifyToggle.isOn = true;
			}
			else
			{
				throw new ArgumentException("Arguemnt was of an unexpected type!");
			}
		}

		private void OnClassifierIDToClassifyWithRemoved(object[] args)
		{
			if (args[0] is string)
			{
				if (args[0] as string == ClassifierID)
					m_UseClassiferToClassifyToggle.isOn = false;
			}
			else
			{
				throw new ArgumentException("Arguemnt was of an unexpected type!");
			}
		}
		#endregion
	}
}
