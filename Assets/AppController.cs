using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour
{
	private VRCredentials m_VRCredentials = new VRCredentials();

	[SerializeField]
	private InputField m_APIKeyField;

	[SerializeField]
	private InputField m_DeleteClassifierIDField;
	 
	public void ChangeCredentials()
	{
		m_VRCredentials.SetVisualRecognitionAPIKey(m_APIKeyField.text);
	}

	public void GetClassifiers()
	{
		m_VRCredentials.GetClassifiers();
	}

	public void DeleteClassifier()
	{
		m_VRCredentials.DeleteClassifier(m_DeleteClassifierIDField.text);
	}
}
