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

namespace IBM.Watson.DeveloperCloud.Demos.FacialRecognition
{
    public class ImageView : View
    {
        #region Private Data
        [SerializeField]
        private RawImage m_RawImage;
        
        #endregion

        #region Public Properties
        /// <summary>
        /// Byte array of image data to be set in the RawImage.
        /// </summary>
        public byte[] ImageData
        {
            get { return m_ImageData; }
            set
            {
                m_ImageData = value;
                SetImage();
            }
        }
        private byte[] m_ImageData;
        #endregion

        #region Private Functions
        private void SetImage()
        {
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(ImageData);
            m_RawImage.texture = tex;
        }
        #endregion
    }
}