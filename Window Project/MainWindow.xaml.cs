//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.Kinect;
    using System.Drawing;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Width of output drawing
        /// </summary>

        //private readonly static float RenderWidth = (float)System.Windows.SystemParameters.PrimaryScreenWidth;
        private readonly static float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        //private readonly static float RenderHeight = (float)System.Windows.SystemParameters.PrimaryScreenHeight;//480.0f;
        private readonly static float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly System.Windows.Media.Brush centerPointBrush = System.Windows.Media.Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly System.Windows.Media.Brush trackedJointBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly System.Windows.Media.Brush inferredJointBrush = System.Windows.Media.Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly System.Windows.Media.Pen trackedBonePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly System.Windows.Media.Pen inferredBonePen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Gray, 1);

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        public static Random ran = new Random();

        public static Balls[] balls = {
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 1),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 2),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 3),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 4),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 5),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 6),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 7),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 8),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 9),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 10),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 11),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 12),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 13),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 14),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 15),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 16),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 17),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 18),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 19),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 20),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 21),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 22),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 23),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 24),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 25),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 26),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 27),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 28),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 29),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 30),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 31),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 32),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 33),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 34),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 35),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 36),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 37),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 38),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 39),
            new Balls((float)ran.Next((int) RenderWidth), (float)ran.Next((int) RenderHeight), 0, 0, 0, 0, 10, 40)
        };

        public static Dictionary<int, System.Windows.Point> Oldhead = new Dictionary<int, System.Windows.Point>();
        public static OldBone OBone = new OldBone();

        



        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            ResizeMode = ResizeMode.NoResize;
            System.Windows.Forms.Cursor.Hide();
            this.Background = System.Windows.Media.Brushes.Black;
            InitializeComponent();

        }

        public class Balls
        {
            private double randX = (ran.NextDouble() * 0.2f) - 0.1;
            private double randY = (ran.NextDouble() * 0.2f) - 0.1;
            private bool timeSwitch = true;
            //position of ball
            public System.Windows.Point position;
            //Velocity of ball
            public float dX;
            public float dY;
            //acceleration of ball
            public float ax;
            public float ay;
            //Ball ID
            private int id;
            //Ball rad
            private float radius;
            //ball mass
            private float mass;
            private char colorChar;
            public Balls(float x, float y, float deltaX, float deltaY, float accX, float accY, float r, int i)
            {
                position.X = x;
                position.Y = y;
                dX = deltaX;
                dY = deltaY;
                ax = accX;
                ay = accY;
                radius = r;
                id = i;
                mass = radius * 10.0f;
                colorChar = 'g';
            }
            public float getMass()
            {
                return mass;
            }
            public void update()
            {
                //Switches between gravity and no gravity
                if(DateTime.Now.Minute%2 == 0)
                {
                    ax = -dX * 0.01f;
                    ay = -dY * 0.01f;
                    dX += ax + (float)randX;
                    dY += ay + (float)randY;
                    position.X += dX;
                    position.Y += dY;

                    if(dX < 0)
                    {
                        randX = -Math.Abs(randX);
                    }
                    else
                    {
                        randX = Math.Abs(randX);
                    }

                    if (dY < 0)
                    {
                        randY = -Math.Abs(randX);
                    }
                    else
                    {
                        randY = Math.Abs(randX);
                    }

                    if (Math.Abs(((dX * dX) + (dY * dY))) < 0.08f)
                    {
                        dX = 0;
                        dY = 0;
                    }
                    checkBoundaryCollision();
                }
                else
                {
                    //Friction
                    ax = -dX * 0.01f;
                    ay = -dY * 0.01f;
                    dX += ax;
                    dY += ay + 0.98f;
                    position.X += dX;
                    position.Y += dY;
                    if (Math.Abs(((dX * dX) + (dY * dY))) < 0.08f)
                    {
                        dX = 0;
                        dY = 0;
                    }
                    checkBoundaryCollision();
                }
                
            }
            public char getColor()
            {
                return colorChar;
            }

            public void changeColor()
            {
                char[] charList = new char[6];
                charList[0] = 'r';
                charList[1] = 'o';
                charList[2] = 'y';
                charList[3] = 'g';
                charList[4] = 'b';
                charList[5] = 'p';

                int randomNumberIndex = (int)(ran.Next(0, 6));

                colorChar = charList[randomNumberIndex];
            }
            public int getid()
            {
                return id;
            }
            public void setid(int num)
            {
                id = num;
            }
            public float getdX()
            {
                return dX;
            }
            public float getdY()
            {
                return dY;
            }
            public float getRadius()
            {
                return radius;
            }
            public System.Windows.Point getPosition()
            {
                return position;
            }
            public float getAccelerationX()
            {
                return ax;
            }
            public float getAccelerationY()
            {
                return ay;
            }
            public void checkBoundaryCollision()
            {
                if(DateTime.Now.Minute%2 == 0)
                {
                    if (position.X > RenderWidth + radius)
                    {
                        position.X = 0 - radius;
                    }
                    else if (position.X < 0 - radius)
                    {
                        position.X = RenderWidth + radius;
                    }
                    else if (position.Y > RenderHeight + radius)
                    {
                        position.Y = 0 - radius;
                    }
                    else if (position.Y < 0 - radius)
                    {
                        position.Y = RenderHeight + radius;
                    }
                }
                else
                {
                    if (position.X > RenderWidth + radius)
                    {
                        position.X = 0 - radius;
                    }
                    else if (position.X < 0 - radius)
                    {
                        position.X = RenderWidth + radius;
                    }
                    else if (position.Y > RenderHeight - radius)
                    {
                        position.Y = RenderHeight - radius;
                        dY *= -1;
                    }
                }
            }
            public void checkBallCollision(Balls other)
            {
                Boolean overlap = false;
                //Distance between two points
                float fDistance = (float)Math.Sqrt((((position.X - other.getPosition().X) * (position.X - other.getPosition().X)) + ((position.Y - other.getPosition().Y) * (position.Y - other.getPosition().Y))));
                //Normal
                float nx = (float)(other.position.X - position.X) / fDistance;
                float ny = (float)(other.position.Y - position.Y) / fDistance;
                //Tangent
                float tx = -ny;
                float ty = nx;
                //Dot Product Tangent
                float dpTan1 = dX * tx + dY * ty;
                float dpTan2 = other.getdX() * tx + other.getdY() * ty;
                //Dot Product Normal
                float dpNorm1 = dX * nx + dY * ny;
                float dpNorm2 = other.getdX() * nx + other.getdY() * ny;
                //Momentum
                float m1 = (dpNorm1 * (mass - other.mass) + 2.0f * other.mass * dpNorm2) / (mass + other.mass);
                float m2 = (dpNorm2 * (other.mass - mass) + 2.0f * mass * dpNorm1) / (mass + other.mass);
                float fOverlap = 0.5f * (fDistance - radius - other.getRadius());
                if (fDistance < (radius + other.getRadius()))
                {
                    overlap = true;
                }
                if (id != other.getid())
                {
                    if (overlap)
                    {
                        position.X -= fOverlap * (position.X - other.getPosition().X) / fDistance;
                        position.Y -= fOverlap * (position.Y - other.getPosition().Y) / fDistance;

                        other.position.X += fOverlap * (position.X - other.getPosition().X) / fDistance;
                        other.position.Y += fOverlap * (position.Y - other.getPosition().Y) / fDistance;

                        dX = tx * dpTan1 + nx * m1;
                        dY = ty * dpTan1 + ny * m1;

                        other.dX = tx * dpTan2 + nx * m2;
                        other.dY = ty * dpTan2 + ny * m2;
                        changeColor();
                        other.changeColor();
                    }
                }
            }
        }

        public class MrSpooks
        {
            private int id;
            public MrSpooks()
            {
                id = 1;
            }
            public void newId()
            {
                id++;
            }

            public void boneShadow(System.Windows.Point sp, System.Windows.Point ep, DrawingContext draw)
            {
                System.Windows.Media.Pen blackPen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.Black, 9);
                draw.DrawLine(blackPen, sp, ep);

                draw.DrawEllipse(System.Windows.Media.Brushes.Black, null, sp, 4, 4);
                draw.DrawEllipse(System.Windows.Media.Brushes.Black, null, ep, 4, 4);
            }

            public void boneEnhance(System.Windows.Point sp, System.Windows.Point ep, DrawingContext draw, JointType startJoint, JointType endJoint)
            {
                System.Windows.Media.Pen pen = new System.Windows.Media.Pen(System.Windows.Media.Brushes.WhiteSmoke, 12);
                
                Tuple<JointType, JointType> joint = new Tuple<JointType, JointType>(startJoint, endJoint);
                int radius = 6;
                draw.DrawLine(pen, sp, ep);
                draw.DrawEllipse(System.Windows.Media.Brushes.WhiteSmoke, null, sp, radius, radius);
                draw.DrawEllipse(System.Windows.Media.Brushes.WhiteSmoke, null, ep, radius, radius);
                
                for (int i = 0; i < balls.Length; i++)
                {
                    float mass = 25.0f * 10.0f;
                    float fLineX1 = (float)(ep.X - sp.X);
                    float fLineY1 = (float)(ep.Y - sp.Y);
                    float fLineX2 = (float)(balls[i].getPosition().X - sp.X);
                    float fLineY2 = (float)(balls[i].getPosition().Y - sp.Y);
                    float fEdgeLength = (fLineX1 * fLineX1) + (fLineY1 * fLineY1);
                    float t = Math.Max(0, Math.Min(fEdgeLength, ((fLineX1 * fLineX2) + (fLineY1 * fLineY2)))) / fEdgeLength;
                    float fClosestPointX = (float)(sp.X + t * fLineX1);
                    float fClosestPointY = (float)(sp.Y + t * fLineY1);
                    float dX;
                    float dY;
                    System.Windows.Point closest = new System.Windows.Point();
                    closest.X = fClosestPointX;
                    closest.Y = fClosestPointY;
                    if (!OBone.used(id, joint, i))
                    {
                        OBone.setKey(id, joint, i, closest);
                        dX = -(balls[i].getdX());
                        dY = -(balls[i].getdY());
                    }
                    else
                    {
                        dX = (float)(fClosestPointX - OBone.keyLock(id, joint, i).X);
                        dY = (float)(fClosestPointY - OBone.keyLock(id, joint, i).Y);
                        OBone.changeKey(id, joint, i, closest);
                    }
                    Boolean overlap = false;
                    float fDistance = (float)Math.Sqrt((((fClosestPointX - balls[i].getPosition().X) * (fClosestPointX - balls[i].getPosition().X)) + ((fClosestPointY - balls[i].getPosition().Y) * (fClosestPointY - balls[i].getPosition().Y))));
                    float nx = (float)(balls[i].position.X - fClosestPointX) / fDistance;
                    float ny = (float)(balls[i].position.Y - fClosestPointY) / fDistance;
                    float tx = -ny;
                    float ty = nx;
                    float dpTan1 = dX * tx + dY * ty;
                    float dpTan2 = balls[i].getdX() * tx + balls[i].getdY() * ty;
                    float dpNorm1 = dX * nx + dY * ny;
                    float dpNorm2 = balls[i].getdX() * nx + balls[i].getdY() * ny;
                    float m1 = (dpNorm1 * (mass - balls[i].getMass()) + 2.0f * balls[i].getMass() * dpNorm2) / (mass + balls[i].getMass());
                    float m2 = (dpNorm2 * (balls[i].getMass() - mass) + 2.0f * mass * dpNorm1) / (mass + balls[i].getMass());
                    float fOverlap = 0.5f * (fDistance - radius - balls[i].getRadius());
                    if (fDistance < (radius + balls[i].getRadius()))
                    {
                        overlap = true;
                    }
                    if (overlap)
                    {
                        balls[i].position.X += fOverlap * (fClosestPointX - balls[i].getPosition().X) / fDistance;
                        balls[i].position.Y += fOverlap * (fClosestPointY - balls[i].getPosition().Y) / fDistance;
                        balls[i].dX = tx * dpTan2 + nx * m2;
                        balls[i].dY = ty * dpTan2 + ny * m2;
                    }
                }
            }
            public void drawHead(System.Windows.Point point, DrawingContext draw, double distanceRatio)
            {
                //Draws Head and checks ball for collision
                for (int i = 0; i < balls.Length; i++)
                {
                    Boolean overlap = false;
                    float mass = 25.0f * 10.0f;
                    float dX;
                    float dY;
                    if (Oldhead.ContainsKey(id))
                    {
                        dX = (float)(point.X - Oldhead[id].X);
                        dY = (float)(point.Y - Oldhead[id].Y);
                    }
                    else
                    {
                        dX = -(balls[i].getdX());
                        dY = -(balls[i].getdY());
                        Oldhead.Add(id, point);
                    }

                    float fDistance = (float)Math.Sqrt((((point.X - balls[i].getPosition().X) * (point.X - balls[i].getPosition().X)) + ((point.Y - balls[i].getPosition().Y) * (point.Y - balls[i].getPosition().Y))));
                    float nx = (float)(balls[i].position.X - point.X) / fDistance;
                    float ny = (float)(balls[i].position.Y - point.Y) / fDistance;
                    float tx = -ny;
                    float ty = nx;
                    float dpTan1 = dX * tx + dY * ty;
                    float dpTan2 = balls[i].getdX() * tx + balls[i].getdY() * ty;
                    float dpNorm1 = dX * nx + dY * ny;
                    float dpNorm2 = balls[i].getdX() * nx + balls[i].getdY() * ny;
                    float m1 = (dpNorm1 * (mass - balls[i].getMass()) + 2.0f * balls[i].getMass() * dpNorm2) / (mass + balls[i].getMass());
                    float m2 = (dpNorm2 * (balls[i].getMass() - mass) + 2.0f * mass * dpNorm1) / (mass + balls[i].getMass());
                    float fOverlap = 0.5f * (fDistance - 25f - balls[i].getRadius());
                    if (fDistance < (25 + balls[i].getRadius()))
                    {
                        overlap = true;
                    }
                    if (overlap)
                    {
                        balls[i].position.X += fOverlap * (point.X - balls[i].getPosition().X) / fDistance;
                        balls[i].position.Y += fOverlap * (point.Y - balls[i].getPosition().Y) / fDistance;
                        balls[i].dX = tx * dpTan2 + nx * m2;
                        balls[i].dY = ty * dpTan2 + ny * m2;
                    }
                }
                Oldhead[id] = point;
                double r = (distanceRatio);
                draw.DrawEllipse(System.Windows.Media.Brushes.WhiteSmoke, null, point, 25, 25);
                //draw.DrawEllipse(System.Windows.Media.Brushes.Black, null, point, 23, 23);
                
            }
        }

        public class OldBone
        {
            private Dictionary<string, System.Windows.Point> Dict;
            public OldBone()
            {
                Dict = new Dictionary<string, System.Windows.Point>();
            }
            public void setKey(int KeyId, Tuple<JointType, JointType> KeyJoint, int KeyNumber, System.Windows.Point pt)
            {
                string key = KeyId + KeyJoint.Item1.ToString() + KeyJoint.Item2.ToString() + KeyNumber;
                Dict.Add(key, pt);
            }
            public void changeKey(int KeyId, Tuple<JointType, JointType> KeyJoint, int KeyNumber, System.Windows.Point pt)
            {
                string key = KeyId + KeyJoint.Item1.ToString() + KeyJoint.Item2.ToString() + KeyNumber;
                Dict[key] = pt;
            }
            public System.Windows.Point keyLock(int KeyId, Tuple<JointType, JointType> KeyJoint, int KeyNumber)
            {
                string key = KeyId + KeyJoint.Item1.ToString() + KeyJoint.Item2.ToString() + KeyNumber;
                return Dict[key];
            }
            public bool used(int KeyId, Tuple<JointType, JointType> KeyJoint, int KeyNumber)
            {
                string key = KeyId + KeyJoint.Item1.ToString() + KeyJoint.Item2.ToString() + KeyNumber;
                return Dict.ContainsKey(key);
            }
        }



        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            

            Image.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable(new TransformSmoothParameters()
                {
                    Smoothing = 0.75f,
                    Correction = 0.3f,
                    Prediction = 1.0f,
                    JitterRadius = 0.05f,
                    MaxDeviationRadius = 0.04f
                });



                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                //this.sensor.DepthFrameReady += this.DepthFrameReady;

                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }
            
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.Stop();
            }
        }

        

        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }


            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(System.Windows.Media.Brushes.Black, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                for (int i = 0; i < balls.Length; i++)
                {
                    if(balls[i].getColor() == 'g')
                    {
                        dc.DrawEllipse(System.Windows.Media.Brushes.Lime, null, balls[i].getPosition(), balls[i].getRadius(), balls[i].getRadius());
                    }
                    else if (balls[i].getColor() == 'r')
                    {
                        dc.DrawEllipse(System.Windows.Media.Brushes.Red, null, balls[i].getPosition(), balls[i].getRadius(), balls[i].getRadius());
                    }
                    else if (balls[i].getColor() == 'b')
                    {
                        dc.DrawEllipse(System.Windows.Media.Brushes.Blue, null, balls[i].getPosition(), balls[i].getRadius(), balls[i].getRadius());
                    }
                    else if (balls[i].getColor() == 'y')
                    {
                        dc.DrawEllipse(System.Windows.Media.Brushes.Yellow, null, balls[i].getPosition(), balls[i].getRadius(), balls[i].getRadius());
                    }
                    else if (balls[i].getColor() == 'p')
                    {
                        dc.DrawEllipse(System.Windows.Media.Brushes.Purple, null, balls[i].getPosition(), balls[i].getRadius(), balls[i].getRadius());
                    }
                    else if (balls[i].getColor() == 'o')
                    {
                        dc.DrawEllipse(System.Windows.Media.Brushes.Orange, null, balls[i].getPosition(), balls[i].getRadius(), balls[i].getRadius());
                    }
                    //dc.DrawEllipse(System.Windows.Media.Brushes.Black, null, balls[i].getPosition(), balls[i].getRadius()-2, balls[i].getRadius()-2);
                    balls[i].update();
                }

                for (int i = 0; i < balls.Length; i++)
                {
                    for (int j = 0; j < balls.Length; j++)
                    {
                        if (i != j)
                        {
                            balls[i].checkBallCollision(balls[j]);
                        }
                    }                
                }

                MrSpooks spooks = new MrSpooks();

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        //RenderClippedEdges(skel, dc);
                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc, spooks);
                            spooks.newId();
                        }
                    }
                }

                //Fixes Disapearing Ball Glitch
                for(int i = 0; i < balls.Length; i++)
                {
                    if(Double.IsNaN(balls[i].getPosition().X) || Double.IsNaN(balls[i].getPosition().Y))
                    {
                        System.Windows.Point ErrorPoint = new System.Windows.Point();
                        ErrorPoint.X = (float)ran.Next((int)RenderWidth);
                        ErrorPoint.Y = (float)ran.Next((int)RenderHeight);
                        balls[i].position = ErrorPoint;
                        balls[i].dX = 0;
                        balls[i].dY = 0;
                        balls[i].ax = 0;
                        balls[i].ay = 0;
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                
            }
        }
        
        

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext, MrSpooks s)
        {
            MrSpooks spooks = s;

            
            // Render Torso
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.Head].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), drawingContext, JointType.Head, JointType.ShoulderCenter);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderLeft].Position), drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderRight].Position), drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position), drawingContext, JointType.ShoulderCenter, JointType.Spine);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position), drawingContext, JointType.Spine, JointType.HipCenter);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HipLeft].Position), drawingContext, JointType.HipCenter, JointType.HipLeft);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HipRight].Position), drawingContext, JointType.HipCenter, JointType.HipRight);


            // Left Arm
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ElbowLeft].Position), drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ElbowLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.WristLeft].Position), drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.WristLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HandLeft].Position), drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ElbowRight].Position), drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.ElbowRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.WristRight].Position), drawingContext, JointType.ElbowRight, JointType.WristRight);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.WristRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HandRight].Position), drawingContext, JointType.WristRight, JointType.HandRight);


            // Left Leg
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.HipLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.KneeLeft].Position), drawingContext, JointType.HipLeft, JointType.KneeLeft);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.KneeLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.AnkleLeft].Position), drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.AnkleLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.FootLeft].Position), drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.HipRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.KneeRight].Position), drawingContext, JointType.HipRight, JointType.KneeRight);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.KneeRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.AnkleRight].Position), drawingContext, JointType.KneeRight, JointType.AnkleRight);
            spooks.boneEnhance(SkeletonPointToScreen(skeleton.Joints[JointType.AnkleRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.FootRight].Position), drawingContext, JointType.AnkleRight, JointType.FootRight);





















            /*// Render Torso
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.Head].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderLeft].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderRight].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HipLeft].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HipRight].Position), drawingContext);


            // Left Arm
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ElbowLeft].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ElbowLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.WristLeft].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.WristLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HandLeft].Position), drawingContext);

            // Right Arm
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ShoulderRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.ElbowRight].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.ElbowRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.WristRight].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.WristRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.HandRight].Position), drawingContext);


            // Left Leg
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.HipLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.KneeLeft].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.KneeLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.AnkleLeft].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.AnkleLeft].Position), SkeletonPointToScreen(skeleton.Joints[JointType.FootLeft].Position), drawingContext);

            // Right Leg
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.HipRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.KneeRight].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.KneeRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.AnkleRight].Position), drawingContext);
            spooks.boneShadow(SkeletonPointToScreen(skeleton.Joints[JointType.AnkleRight].Position), SkeletonPointToScreen(skeleton.Joints[JointType.FootRight].Position), drawingContext);
            */
            double distance = Math.Sqrt(((SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position).X - SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position).X) * (SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position).X - SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position).X)) + ((SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position).Y - SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position).Y) * (SkeletonPointToScreen(skeleton.Joints[JointType.Spine].Position).Y - SkeletonPointToScreen(skeleton.Joints[JointType.HipCenter].Position).Y)));
            spooks.drawHead(this.SkeletonPointToScreen(skeleton.Joints[JointType.Head].Position), drawingContext, distance);


        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private System.Windows.Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new System.Windows.Point(depthPoint.X, depthPoint.Y);
        }

        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        /// 
    }
}

