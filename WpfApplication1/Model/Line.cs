using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Model
{
    public class Line : NotifyBase
    {
        private Shape _from;
        public Shape From { get { return _from; } set { _from = value; NotifyPropertyChanged(); } }

        private Shape _to;
        public Shape To { get { return _to; } set { _to = value; NotifyPropertyChanged(); } }

        
        public double _isSelected = 0;
        public double IsSelected { get { return _isSelected; } set { _isSelected = value; NotifyPropertyChanged(); NotifyPropertyChanged(); } }

        //DO NOT DELETE EMPTY CONSTRUCTOR
        public Line()
        {

        }

        //Creates a line from a serialized line, by finding the equivalents using ID.
        public Line(serialLine l, ObservableCollection<Shape> shapes)
        {
            //Initializing
            bool toFound = false;
            bool fromFound = false;
            //Finding the right shapes
            foreach (Shape s in shapes)
            {
                if (l.IDfrom == s.UID)
                {
                    _from = s;
                    toFound = true;
                } else if (l.IDto == s.UID)
                {
                    _to = s;
                    fromFound = true;
                }
            }
            //Check shapes are found
            if (!toFound || !fromFound)
            {
                throw new Exception("Load failed");//Maybe redo this?
            }
            //Constructor finished
        }
    }
    
    //SerialLine is necessary to serialize lines, since we need to use some sort of ID instead of
    //Just a reference (We can't serialize a reference)
    public class serialLine{

        public int IDfrom;
        public int IDto;

        //Do not delete the empty constructor
        //(It breaks serialization)
        public serialLine()
        {

        }

        public serialLine(Line line)
        {
            IDfrom = line.From.UID;
            IDto = line.To.UID;
        }
    }

}