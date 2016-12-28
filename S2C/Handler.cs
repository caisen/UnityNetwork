﻿using System;
using System.Net;

namespace MonoLightTech.UnityNetwork.S2C
{
    public sealed class Handler
    {
        public delegate void InitializeHandler(bool success, Exception exception = null);
        public delegate void TerminateHandler(bool failure, Exception exception = null);

        public delegate Pair<bool, Packet> LoginHandler(Connection connection, Packet packet);

        public delegate void ConnectHandler(Connection connection);
        public delegate void LatencyHandler(Connection connection, float latency);
        public delegate void DisconnectHandler(Connection connection, Packet packet);

        public delegate void DataHandler(Connection connection, Delivery delivery, Packet packet, Channel channel);
        public delegate void ForeignDataHandler(IPEndPoint ipEndPoint, byte[] data);

        public delegate void LogHandler(LogType type, string message, object argument = null);

        public InitializeHandler Initialize { get; private set; }
        public TerminateHandler Terminate { get; private set; }

        public LoginHandler Login { get; private set; }

        public ConnectHandler Connect { get; private set; }
        public LatencyHandler Latency { get; private set; }
        public DisconnectHandler Disconnect { get; private set; }

        public DataHandler Data { get; private set; }
        public ForeignDataHandler ForeignData { get; private set; }

        public LogHandler Log { get; private set; }

        public Handler(
            InitializeHandler initialize,
            TerminateHandler terminate,
            LoginHandler login,
            ConnectHandler connect,
            LatencyHandler latency,
            DisconnectHandler disconnect,
            DataHandler data,
            ForeignDataHandler foreignData,
            LogHandler log)
        {
            Initialize = initialize;
            Terminate = terminate;
            Login = login;
            Connect = connect;
            Latency = latency;
            Disconnect = disconnect;
            Data = data;
            ForeignData = foreignData;
            Log = log;
        }
    }
}