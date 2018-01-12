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
            AnimationMessagesBG.Add(new AnimMessage(this));
            AnimationMessagesFG.Add(new AnimMessage(this));
        }

        // ///////////////////////////////
        // IAnimatedControl implementation
        // ///////////////////////////////
        public void OnMouseEnterWrapper(EventArgs e) { base.OnMouseEnter(e); }
        public void OnMouseLeaveWrapper(EventArgs e) { base.OnMouseEnter(e); }
    }
}
