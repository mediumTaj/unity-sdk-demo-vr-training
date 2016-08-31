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
using IBM.Watson.DeveloperCloud.Widgets;
using UnityEngine.UI;

namespace IBM.Watson.DeveloperCloud.Demos.VRTraining
{
    public class MatchCameraAspectRatio : MonoBehaviour
    {
        [SerializeField]
        private WebCamWidget m_WebCamWidget;
        private AspectRatioFitter m_AspectRatioFitter;
        //private RectTransform m_RawImageRectTransform;
        //private RectTransform m_PanelRectTransform;

        private float m_RequestedAspectRatio;

        void OnEnable()
        {
            //m_RawImageRectTransform = gameObject.GetComponent<RectTransform>();
            //m_PanelRectTransform = gameObject.GetComponentInParent<RectTransform>();

            m_RequestedAspectRatio = (m_WebCamWidget.RequestedWidth* 1.0f) / (m_WebCamWidget.RequestedHeight * 1.0f);

            m_AspectRatioFitter = gameObject.GetComponent<AspectRatioFitter>();
            m_AspectRatioFitter.aspectRatio = m_RequestedAspectRatio;
        }

        void OnResize()
        {
            //float screenWidth = m_PanelRectTransform.rect.width;
            //float screenHeight = m_PanelRectTransform.rect.height;

            //float m_ScreenAspectRatio = screenWidth / screenHeight;

            //// m_PanelRectTransform.rect.width = screenWidth /

            //float rawImageWidth = (screenHeight <= screenWidth * m_RequestedAspectRatio) ? screenWidth * m_RequestedAspectRatio : screenWidth;
            //float rawImageHeight = (screenWidth <= screenHeight * 1 / m_RequestedAspectRatio) ? screenHeight * m_RequestedAspectRatio : screenHeight;
            



        }
    }
}
