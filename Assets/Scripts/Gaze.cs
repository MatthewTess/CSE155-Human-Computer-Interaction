using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System.Diagnostics;
using System.IO;

public class Gaze : MonoBehaviour
{
    Thread mThread;
    public bool closeTrackerOnQuit = true;

    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;

    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    public Vector3 receivedPos = Vector3.zero;

    bool running;
    bool isRecievingData = false;
    Process eyetracker;

    static Gaze instance;
    public static bool IsTracking(){
        return instance.isRecievingData;
    }

    private Process run_cmd(string cmd, string args)
    {
        ProcessStartInfo start = new ProcessStartInfo();

        start.FileName = cmd;//cmd is full path to python.exe
        start.Arguments = args;//args is path to .py file and any cmd line args

        start.UseShellExecute = true;
        start.RedirectStandardOutput = false;
        start.CreateNoWindow = true;

        Process process = Process.Start(start);
        return process;
    }
    private Process CheckIfTrackerRunning(string processName = "eyetrack")
    {
        //NOTE: GetProcessByName() doesn't seem to work on Win7
        //Process[] running = Process.GetProcessesByName("notepad");
        Process result = null;
        Process[] running = Process.GetProcesses();
        foreach (Process process in running)
        {
            try
            {
                print(process.ProcessName);
                if (!process.HasExited && process.ProcessName == processName)
                {
                    result = process;
                    break;
                }
            }
            catch (System.InvalidOperationException)
            {
                //do not$$anonymous$$ng
                UnityEngine.Debug.Log("***** InvalidOperationException was caught!");
            }
        }
        return result;
    }
    private void Start()
    {
        /*Process runningTracker = CheckIfTrackerRunning();
        if (runningTracker != null)
        {
            UnityEngine.Debug.Log("Tracker is already running.");
            eyetracker = runningTracker;
        }
        else
        {
            UnityEngine.Debug.Log("Tracker is not running. Starting tracker.");
            eyetracker = run_cmd(Directory.GetCurrentDirectory() + "\\Assets\\Tracker\\eyetrack.exe", "");
        }*/
        instance = this;
        ThreadStart ts = new ThreadStart(EstablishConnection);
        mThread = new Thread(ts);
        mThread.Start();
    }
    void OnApplicationQuit()
    {
        if (closeTrackerOnQuit)
        {
            running = false;
            if (mThread != null)
            {
                mThread.Abort();
            }

            ///UnityEngine.Debug.Log("Closing tracker.");
            //eyetracker.Kill();
        }
    }
    void EstablishConnection()
    {
        bool connected = false;
        while (!connected)
        {
            try
            {
                localAdd = IPAddress.Parse(connectionIP);
                listener = new TcpListener(IPAddress.Any, connectionPort);
                listener.Start();

                client = listener.AcceptTcpClient();

                connected = true;
                running = true;

                while (running)
                {
                    SendAndReceiveData();
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.Log("Connection refused. Retrying in 1 second.");
                Thread.Sleep(1000);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }

                if (client != null)
                {
                    client.Close();
                }
            }
        }
    }
    void SendAndReceiveData()
    {
        try
        {
            if (client == null)
            {
                UnityEngine.Debug.Log("Connection lost. Retrying in 1 second.");
                Thread.Sleep(1000);
                return;
            }
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            //---receiving Data from the Host----
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

            if (dataReceived != null)
            {
                isRecievingData = true;
                //---Using received data---
                receivedPos = StringToVector3(dataReceived); //<-- assigning receivedPos value from Python
            }
            else
            {
                isRecievingData = false;
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("Error in SendAndReceiveData: " + e.StackTrace);
        }
    }
    public static Vector3 StringToVector3(string sVector)
    {
        Vector3 result = Vector3.zero;
        try
        {
            // Remove the parentheses
            if (sVector.StartsWith("(") && sVector.EndsWith(")"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log("Error in StringToVector3: " + e.Message);
        }

        return result;
    }
}