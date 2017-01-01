using System;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using MonoLightTech.UnityNetwork.S2C;

internal static class ChatServer
{
    private static Server _server;

    private static void Main()
    {
        Handler handler = new Handler(
            _HandleInitialize,
            _HandleTerminate,
            _HandleLogin,
            _HandleConnect,
            _HandleLatency,
            _HandleDisconnect,
            _HandleData,
            _HandleForeignData,
            _HandleLog);

        Config config = new Config("S2C_Chat", 9696);

        _server = new Server(handler, config);
        _server.Initialize();

        Console.WriteLine("Press [ESC] to terminate.");
        while (Console.ReadKey().Key != ConsoleKey.Escape) { }
        Console.WriteLine("Terminating...");

        _server.Terminate();
    }

    private static void _HandleInitialize(bool success, Exception exception)
    {
        Console.WriteLine("Initialize => [Success: " + success + "] [Exception: " + (exception == null ? "null" : exception.ToString()) + "]");

        if (!success)
        {
            Environment.Exit(1);
        }
    }

    private static void _HandleTerminate(bool failure, Exception exception)
    {
        Console.WriteLine("Terminate => [Failure: " + failure + "] [Exception: " + (exception == null ? "null" : exception.ToString()) + "]");
    }

    private static Pair<bool, Packet> _HandleLogin(Connection connection, Packet packet)
    {
        string username = packet.GetString("Username");
        Packet loginPacket = new Packet();

        if (string.IsNullOrEmpty(username))
        {
            loginPacket.SetString("Reason", "Username required!");
            return new Pair<bool, Packet>(false, loginPacket);
        }

        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9]{4,16}$"))
        {
            loginPacket.SetString("Reason", "Bad username!");
            return new Pair<bool, Packet>(false, loginPacket);
        }

        if (_server.Connections.Any(x => x.Packet.GetString("Username").ToUpper().Equals(username.ToUpper())))
        {
            loginPacket.SetString("Reason", "Username in use!");
            return new Pair<bool, Packet>(false, loginPacket);
        }

        return new Pair<bool, Packet>(true, null);
    }

    private static void _HandleConnect(Connection connection)
    {
        Console.WriteLine("Connect => [IPEndPoint: " + connection.IPEndPoint + "] [Username: " + connection.Packet.GetString("Username") + "]");
    }


    private static void _HandleLatency(Connection connection, float latency)
    {

    }

    private static void _HandleDisconnect(Connection connection, Packet packet)
    {
        Console.WriteLine("Disconnect => [IPEndPoint: " + connection.IPEndPoint + "] [Username: " + connection.Packet.GetString("Username") + "] [Reason: " + packet.GetString("Reason") + "]");
    }

    private static void _HandleData(Connection connection, Delivery delivery, Packet packet, Channel channel)
    {
        Console.WriteLine("Data => [IPEndPoint: " + connection.IPEndPoint + "] [Username: " + connection.Packet.GetString("Username") + "] [Delivery: " + delivery + "] [Channel: " + channel + "]");
        Console.WriteLine(packet);
    }

    private static void _HandleForeignData(IPEndPoint ipEndPoint, byte[] data)
    {
        Console.WriteLine("ForeignData => [IPEndPoint: " + ipEndPoint + "] [Length: " + data.Length + "] [Text: " + Extensions.GetString(data) + "]");
    }

    private static void _HandleLog(LogType type, string message, object argument)
    {
        Console.WriteLine("Log => [Type: " + type + "] [Message: " + message + "] [Argument: " + (argument == null ? "null" : argument.ToString()) + "]");
    }
}