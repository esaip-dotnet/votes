using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Net;
using System.Timers;
using System.Configuration;


namespace TD_YMCA
{
    public partial class MainWindow : Window
    {
        private const float InferredZPositionClamp = 0.1f;
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));
        private DrawingGroup drawingGroup;
        private DrawingImage imageSource;
        private KinectSensor kinectSensor = null;
        private CoordinateMapper coordinateMapper = null;
        private BodyFrameReader bodyFrameReader = null;
        private Body[] bodies = null;
        private List<Tuple<JointType, JointType>> bones;
        private int displayWidth;
        private int displayHeight;
        private Pen bodyColor;
        private Boolean hasVoted = false;
        private String choix = "";
        private List<String> listeVotant = new List<string>();
        private Timer timer = new Timer(1000);
        private int countDown;


        public MainWindow()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;
            FrameDescription frameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;
            this.displayWidth = frameDescription.Width;
            this.displayHeight = frameDescription.Height;
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.bones = new List<Tuple<JointType, JointType>>();
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.HandLeft));
            this.bodyColor = new Pen(Brushes.Black, 6);
            this.kinectSensor.Open();
            this.drawingGroup = new DrawingGroup();
            this.imageSource = new DrawingImage(this.drawingGroup);
            this.DataContext = this;
            this.InitializeComponent();

            StreamReader streamReader = new StreamReader("liste_votant.csv");
            string ligne = streamReader.ReadLine();
            while (ligne != null)
            {
                this.listeVotant.Add(ligne);
                ligne = streamReader.ReadLine();
            }
            streamReader.Close();

            timer.Elapsed += OnTimedEvent;
        }
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.bodyFrameReader != null)
                this.bodyFrameReader.FrameArrived += this.Reader_FrameArrived;
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
                this.bodyFrameReader.Dispose();
            if (this.kinectSensor != null)
                this.kinectSensor.Close();
        }
        private void Reader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;
            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                        this.bodies = new Body[bodyFrame.BodyCount];
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }
            if (dataReceived)
            {
                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    dc.DrawRectangle(Brushes.White, null, new Rect(0.0, 0.0, this.displayWidth, this.displayHeight));
                    foreach (Body body in this.bodies)
                    {
                        Pen drawPen = this.bodyColor;
                        if (body.IsTracked)
                        {
                            IReadOnlyDictionary<JointType, Joint> joints = body.Joints;
                            Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();
                            foreach (JointType jointType in joints.Keys)
                            {
                                CameraSpacePoint position = joints[jointType].Position;
                                if (position.Z < 0)
                                    position.Z = InferredZPositionClamp;
                                DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                                jointPoints[jointType] = new Point(depthSpacePoint.X, depthSpacePoint.Y);
                            }
                            this.DrawBody(joints, jointPoints, dc, drawPen);
                        }
                    }
                }
            }
        }
        private Point TransformCameraPoint(CameraSpacePoint input)
        {
            double X = 320.0 + 320.0 * input.X;
            double Y = 240.0 - 240.0 * input.Y;
            return new Point(X, Y);
        }
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {
            Point PosMainGauche = TransformCameraPoint(joints[JointType.HandLeft].Position);
            Point PosMainDroite = TransformCameraPoint(joints[JointType.HandRight].Position);
            Point PosCentreEpaule = TransformCameraPoint(joints[JointType.SpineShoulder].Position);

            this.CheckForVoting(PosMainGauche, PosMainDroite, PosCentreEpaule);

            foreach (var bone in this.bones)
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
        }
        private void CheckForVoting(Point PosMainGauche, Point PosMainDroite, Point PosCentreEpaule)
        {
            if (listeVotant.Count != 0)
            {
                //GUI update
                Votant.Text = listeVotant[0];

                //if lef hand up and right hand down
                if (PosMainGauche.Y < PosCentreEpaule.Y && PosMainDroite.Y > PosCentreEpaule.Y)
                {
                    choix = "Gauche";
                    if (timer.Enabled == false)
                    {
                        timer.Enabled = true;
                        countDown = 5;
                    }

                }
                //if right hand up and lef hand down
                else if (PosMainDroite.Y < PosCentreEpaule.Y && PosMainGauche.Y > PosCentreEpaule.Y)
                {
                    choix = "Droite";
                    if (timer.Enabled == false)
                    {
                        timer.Enabled = true;
                        countDown = 5;
                    }
                }
                //if is not in position
                else
                {
                    timer.Enabled = false;
                }
                //triggered by timer end
                if (hasVoted)
                {
                    //Json creating
                    String dataJson = "{";
                    if (choix == "Droite")
                    {
                        dataJson += "'choix':1,";
                        dataJson += "'prenom':'";
                        dataJson += listeVotant[0] + "'";
                        dataJson += "}";
                    }
                    else
                    {
                        dataJson += "'choix':2,";
                        dataJson += "'prenom':'";
                        dataJson += listeVotant[0] + "'";
                        dataJson += "}";

                    }
                    //vote sending
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://coreosjpg.cloudapp.net/api/votes/Elections/BDE/Votes");
                    request.ContentType = "application/json";
                    request.Method = "POST";
                    StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
                    streamWriter.Write(dataJson);
                    streamWriter.Close();
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    listeVotant.Remove(listeVotant[0]);
                    hasVoted = false;

                }

            }
        }

        private void DrawBone(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, JointType jointType0, JointType jointType1, DrawingContext drawingContext, Pen drawingPen)
        {
            Joint joint0 = joints[jointType0];
            Joint joint1 = joints[jointType1];
            if (joint0.TrackingState == TrackingState.NotTracked ||
            joint1.TrackingState == TrackingState.NotTracked)
                return;
            drawingContext.DrawLine(drawingPen, jointPoints[jointType0], jointPoints[jointType1]);
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Handler called every second
            --countDown;
            if (countDown == 0)
            {
                hasVoted = true;
                timer.Enabled = false;
            }
        }


    }
}