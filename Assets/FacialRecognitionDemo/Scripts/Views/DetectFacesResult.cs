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
	public class DetectFacesResult : MonoBehaviour
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
			FacesTopLevelMultiple multipleImages = AppData.Instance.DetectFacesResult;
			if (multipleImages != null)
			{
				m_Text.text += string.Format("\nimages processed: {0}", multipleImages.images_processed);
				if (multipleImages.images != null && multipleImages.images.Length > 0)
					foreach (FacesTopLevelSingle faces in multipleImages.images)
					{
						if(faces.faces != null && faces.faces.Length > 0)
						foreach (OneFaceResult face in faces.faces)
						{
							if (face.face_location != null)
							{
								m_Text.text += string.Format("\n\tFace location: {0}, {1}, {2}, {3}", face.face_location.left, face.face_location.top, face.face_location.width, face.face_location.height);
							}
							if (face.gender != null)
							{
								m_Text.text += string.Format("\n\tGender: {0}, Score: {1}", face.gender.gender, face.gender.score);
							}
							if (face.age != null)
							{
								m_Text.text += string.Format("\n\tAge Min: {0}, Age Max: {1}, Score: {2}", face.age.min, face.age.max, face.age.score);
							}
							if (face.identity != null)
							{
								m_Text.text += string.Format("\n\tName: {0}, Score: {1}, Type Heiarchy: {2}", face.identity.name, face.identity.score, face.identity.type_hierarchy);
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
