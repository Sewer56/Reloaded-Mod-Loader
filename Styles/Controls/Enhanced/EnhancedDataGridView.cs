/*
    [Reloaded] Mod Loader Launcher
    The launcher for a universal, powerful, multi-game and multi-process mod loader
    based off of the concept of DLL Injection to execute arbitrary program code.
    Copyright (C) 2018  Sewer. Sz (Sewer56)

    [Reloaded] is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    [Reloaded] is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>
*/

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Reloaded_GUI.Styles.Controls.Enhanced
{
    /// <summary>
    /// Customized DataGridView with support for dragging items.
    /// </summary>
    public class EnhancedDataGridView : DataGridView
    {
        ///////////////////////////
        // Scrolling Implementation
        ///////////////////////////

        /// <summary>
        /// Empty delegate for fiding events when user swaps two items by holding CTRL while scrolling mouse wheel.
        /// </summary>
        public delegate void OnWheelDelegate(int lastRow);

        /// <summary>
        /// The amount of scrolling necessary for the user to move to the next item.
        /// </summary>
        private const int ScrollSensitivity = 120;

        /// <summary>
        /// Stores the current change from 0 to the current scrolled to position.
        /// </summary>
        private int _currentScrollDelta;

        /// <summary>
        /// Stores the dimension of the listview row (rectangle) that is to be dragged. 
        /// </summary>
        private Rectangle _dimensionsOfRowToDrag;

        /// <summary>
        /// Stores the individual cell that the user clicks to start the drag operation.
        /// </summary>
        private DataGridViewRow _rowToDrag;

        /// <summary>
        /// Override the font property to set the font for default cells.
        /// </summary>
        public override Font Font
        {
            get => DefaultCellStyle.Font;
            set => DefaultCellStyle.Font = value;
        }

        /// <summary>
        /// Constructor for the custom class.
        /// </summary>
        public EnhancedDataGridView()
        {
            // Add wheel support
            MouseWheel += AnimatedDataGridView_MouseWheel;

            // Redirect all painting to us.
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Drag & Drop
            DragEnter += EnhancedDataGridView_DragEnter;
            MouseMove += EnhancedDataGridView_MouseMove;
            MouseDown += EnhancedDataGridView_MouseDown;
            DragOver += EnhancedDataGridView_DragOver;
            DragDrop += EnhancedDataGridView_DragDrop;
        }

        /////////////////////////////
        // Drag & Drop Implementation
        /////////////////////////////

        /// <summary>
        /// Allows/disables drag & drop reordering.
        /// </summary>
        [Category("| Custom Options")][Description("Allows reordering (for single row) via Drag + Drop & Ctrl+Scroll. Requires AllowDrop to be set to true.")]
        public bool ReorderingEnabled { get; set; }

        /// <summary>
        /// Stores the row index where the row drag operation started from.
        /// </summary>
        public int DragRowIndex { get; set; }

        /// <summary>
        /// The entry point for the dragging handler.
        /// Obtains the row the mouse hovers above when the user clicks the left mouse button to initiate dragging.
        /// Obtains the amount of space mouse needs to move to start the dragging operation.
        /// </summary>
        private void EnhancedDataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            // Reset currently dragged rectangle.
            _dimensionsOfRowToDrag = Rectangle.Empty;

            // Get the index of the item the mouse has hit below.
            var hittestInfo = HitTest(e.X, e.Y);

            // Check if the mouse is over an item in the DataGridView.
            // Otherwise do nothing.
            if (hittestInfo.RowIndex != -1 && hittestInfo.ColumnIndex != -1)
            {
                // Set row index for dragging.
                DragRowIndex = hittestInfo.RowIndex;

                // Assign row the user clicks to start drag operation. 
                _rowToDrag = Rows[DragRowIndex];

                if (_rowToDrag != null)
                {
                    // The DragSize indicates the size that the mouse can move 
                    // before a drag event should be started.                
                    Size dragSize = SystemInformation.DragSize;

                    // e.X = X Mouse location in terms of screen coordinates.
                    // e.Y = Y Mouse location in terms of screen coordinates.

                    // e.X - dragSize.Width / 2 = Left side of rectangle

                    // Create a rectangle using the DragSize, with the mouse position being
                    // at the center of the rectangle.
                    _dimensionsOfRowToDrag = 
                    new Rectangle
                    (
                        // The location of the rectangle, top left corner.
                        new Point
                        (
                            e.X - dragSize.Width, // Left Edge
                            e.Y - dragSize.Height // Top Edge
                        ), 

                        // Set the drag size.
                        new Size
                        (
                            dragSize.Width * 2, // Width to Right Edge
                            dragSize.Height * 2 // Width to Bottom Edge
                        )
                    );
                }
            }
        }

        /// <summary>
        /// When the user moves the mouse inside the DataGridView.
        /// Checks if the left mouse button is held and triggers a drag and drop operation if
        /// the mouse is outside of the calculated area for starting the drag operation.
        /// Triggers if the user is currently dragging the object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnhancedDataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            // Check for left mouse button press.
            // Check if drag/drop is enabled.
            if (e.Button.HasFlag(MouseButtons.Left) && ReorderingEnabled)
                if (_dimensionsOfRowToDrag != Rectangle.Empty && !_dimensionsOfRowToDrag.Contains(e.X, e.Y))
                    DoDragDrop(_rowToDrag, DragDropEffects.Copy);
        }

        /// <summary>
        /// When user starts dragging into listview, set the drag effect to copy item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnhancedDataGridView_DragEnter(object sender, DragEventArgs e) { e.Effect = DragDropEffects.Copy; }

        /// <summary>
        /// When the object is dragged inside the area of the bounds of the control. 
        /// Set drag effect to copy item.
        /// </summary>
        private void EnhancedDataGridView_DragOver(object sender, DragEventArgs e) { e.Effect = DragDropEffects.Copy; }

        /// <summary>
        /// Occurs when the user releases the mouse button to complete the drag/drop operation.
        /// Checks the row index which overlaps the current selection, removing the
        /// original item and placing it inside at the new row index.
        /// </summary>
        /// <param name="sender">Nothing special in there.</param>
        /// <param name="e">Drag event arguments contain a copy of the original data we started dragging.</param>
        protected virtual void EnhancedDataGridView_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations set in DragEventArgs e for this events are relative 
            // to the screen (and not the window), they must be converted to client coordinates.
            Point clientPoint = PointToClient(new Point(e.X, e.Y));

            // If the drag operation was a copy (our operation). 
            // then find the row the mouse currently overlaps and insert our
            // new row into that slot.

            // First the original is deleted, then is inserted into the
            // row index which overlaps the mouse at the end of the drag operation.
            if (e.Effect == DragDropEffects.Copy)
            {
                // Obtain the row to add.
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));

                // Obtain the Hit Testing information to obtain row the mouse is currently above.
                var hitTest = HitTest(clientPoint.X, clientPoint.Y);

                // Check if the drag operation is hovering over a row.
                if (hitTest.ColumnIndex != -1 && hitTest.RowIndex != -1)
                {
                    // Remove the original row.
                    Rows.RemoveAt(DragRowIndex);

                    // Insert the original row at new index where the mouse hit.
                    Rows.Insert(hitTest.RowIndex, row);

                    // Set the currently selected row to dropped row.
                    Rows[hitTest.RowIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// Event fires when the user swaps two items by holding CTRL while scrolling the mouse wheel.
        /// </summary>
        public event OnWheelDelegate OnRowSwapped;

        /// <summary>
        /// Custom keyboard scrolling implementation.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                IncrementRowIndex();
            else if (e.KeyCode == Keys.Up)
                DecrementRowIndex();
            else if (e.KeyCode == Keys.Left)
                SelectFirstRow();
            else if (e.KeyCode == Keys.Right) SelectLastRow();
        }

        /// <summary>
        /// Implement selection of the next/previous row with the use of scrolling
        /// using the mouse wheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimatedDataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            // Append to the current Delta
            _currentScrollDelta = _currentScrollDelta + e.Delta;

            // Obtain the amount of lines to scroll.
            // MouseWheelScrollLines = User's set amount of lines to scroll as set in Windows' settings.
            int linesToScroll = SystemInformation.MouseWheelScrollLines * _currentScrollDelta / ScrollSensitivity;

            // Scroll either up or down.
            if (linesToScroll >= 1) // Scroll up
            {
                // If Control is Held & Reorder is Enabled, Reorder As Well as Changing Selection.
                if (ModifierKeys.HasFlag(Keys.Control) && ReorderingEnabled)
                    SwapRowUpwards();
                else
                    DecrementRowIndex();

                _currentScrollDelta = 0;
            }
            else if (linesToScroll <= -1) // Scroll down
            {
                // If Control is Held & Reorder is Enabled, Reorder As Well as Changing Selection.
                if (ModifierKeys.HasFlag(Keys.Control) && ReorderingEnabled)
                    SwapRowDownwards();
                else
                    IncrementRowIndex();

                _currentScrollDelta = 0;
            }
        }

        /// <summary>
        /// Increments the row index by one, if we are on the last item, the first item
        /// will be selected.
        /// </summary>
        private void IncrementRowIndex()
        {
            // Check if there is a selected row.
            if (SelectedCells.Count >= 1)
            {
                // Get Next Index
                int nextRow = SelectedCells[0].RowIndex + 1;

                if (nextRow > Rows.Count - 1)
                {
                    Rows[0].Selected = true;
                    CurrentCell = Rows[0].Cells[0];
                }
                else
                {
                    Rows[nextRow].Selected = true;
                    CurrentCell = Rows[nextRow].Cells[0];
                }
            }
        }

        /// <summary>
        /// Decrements the row index by one, if we are on the first item, the first item
        /// will be selected.
        /// </summary>
        private void DecrementRowIndex()
        {
            // Check if there is a selected row.
            if (SelectedCells.Count >= 1)
            {
                // Get Next Index
                int nextRow = SelectedCells[0].RowIndex - 1;

                if (nextRow < 0)
                {
                    Rows[Rows.Count - 1].Selected = true;
                    CurrentCell = Rows[Rows.Count - 1].Cells[0];
                }
                else
                {
                    Rows[nextRow].Selected = true;
                    CurrentCell = Rows[nextRow].Cells[0];
                }
            }
        }

        /// <summary>
        /// Swaps the current selected row with the row that is above the current row.
        /// </summary>
        private void SwapRowUpwards()
        {
            // Get Index of Row Above
            int nextRowIndex = SelectedCells[0].RowIndex - 1;

            // Get current row.
            int currentRowIndex = SelectedCells[0].RowIndex;

            // Obtain current row.
            DataGridViewRow currentRow = Rows[currentRowIndex];

            // Select last row index.
            if (nextRowIndex < 0) // Swap first row with last row.
            {
                // Get rows.
                DataGridViewRow lastRow = Rows[Rows.Count - 1];

                // Remove first and last item.
                Rows.RemoveAt(currentRowIndex);
                Rows.RemoveAt(Rows.Count - 1);

                // Insert items in swapped order.
                Rows.Insert(currentRowIndex, lastRow);
                Rows.Add(currentRow);

                // Make the last row visible.
                FirstDisplayedScrollingRowIndex = Rows.Count - 1;
                Rows[Rows.Count - 1].Selected = true;
            }
            else
            {
                // Remove current item & insert above next item.
                Rows.RemoveAt(currentRowIndex);
                Rows.Insert(nextRowIndex, currentRow);

                // Make the last row visible.
                FirstDisplayedScrollingRowIndex = nextRowIndex;
                Rows[nextRowIndex].Selected = true;
            }

            // Invoke swap delegate
            OnRowSwapped?.Invoke(currentRowIndex);
        }

        /// <summary>
        /// Swaps the current selected row with the row that is below the current row.
        /// </summary>
        private void SwapRowDownwards()
        {
            // Get Next Index
            int nextRowIndex = SelectedCells[0].RowIndex + 1;

            // Get current row.
            int currentRowIndex = SelectedCells[0].RowIndex;

            // Get rows.
            DataGridViewRow currentRow = Rows[currentRowIndex];

            // If the row is the last row.
            if (nextRowIndex > Rows.Count - 1)
            {
                // Get rows.
                DataGridViewRow firstRow = Rows[0];

                // Remove last and first row.
                Rows.RemoveAt(currentRowIndex);
                Rows.RemoveAt(0);

                // Insert rows swapped.
                Rows.Insert(0, currentRow);
                Rows.Add(firstRow);

                // Show 1st row
                FirstDisplayedScrollingRowIndex = 0;
                Rows[0].Selected = true;
            }
            else
            { 
                // Remove last and first row.
                Rows.RemoveAt(currentRowIndex);
                Rows.Insert(nextRowIndex, currentRow);

                // Show next row.
                FirstDisplayedScrollingRowIndex = nextRowIndex;
                Rows[nextRowIndex].Selected = true;
            }

            // Invoke swap delegate
            OnRowSwapped?.Invoke(currentRowIndex);
        }

        /// <summary>
        /// Selects the first row of the DataGridView
        /// </summary>
        private void SelectFirstRow()
        {
            Rows[0].Selected = true;
            FirstDisplayedScrollingRowIndex = 0;
        }

        /// <summary>
        /// Selects the last row of the DataGridView
        /// </summary>
        private void SelectLastRow()
        {
            Rows[Rows.Count - 1].Selected = true;
            FirstDisplayedScrollingRowIndex = Rows.Count - 1;
        }
    }
}
