using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Client
{
    class Host
    {
        protected string ipAddr;//S/E
        protected int port;//S/E
        protected Socket sock;//socket object
        protected string userName;//stores the user's given name
        public Host(string ipAddress, int mPort)//constructor to setup ip and port gotten from Main
        {
            ipAddr = ipAddress;
            port = mPort;
            Console.Write("Enter the name you wish to appear as: ");//prompt for name
            userName = Console.ReadLine();
            StartHost();//start the connection
        }
        public void StartHost()//starts the connection
        {
            //IPAddress ip = IPAddress.Parse(ipAddr);//parse the ip string into IPAddress type
            TcpListener listen = new TcpListener(IPAddress.Any, port);//setup a listener
            listen.Start();//start the listener, but don't accept connections yet
            try
            {
                Console.WriteLine("Waiting for connection...");//tell user that we're waiting for client to connect
                sock = listen.AcceptSocket();//waits here until client connects
                Console.WriteLine("Connected to client");//if it got here, then client has connected, inform user
            }
            catch (Exception ex)//catchall
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Listen()//listen for messages from the client, runs at the same time as speak(threading yay!)
        {
            try
            {
                StreamReader sr = new StreamReader(new NetworkStream(sock));//setup a streamreader to read client's messages
                string clientName = sr.ReadLine();//the first line the client sends is it's name
                while (true)//infinite loop so we can always talk
                {
                    string clientResponse = sr.ReadLine();//read the clients response into a string, waits here for a response
                    ClearCurrentConsoleLine();//if it got here then a response happened, clears the Host's name and colon
                    Console.WriteLine(clientName + ": " + clientResponse);//write out the client's name and their response
                    Console.Write(userName + ": ");//redraw the host name and colon, as though it is on it's own line seperated from chat
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Speak()//send out messages to client, runs along with listen(yay threading!)
        {
            StreamWriter sw = new StreamWriter(new NetworkStream(sock));//connects the streamwriter to the client
            sw.WriteLine(userName);//write out the username, always the first thing
            while(true)//loop to allow for constant chatting
            {
                Console.Write(userName + ": ");//write out the host's name and colon, user's text goes immediately after on same line
                sw.WriteLine(Console.ReadLine());//write out what host says to the network stream, sends to client to display
                sw.Flush();//doesn't wait for a full packet, send immediately
            }
        }
        public static void ClearCurrentConsoleLine()//clears the line that cursor is currently on
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
