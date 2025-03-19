using System;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using RealityEditor;


public class PhotonDataSync : NetworkBehaviour
{
    private ReConstructSpotNetworking _generateSpot;
    
    [Networked, OnChangedRender(nameof(OnUrlIDChanged))]
    public string NetworkedUrlID { get; set; }
    
    [Networked, OnChangedRender(nameof(OnPromptChanged))]
    public string NetworkedPrompt { get; set; }
    
    private void Start()
    {
        _generateSpot = GetComponent<ReConstructSpotNetworking>();
        _generateSpot.URLID = NetworkedUrlID; 

    }
    // Method to detect changes to the networked string
    void OnUrlIDChanged()
    {
        _generateSpot = GetComponent<ReConstructSpotNetworking>();
        Debug.Log("Networked urlid changed to: " + NetworkedUrlID);
        _generateSpot.URLID = NetworkedUrlID; 
    }
    void OnPromptChanged()
    {
        _generateSpot = GetComponent<ReConstructSpotNetworking>();
        Debug.Log("Networked prompt changed to: " + NetworkedPrompt);
        _generateSpot.prompt = NetworkedPrompt; 
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
}