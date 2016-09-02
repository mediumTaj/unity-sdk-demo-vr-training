using UnityEngine;
using System.Collections;

public class AppController : MonoBehaviour
{
	private VRCredentials m_VRCredentials = new VRCredentials();

	public void ChangeCredentials()
	{
		m_VRCredentials.SetVisualRecognitionAPIKey("");
	}
}
