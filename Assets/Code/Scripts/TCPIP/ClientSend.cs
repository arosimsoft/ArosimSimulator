using Assets.Code.Scripts;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        ClientController.instance.tcp.SendData(packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {

        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            packet.Write(ClientController.instance.myId);
            packet.Write(" ");

            SendTCPData(packet);
        }
    }

    internal static void UDPTestReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.updTestReceived))
        {
            packet.Write("Received a UDP packet.");

            SendUDPData(packet);
        }
    }

    private static void SendUDPData(Packet packet)
    {
        packet.WriteLength();
        ClientController.instance.udp.SendData(packet);
    }

    public static void AllCoordinates()
    {
        // TODO: Get all information of robot joints
        string all_coords = "";

        foreach (GameObject link in SimulationManager.jointLinks)
        {
            Vector3 link_pos = link.transform.position;
            Vector3 link_rot = link.transform.localEulerAngles;


            all_coords += $"{link.name},{link_pos.x:F6},{link_pos.y:F6},{link_pos.z:F6},{link_rot.x:F6},{link_rot.y:F6},{link_rot.z:F6};";
        }

        using (Packet packet = new Packet((int)ClientPackets.AllCoordinates))
        {
            //packet.Write(all_coords.Length);
            packet.Write(all_coords);

            SendUDPData(packet);
        }
    }

    #endregion
}