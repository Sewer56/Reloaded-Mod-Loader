using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SonicHeroes.Networking
{
    /// <summary>
    /// This class defines the means by which one individual may host a server to which they and the other clients can easily exchange data. By default, this is used for communication between the individual modules and the mod launcher/manager, but may be utilized in its own way to create e.g. Networked Multiplayer
    /// </summary>
    public class WebSocket_Host
    {
        /// <summary>
        /// The socket we will be using to communicate with the clients.
        /// </summary>
        private Socket ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// Defines a list of the clients that we will be serving!
        /// </summary>
        public List<Socket> ClientSockets = new List<Socket>();
        /// <summary>
        /// The amount of buffer with each sent message.
        /// </summary>
        public static byte[] Buffer = new byte[1024];
        /// <summary>
        /// What kuind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        IPAddress IPAddressType;

        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(byte[] Data, Socket SocketX);

        /// <summary>
        /// The method used alongside the delegate to process the individual bytes of the delegate.
        /// </summary>
        public ProcessBytesDelegate ProcessBytesMethod;

        /// <summary>
        /// Sets up the websocket server over which communication will occur on.
        /// </summary>
        public void SetupServer(IPAddress IPAddressTypeX)
        {
            // Set of IP address.
            IPAddressType = IPAddressTypeX;

            // Listen to any IP Address
            ServerSocket.Bind(new IPEndPoint(IPAddressType, 13370));

            // Maximum 100 clients can be left as pending.
            // We are going to be sending messages async anyway, so worry needs not placed at the performance on the game end.
            ServerSocket.Listen(100);

            // Start accepting connections!
            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        /// <summary>
        /// This will be ran when a new connection from a client is established.
        /// </summary>
        /// <param name="asyncResult"></param>
        public void AcceptCallback(IAsyncResult asyncResult)
        {
            Console.WriteLine(GetCurrentTime() + "Client Connected!");
            // Set up a socket responsible for communication with the client.
            Socket SocketX = ServerSocket.EndAccept(asyncResult);
            // Add this socket to the list of sockets.
            ClientSockets.Add(SocketX);
            // Start receiving data!
            SocketX.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), SocketX);
            // Start accepting new connections again!
            ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        public void ReceiveCallback(IAsyncResult asyncResult)
        {
            // This socket is the same socket as in AcceptCallback
            Socket SocketX = (Socket)asyncResult.AsyncState;
            try
            {
                // Gets the length of data that has been received.
                int Received = SocketX.EndReceive(asyncResult);
                // Create a new buffer with the data which has been received.
                byte[] DataBuffer = new byte[Received];
                // Copy the received data.
                Array.Copy(Buffer, DataBuffer, Received);

                // Send the buffer to the subscribing method!
                ProcessBytesMethod(DataBuffer, SocketX);

                // Accept connections again!
                SocketX.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), SocketX);
            }
            catch (Exception Ex) { }
        }

        /// <summary>
        /// Resolves current time to be appended to a message.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime() { return "[" + DateTime.Now.ToString("hh:mm:ss") + "] "; }

        /// <summary>
        /// Writes a centered line to the console.
        /// </summary>
        /// <param name=""></param>
        /// <param name="Message"></param>
        public static void ConsoleX_WriteLine_Center(string Message)
        {
            Console.SetCursorPosition((Console.WindowWidth - Message.Length) / 2, Console.CursorTop);
            Console.WriteLine(Message);
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }

    /// <summary>
    /// This class defines the means by which one individual may connect to an existing server to easily exchange data. By default, this is used for communication between the individual modules and the mod launcher/manager, but may be utilized in its own way to create e.g. Networked Multiplayer
    /// </summary>
    public class WebSocket_Client
    {
        /// <summary>
        /// The socket we will be using to communicate with the server.
        /// </summary>
        private Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        /// <summary>
        /// The amount of buffer with each sent message.
        /// </summary>
        public static byte[] Buffer = new byte[1024];
        /// <summary>
        /// What kuind of addresses should we accept. (Loopback for local, Any for external connections)
        /// </summary>
        IPAddress IPAddressType;

        /// <summary>
        /// Delegate to allow the hooking class to call the method which would be of choice to the creator/one who set up this class.
        /// </summary>
        public delegate void ProcessBytesDelegate(byte[] Data, Socket SocketX);

        /// <summary>
        /// The method used alongside the delegate to process the individual bytes of the delegate.
        /// </summary>
        public ProcessBytesDelegate ProcessBytesMethod;

        /// <summary>
        /// Sets up the websocket server over which communication will occur on. Returns true on successful attempt.
        /// </summary>
        public bool SetupClient(IPAddress IPAddressTypeX)
        {
            // Set of IP address.
            IPAddressType = IPAddressTypeX;

            // Amount of tries we tried to connect.
            int ConnectionAttempts = 0;
            while (!ClientSocket.Connected)
            {
                if (ConnectionAttempts > 10) { return false; }
                try { ClientSocket.Connect(IPAddress.Loopback, 13370); return true; }
                catch (SocketException Ex) { ConnectionAttempts += 1; } // If you can't connect, you have no hope!
            }
            return false;
        }

        /// <summary>
        /// Send the data in a byte array to the server. Expects a delegate to be assigned to ProcessBytesMethod which will handle the data.
        /// </summary>
        /// <param name="Data"></param>
        public void SendData(SonicHeroes.Networking.Client_Functions.Message_Type Message_Type, byte[] Data, bool ExpectResponse)
        {
            // Set up the data to be sent to the client.
            byte[] Message = Network_Message.Build_Network_Message(Message_Type, Data);
            ClientSocket.Send(Message); // Send serialized Message!

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (ExpectResponse)
            {
                int DataLength = ClientSocket.Receive(Buffer);
                byte[] ReceiveBuffer = new byte[DataLength];
                Array.Copy(Buffer, ReceiveBuffer, DataLength);
                // Process the received bytes.
                ProcessBytesMethod(ReceiveBuffer, ClientSocket);
            }
        }

        /// <summary>
        /// Send the data in a byte array to the server. Instead of expecting a delegate to be assigned, it returns the data received from the host as a byte[] array.
        /// </summary>
        /// <param name="Data"></param>
        public byte[] SendData_Alternate(SonicHeroes.Networking.Client_Functions.Message_Type Message_Type, byte[] Data, bool ExpectResponse)
        {
            // Set up the data to be sent to the client.
            byte[] Message = Network_Message.Build_Network_Message(Message_Type, Data);
            ClientSocket.Send(Message); // Send serialized Message!

            // If we want a response from the client, receive it, copy to a buffer array and send it back to the method delegate linked to the method we want to process the outcome with.
            if (ExpectResponse)
            {
                int DataLength = ClientSocket.Receive(Buffer);
                byte[] ReceiveBuffer = new byte[DataLength];
                Array.Copy(Buffer, ReceiveBuffer, DataLength);
                // Process the received bytes.
                return ReceiveBuffer;
            }
            return null;
        }

    }

    /// <summary>
    /// Client functions for communication with the mod loader, returns and sends data..
    /// </summary>
    public static class Client_Functions
    {
        /// <summary>
        /// Different Message Types for the Mod Loader.
        /// </summary>
        public enum Message_Type
        {
            /// <summary>
            /// Just a confirmation of operation being performed successfully
            /// </summary>
            Reply_Okay = 0x0,
            /// <summary>
            /// Prints message to mod loader's command line.
            /// </summary>
            Client_Call_Send_Message = 0x1,
            /// <summary>
            /// Mod loader starts polling the controllers and will update them 60 times per second.
            /// </summary>
            Client_Call_Start_Controller_Server = 0x2,
            /// <summary>
            /// Tells the server to send back the controller state to the client as a serialized object.
            /// </summary>
            Client_Call_Get_Controller = 0x3,
            /// <summary>
            /// Call all functions which are subscribed to a particular address.
            /// </summary>
            Client_Call_Call_Subscribed_Function = 0x4,
            /// <summary>
            /// Tell the mod loader to subscribe a specific method delegate to be ran when a call to a function of this address is made.
            /// </summary>
            Client_Call_Subscribe_DLL_Function = 0x5,
            /// <summary>
            /// Tell the client that the function has already been subscribed, thus will be shared with another mod.
            /// </summary>
            Reply_Function_Already_Hooked = 0x6,
            /// <summary>
            /// Used to ask the mod loader whether an address is already hooked.
            /// </summary>
            Client_Call_Check_Address_Hook_State = 0x7,
        }

        /// <summary>
        /// Generates bytes to be sent to the mod loader for the subscribe function for each hook type, allowing multiple hooks to be called and executed on one method at once.
        /// </summary>
        public static byte[] Serialize_Subscribe_Hook_Handler(IntPtr Hook_Address, IntPtr Method_Address)
        {
            byte[] Hook_Handler_Bytes = new byte[8];
            Array.Copy(BitConverter.GetBytes((int)Hook_Address), 0, Hook_Handler_Bytes, 0, 4);
            Array.Copy(BitConverter.GetBytes((int)Method_Address), 0, Hook_Handler_Bytes, 4, 4);
            return Hook_Handler_Bytes;
        }

        /// <summary>
        /// Generates bytes to be sent to the mod loader for the subscribe function for each hook type, allowing multiple hooks to be called and executed on one method at once.
        /// </summary>
        public static Multi_Hook_Handler Deserialize_Subscribe_Hook_Handler(byte[] Data)
        {
            Multi_Hook_Handler Hook_Handler = new Multi_Hook_Handler();
            Hook_Handler.Hook_Address = BitConverter.ToInt32(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Data, 4, 0), 0);
            Hook_Handler.Method_Address = BitConverter.ToInt32(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Data, 4, 4), 0);
            return Hook_Handler;
        }

        /// <summary>
        /// Serialize controller inputs into a simple byte[] array such that they can be sent from client to server and vice versa.
        /// </summary>
        /// <param name="Controller_Inputs"></param>
        /// <returns></returns>
        public static byte[] Serialize_Controller_Inputs_Manual(SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Inputs_Generic Controller_Inputs)
        {
            byte[] Controller_Inputs_New = new byte[25];
            // Sticks
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.LeftStick.X), 0, Controller_Inputs_New, 0, sizeof(short));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.LeftStick.Y), 0, Controller_Inputs_New, 2, sizeof(short));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.RightStick.X), 0, Controller_Inputs_New, 4, sizeof(short));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.RightStick.Y), 0, Controller_Inputs_New, 6, sizeof(short));

            // Triggers
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.LeftTriggerPressure), 0, Controller_Inputs_New, 8, sizeof(short));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.RightTriggerPressure), 0, Controller_Inputs_New, 10, sizeof(short));

            // Axis
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_A), 0, Controller_Inputs_New, 12, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_B), 0, Controller_Inputs_New, 13, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_Back), 0, Controller_Inputs_New, 14, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_L1), 0, Controller_Inputs_New, 15, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_L3), 0, Controller_Inputs_New, 16, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_R1), 0, Controller_Inputs_New, 17, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_R3), 0, Controller_Inputs_New, 18, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_Start), 0, Controller_Inputs_New, 19, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_X), 0, Controller_Inputs_New, 20, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Button_Y), 0, Controller_Inputs_New, 21, sizeof(bool));
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerButtons.Optional_Button_Guide), 0, Controller_Inputs_New, 22, sizeof(bool));

            // DPAD
            Array.Copy(BitConverter.GetBytes(Controller_Inputs.ControllerDPad), 0, Controller_Inputs_New, 23, sizeof(short));

            return Controller_Inputs_New;
        }

        /// <summary>
        /// Deserialize controller inputs from a simple byte[] array which was transmitted by another client.
        /// </summary>
        /// <param name="Controller_Inputs"></param>
        /// <returns></returns>
        public static SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Inputs_Generic Deserialize_Controller_Inputs_Manual(byte[] Controller_Inputs_Serialized)
        {
            SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Inputs_Generic Controller_Inputs = new SonicHeroes.Controller.Sonic_Heroes_Joystick.Controller_Inputs_Generic();
            Controller_Inputs.LeftStick.X = BitConverter.ToInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 0),0);
            Controller_Inputs.LeftStick.Y = BitConverter.ToInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 2),0);
            Controller_Inputs.RightStick.X = BitConverter.ToInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 4),0);
            Controller_Inputs.RightStick.Y = BitConverter.ToInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 6), 0);
            Controller_Inputs.LeftTriggerPressure = BitConverter.ToInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 8), 0);
            Controller_Inputs.RightTriggerPressure = BitConverter.ToInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 10), 0);

            // Buttons | Start Offset = 0x12
            Controller_Inputs.ControllerButtons.Button_A = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 12), 0);
            Controller_Inputs.ControllerButtons.Button_B = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 13), 0);
            Controller_Inputs.ControllerButtons.Button_Back = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 14), 0);
            Controller_Inputs.ControllerButtons.Button_L1 = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 15), 0);
            Controller_Inputs.ControllerButtons.Button_L3 = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 16), 0);
            Controller_Inputs.ControllerButtons.Button_R1 = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 17), 0);
            Controller_Inputs.ControllerButtons.Button_R3 = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 18), 0);
            Controller_Inputs.ControllerButtons.Button_Start = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 19), 0);
            Controller_Inputs.ControllerButtons.Button_X = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 20), 0);
            Controller_Inputs.ControllerButtons.Button_Y = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 21), 0);
            Controller_Inputs.ControllerButtons.Optional_Button_Guide = BitConverter.ToBoolean(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 1, 22), 0);

            // DPAD
            Controller_Inputs.ControllerDPad = BitConverter.ToUInt16(SonicHeroes.Misc.SonicHeroes_Miscallenous.Get_Byte_Range_From_Array(Controller_Inputs_Serialized, 2, 23), 0);

            return Controller_Inputs;
        }

        /// <summary>
        /// Struct which holds addresses obtained from Marshals to allow multiple mods hooked onto one address to all be executed.
        /// </summary>
        public struct Multi_Hook_Handler
        {
            /// <summary>
            /// Address which was either hooked or injected.
            /// </summary>
            public int Hook_Address;
            /// <summary>
            /// The address returned from marshalling the delegate i.e. Destination Address.
            /// </summary>
            public int Method_Address;
        }
    }

    /// <summary>
    /// This is a container used for carrying a message from point A to point B.
    /// </summary>
    public class Network_Message
    {
        /// <summary>
        /// Builds a message to be sent to the machine on the other side.
        /// </summary>
        /// <param name="Message_Type"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] Build_Network_Message(Client_Functions.Message_Type Message_Type, byte[] Data)
        {
            byte[] Data_To_Return = new byte[Data.Length + 1];
            Data_To_Return[0] = (byte)Message_Type;
            Array.Copy(Data, 0, Data_To_Return, 1, Data.Length);
            return Data_To_Return;
        }
    }
   
}
