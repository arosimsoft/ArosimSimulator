using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        string msg = packet.ReadString();
        int myId = packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        ClientController.instance.myId = myId;
        ClientSend.WelcomeReceived();

        ClientController.instance.udp.Connect(((IPEndPoint)ClientController.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void UDPTest(Packet packet)
    {
        string msg = packet.ReadString();

        Debug.Log($"Received packet via UDP. Contains message: {msg}");
        ClientSend.UDPTestReceived();
    }

    internal static void SimulatorCommand(Packet packet)
    {
        // Get data from packet
        // int id = packet.ReadInt();
        string command = packet.ReadString();

        Debug.Log($"Server sends: {command}");

        // Executing command
        CommandManager.HandleCommand(command);

    }
}