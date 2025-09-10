using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using RealityEditor;


public class PhotonDataSync : NetworkBehaviour
{
    //we are handeling our networking in ReConstructSpot, don't forget!!!!
    private ReConstructSpot _generateSpot; //this used to be called networking, but is now just ReConstructSpot. OOF
    
    [Networked, OnChangedRender(nameof(OnUrlIDChanged))]
    public string NetworkedUrlID { get; set; }
    
    [Networked, OnChangedRender(nameof(OnPromptChanged))]
    public string NetworkedPrompt { get; set; }

    [Networked, OnChangedRender(nameof(OnPromptChanged))]
    public int NetworkedVersion { get; set; }
    
    private void Start()
    {
        _generateSpot = GetComponent<ReConstructSpot>();
        _generateSpot.URLID = NetworkedUrlID; 

    }
    // Method to detect changes to the networked string
    void OnUrlIDChanged()
    {
        _generateSpot = GetComponent<ReConstructSpot>();
        Debug.Log("Networked urlid changed to: " + NetworkedUrlID);
        _generateSpot.URLID = NetworkedUrlID; 
    }
    void OnPromptChanged()
    {
        _generateSpot = GetComponent<ReConstructSpot>();
        Debug.Log("Networked prompt changed to: " + NetworkedPrompt);
        _generateSpot.prompt = NetworkedPrompt; 
    }
    void OnVersionChanged()
    {
        _generateSpot = GetComponent<ReConstructSpot>();
        Debug.Log("Networked version changed to: " + NetworkedVersion);
        _generateSpot.Version = NetworkedVersion; 
    }
    
    public void UpdateURLID(string newUrlID)
    {
        if (HasStateAuthority)
        {
            // Change the string value here, which will then be synchronized across all clients
            NetworkedUrlID = newUrlID;
        }
    }
    public void UpdatePrompt(string newUrlID)
    {
        if (HasStateAuthority)
        {
            // Change the string value here, which will then be synchronized across all clients
            NetworkedPrompt = newUrlID;
        }
    }
    public void UpdateVersion(int newVersion)
    {
        if (HasStateAuthority)
        {
            // Change the int value here, which will then be synchronized across all clients
            NetworkedVersion = newVersion;
        }
    }
}