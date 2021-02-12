﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using MeetingSystem;
using MeetingSystem.Server;

// -------------------------------------------------
// Developed By : Ragheed Al-Tayeb
// e-Mail       : ragheedemail@gmail.com
// Date         : April 2012
// -------------------------------------------------

namespace rtaNetworking.Streaming
{

    /// <summary>
    /// Provides a streaming server that can be used to stream any images source
    /// to any client.
    /// </summary>
    public class ImageStreamingServer : Server,IDisposable
    {

        private List<Socket> _Clients;
        private Thread _Thread;
        private static ImageStreamingServer instance = null;
        private static Thread _snapshotThread;
        private static MemoryStream lastSnapshotImageStream = null;
        private static byte[] imageBytes = null;

        public static ImageStreamingServer getInstance()
        {
            if (instance == null)
                instance = new ImageStreamingServer();
            return instance;
        }

        private ImageStreamingServer()
        {
            _Clients = new List<Socket>();
            _Thread = null;

            this.Interval = -1;
        }

        /// <summary>
        /// Gets or sets the interval in milliseconds (or the delay time) between 
        /// the each image and the other of the stream (the default is . 
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// Gets a collection of client sockets.
        /// </summary>
        public IEnumerable<Socket> Clients { get { return _Clients; } }

        /// <summary>
        /// Returns the status of the server. True means the server is currently 
        /// running and ready to serve any client requests.
        /// </summary>
        public bool IsRunning { get { return (_Thread != null && _Thread.IsAlive); } }

        /// <summary>
        /// Starts the server to accepts any new connections on the specified port.
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {

            lock (this)
            {
                _Thread = new Thread(new ParameterizedThreadStart(ServerThread));
                _Thread.Name = "ServerThread";
                _Thread.IsBackground = true;
                _snapshotThread = new Thread(UpdateSnapShotImage);
                _Thread.Start(port);
                _snapshotThread.Start();
            }
        }

        /// <summary>
        /// Starts the server to accepts any new connections on the default port (8080).
        /// </summary>
        public override void start()
        {
            this.Start(Info.ScreenServicePort);
        }

        public override void release() { }

        public override void stop()
        {
            if (this.IsRunning)
            {
                try
                {
                    //_Thread.Join();
                    //_Thread.Abort();
                    _Thread.Interrupt();
                }
                catch (Exception e)
                {
                    notifyClientExceptionOcurred(e);
                    EventLog.Write("stop " + e.ToString());
                }
                finally
                {

                    lock (_Clients)
                    {

                        foreach (var s in _Clients)
                        {
                            try
                            {
                                s.Close();
                            }
                            catch { }
                        }
                        _Clients.Clear();

                    }

                    _Thread = null;
                }
            }
        }

        /// <summary>
        /// This the main thread of the server that serves all the new 
        /// connections from clients.
        /// </summary>
        /// <param name="state"></param>
        private void ServerThread(object state)
        {

            try
            {
                Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                Server.Bind(new IPEndPoint(IPAddress.Any, (int)state));
                Server.Listen(10);

                System.Diagnostics.Debug.WriteLine(string.Format("Server started on port {0}.", state));
                foreach (Socket client in Server.IncommingConnectoins())
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), client);

            }
            catch { }

            this.stop();
        }

        /// <summary>
        /// Each client connection will be served by this thread.
        /// </summary>
        /// <param name="client"></param>
        private void ClientThread(object client)
        {

            Socket socket = (Socket)client;

            System.Diagnostics.Debug.WriteLine(string.Format("New client from {0}", socket.RemoteEndPoint.ToString()));

            lock (_Clients)
                _Clients.Add(socket);

            try
            {

                using (var wr = new MjpegWriter(new NetworkStream(socket, true)))
                {
                    // Writes the response header to the client.
                    wr.WriteHeader();
                    while (imageBytes != null)
                    {
                        try
                        {
                            wr.WriteBytes(imageBytes);
                        }
                        catch (IOException e)
                        {
                            System.Diagnostics.Debug.WriteLine(e.ToString());
                            break;
                        }
                        Thread.Sleep(100);
                    }

//                    while(lastSnapshotImageStream != null)
//                    {
//                        lock (lastSnapshotImageStream)
//                        {
//                            lastSnapshotImageStream.Position = 0;
//                            wr.Write(lastSnapshotImageStream);
//                        }
//                    }
//
////                    // Streams the images from the source to the client.
////                    foreach (var imgStream in Screen.Streams(this.ImagesSource))
////                    {
//
//                    //}
                }
            }
            catch { }
            finally
            {
                lock (_Clients)
                    _Clients.Remove(socket);
            }
        }


         private static  ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
         private static  EncoderParameters myEncoderParameters = new EncoderParameters(1);
         private static  System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
         private static  EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);

        private static void UpdateSnapShotImage()
        {
            lastSnapshotImageStream = new MemoryStream();
            var e = Screen.Snapshots().GetEnumerator();
            while (e.MoveNext())
            {
                myEncoderParameters.Param[0] = myEncoderParameter;
                lock (lastSnapshotImageStream)
                {
                    lastSnapshotImageStream.Position = 0;
                    lastSnapshotImageStream.SetLength(0);
                    Image img = e.Current;
                    img.Save(lastSnapshotImageStream, jpgEncoder, myEncoderParameters);
                    imageBytes = lastSnapshotImageStream.ToArray();
                    File.WriteAllBytes(Path.Combine(MeetingSystemForm.WEB_PATH, "..", "streaming", "image.jpg"), imageBytes);
                    Thread.Sleep(16);
                }
            }
            e.Dispose();
            lastSnapshotImageStream = null;
        }


        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.stop();
        }

        #endregion
    }

    static class SocketExtensions
    {

        public static IEnumerable<Socket> IncommingConnectoins(this Socket server)
        {
            while (true)
                yield return server.Accept();
        }


    }


    static class Screen
    {


        public static IEnumerable<Image> Snapshots()
        {
            //return Screen.Snapshots(853, 480, true);
            return Screen.Snapshots(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, true);
        }

        /// <summary>
        /// Returns a 
        /// </summary>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        public static IEnumerable<Image> Snapshots(int width, int height, bool showCursor)
        {
            Size size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);

            Bitmap srcImage = new Bitmap(size.Width, size.Height);
            Graphics srcGraphics = Graphics.FromImage(srcImage);

            bool scaled = (width != size.Width || height != size.Height);
            Bitmap dstImage = srcImage;
            Graphics dstGraphics = srcGraphics;

            if (scaled)
            {
                dstImage = new Bitmap(width, height);
                dstGraphics = Graphics.FromImage(dstImage);
            }

            Rectangle src = new Rectangle(0, 0, size.Width, size.Height);
            Rectangle dst = new Rectangle(0, 0, width, height);
            Size curSize = new Size(32, 32);

            while (true)
            {
                srcGraphics.CopyFromScreen(0, 0, 0, 0, size);

                if (showCursor)
                    Cursors.Default.Draw(srcGraphics, new Rectangle(Cursor.Position, curSize));

                if (scaled)
                {
                    dstGraphics.DrawImage(srcImage, dst, src, GraphicsUnit.Pixel);
                }
                yield return dstImage;

            }

            srcGraphics.Dispose();
            dstGraphics.Dispose();

            srcImage.Dispose();
            dstImage.Dispose();

            yield break;
        }

    }

}
