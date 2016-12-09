using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Risk.Model;
using Risk.Command;
using Risk.LoadSave;

namespace Risk.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; set; }
        public ObservableCollection<Line> Lines { get; set; }


        //this is for the temporary line used while adding a line
        public ObservableCollection<Line> TempLines { get; set; }

        // Enable to call the methods from those classes
        private _serializer _serializer = _serializer.Instance;
        private UndoRedoController _undoRedoController = UndoRedoController.Instance;

        private bool _isAddingLine;
        // Used for saving the shape that a line is drawn from, while it is being drawn.
        private Shape _addingLineFrom;

        public bool _isDataEditable = false;
        public bool isDataEditable
        {
            get { return _isDataEditable; }
            set { _isDataEditable = value; RaisePropertyChanged();
            }
        }

        //Used when no shape is selected. Thus, when no shape is selected, this is used.
        private Shape dummyShape = new Shape(0, 0, 0, 0);

        private Shape standardRefShape = new Shape(0, 0, 0, 0);

        public double xselected
        {
            get { return selShape.X; }
            set
            {
                double xstart = selShape.X;
                double dx = value - xstart;
                if (!(dx < 0.0001 && dx > -0.0001) )//Floating point arithmetics
                {
                    _undoRedoController.AddAndExecute(new EditShapeCommand(selShape, dx, 0, 0, 0));
                }
                RaisePropertyChanged();
            }
        }
        public double yselected
        {
            get { return selShape.Y; }
            set
            {
                double ystart = selShape.Y;
                double dy = value - ystart;
                if (!(dy < 0.0001 && dy > -0.0001))//Floating point arithmetics
                {
                    _undoRedoController.AddAndExecute(new EditShapeCommand(selShape, 0, dy, 0, 0));
                }
                RaisePropertyChanged();

            }
        }
        public double widthselected
        {
            get { return selShape.Width; }
            set
            {
                double widthstart = selShape.Width;
                double dwidth = value - widthstart;
                if (!(dwidth < 0.0001 && dwidth > -0.0001))//Floating point arithmetics
                {
                    _undoRedoController.AddAndExecute(new EditShapeCommand(selShape, 0, 0, dwidth, 0));
                }
                RaisePropertyChanged();

            }
        }
        public double heightselected
        {
            get { return selShape.Height; }
            set
            {
                double heightstart = selShape.Height;
                double dheight = value - heightstart;
                if (!(dheight < 0.0001 && dheight > -0.0001))//Floating point arithmetics
                {
                    _undoRedoController.AddAndExecute(new EditShapeCommand(selShape, 0, 0, 0, dheight));
                }
                RaisePropertyChanged();
            }
        }
        public Shape selShape
        {
            get { return selectedShape; }
            set { selectedShape = value;
                heightselected = selectedShape.Height;
                widthselected = selectedShape.Width;
                xselected = selectedShape.X;
                yselected = selectedShape.Y;
                RaisePropertyChanged();
            }
        }

        private Shape tempLineShape = new Shape(0,0,0,0);
        //For templine: To will always be tempLineShape, placed to follow the mouse
        //From will be the starting point of the new line.
        private Line tempLine = new Line();
        private bool tempLineInUse = false;

        public double ModeOpacity => _isAddingLine ? 0.4 : 1.0;
        // Saves the initial point that the shape has during a move operation.
        private Point _initialShapePosition;
        // Saves the initial point that the mouse has during a move operation.
        private Point initialMousePosition;
        private bool isDragging = false;
        private bool isMarked = false;//Indicates if something is marked
        private bool isMarkedShape = false;//Indicates if it is a shape or a line that's marked (For data in the side)
        public Shape selectedShape;//A marked shape
        private Line selectedLine;//A marked Line
        //private Shape _selectedObject;
