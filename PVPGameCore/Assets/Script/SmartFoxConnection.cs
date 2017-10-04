using UnityEngine;
using System.Collections;
using Sfs2X;
using Sfs2X.Core;
using UnityEngine.UI;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.ObjectModel;

public class SmartFoxConnection : MonoBehaviour
{

    private string defaultHost = "localhost";
    private int defaultTcPort = 9933;
    private int defaultWsPort = 8080;
    SmartFox sfs = new SmartFox();
    public string ZoneName = "BasicExamples";
    public string UserName = "";
    public Button button;
    void Start()
    {
        Connection();
    }
    void Connection()
    {
        sfs.ThreadSafeMode = true;
        sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExntensionReponse);
    }

    public void CallConnectSmartFox()
    {

        sfs.Connect(defaultHost, defaultTcPort);
        
    }
    private void OnConnection(BaseEvent evt)
    {
        if ((bool)evt.Params["success"])
        {
            Debug.Log("Connection established successfully");
            Debug.Log("SFS2X API version: " + sfs.Version);
            Debug.Log("Connection mode is: " + sfs.ConnectionMode);
            sfs.Send(new LoginRequest(UserName, "mypassword", ZoneName));

        }
        else
        {
            Debug.Log("Connection failed; is the server running at all?");

            // Remove SFS2X listeners and re-enable interface

        }
    }
    void OnLogin(BaseEvent evt)
    {
        Debug.Log("Login in :" + evt.Params["user"]);
        ISFSObject objOut = new SFSObject();
         DataObject data = new DataObject();
        //objOut.PutClass("dataObj", data);
        objOut.PutInt("numberA", 5);
        sfs.Send(new ExtensionRequest("ABC", objOut));
    }
    void OnExntensionReponse(BaseEvent e)
    {
        string cmd = (string)e.Params["cmd"];
        Debug.Log(cmd);
        ISFSObject objIn = (SFSObject)e.Params["params"];
        if (cmd == "ABC")
        {
            //DataObject data  = (DataObject) objIn.GetClass("dataObj");
            Debug.Log(objIn.GetInt("A"));
        }
    }
    public static object ByteArrayToObject(byte[] arrBytes)
    {
        MemoryStream memStream = new MemoryStream();
        BinaryFormatter binForm = new BinaryFormatter();

        memStream.Write(arrBytes, 0, arrBytes.Length);
        memStream.Seek(0, SeekOrigin.Begin);

        object obj = (object)binForm.Deserialize(memStream);

        return obj;
    }
    void OnLoginError(BaseEvent evt)
    {
        Debug.Log("Login Error : " + evt.Params["errorCode"] + " " + evt.Params["errorMessage"]);
    }
    private void OnConnectionLost(BaseEvent evt)
    {
        Debug.Log("Connection was lost; reason is: " + (string)evt.Params["reason"]);

        // Remove SFS2X listeners and re-enable interface
        //   reset();
    }
    // Update is called once per frame
    void Update()
    {
        sfs.ProcessEvents();
    }
}
