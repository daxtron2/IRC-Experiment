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
    class Client
    {
        private string ipAddr;//s/e
        private int port;//s/e
        protected string userName;//s/e

        public Client(string ipAddr, int port)//same as host
        {
            this.ipAddr = ipAddr;
            this.port = port;
            Console.Write("Enter the name you wish to appear as: ");
            userName = Console.ReadLine();
            StartClient();
        }

        TcpClient socket = null;
        private void StartClient()
        {

            bool connected = false;//used to see if currently connected to Host
            while (connected == false)//while we're not connected
            {
                try
                {
                    socket = new TcpClient(ipAddr, port);//try to connect
                }
                catch (Exception ex)//if we get an exception, we didn't connect
                {
                    connected = false;//keep it false
                }
                try
                {
                    if (socket.Connected == true)//check to see if the socket is connected
                    {
                        connected = true;//if it is, set to true and drop from loop
                    }
                }
                catch//otherwise it throws an exception
                {
                    //tell the user why, and sleep for a second so as to not spam the console/network with requests
                    Console.WriteLine("Could not connect, retrying...");
                    Console.WriteLine("This means the server is not currently active on the given IP and/or Port: " + ipAddr + ":" + port);
                    Thread.Sleep(1000);
                }
            }
            Console.WriteLine("Successfully connected to Server!");//after we're out of the loop, tell them we connected
        }
        public void Listen()//listen for host's messages, runs at the same time as speak
        {
            try
            {
                StreamReader sr = new StreamReader(socket.GetStream());//setup the SR
                string hostName = sr.ReadLine();//get the host's name, always the first thing sent between the two
                while (true)//loop for infinite chatting
                {
                    string hostResponse = sr.ReadLine();//get the Host's line of text
                    ClearCurrentConsoleLine();//clear the client's name and colon
                    Console.WriteLine(hostName + ": " + hostResponse);//write out the host's text w/ their name
                    Console.Write(userName + ": ");//redraw the client's name and colon
                }
            }
            catch (Exception ex)//catchall
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Speak()//write out client's message to the host, runs concurrently with listen
        {
            try
            {
                StreamWriter sw = new StreamWriter(socket.GetStream());//setup the SW with the host's stream
                sw.WriteLine(userName);//give the host the client's chosen name, always first thing
                while (true)//infinite chatting ability
                {
                    Console.Write(userName + ": ");//Write out the user's name and colon
                    string userTyped = Console.ReadLine();//get the user's input
                    sw.WriteLine(userTyped);//send the client's input to the host
                    sw.Flush();//flush the stream, don't wait for a full packet
                }
            }
            catch(Exception ex)//catchalls are fun
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ClearCurrentConsoleLine()//clears the line that the console's cursor is on, always the last one in this program
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}   

    

