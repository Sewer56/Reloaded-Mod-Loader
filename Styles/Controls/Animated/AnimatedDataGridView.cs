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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Reloaded_GUI.Styles.Animation;
using Reloaded_GUI.Styles.Controls.Enhanced;

namespace Reloaded_GUI.Styles.Controls.Animated
{
    /// <summary>
    /// Animated Data Grid View, Provides a Custom Implementation of the Animation Functions.
    /// </summary>
    public class AnimatedDataGridView : EnhancedDataGridView, IAnimatedControl
    {
        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public AnimatedDataGridView()
        {
            // 2x Buffering
            DoubleBuffered = true;

            // Instantiate all of the animation messages.
            AnimProperties = new AnimProperties();
            AnimationMessagesBg = new List<AnimMessage>();
            AnimationMessagesFg = new List<AnimMessage>();

            // Set delegate for swapping items with CTRL + Mouse Wheel
            OnRowSwapped += AnimateSwap;

            // Set initial index for last animation (default selection)
            LastIndex = 0;
        }

        /// <summary>
        /// Defines the index of the last pressed button/element.
        /// </summary>
        private int LastIndex { get; set; }

        /// <summary>
        /// Stores the currently active messages for animations for each button.
        /// </summary>
        private List<AnimMessage> AnimationMessagesBg { get; set; }

        /// <summary>
        /// Stores the currently active messages for animations for each button.
        /// </summary>
        private List<AnimMessage> AnimationMessagesFg { get; set; }

        /// <summary>
        /// Stores the animation properties for backcolor and forecolor blending.
        /// </summary>
        public AnimProperties AnimProperties { get; set; }

        // ///////////////////////////////
        // IAnimatedControl implementation
        // ///////////////////////////////
        public void OnMouseEnterWrapper(EventArgs e) { OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { OnMouseEnter(e); }

        /// <summary>
        /// Stops ongoing animations.
        /// </summary>
        public void KillAnimations()
        {
            foreach (AnimMessage animMessage in AnimationMessagesBg) animMessage.PlayAnimation = false;
            foreach (AnimMessage animMessage in AnimationMessagesFg) animMessage.PlayAnimation = false;
        }

        // ////////////////////////
        // Animation Implementation
        // ////////////////////////
        protected override void OnSelectionChanged(EventArgs e)
        {
            // Call Base Constructor
            base.OnSelectionChanged(e);

            // Terminate Existing Messages/Effects and Start MouseLeaveAnimation on those.
            for (int x = 0; x < Rows.Count; x++)
            {
                // Play Leave Animation for previously selected.
                // You only want to cancel last animation.
                // This loop gets current item to set next item to animate leave for.
                // Note: At this point Rows[x].Selected is outdated and actually is the value of the last row.
                // Note: SelectedCells[0].RowIndex stores the actual selected row index.
                if (Rows[x].Selected)
                {
                    try
                    {
                        AnimateLeaveAtIndex(LastIndex, x);
                        return;
                    }
                    // Last index does not exist. Ignore.
                    // Can happen if last index was at the end of the datagridview and was removed since.
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
            var newMessages = AnimDispatcher.AnimateMouseLeave(null, Rows[leaveIndex].DefaultCellStyle, AnimProperties, AnimationMessagesBg[leaveIndex], AnimationMessagesFg[leaveIndex], DefaultCellStyle.SelectionBackColor);

            // Retrieve the new message instances.
            AnimationMessagesBg[leaveIndex] = newMessages.Item1;
            AnimationMessagesFg[leaveIndex] = newMessages.Item2;

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
            foreach (AnimMessage message in AnimationMessagesBg) message.PlayAnimation = false;
            foreach (AnimMessage message in AnimationMessagesFg) message.PlayAnimation = false;

            // Create animation messages of count equal to newly added rows.
            AnimMessage[] animMessageBg = new AnimMessage[RowCount];
            AnimMessage[] animMessageFg = new AnimMessage[RowCount];

            // Fill array.
            for (int x = 0; x < RowCount; x++)
            {
                animMessageBg[x] = new AnimMessage(Rows[x].DefaultCellStyle);
                animMessageFg[x] = new AnimMessage(Rows[x].DefaultCellStyle);
            }

            // Assign list of animations.
            AnimationMessagesBg = animMessageBg.ToList();
            AnimationMessagesFg = animMessageFg.ToList();
        }

        /// <summary>
        /// Resets all of the colours then fades out the last item location.
        /// </summary>
        private void AnimateSwap(int itemToAnimate)
        {
            // Reset animations.
            ResetAnimationsOnRowAdd();

            // Reset all colours. (After killing animations)
            for (int x = 0; x < Rows.Count; x++)
            {
                Rows[x].DefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
                Rows[x].DefaultCellStyle.ForeColor = DefaultCellStyle.ForeColor;
            }

            // Animate the last index row.
            AnimateLeaveAtIndex(itemToAnimate, SelectedCells[0].RowIndex);
        }

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
            int nextRow = DragRowIndex + 1;
            if (nextRow < Rows.Count)
            {
                Rows[DragRowIndex + 1].DefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
                Rows[DragRowIndex + 1].DefaultCellStyle.ForeColor = DefaultCellStyle.ForeColor;
            }

            // Animate the last selection.
            AnimateLeaveAtIndex(DragRowIndex, SelectedCells[0].RowIndex);
        }
    }
}
