using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class ClientObject
    {
        public string Id { get; private set;  } = Guid.NewGuid().ToString();
        public ClientGame clientGame { get; private set; } = new ClientGame();

        protected internal StreamWriter Writer { get; }
        protected internal StreamReader Reader { get; }

        TcpClient tcpClient;
        ServerObject serverObject;

        public ClientObject(TcpClient client, ServerObject server)
        {
            tcpClient = client;
            serverObject = server;
            var stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream);
        }
        public async Task ProcessAsync()
        {
            try
            {
                string? userName = await Reader.ReadLineAsync();
                string? message = $"{userName} вошел в чат";

                await serverObject.BroadcastMessageAsync(message, Id);
                Console.WriteLine(message);

                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message == null) continue;
                        message = $"{userName}: {message}";
                        Console.WriteLine(message);
                        await serverObject.BroadcastMessageAsync(message, Id);
                    }
                    catch
                    {
                        message = $"{userName} покинул чат";
                        Console.WriteLine(message);
                        await serverObject.BroadcastMessageAsync(message, Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                serverObject.RemoveConnection(Id);
            }
        }

        protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            tcpClient.Close();
        }
    }
}
