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
        private Shape from;
        public Shape From { get { return from; } set { from = value; NotifyPropertyChanged(); } }

        private Shape to;
        public Shape To { get { return to; } set { to = value; NotifyPropertyChanged(); } }


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
                    from = s;
                    toFound = true;
                } else if (l.IDto == s.UID)
                {
                    to = s;
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