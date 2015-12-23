/**
 * @Authors : Esaip's students
 * @Promotion : IR2016
 * @Tutor : Jean-Philippe Gouigoux
 * @Date 20/12/2015 (DD/MM/YY)
 * @Brief : Kinect vote application 
 * Left hand = No
 * Right hand = Yes
 **/
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
namespace VoteByKinectProject
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
        private String choice = "";
        private Timer timer = new Timer(1000);
        private int countDown;
        private String url = VoteByKinectProject.Properties.Settings.Default.url.ToString();
        
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
                else
                {
                    Votant.IsEnabled = true;
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

        //When skeleton detected
        private void DrawBody(IReadOnlyDictionary<JointType, Joint> joints, IDictionary<JointType, Point> jointPoints, DrawingContext drawingContext, Pen drawingPen)
        {

            if (Votant.Text != "")
            {
                Votant.IsEnabled = false;
                checkHandsPosition(joints);
               
                if (hasVoted)
                {
                    String dataJson = parseJson(choice);
                    sendVote(dataJson);

                    hasVoted = false;
                    Votant.Text = "";
                }
            }

            foreach (var bone in this.bones)
                this.DrawBone(joints, jointPoints, bone.Item1, bone.Item2, drawingContext, drawingPen);
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

        private void checkHandsPosition(IReadOnlyDictionary<JointType, Joint> joints)
        {
            Point HandLeftPos = TransformCameraPoint(joints[JointType.HandLeft].Position);
            Point HandRightPos = TransformCameraPoint(joints[JointType.HandRight].Position);
            Point SpineShoulderPos = TransformCameraPoint(joints[JointType.SpineShoulder].Position);
            Point ElbowLeftPos = TransformCameraPoint(joints[JointType.ElbowLeft].Position);
            Point ElbowRightPos = TransformCameraPoint(joints[JointType.ElbowRight].Position);

            //If right hand raised
            if (HandRightPos.Y < SpineShoulderPos.Y && HandLeftPos.Y > SpineShoulderPos.Y)
            {
                          
                if (timer.Enabled == false)
                {
                    timer.Enabled = true;
                    countDown = 3;
                    choice = "Oui";    
                }
                Vote.Content = "Vous allez voter " + choice + " dans " + countDown;  
            }

            //If left hand raised  
            else if (HandLeftPos.Y < SpineShoulderPos.Y && HandRightPos.Y > SpineShoulderPos.Y)
            {               
                if (timer.Enabled == false)
                {
                    timer.Enabled = true;
                    countDown = 3;
                    choice = "Non";
                }
                Vote.Content = "Vous allez voter " + choice + " dans " + countDown;  

            }
            else
            {
                timer.Enabled = false;
                Vote.Content = ""; 
            }                               
        }

        //Handler for timer called every seconds
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {          
            --countDown;
            
            if (countDown == 0)
            {
                hasVoted = true;
                timer.Enabled = false;
            }
        }     

        private String parseJson(String choix)
        {
            return "{\"choix\":\"" + choix + "\", \"prenom\":\"" + Votant.Text + "\"})";
        }

        private void sendVote(String dataJson)
        {
            String route = url + "/api/votes/Elections/BDE/Votes";
            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(route);
            request.ContentType = "application/json";
            request.Method = "POST";

            StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
            streamWriter.Write(dataJson);
            streamWriter.Close();
        }

    }
}
