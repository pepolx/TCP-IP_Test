﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPIP_Test
{
    internal class Server
    {

        public void StartServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888); //serwer localhost
            server.Start();
            Console.WriteLine("Serwer jest uruchomiony.");
        }
        public void HandleClient(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] buffer = new byte[1024];//miejsce na dane
            int bytesRead;

            while (true)
            {
                bytesRead = 0;//zeruje po przeslaniu
                try
                {
                    bytesRead = clientStream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    break;
                }

                string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Otrzymano: " + data);

                if (data == "TRIGGER")
                {
                    // symulacja opóźnienia i wysłanie odpowiedzi
                    Thread.Sleep(1000);
                    string response = RandomStringGenerator.Letters(15);
                    byte[] responseData = Encoding.ASCII.GetBytes(response);
                    clientStream.Write(responseData, 0, responseData.Length);
                    Console.WriteLine("Wyslano odpowiedź: " + response);
                }
            }

            tcpClient.Close();
        }
    }
}