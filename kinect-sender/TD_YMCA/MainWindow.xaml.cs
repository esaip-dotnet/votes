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
        //private DetecteurLettres MoteurDetection = new DetecteurLettres();
        //private char DerniereLettre;
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
            this.listeVotant.Add("Baptiste");
            this.listeVotant.Add("Guénaël");
            this.listeVotant.Add("Aymeric");
            this.listeVotant.Add("Thanh Tuan");
            this.listeVotant.Add("Yohann");
            this.listeVotant.Add("Corentin");
            this.listeVotant.Add("Gilles");
            this.listeVotant.Add("Fadwa");
            this.listeVotant.Add("Adel");
            this.listeVotant.Add("Quentin");
            this.listeVotant.Add("Antoine");
            this.listeVotant.Add("Alexandre");
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
            Point PosCoudeGauche = TransformCameraPoint(joints[JointType.ElbowLeft].Position);
            Point PosCoudeDroit = TransformCameraPoint(joints[JointType.ElbowRight].Position);
            //Debug.WriteLine(string.Format("MG:{0} / MD:{1} / CE:{2} / CG:{3} / CD:{4}", PosMainGauche, PosMainDroite, PosCentreEpaule, PosCoudeGauche, PosCoudeDroit));
            //Debug.WriteLine(string.Format("MG:{0}-{1} / MD:{2}-{3} / CE:{4}-{5} / CG:{6}-{7} / CD:{8}-{9}", Convert.ToInt16(PosMainGauche.X), Convert.ToInt16(PosMainGauche.Y), Convert.ToInt16(PosMainDroite.X),
            //Convert.ToInt16(PosMainDroite.Y), Convert.ToInt16(PosCentreEpaule.X), Convert.ToInt16(PosCentreEpaule.Y), Convert.ToInt16(PosCoudeGauche.X), Convert.ToInt16(PosCoudeGauche.Y), Convert.ToInt16(PosCoudeDroit.X), Convert.ToInt16(PosCoudeDroit.Y)));
            //File.AppendAllText(@"C:\temp\kinectdata.csv", string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}" + Environment.NewLine, Convert.ToInt16(PosMainGauche.X), Convert.ToInt16(PosMainGauche.Y), Convert.ToInt16(PosMainDroite.X), Convert.ToInt16(PosMainDroite.Y), Convert.ToInt16(PosCentreEpaule.X), Convert.ToInt16(PosCentreEpaule.Y), Convert.ToInt16(PosCoudeGauche.X), Convert.ToInt16(PosCoudeGauche.Y), Convert.ToInt16(PosCoudeDroit.X), Convert.ToInt16(PosCoudeDroit.Y)));
            //Votant.Text(ListeVotant[0]);

            if (listeVotant.Count != 0)
            {
                Votant.Text = listeVotant[0];

                //Si main gauche levée
                if (PosMainGauche.Y < PosCentreEpaule.Y && PosMainDroite.Y > PosCentreEpaule.Y)
                {
                    //Console.WriteLine("Main gauche");
                    choix = "Gauche";
                    if (timer.Enabled == false) {
                        timer.Enabled = true;
                        countDown = 5;
                    }
                    
                }
                //Si main droit levée
                else if (PosMainDroite.Y < PosCentreEpaule.Y && PosMainGauche.Y > PosCentreEpaule.Y)
                {
                    //Console.WriteLine("Main gauche");
                    choix = "Droite";
                    if (timer.Enabled == false)
                    {
                        timer.Enabled = true;
                        countDown = 5;
                    }
                }
                else
                {
                    timer.Enabled = false;
                }

                if (hasVoted)
                {
                    //Écriture du Json
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

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Handler appelé toutes les secondes.
            --countDown;
            if (countDown == 0)
            {
                hasVoted = true;
                timer.Enabled = false;
            }
        }


    }
}