using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Risk.Model;
using Risk.Command;

namespace Risk.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; set; }

        public ObservableCollection<Line> Lines { get; set; }

        private UndoRedoController undoRedoController = UndoRedoController.Instance;

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }


        public ICommand AddShapeCommand { get; }
        public ICommand RemoveShapeCommand { get; }
        public ICommand AddLineCommand { get; }
        public ICommand RemoveLinesCommand { get; }

        public MainViewModel()
        {
            ////if (IsInDesignMode)

            Shapes = new ObservableCollection<Shape>() {
                //new Shape() { X = 400, Y = 400, Width = 80, Height = 80 },
               //new Shape() { X = 250, Y = 250, Width = 100, Height = 100 }
            };
            Lines = new ObservableCollection<Line>() { 
              //  new Line() { From = Shapes.ElementAt(0), To = Shapes.ElementAt(1) }
            };

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);
            
            AddShapeCommand = new RelayCommand(AddShape);
             /* RemoveShapeCommand = new RelayCommand<IList>(RemoveShape, CanRemoveShape);
             * AddLineCommand = new RelayCommand(AddLine);
             * RemoveLinesCommand = new RelayCommand<IList>(RemoveLines, CanRemoveLines);
             */

            // The commands are given the methods they should use to execute, and find out if they can execute.
            /*
             * MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
             * MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
             * MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);
             */
        }

        private void AddShape()
        {
            Random rand = new Random();
            undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(rand.Next(0,500), rand.Next(0,1000), 50, 50)));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(100,100,100,100)));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
            //Shapes[0].X = 3;
        }
    }
}