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


        private bool isAddingLine;
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape addingLineFrom;

        public double ModeOpacity => isAddingLine ? 0.4 : 1.0;
        // Saves the initial point that the shape has during a move operation.
        private Point initialShapePosition;

        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }


        public ICommand AddShapeCommand { get; }
        public ICommand RemoveShapeCommand { get; }
        public ICommand AddLineCommand { get; }
        public ICommand RemoveLinesCommand { get; }

        public ICommand MouseDownShapeCommand { get; }
        public ICommand MouseMoveShapeCommand { get; }
        public ICommand MouseUpShapeCommand { get; }

        public MainViewModel()
        {
            ////if (IsInDesignMode)

            Shapes = new ObservableCollection<Shape>()
            {
                //new Shape() { X = 400, Y = 400, Width = 80, Height = 80 },
                //new Shape() { X = 250, Y = 250, Width = 100, Height = 100 }
            };
            Lines = new ObservableCollection<Line>()
            {
                //  new Line() { From = Shapes.ElementAt(0), To = Shapes.ElementAt(1) }
            };

            UndoCommand = new RelayCommand(undoRedoController.Undo, undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(undoRedoController.Redo, undoRedoController.CanRedo);

            // The commands are given the methods they should use to execute, and find out if they can execute.

            AddShapeCommand = new RelayCommand(AddShape);
            // RemoveShapeCommand = new RelayCommand<IList>(RemoveShape, CanRemoveShape);
            AddLineCommand = new RelayCommand(AddLine);
            //RemoveLinesCommand = new RelayCommand<IList>(RemoveLines, CanRemoveLines);

            // The commands are given the methods they should use to execute, and find out if they can execute.
            /*
             * 
             * 
             * MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
             * MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
             * MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);
             */
            MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
            //MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
            MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);
        }

        private void AddLine()
        {
            isAddingLine = true;
            RaisePropertyChanged(() => ModeOpacity);
        }

        private void AddShape()
        {
            Random rand = new Random();
            undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(rand.Next(0, 500), rand.Next(0, 1000), 50, 50)));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape(100,100,100,100)));
            //undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
            //Shapes[0].X = 3;
        }
        private Line TargetLine(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            return (Line)shapeVisualElement.DataContext;
        }


        private Shape TargetShape(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // From the shapes visual element, the Shape object which is the DataContext is retrieved.
            return (Shape)shapeVisualElement.DataContext;
        }

        private void MouseDownShape(MouseButtonEventArgs e)
        {

        }

        private void MouseMoveShape(MouseButtonEventArgs e)
        {

        }

        private void MouseUpShape(MouseButtonEventArgs e)
        {
            // Used for adding a Line.
            if (isAddingLine)
            {
                // Because a MouseUp event has happened and a Line is currently being drawn, 
                //  the Shape that the Line is drawn from or to has been selected, and is here retrieved from the event parameters.
                var shape = TargetShape(e);
                // This checks if this is the first Shape chosen during the Line adding operation, 
                //  by looking at the addingLineFrom variable, which is empty when no Shapes have previously been choosen.
                // If this is the first Shape choosen, and if so, the Shape is saved in the AddingLineFrom variable.
                //  Also the Shape is set as selected, to make it look different visually.
                if (addingLineFrom == null) { addingLineFrom = shape; addingLineFrom.IsSelected = true; }
                // If this is not the first Shape choosen, and therefore the second, 
                //  it is checked that the first and second Shape are different.
                else if (addingLineFrom.Number != shape.Number)
                {
                    // Now that it has been established that the Line adding operation has been completed succesfully by the user, 
                    //  a Line is added using an 'AddLineCommand', with a new Line given between the two shapes chosen.
                    //undoRedoController.AddAndExecute(new AddLineCommand(Lines, new Line() { From = addingLineFrom, To = shape }));
                    // The property used for visually indicating that a Line is being Drawn is cleared, 
                    //  so the View can return to its original and default apperance.
                    addingLineFrom.IsSelected = false;
                    // The 'isAddingLine' and 'addingLineFrom' variables are cleared, 
                    //  so the MainViewModel is ready for another Line adding operation.
                    isAddingLine = false;
                    addingLineFrom = null;
                    // The property used for visually indicating which Shape has already chosen are choosen is cleared, 
                    //  so the View can return to its original and default apperance.
                    RaisePropertyChanged(() => ModeOpacity);
                }
            }
            // Used for moving a Shape.
            else
            {
                /*         // The Shape is gotten from the mouse event.
                         var shape = TargetShape(e);
                         // The mouse position relative to the target of the mouse event.
                         var mousePosition = RelativeMousePosition(e);

                         // The Shape is moved back to its original position, so the offset given to the move command works.
                         shape.X = initialShapePosition.X;
                         shape.Y = initialShapePosition.Y;

                         // Now that the Move Shape operation is over, the Shape is moved to the final position, 
                         //  by using a MoveNodeCommand to move it.
                         // The MoveNodeCommand is given the offset that it should be moved relative to its original position, 
                         //  and with respect to the Undo/Redo functionality the Shape has only been moved once, with this Command.
                         undoRedoController.AddAndExecute(new MoveShapeCommand(shape, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));

                         // The mouse is released, as the move operation is done, so it can be used by other controls.
                         e.MouseDevice.Target.ReleaseMouseCapture(); */
            }
        }
    }
}

