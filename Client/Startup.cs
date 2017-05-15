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
    class Startup
    {
        static public int port;//store the port value
        static public string ipAddr;//store the ipaddress
        static void Main(string[] args)
        {
            Thread listenThd;//two threads so we can both
            Thread speakThd; //receive and send text at the same time

            Console.Write("Type 1 for client, Type 2 for Host: ");//prompt for host or client
            int clientOrHost;//stores the 1 or 2
            int.TryParse(Console.ReadLine(), out clientOrHost);//parses their response into the above int

            
            Console.Write("Enter a port: ");//prompt for a port, defaults to 2112
            int.TryParse(Console.ReadLine(), out port);//outs into port int

            if (clientOrHost == 1)
            {
                Console.Write("Enter an IP Address: ");//prompt for an ip, defaults to loopback
                ipAddr = Console.ReadLine();//puts it in ipAddr string
            }
            if(ipAddr == "")//if the string is blank, ie they pressed enter w/o typing
            {
                ipAddr = "127.0.0.1";//set it to default
            }
            if(port == 0)//if they didn't input a port, ie enter w/o typing
            {
                port = 2112;//default port
            }

            if (clientOrHost == 1)//if they chose client
            {
                Client client = new Client(ipAddr, port);//setup a client obj
                listenThd = new Thread(client.Listen);//assign the threads
                speakThd = new Thread(client.Speak);//^^^^

                listenThd.Start();//start the threads
                speakThd.Start();//^^^^^
                
            }
            else if(clientOrHost == 2)//else, if they picked host
            {
                Host host = new Host(ipAddr, port);//same as client but using Host's methods, slightly different
                listenThd = new Thread(host.Listen);//^^
                speakThd = new Thread(host.Speak);//^^^
                
                listenThd.Start();//^^^^
                speakThd.Start();//^^^^^
            }
        }
    }
}
