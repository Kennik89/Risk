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
    //[DataContract]
    public class Shape : NotifyBase
    {

        private static Random _rand = new Random();

        private static int _counter = 0;

        public int Number { get; }

        private double _x = _rand.Next(0, 700);
        private double _y = _rand.Next(0, 500);
        private double _width = 50;
        private double _height = 50;

        //[DataMember]
        public double X
        {
            get { return _x; }
            set { _x = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => CanvasCenterX); }
        }

        //[DataMember]
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

        private bool _isSelected;

        public bool IsSelected { get { return _isSelected; } set { _isSelected = value; NotifyPropertyChanged(); NotifyPropertyChanged(() => SelectedColor); } }
        public Brush SelectedColor => IsSelected ? Brushes.Red : Brushes.Yellow;

        public Shape(double posX, double posY, double posHeight, double posWidth)
        {
            Number = ++_counter;
            this.X = posX;
            this.Y = posY;
            this.Height = posHeight;
            this.Width = posWidth;
        }

        public Shape()
        {
            Number = ++_counter;
        }

        public override string ToString() => Number.ToString();
    }
}
