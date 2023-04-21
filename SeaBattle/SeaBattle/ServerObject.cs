using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class ServerObject
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 1234);
        public List<ClientObject> clients { get; private set; } = new List<ClientObject>();

        protected internal void RemoveConnection(string ID)
        {
            ClientObject? client = clients.FirstOrDefault(c => c.Id == ID);
        }
        protected internal async Task ListenAsync()
        {
            try
            {
                listener.Start();

                TcpClient tcpClient = await listener.AcceptTcpClientAsync();

                ClientObject clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject);
                _ = Task.Run(clientObject.ProcessAsync);

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Disconnect(); 
            }
            
        }
        protected internal async Task BroadcastMessageAsync(string message, string id)
        {
            foreach (var client in clients)
            {
                if (client.Id != id)
                {
                    await client.Writer.WriteLineAsync(message);
                    await client.Writer.FlushAsync();
                }
            }
        }
        protected internal void Disconnect()
        {
            foreach (var client in clients)
            {
                client.Close();
            }

            listener.Stop();
        }
    }
}
