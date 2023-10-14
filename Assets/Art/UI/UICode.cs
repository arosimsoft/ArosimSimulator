using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Assets.Code.Scripts;
using Unity.VisualScripting;
using UnityEditor.UI;
using System;

public class UICode : MonoBehaviour
{

    public static UICode instance;
    private UIDocument menu;


    private Label lblState;

    private Button btnConnect;
    public TextField ipAddressField;
    public TextField portField;


    [SerializeField]
    public List<GameObject> jointLinks;

    private List<Label> jointLabels = new();


    private void OnEnable()
    {
        menu = GetComponent<UIDocument>();
        VisualElement root = menu.rootVisualElement;

        VisualElement coordBox = root.Q<VisualElement>("CoordinateBox");

        Label lbl_robot = new Label($"Robot Joints:");
        lbl_robot.style.color = Color.white;

        coordBox.Add(lbl_robot);

        foreach (GameObject link in jointLinks)
        {
            Label lbl_joint = new($"{link.name}: ")
            {
                name = $"lbl_{link.name}"
            };
            lbl_joint.style.color = Color.white;
            coordBox.Add(lbl_joint);

            jointLabels.Add(lbl_joint);
            SimulationManager.jointLinks.Add(link);
        }

        lblState = root.Q<Label>("LblState");
        ipAddressField = root.Q<TextField>("IpAddressField");
        portField = root.Q<TextField>("PortField");

        btnConnect = root.Q<Button>("ButtonConnect");
        btnConnect.clicked += ManageConnection;
    }

    private void Start()
    {
        ipAddressField.value = "127.0.0.1";
        portField.value = "585";

    }


    private void Update()
    {

        updateJointCoordinates();
        updateStateLabel();
    }

    private void updateStateLabel()
    {
        if (SimulationManager.controlActivated)
        {
            lblState.text = "Estado: Control Robot";
        }
        else
        {
            lblState.text = "Estado: Cámara";
        }
    }

    private void updateJointCoordinates()
    {

        foreach (GameObject link in jointLinks)
        {
            Vector3 link_pos = link.transform.position;
            Vector3 link_rot = link.transform.localEulerAngles;

            Label lbl_joint = jointLabels.Find(x => x.name.Contains(link.name));
            lbl_joint.text = $"{link.name}:\nPOS x->{link_pos.x:F4} y->{link_pos.y:F4} z->{link_pos.z:F4} \nROT x->{link_rot.x:F2}˚ y->{link_rot.y:F2}˚ z->{link_rot.z:F2}˚";
        }

    }

    public void ManageConnection()
    {
        if (!SimulationManager.connectedToServer)
        {
            try
            {
                Debug.Log("Connecting to server...");

                ClientController.instance.ip = ipAddressField.value.Trim();
                ClientController.instance.portConnection = int.Parse(portField.value);

                ClientController.instance.ConnectToServer();
            }
            catch (Exception ex)
            {
                Debug.Log($"Connection failed... {ex}");
                return;
            }
            Debug.Log("Connection succesfully");
            btnConnect.text = "Disconect to Server";
            SimulationManager.connectedToServer = true;
        }
        else
        {
            // Procedure to disconnect to Server
            ClientController.instance.Disconnect();
            Debug.Log("Disconnected from server.");

        }


    }
}
