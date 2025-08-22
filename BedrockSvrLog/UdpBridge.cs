using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BedrockSvrLog;

public class UdpBridge
{
    private List<string> MessageBuffer { get; } = new List<string>();

    public void startUdpListner(int port = 5550)
    {
        UdpClient? server = null;
        try
        {
            server = new UdpClient();
            server.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            server.Client.Bind(new IPEndPoint(IPAddress.Any, port));
            Console.WriteLine($"UDP server listening on port {port}");

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                byte[] data = server.Receive(ref remoteEP);
                string message = Encoding.UTF8.GetString(data);
                //Console.WriteLine("API Bridge Received: " + message);
                MessageBuffer.Add(message);

                byte[] reply = Encoding.UTF8.GetBytes("Recived at" + DateTime.Now);
                server.Send(reply, reply.Length, remoteEP);
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket error in UDP Listener: {ex.Message} (ErrorCode: {ex.ErrorCode})");
            Console.WriteLine("Try running as administrator or using a different port above 1024.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UDP Listener: {ex.Message}");
        }
        finally
        {
            server?.Close();
        }
    }

    public List<string> GetMessages()
    {
        if(MessageBuffer.Count == 0)
        {
            return new List<string>();
        }
        var messages = new List<string>(MessageBuffer);
        MessageBuffer.Clear(); // Clear the buffer after retrieving messages
        return messages;
    }

    public bool IsMessageBufferEmpty()
    {
        return MessageBuffer.Count == 0;
    }
}
