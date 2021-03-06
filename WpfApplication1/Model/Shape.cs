﻿using System;
using System.Runtime.Serialization;

namespace Model
{
    [DataContract]
    [KnownType(typeof(NotifyBase))]
    public class Shape : NotifyBase
    {

        private static Random _rand = new Random();

        [DataMember]
        private static int _counter = 0;

        [DataMember]
        public int Uid { get; set; }//A setter is necessary for the serializer to work

        [DataMember]
        private double _x = _rand.Next(0, 700);
        [DataMember]
        private double _y = _rand.Next(0, 500);
        [DataMember]
        private double _width = 50;
        [DataMember]
        private double _height = 50;

        public double X
        {
            get { return _x; }
            set { _x = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterX); }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterY); }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterX); NotifyPropertyChanged(() => CenterX); }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterY); NotifyPropertyChanged(() => CenterY); }
        }

        public double CanvasCenterX
        {
            get { return X + Width / 2; }
            set { X = value - Width / 2; NotifyPropertyChanged(() => X); }
        }

        public double CanvasCenterY
        {
            get { return Y + Height / 2; }
            set { Y = value - Height / 2; NotifyPropertyChanged(() => Y); }
        }

        public double CenterX => Width / 2;

        public void Notify()
        {
            NotifyPropertyChanged();
        }

        public double CenterY => Height / 2;

        //This indicates whether it is marked or not. This has to be a double,
        //Since it binds to the wpf, where opacity for the "glow" is either 1
        //(Visible) or 0 (Invisible)
        private double _isSelected = 0;
        public double IsSelected { get { return _isSelected; } set { _isSelected = value; NotifyPropertyChanged(); NotifyPropertyChanged(); } }

        public Shape(double posX, double posY, double posHeight, double posWidth)
        {
            Uid = ++_counter;
            this.X = posX;
            this.Y = posY;
            this.Height = posHeight;
            this.Width = posWidth;
        }

        public Shape()
        {
            Uid = ++_counter;
        }

        public override string ToString() => Uid.ToString();
    }
}
