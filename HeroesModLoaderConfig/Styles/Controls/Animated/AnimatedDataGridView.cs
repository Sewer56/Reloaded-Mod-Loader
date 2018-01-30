using HeroesModLoaderConfig.Styles.Animation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HeroesModLoaderConfig.Styles.Animation.AnimMessage;
using static HeroesModLoaderConfig.Styles.Animation.AnimOverrides;

namespace HeroesModLoaderConfig.Styles.Controls.Animated
{
    /// <summary>
    /// Animated Data Grid View, Provides a Custom Implementation of the Animation Functions.
    /// </summary>
    class AnimatedDataGridView : DataGridView, IAnimatedControl
    {
        /// <summary>
        /// Stores the animation properties for backcolor and forecolor blending.
        /// </summary>
        public AnimProperties AnimProperties { get; set; }

        /// <summary>
        /// Defines the index of the last pressed button/element.
        /// </summary>
        private int LastIndex { get; set; }

        /// <summary>
        /// Stores the currently active messages for animations for each button.
        /// </summary>
        private List<AnimMessage> AnimationMessagesBG { get; set; }

        /// <summary>
        /// Stores the currently active messages for animations for each button.
        /// </summary>
        private List<AnimMessage> AnimationMessagesFG { get; set; }

        /// <summary>
        /// Override the font property to set the font for default cells.
        /// </summary>
        public override Font Font
        {
            get { return this.DefaultCellStyle.Font; }
            set { this.DefaultCellStyle.Font = value; }
        }

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedDataGridView()
        {
            // Instantiate all of the animation messages.
            this.AnimProperties = new AnimProperties();
            AnimationMessagesBG = new List<AnimMessage>();
            AnimationMessagesFG = new List<AnimMessage>();
            this.MouseWheel += AnimatedDataGridView_MouseWheel;
            LastIndex = 0;
        }

        // ////////////////////////
        // Animation Implementation
        // ////////////////////////
        protected override void OnSelectionChanged(EventArgs e)
        {
            // Call Base Constructor
            base.OnSelectionChanged(e);

            // Terminate Existing Messages/Effects and Start MouseLeaveAnimation on those.
            for (int x = 0; x < this.Rows.Count; x++)
            {
                // Play Entry Animation if selected.
                if (this.Rows[x].Selected)
                {
                    // Play Enter Animation
                    var newMessages = AnimHandler.AnimateMouseLeave(e, this.Rows[LastIndex].DefaultCellStyle, AnimProperties, AnimationMessagesBG[LastIndex], AnimationMessagesFG[LastIndex], DefaultCellStyle.SelectionBackColor);

                    // Retirieve the new messages.
                    AnimationMessagesBG[LastIndex] = newMessages.Item1;
                    AnimationMessagesFG[LastIndex] = newMessages.Item2;

                    // Set the index.
                    LastIndex = x;
                    return;
                }
            }
        }

        /// <summary>
        /// Append the animation leave messages for the current new rows.
        /// </summary>
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            LastIndex = 0;
            AnimationMessagesBG.Add(new AnimMessage(this));
            AnimationMessagesFG.Add(new AnimMessage(this));
        }

        ///////////////////////////
        // Scrolling Implementation
        ///////////////////////////

        /// <summary>
        /// Custom keyboard scrolling implementation.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) { IncrementRowIndex(); }
            else if (e.KeyCode == Keys.Up) { DecrementRowIndex(); }
            else if (e.KeyCode == Keys.Left) { SelectFirstRow(); }
            else if (e.KeyCode == Keys.Right) { SelectLastRow(); }
        }

        /// <summary>
        /// Stores the current change from 0 to the current scrolled to position.
        /// </summary>
        private int currentScrollDelta = 0;

        /// <summary>
        /// The amount of scrolling necessary for the user to move to the next item.
        /// </summary>
        const int scrollSensitivity = 120;

        /// <summary>
        /// Implement selection of the next/previous row with the use of scrolling
        /// using the mouse wheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimatedDataGridView_MouseWheel(object sender, MouseEventArgs e)
        {
            // Append to the current Delta
            currentScrollDelta = currentScrollDelta + e.Delta;

            // Obtain the amount of lines to scroll.
            // MouseWheelScrollLines = User's set amount of lines to scroll as set in Windows' settings.
            int linesToScroll = SystemInformation.MouseWheelScrollLines * currentScrollDelta / scrollSensitivity;

            // Deselect current item.
            if (linesToScroll >= 1)
            {
                try { DecrementRowIndex(); } catch { }
                currentScrollDelta = 0;
            }
            else if (linesToScroll <= -1)
            {
                try { IncrementRowIndex(); } catch { }
                currentScrollDelta = 0;
            }
        }

        /// <summary>
        /// Increments the row index by one, if we are on the last item, the first item
        /// will be selected.
        /// </summary>
        private void IncrementRowIndex()
        {
            // Get Next Index
            int nextRow = SelectedCells[0].RowIndex + 1;

            if (nextRow > Rows.Count - 1)
            {
                Rows[0].Selected = true;
                FirstDisplayedScrollingRowIndex = 0;
            }
            else
            {
                Rows[nextRow].Selected = true;
                FirstDisplayedScrollingRowIndex = nextRow;
            } 
        }

        /// <summary>
        /// Decrements the row index by one, if we are on the first item, the first item
        /// will be selected.
        /// </summary>
        private void DecrementRowIndex()
        {
            // Get Next Index
            int nextRow = SelectedCells[0].RowIndex - 1;

            if (nextRow < 0)
            {
                FirstDisplayedScrollingRowIndex = Rows.Count - 1;
                Rows[Rows.Count - 1].Selected = true;
            }
            else
            {
                FirstDisplayedScrollingRowIndex = nextRow;
                Rows[nextRow].Selected = true;
            }
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

        // ///////////////////////////////
        // IAnimatedControl implementation
        // ///////////////////////////////
        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }
    }
}