//        private Shape _selectedShape;
        private Shape _holdingShape;

        #region ICommand getters
        /*  UNDO/REDO   */
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        /*  Right Click Commands */
        public ICommand AddShapeOnCanvasCommand { get; }
        public ICommand AddShapeWithLineCanvasCommand { get; }
        public ICommand DeleteLineCommand { get; }
        public ICommand DeleteShapeCommand { get; }
        public ICommand AddLineToShapeCommand { get; }



        /*  MAP FUNCTIONALITIES */
        public ICommand AddShapeCommand { get; }
        public ICommand AddLineCommand { get; }
        public ICommand DeleteCommand { get; }

        /*  FUNCTIONALITY FROM CONTEXTMENU */
        public ICommand AddShapeFromContextCommand { get; }
        public ICommand AddLineFromContextCommand { get; }

        /*  MOUSE CONTROLLER    */
        public ICommand MouseDownShapeCommand { get; }
        public ICommand MouseMoveShapeCommand { get; }
        public ICommand MouseUpShapeCommand { get; }

        //Line clicks
        public ICommand MouseDownLineCommand { get; }
        public ICommand MouseMoveLineCommand { get; }
        public ICommand MouseUpLineCommand { get; }

        //Canvas clicks
        public ICommand MouseDownCanvasCommand { get; }
        public ICommand MouseMoveCanvasCommand { get; }
        public ICommand MouseUpCanvasCommand { get; }

        /*  FILE MENU CONTROLLER */
        public ICommand NewMapCommand { get; }
        public ICommand LoadMapCommand { get; }
        public ICommand SaveMapCommand { get; }
        public ICommand ExitCommand { get; }

        /*  GAME CONTROLLER */
        public ICommand StartCommand { get; }

        /* EDIT CONTROLLER */
        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        #endregion

        public MainViewModel()
        {
            Shapes = new ObservableCollection<Shape>();
            Lines = new ObservableCollection<Line>();

            UndoCommand = new RelayCommand(Undo, _undoRedoController.CanUndo);
            RedoCommand = new RelayCommand(Redo, _undoRedoController.CanRedo);

            AddShapeCommand = new RelayCommand(AddShape);
            AddLineCommand = new RelayCommand(AddLine);
            DeleteCommand = new RelayCommand(Delete);

            MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
            MouseMoveShapeCommand = new RelayCommand<MouseEventArgs>(MouseMoveShape);
            MouseUpShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpShape);

            MouseDownLineCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownLine);
            MouseMoveLineCommand = new RelayCommand<MouseEventArgs>(MouseMoveLine);
            MouseUpLineCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpLine);

            MouseDownCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownCanvas);
            MouseMoveCanvasCommand = new RelayCommand<MouseEventArgs>(MouseMoveCanvas);
            MouseUpCanvasCommand = new RelayCommand<MouseButtonEventArgs>(MouseUpCanvas);

            NewMapCommand = new RelayCommand(NewMap);
            LoadMapCommand = new RelayCommand(LoadMap);
            SaveMapCommand = new RelayCommand(SaveMap);
            ExitCommand = new RelayCommand(Exit);
            StartCommand = new RelayCommand(StartMap);
            CutCommand = new RelayCommand(Cut);
            CopyCommand = new RelayCommand(Copy);
            PasteCommand = new RelayCommand(Paste);

            AddShapeOnCanvasCommand = new RelayCommand(AddShapeOnCanvas);
            AddShapeWithLineCanvasCommand = new RelayCommand(AddShapeWithLineCanvas);
            DeleteLineCommand = new RelayCommand(DeleteLine);
            DeleteShapeCommand = new RelayCommand(DeleteShape);
            AddLineToShapeCommand = new RelayCommand(AddLineToShape);


        //Sets the selected shape
        selectedShape = dummyShape;
            //Assigns the temporary line

            TempLines = new ObservableCollection<Line>();
            tempLine.To = tempLineShape;
            tempLine.From = tempLineShape;
            TempLines.Add(tempLine);
        }

        /* METHOD THAT WILL BE CALLED WHEN THE BUTTON IS PRESSED */
        private void NewMap()
        {
            Lines.Clear();
            Shapes.Clear();
            _undoRedoController.Clear();
        }

        private void LoadMap()
        {
            if (_serializer.Load(Shapes, Lines))
            {
                //Clearer undo-redo-stacken. Shapes og Lines bliver clearet i load,
                //Da disse ikke m� cleares efter alt er loadet, men kun skal cleares
                //Hvis loaden er succesfuld.
                _undoRedoController.Clear();
            }
        }

        private void SaveMap()
        {
            try
            {
                ThreadStart save = new ThreadStart(saveThread);
                Thread s = new Thread(save);
                s.SetApartmentState(ApartmentState.STA);
                s.Start();
                /*Console.WriteLine("Main Thread waiting 10s");
                System.Threading.Thread.Sleep(10000);
                Console.WriteLine("Main Thread waited 10s");
                */
            }

            catch (SerializationException serExc)
            {
                Console.WriteLine("Serialization Failed");
                Console.WriteLine(serExc.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine(
                "The serialization operation failed: {0} StackTrace: {1}",
                exc.Message, exc.StackTrace);
            }
            finally
            {
                Console.WriteLine("Serization Successed");
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Undo()
        {
            _undoRedoController.Undo();
            selShape = selShape;//This is made to make sure a
            //RaisePropertyChanged is called on the selected shape for the
            //Sidepanel
        }
        private void Redo()
        {
            _undoRedoController.Redo();
            selShape = selShape;//This is made to make sure a
            //RaisePropertyChanged is called on the selected shape for the
            //Sidepanel
        }

        private void StartMap()
        {
            throw new NotImplementedException();
        } // Not implemented yet

        private void Cut()
        {
            throw new NotImplementedException();
        }      // Not implemented yet

        private void Copy()
        {
            if (selectedShape != null)
            {
                _holdingShape = selectedShape;
            }
        }

        private void Paste()
        {
          
            //_holdingLines = Lines.Where(x => _holdingShape.Any(y => y.UID == x.From.UID || y.UID == x.To.UID)).ToList(); // List<Shape>
            //_undoRedoController.AddAndExecute(new PasteCommand(Shapes, new Shape()));

            //MouseDownShapeCommand = new RelayCommand<MouseButtonEventArgs>(MouseDownShape);
            _undoRedoController.AddAndExecute(new PasteShapeCommand(Shapes, Lines, _holdingShape, new Shape()));
        }    // Not implemented yet

        private void AddLine()
        {
            _isAddingLine = true;
            RaisePropertyChanged(() => ModeOpacity);
            isDataEditable = false;
        }

        private void AddShape()
        {
            _undoRedoController.AddAndExecute(new AddShapeCommand(Shapes, new Shape()));
        }

        private void Delete() 
        {
            //isMarked? -> lookUp(Shape or Line) -> call the remove
            if (isMarked && isMarkedShape && selectedShape != null)
            {
                _undoRedoController.AddAndExecute(new RemoveShapesCommand(Shapes, Lines, selectedShape)); // Shape only
            } else if (isMarked && !isMarkedShape && selectedLine != null)
            {
                List<Line> removingLines = new List<Line>();
                removingLines.Add(selectedLine);
                _undoRedoController.AddAndExecute(new RemoveLinesCommand(Lines, removingLines));
            }

        }

        /* NON-BUTTON METHODS */

        private void saveThread()
        {
            _serializer.Save(Shapes, Lines);
        }

        private Line TargetLine(MouseEventArgs e)//Brugt til at fange linjen
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
        /* public void CountryButton()
         {
             //Ensures that the event handler is there.
             AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseDownShape), true);
                   }*/

        private void MouseUpShape(MouseButtonEventArgs e)
        {
            Console.WriteLine("MouseUpShape");
            if (isDragging)
            {
                endDrag(e);
                isMarkedShape = true;//Indicates that the marked object is a Shape
                isMarked = true;//Indicates that something is marked
                                //removes glow from previous
                isDataEditable = true;
                removeOldGlow();
                //Selects shape
                selShape = TargetShape(e);
                //Sets glow on current
                selShape.IsSelected = 1;
            }
            e.Handled = true;
        }

        private void MouseMoveShape(MouseEventArgs e)//If the mouse is getting moved
        {
            if (isDragging)
            {
                // The Shape is gotten from the mouse event.
                var shape = TargetShape(e);
                // The mouse position relative to the target of the mouse event.
                var mousePosition = RelativeMousePosition(e);

                // The Shape is moved by the offset between the original and current mouse position.
                // The View (GUI) is then notified by the Shape, that its properties have changed.
                shape.X = _initialShapePosition.X + (mousePosition.X - initialMousePosition.X);
                shape.Y = _initialShapePosition.Y + (mousePosition.Y - initialMousePosition.Y);
            } else if (tempLineInUse)
            {
                tempLine.To = TargetShape(e);
            }
            e.Handled = true;

        }

        public void MouseDownShape(MouseButtonEventArgs e)
        {
            //CountryButton b = (CountryButton)sender;
            //            Canvas c = (Canvas)b.Parent;

            // Used for adding a Line.
            Console.WriteLine("Test: MouseDownShape happened");
            if (_isAddingLine)
            {
                addLineClick(e);
            }
            // Used for moving a Shape.
            else
            {
                //Moving shape, if there is no line being added.
                startDrag(e);
            }
            e.Handled = true;
        }

        private void removeOldGlow()
        {
            if (selectedLine != null)
            {
                selectedLine.IsSelected = 0;
            }
            if (selShape != null)
            {
                selShape.IsSelected = 0;
                selShape = dummyShape;
            }
        }

        private void MouseDownLine(MouseButtonEventArgs e)
        {
            //Removes glow from other lines
            removeOldGlow();

            isMarkedShape = false;//Indicates that the marked object is not a shape (it is then a line)
            isMarked = true;//Indicates that something is marked
            isDataEditable = false;


            //Selects new line.
            selectedLine = TargetLine(e);
            selectedLine.IsSelected = 1;
            e.MouseDevice.Target.CaptureMouse();
            e.Handled = true;

        }
        private void MouseUpLine(MouseButtonEventArgs e)
        {
            Console.WriteLine("MouseUpLine");
            e.MouseDevice.Target.ReleaseMouseCapture();
            e.Handled = true;

        }
        private void MouseMoveLine(MouseEventArgs e)
        {
            if (_isAddingLine && tempLineInUse)
            {
                if (!(tempLine.To == tempLineShape))
                {
                    tempLine.To = tempLineShape;
                }
                Point pos = RelativeMousePositionCanvas(e);
                tempLineShape.X = pos.X;
                tempLineShape.Y = pos.Y;
            }
            e.Handled = true;

        }

        private void MouseDownCanvas(MouseButtonEventArgs e)
        {
            //Removes glow from other lines
            removeOldGlow();
            isMarked = false;
            //e.Handled = true;
            isDataEditable = false;



        }
        private void MouseUpCanvas(MouseButtonEventArgs e)
        {
            Console.WriteLine("MouseUpCanvas");
            e.MouseDevice.Target.ReleaseMouseCapture();
            //e.Handled = true;

        }
        private void MouseMoveCanvas(MouseEventArgs e)
        {
            if (_isAddingLine && tempLineInUse)
            {
                if (!(tempLine.To == tempLineShape)){
                    tempLine.To = tempLineShape;
                }

                Point pos = RelativeMousePositionCanvas(e);
                tempLineShape.X = pos.X;
                tempLineShape.Y = pos.Y;
            }
            e.Handled = true;

            //Do nothing right now
        }

        private void addLineClick(MouseButtonEventArgs e) //A click that adds a line
        {
            Console.WriteLine("Test: MouseDownShape happened and isAddingLine==true");
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Console.WriteLine("Test: Mouse in Downstate");
                var shape = (Shape)(((FrameworkElement)e.MouseDevice.Target).DataContext);
                //TargetShape(e);

                if (_addingLineFrom == null)
                {
                    _addingLineFrom = shape;
                    _addingLineFrom.IsSelected = 1;
                    Point pos = RelativeMousePosition(e);
                    tempLine.From = shape;
                    tempLineShape.X = pos.X;
                    tempLineShape.Y = pos.Y;
                    tempLineInUse = true;
                }
                // If this is not the first Shape choosen, and therefore the second, 
                //  it is checked that the first and second Shape are different.
                else if (_addingLineFrom.UID != shape.UID)
                {
                    _undoRedoController.AddAndExecute(new AddLineCommand(Lines,
                        new Line() { From = _addingLineFrom, To = shape }));
                    _addingLineFrom.IsSelected = 0;
                    // The 'isAddingLine' and 'addingLineFrom' variables are cleared
                    _isAddingLine = false;
                    _addingLineFrom = null;
                    // The property used for visually indicating which Shape has already chosen are choosen is cleared, 
                    //  so the View can return to its original and default apperance.
                    RaisePropertyChanged(() => ModeOpacity);

                    tempLine.From = tempLineShape;
                    tempLine.To = tempLineShape;
                    //Marks that the temporary line isn't in use.
                    tempLineInUse = false;

                    if (isMarked && isMarkedShape) { isDataEditable = true; }
                }
            }
        }

        private void startDrag(MouseButtonEventArgs e)
        {
            // The Shape is gotten from the mouse event.
            var shape = TargetShape(e);
            // The mouse position relative to the target of the mouse event.
            var mousePosition = RelativeMousePosition(e);

            initialMousePosition = mousePosition;
            _initialShapePosition = new Point(shape.X, shape.Y);

            // The mouse is captured, so the current shape will always be the target of the mouse events, 
            //  even if the mouse is outside the application window.
            e.MouseDevice.Target.CaptureMouse();

            isDragging = true;
        }

        private void endDrag(MouseButtonEventArgs e)
        {
            // The Shape is gotten from the mouse event.
            var shape = TargetShape(e);
            // The mouse position relative to the target of the mouse event.
            var mousePosition = RelativeMousePosition(e);
            // The Shape is moved back to its original position, so the offset given to the move command works.
            shape.X = _initialShapePosition.X;
            shape.Y = _initialShapePosition.Y;

            _undoRedoController.AddAndExecute(new MoveShapeCommand(shape, mousePosition.X - initialMousePosition.X, mousePosition.Y - initialMousePosition.Y));

            // The mouse is released, as the move operation is done, so it can be used by other controls.
            e.MouseDevice.Target.ReleaseMouseCapture();
            //Indicates that drag has ended.
            isDragging = false;
        }

        private Point RelativeMousePosition(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var shapeVisualElement = (FrameworkElement)e.MouseDevice.Target;
            // The canvas holding the shapes visual element, is found by searching up the tree of visual elements.
            var canvas = FindParentOfType<Canvas>(shapeVisualElement);
            // The mouse position relative to the canvas is gotten here.
            return Mouse.GetPosition(canvas);
        }

        private Point RelativeMousePositionCanvas(MouseEventArgs e)
        {
            // Here the visual element that the mouse is captured by is retrieved.
            var canvas = (FrameworkElement)e.MouseDevice.Target;
            // The mouse position relative to the canvas is gotten here.
            return Mouse.GetPosition(canvas);
        }

        private static T FindParentOfType<T>(DependencyObject o)
        {
            dynamic parent = VisualTreeHelper.GetParent(o);
            return parent.GetType().IsAssignableFrom(typeof(T)) ? parent : FindParentOfType<T>(parent);
        }



        private void AddShapeOnCanvas()
        {

        }
        private void AddShapeWithLineCanvas()
        {

        }
         private void DeleteLine()
        {

        }
        private void DeleteShape()
        {

        }
        private void AddLineToShape()
        {

        }

    }

}