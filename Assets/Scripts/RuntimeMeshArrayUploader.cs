using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using RealityEditor;

public class RuntimeMeshArrayUploader : MonoBehaviour
{
    RealityEditorManager realityEditorManager;
    // public GameObject parentObject; // Parent GameObject
    // public GameObject[] targetGameObjects;
    public string serverURL = "http://<YOUR_SERVER_IP>:5000/uploadMesh";


    public void Start()
    {
        realityEditorManager =FindAnyObjectByType<RealityEditorManager>();

        serverURL=realityEditorManager.ServerURL+":"+realityEditorManager.Port+"/uploadMesh";
    }

    public void UploadAllMeshes(GameObject  parentObject, GameObject [] targetGameObjects,string urlid)
    {
        foreach (GameObject go in targetGameObjects)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            if (mf != null)
            {
                string objData = MeshToOBJLocalSpace(mf, parentObject.transform, go.name);
                StartCoroutine(UploadOBJ(objData, go.name,urlid));
            }
            else
            {
                Debug.LogWarning($"GameObject '{go.name}' has no MeshFilter.");
            }
        }
    }

    string MeshToOBJLocalSpace(MeshFilter meshFilter, Transform parentTransform, string objectName)
    {
        Mesh mesh = meshFilter.mesh;
        Transform tf = meshFilter.transform;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"# OBJ exported from Unity in local space relative to parent - {objectName}");

        // Convert vertices to local space of parent
        foreach (Vector3 v in mesh.vertices)
        {
            Vector3 worldV = tf.TransformPoint(v);
            Vector3 localV = parentTransform.InverseTransformPoint(worldV);
            sb.AppendLine($"v {-localV.x} {localV.y} {localV.z}");
        }
        sb.AppendLine();

        // Convert normals to local space of parent
        foreach (Vector3 n in mesh.normals)
        {
            Vector3 worldN = tf.TransformDirection(n);
            Vector3 localN = parentTransform.InverseTransformDirection(worldN).normalized;
            sb.AppendLine($"vn {-localN.x} {localN.y} {localN.z}");
        }
        sb.AppendLine();

        foreach (Vector2 uv in mesh.uv)
            sb.AppendLine($"vt {uv.x} {uv.y}");
        sb.AppendLine();

        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            sb.AppendLine($"f {triangles[i] + 1}/{triangles[i] + 1}/{triangles[i] + 1} " +
                          $"{triangles[i + 1] + 1}/{triangles[i + 1] + 1}/{triangles[i + 1] + 1} " +
                          $"{triangles[i + 2] + 1}/{triangles[i + 2] + 1}/{triangles[i + 2] + 1}");
        }

        return sb.ToString();
    }

    IEnumerator UploadOBJ(string objData, string filename,string urlid)
    {
        WWWForm form = new WWWForm();
        byte[] objBytes = Encoding.UTF8.GetBytes(objData);
        form.AddBinaryData("file", objBytes, filename + ".obj", "text/plain");
        form.AddField("URLID",urlid);

        UnityWebRequest www = UnityWebRequest.Post(serverURL, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
            Debug.Log($"Mesh '{filename}' uploaded successfully!");
        else
            Debug.LogError($"Upload '{filename}' failed: {www.error}");
    }
}
