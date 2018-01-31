using HeroesModLoaderConfig.Styles.Animation;
using HeroesModLoaderConfig.Styles.Controls.Enhanced;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace HeroesModLoaderConfig.Styles.Controls.Animated
{
    /// <summary>
    /// Animated Data Grid View, Provides a Custom Implementation of the Animation Functions.
    /// </summary>
    class AnimatedDataGridView : EnhancedDataGridView, IAnimatedControl
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

            // Set delegate for swapping items with CTRL + Mouse Wheel
            this.OnRowSwapped += AnimateSwap;

            // Set initial index for last animation (default selection)
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
                // Play Leave Animation for previously selected.
                // You only want to cancel last animation.
                // This loop gets current item to set next item to animate leave for.
                // Note: At this point Rows[x].Selected is outdated and actually is the value of the last row.
                // Note: SelectedCells[0].RowIndex stores the actual selected row index.
                if (this.Rows[x].Selected)
                {
                    try
                    {
                        AnimateLeaveAtIndex(LastIndex, x);
                        return;
                    }
                    // Last index does not exist.
                    // Can happen if last index was at the end of the datagridview and was removed since.
                    // Ignore :V
                    catch { }
                }
            }
        }

        /// <summary>
        /// Plays the leave animation for a specific particular row at the passed in index.
        /// </summary>
        /// <param name="leaveIndex">The index of the row for which to play the leave animation for.</param>
        /// <param name="lastIndex">The current selection index.</param>
        private void AnimateLeaveAtIndex(int leaveIndex, int lastIndex)
        {
            // Play Leave Animation for the last item.
            var newMessages = AnimHandler.AnimateMouseLeave(null, this.Rows[leaveIndex].DefaultCellStyle, AnimProperties, AnimationMessagesBG[leaveIndex], AnimationMessagesFG[leaveIndex], DefaultCellStyle.SelectionBackColor);

            // Retrieve the new message instances.
            AnimationMessagesBG[leaveIndex] = newMessages.Item1;
            AnimationMessagesFG[leaveIndex] = newMessages.Item2;

            // Set the index of the last item.
            LastIndex = lastIndex;
        }

        /// <summary>
        /// Append the animation leave messages for the current new rows.
        /// </summary>
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            // Run base.
            base.OnRowsAdded(e);

            // Reset animations.
            ResetAnimationsOnRowAdd();
        }

        /// <summary>
        /// Resets all animations and sets up the list of animation states.
        /// </summary>
        private void ResetAnimationsOnRowAdd()
        {
            // Cancel all current animations.
            foreach (AnimMessage message in AnimationMessagesBG) { message.PlayAnimation = false; }
            foreach (AnimMessage message in AnimationMessagesFG) { message.PlayAnimation = false; }

            // Create animation messages of count equal to newly added rows.
            AnimMessage[] animMessageBG = new AnimMessage[RowCount];
            AnimMessage[] animMessageFG = new AnimMessage[RowCount];

            // Fill array.
            for (int x = 0; x < RowCount; x++)
            {
                animMessageBG[x] = new AnimMessage();
                animMessageFG[x] = new AnimMessage();
            }

            // Assign list of animations.
            AnimationMessagesBG = animMessageBG.ToList();
            AnimationMessagesFG = animMessageFG.ToList();
        }

        /// <summary>
        /// Resets all of the colours then fades out the last item location.
        /// </summary>
        private void AnimateSwap(int itemToAnimate)
        {
            // Reset animations.
            ResetAnimationsOnRowAdd();

            // Reset all colours. (After killing animations)
            for (int x = 0; x < this.Rows.Count; x++)
            {
                Rows[x].DefaultCellStyle.BackColor = this.DefaultCellStyle.BackColor;
                Rows[x].DefaultCellStyle.ForeColor = this.DefaultCellStyle.ForeColor;
            }

            // Animate the last index row.
            AnimateLeaveAtIndex(itemToAnimate, SelectedCells[0].RowIndex);
        }

        // ///////////////////////////////
        // IAnimatedControl implementation
        // ///////////////////////////////
        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }

        // //////////////////////////////////////////////////
        // Override for EnhancedListView's Drag/Drop Finisher
        // //////////////////////////////////////////////////

        /// <summary>
        /// See parent/base class implementation.
        /// Fixes possible case of items with leftover highlight colour after
        /// the mouse is released.
        /// </summary>
        protected override void EnhancedDataGridView_DragDrop(object sender, DragEventArgs e)
        {
            // Call base method.
            base.EnhancedDataGridView_DragDrop(sender, e);

            // Reset colour for last selection + 1 (if exists)
            // When the user drops the item into a new location, when the original location of the item removed
            // the next element would automatically be marked as selected. 

            // Problem:
            // If an item is inserted, the selection below would still remain selected, but if we set the selection.
            // To our new dragged element, a leave animation would play, causing leave animations to be played on 2 items.
            // Extra note: Leave animation terminated immediately due to OnRowsAdded() cancelling it.

            // This workaround sets the fore and backcolor of the row as if it were never to be animated in the first place.
            int nextRow = base.DragRowIndex + 1;
            if (nextRow < this.Rows.Count)
            {
                Rows[base.DragRowIndex + 1].DefaultCellStyle.BackColor = this.DefaultCellStyle.BackColor;
                Rows[base.DragRowIndex + 1].DefaultCellStyle.ForeColor = this.DefaultCellStyle.ForeColor;
            }

            // Animate the last selection.
            AnimateLeaveAtIndex(base.DragRowIndex, SelectedCells[0].RowIndex);
        }
    }
}
