using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Risk.Model
{
    [DataContract]
    [KnownType(typeof(NotifyBase))]
    public class Shape : NotifyBase
    {

        private static Random _rand = new Random();

        [DataMember]
        private static int _counter = 0;

        [DataMember]
        public int UID { get; set; }//A setter is necessary for the serializer to work

        [DataMember]
        private double _x = _rand.Next(0, 700);
        [DataMember]
        private double _y = _rand.Next(0, 500);
        [DataMember]
        private double _width = 50;
        [DataMember]
        private double _height = 50;

        private bool _isSelected = false;

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
        public double CenterY => Height / 2;

        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => SelectedColor); } }
        public Brush SelectedColor => IsSelected ? Brushes.Red : Brushes.Yellow;

        public Shape(double posX, double posY, double posHeight, double posWidth)
        {
            UID = ++_counter;
            this.X = posX;
            this.Y = posY;
            this.Height = posHeight;
            this.Width = posWidth;
        }

        public Shape()
        {
            UID = ++_counter;
        }

        public override string ToString() => UID.ToString();
    }
}
